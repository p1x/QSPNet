using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace QSP.CodeAnalysis {
    public class Emitter {
        private const string RuntimeAssemblyName = "System.Runtime";
        private const string ProgramClassName = "Program";
        private const string CtorMethodName = ".ctor";

        private readonly AssemblyDefinition _assembly;
        private readonly AssemblyDefinition _runtimeAssembly;
        private readonly IReadOnlyList<AssemblyDefinition> _referenceAssemblies;
        private readonly string _defaultNamespace;

        private Emitter(AssemblyDefinition assembly, AssemblyDefinition runtimeAssembly, IReadOnlyList<AssemblyDefinition> referenceAssemblies) {
            _assembly = assembly;
            _runtimeAssembly = runtimeAssembly;
            _referenceAssemblies = referenceAssemblies;
            _defaultNamespace = assembly.Name.Name;
        }

        public static Emitter Create(string moduleName, IEnumerable<string> referenceAssembliesPaths) {
            var assemblyName = new AssemblyNameDefinition(moduleName, new Version(1, 0, 0, 0));
            var assembly = AssemblyDefinition.CreateAssembly(
                assemblyName,
                moduleName,
                ModuleKind.Console
                // new ModuleParameters {
                //     Architecture = TargetArchitecture.I386,
                //     Kind = ModuleKind.Console,
                //     Runtime = TargetRuntime.Net_4_0,
                // }
            );

            var referenceAssemblies = referenceAssembliesPaths.Select(AssemblyDefinition.ReadAssembly).ToList();
            var runtimeAssembly = referenceAssemblies.FirstOrDefault(x => x.Name.Name == RuntimeAssemblyName);
            if (runtimeAssembly != null)
                ImportRuntime(assembly, runtimeAssembly);
            
            return new Emitter(assembly, runtimeAssembly, referenceAssemblies);
        }

        private static void ImportRuntime(AssemblyDefinition targetAssembly, AssemblyDefinition runtimeAssembly) => 
            targetAssembly.MainModule.AssemblyReferences.Add(runtimeAssembly.Name);

        public DiagnosticBag Emit(BoundGlobalScope scope, string outputPath) {
            if (scope.Diagnostics.Any())
                return scope.Diagnostics;
            
            var module = _assembly.MainModule;

            var programType = new TypeDefinition(
                _defaultNamespace,
                ProgramClassName,
                TypeAttributes.Class | TypeAttributes.Public,
                module.TypeSystem.Object
            );
            module.Types.Add(programType);
            
            var mainMethod = EmitMethod("Main", scope.Statements, scope.Variables);
            
            programType.Methods.Add(mainMethod);
            
            _assembly.EntryPoint = mainMethod;
            
            _assembly.Write(outputPath);

            return scope.Diagnostics;
        }

        private MethodDefinition EmitMethod(string name, ImmutableArray<BoundStatement> statements, in ImmutableArray<VariableSymbol> variables) {
            var method = new MethodDefinition(
                name,
                MethodAttributes.Public | MethodAttributes.Static,
                _assembly.MainModule.TypeSystem.Void
            );
            
            // create the method body
            var il = method.Body.GetILProcessor();
            
            il.Append(il.Create(OpCodes.Nop));
            il.Append(il.Create(OpCodes.Ret));

            return method;
        }
    }
}