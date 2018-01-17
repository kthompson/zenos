using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using Mono.Cecil;
using Xunit.Abstractions;
using Zenos.Core;
using Zenos.Framework;
using Zenos.Stages;
using SR = System.Reflection;
using Assert = Xunit.Assert;


namespace Zenos.Tests
{
    public class TestRunner
    {
        #region Properties and Initialization

        public TestRunner(ITestOutputHelper output)
        {
            var arch = new AMD64();
            _compiler = CompilerBuilder.testCompiler(FSharpFuncUtil.Create<string>(output.WriteLine));

            //new CecilToZenos(),
            //new AllocateStorageForVariables(arch),
            //new CilSimplifier(),
            //new PopulateStackType()//,
            //new EmitterStage()
        }

        private readonly Compiler<MethodContext, MethodContext> _compiler;

        #endregion

        #region Test Runners

        public Action<TDelegate> Runs<TDelegate, TResult>(Func<TDelegate, TResult> executor, Action<TResult, TResult> test, Action<MethodContext> testMethodContext)
            where TDelegate : class
        {
            return method =>
            {
                var mc = CreateMethodContext(method);
                //compile
                var context = Compiler.run(_compiler, mc);

                Assert.NotEmpty(context.Code);
                //Assert.Equal(0, context.Code.Length % 16);
                File.WriteAllBytes("temp.bin", context.Code.ToArray());
                //testMethodContext?.Invoke(context);

                //PrintInstructions(context.Instructions);

                var compiled = Function.FromBytes<TDelegate>(context.Code.ToArray());

                //run the compiled method and return output
                var expected = executor(method);
                var nativeResult = executor(compiled.Instance);

                test(expected, nativeResult);
            };
        }

        public static MethodContext CreateMethodContext<TDelegate>(TDelegate method) where TDelegate : class
        {
            var resolver = CreateAssemblyResolver();
            var sourceMethod = GetMethodDefinitionFromLambda(method, resolver);

            var result = MethodContextModule.FromMethodDefinition(sourceMethod);
            Assert.True(result.IsOk);
            //var message = string.Join(Environment.NewLine, result.ErrorValue);
            //throw new ApplicationException(message);

            return result.ResultValue;
        }

        #endregion

        #region Private Static Helper Methods

        private static IAssemblyResolver CreateAssemblyResolver()
        {
            var resolver = new DefaultAssemblyResolver();
            var corlibPath = Path.GetDirectoryName(typeof (object).Assembly.Location);
            resolver.AddSearchDirectory(corlibPath);
            return resolver;
        }

        private static MethodDefinition GetMethodDefinitionFromLambda<TDelegate>(TDelegate action, IAssemblyResolver resolver)
        {
            var delegateAction = action as Delegate;
            if (delegateAction == null)
                throw new InvalidOperationException("action must be a delegate type");

            var lambda = delegateAction.Method;
            var lambdaParent = lambda.DeclaringType;

            var sourceModule = ModuleDefinition.ReadModule(lambdaParent.Assembly.Location, new ReaderParameters{AssemblyResolver = resolver});

            var sourceType = FindType(sourceModule, lambdaParent);
            Assert.NotNull(sourceType);

            var method = sourceType.Methods.First(m => m.Name == lambda.Name);
            Assert.NotNull(method);

            return method;
        }

        private static TypeDefinition FindType(ModuleDefinition sourceModule, Type lambdaParent)
        {
            if (lambdaParent.DeclaringType != null)
            {
                var parent = FindType(sourceModule, lambdaParent.DeclaringType);
                return parent.NestedTypes.First(t => t.Name == lambdaParent.Name);
            }

            return sourceModule.Types.First(t => t.Name == lambdaParent.Name);
        }

        #endregion
    }
}
