using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace QSP.CodeAnalysis {
    public class Emitter {
        private const string RuntimeAssemblyName = "System.Runtime";
        private const string QSPAssemblyName = "QSP.Runtime";
        private const string ProgramClassName = "Program";
        private const string CtorMethodName = ".ctor";
        private const string QSPGlobalName = "QSP.Runtime.Global";

        private readonly AssemblyDefinition _assembly;
        private readonly IReadOnlyList<AssemblyDefinition> _referenceAssemblies;
        private readonly string _defaultNamespace;

        private readonly Dictionary<VariableSymbol, VariableDefinition> _variables = new Dictionary<VariableSymbol, VariableDefinition>();
        
        private Emitter(AssemblyDefinition assembly, IReadOnlyList<AssemblyDefinition> referenceAssemblies) {
            _assembly = assembly;
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
            
            return new Emitter(assembly, referenceAssemblies);
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

            // TODO Cleanup
            _variables.Clear();
            _methods.Clear();
            
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

            foreach (var statement in statements)
                EmitStatement(il, statement);

            il.Append(il.Create(OpCodes.Nop));
            il.Append(il.Create(OpCodes.Ret));

            return method;
        }

        private void EmitStatement(ILProcessor il, BoundStatement statement) {
            switch (statement) {
                case BoundAssignmentStatement x: EmitAssignmentStatement(il, x); break;
                case BoundExpressionStatement x: EmitExpressionStatement(il, x); break;
                case BoundProcedureStatement  x: EmitProcedureStatement(il, x); break;
                default: throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }

        private void EmitAssignmentStatement(ILProcessor il, BoundAssignmentStatement statement) {
            var variableDefinition = TryEmitVariableDefinition(il, statement.Variable);
            EmitExpression(il, statement.Expression);
            il.Emit(OpCodes.Stloc, variableDefinition);
        }

        private void EmitExpressionStatement(ILProcessor il, BoundExpressionStatement statement) {
            EmitExpression(il, statement.Expression);

            var clrTypeName = statement.Expression.Type.ClrType.FullName;
            il.Emit(OpCodes.Call, GetMethodReference(QSPGlobalName, "PrintLineMain", new []{ clrTypeName }));
        }

        private void EmitProcedureStatement(ILProcessor il, BoundProcedureStatement statement) {
            if (statement.Procedure == ProcedureSymbol.PrintLine) {
                foreach (var argument in statement.Arguments)
                    EmitExpression(il, argument);

                var methodName = statement.WithModifier ? "PrintLineMain" : "PrintLine";
                var argumentTypeNames = statement.Arguments.Select(x => x.Type.ClrType.FullName).ToArray();

                il.Emit(OpCodes.Call, GetMethodReference(QSPGlobalName, methodName, argumentTypeNames));


            } else {
                throw new ArgumentException($"Unknown procedure '{statement.Procedure.Name}'", nameof(statement));
            }
        }

        private readonly struct MethodSignature : IEquatable<MethodSignature> {
            public MethodSignature(string typeName, string methodName, string[] parameterTypeNames) {
                if (parameterTypeNames.Length > 5)
                    throw new ArgumentException("parameterTypeNames.Length > 5", nameof(parameterTypeNames));
                
                TypeName = typeName;
                MethodName = methodName;
                ParameterTypeNames = parameterTypeNames;
            }

            public string TypeName { get; }

            public string MethodName { get; }

            public string[] ParameterTypeNames { get; }

            public bool Equals(MethodSignature other) {
                return TypeName == other.TypeName && MethodName == other.MethodName && ParameterTypeNames.Equals(other.ParameterTypeNames);
            }

            public override int GetHashCode() {
                if (ParameterTypeNames.Length == 0)
                    return HashCode.Combine(TypeName, MethodName);
                
                var p = ParameterTypeNames.Length switch {
                    1 => ParameterTypeNames[0].GetHashCode(),
                    2 => HashCode.Combine(ParameterTypeNames[0], ParameterTypeNames[1]),
                    3 => HashCode.Combine(ParameterTypeNames[0], ParameterTypeNames[1], ParameterTypeNames[2]),
                    4 => HashCode.Combine(ParameterTypeNames[0], ParameterTypeNames[1], ParameterTypeNames[2], ParameterTypeNames[3]),
                    5 => HashCode.Combine(ParameterTypeNames[0], ParameterTypeNames[1], ParameterTypeNames[2], ParameterTypeNames[3], ParameterTypeNames[4]),
                    _ => throw new NotSupportedException()
                };

                return HashCode.Combine(TypeName, MethodName, p);
            }

            public override bool Equals(object? obj) => obj is MethodSignature other && Equals(other);
            public static bool operator ==(MethodSignature left, MethodSignature right) => left.Equals(right);
            public static bool operator !=(MethodSignature left, MethodSignature right) => !left.Equals(right);
        }
        
        private readonly Dictionary<MethodSignature, MethodReference> _methods = new Dictionary<MethodSignature, MethodReference>();
        
        private MethodReference GetMethodReference(string typeName, string methodName, string[] argumentTypeNames) {
            var key = new MethodSignature(typeName, methodName, argumentTypeNames);
            if (_methods.TryGetValue(key, out var methodReference))
                return methodReference;

            methodReference = ImportMethodReference(typeName, methodName, argumentTypeNames);
            _methods.Add(key, methodReference);
            return methodReference;
        }
        
        private MethodReference ImportMethodReference(string typeName, string methodName, string[] parameterTypeNames) {
            var foundTypes = _referenceAssemblies.SelectMany(a => a.Modules)
                .SelectMany(m => m.Types)
                .Where(t => t.FullName == typeName)
                .ToArray();
            if (foundTypes.Length == 1)
            {
                var foundType = foundTypes[0];
                var methods = foundType.Methods.Where(m => m.Name == methodName);

                foreach (var method in methods)
                {
                    if (method.Parameters.Count != parameterTypeNames.Length)
                        continue;

                    var allParametersMatch = true;

                    for (var i = 0; i < parameterTypeNames.Length; i++)
                    {
                        if (method.Parameters[i].ParameterType.FullName != parameterTypeNames[i])
                        {
                            allParametersMatch = false;
                            break;
                        }
                    }

                    if (!allParametersMatch)
                        continue;

                    return _assembly.MainModule.ImportReference(method);
                }

                //_diagnostics.ReportRequiredMethodNotFound(typeName, methodName, parameterTypeNames);
                return null!;
            }
            else if (foundTypes.Length == 0)
            {
                //_diagnostics.ReportRequiredTypeNotFound(null, typeName);
            }
            else
            {
                //_diagnostics.ReportRequiredTypeAmbiguous(null, typeName, foundTypes);
            }

            return null!;
        }
        
        private void EmitExpression(ILProcessor il, BoundExpression expression) {
            switch (expression) {
                case BoundLiteralExpression    x: EmitLiteralExpression(il, x); break;
                case BoundVariableExpression   x: EmitVariableExpression(il, x); break;
                case BoundUnaryExpression      x: EmitUnaryExpression(il, x); break;
                case BoundBinaryExpression     x: EmitBinaryExpression(il, x); break;
                case BoundFunctionExpression   x: EmitFunctionExpression(il, x); break;
                case BoundConversionExpression x: EmitConversionExpression(il, x); break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }
        }

        private static void EmitLiteralExpression(ILProcessor il, BoundLiteralExpression expression) {
            if (expression.Type.ClrType == typeof(int))
                il.Emit(OpCodes.Ldc_I4, (int)expression.Value);
            else if (expression.Type.ClrType == typeof(string))
                il.Emit(OpCodes.Ldstr, (string)expression.Value);
            else
                throw new ArgumentOutOfRangeException();
        }

        private void EmitVariableExpression(ILProcessor il, BoundVariableExpression expression) {
            var variableDefinition = TryEmitVariableDefinition(il, expression.Variable);

            il.Emit(OpCodes.Ldloc, variableDefinition);
        }

        private VariableDefinition TryEmitVariableDefinition(ILProcessor il, VariableSymbol variableSymbol) {
            if (_variables.TryGetValue(variableSymbol, out var variableDefinition))
                return variableDefinition;
            
            var typeReference = GetTypeReference(variableSymbol.Type);
            variableDefinition = new VariableDefinition(typeReference);

            il.Body.Variables.Add(variableDefinition);

            _variables.Add(variableSymbol, variableDefinition);

            return variableDefinition;
        }

        private void EmitFunctionExpression(ILProcessor il, BoundFunctionExpression expression) {
            if (expression.Function == FunctionSymbol.Input) {
                foreach (var argument in expression.Arguments) {
                    EmitExpression(il, argument);
                }
                var argumentTypeNames = expression.Arguments.Select(x => x.Type.ClrType.FullName).ToArray();
                il.Emit(OpCodes.Call, GetMethodReference(QSPGlobalName, "Input", argumentTypeNames));
            } else {
                throw new ArgumentException($"Unknown function '{expression.Function.Name}'", nameof(expression));
            }
        }

        private void EmitUnaryExpression(ILProcessor il, BoundUnaryExpression expression) {
            EmitExpression(il, expression.Operand);

            switch (expression.Operator.Kind) {
                case BoundUnaryOperatorKind.Identity:
                    // nop
                    break;
                case BoundUnaryOperatorKind.Negation:
                    il.Emit(OpCodes.Neg);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EmitBinaryExpression(ILProcessor il, BoundBinaryExpression expression) {
            EmitExpression(il, expression.Left);
            EmitExpression(il, expression.Right);
            switch (expression.Operator.Kind) {
                case BoundBinaryOperatorKind.Addition:
                    il.Emit(OpCodes.Add);
                    break;
                case BoundBinaryOperatorKind.Subtraction:
                    il.Emit(OpCodes.Sub);
                    break;
                case BoundBinaryOperatorKind.Multiplication:
                    il.Emit(OpCodes.Mul);
                    break;
                case BoundBinaryOperatorKind.Division:
                    il.Emit(OpCodes.Div);
                    break;
                case BoundBinaryOperatorKind.Modulus:
                    il.Emit(OpCodes.Rem);
                    break;
                case BoundBinaryOperatorKind.Concatenation:
                    il.Emit(OpCodes.Call, GetMethodReference("System.String", "Concat", new []{ "System.String", "System.String" }));
                    break;
                case BoundBinaryOperatorKind.DynamicAddition:
                    il.Emit(OpCodes.Call,
                        GetMethodReference(QSPGlobalName,
                            "DynamicAdd",
                            new[] {
                                expression.Left.Type.ClrType.FullName,
                                expression.Right.Type.ClrType.FullName,
                            }));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EmitConversionExpression(ILProcessor il, BoundConversionExpression expression) {
            EmitExpression(il, expression.Expression);

            var targetType = expression.Type;
            var sourceType = expression.Expression.Type;

            if (targetType == sourceType)
                return;

            if (targetType.ClrType == typeof(int))
                il.Emit(OpCodes.Call, GetMethodReference("System.Convert", "ToInt32", new[] { sourceType.ClrType.FullName }));
            else if (targetType.ClrType == typeof(string))
                il.Emit(OpCodes.Call, GetMethodReference("System.Convert", "ToInt32", new[] { sourceType.ClrType.FullName }));
        }

        private TypeReference GetTypeReference(BoundType boundType) {
            if (boundType.ClrType == typeof(int))
                return _assembly.MainModule.TypeSystem.Int32;
            if (boundType.ClrType == typeof(string))
                return _assembly.MainModule.TypeSystem.String;
            throw new ArgumentOutOfRangeException(nameof(boundType), boundType, null);
        }
    }
}