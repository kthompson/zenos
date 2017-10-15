using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Mono.Cecil;
using Zenos.Core;
using Zenos.Framework;
using Zenos.Stages;
using SR = System.Reflection;
using Assert = Xunit.Assert;


namespace Zenos.Tests
{
    internal static class Test
    {
        #region Properties and Initialization

        static Test()
        {
            var arch = new AMD64();
            _compiler = Compiler.CreateStagedWithAfter(CodePrinter.Value,
                new CecilToZenos(),
                new AllocateStorageForVariables(arch),
                new CilSimplifier(),
                new PopulateStackType()//,
                //new EmitterStage()
            );
        }

        private static readonly ICompiler<IMethodContext> _compiler;

        #endregion

        #region Test Runners

        public static Action<TDelegate> Runs<TDelegate, TResult>(Func<TDelegate, TResult> executor, Action<TResult, TResult> test, Action<IMethodContext> testMethodContext)
            where TDelegate : class
        {
            return method =>
            {
                //update arguments
                try
                {
                    var mc = CreateMethodContext(method);

                    //compile 
                    var context = _compiler.Compile(mc);

                    Assert.NotEmpty(context.Code);
                    Assert.Equal(0, context.Code.Count % 16);
                    File.WriteAllBytes("temp.bin", context.Code.ToArray());
                    testMethodContext?.Invoke(context);

                    PrintInstructions(context.Instruction);

                    var compiled = Function.FromBytes<TDelegate>(context.Code.ToArray());

                    //run the compiled method and return output
                    var result = executor(method);
                    var nativeResult = executor(compiled.Instance);

                    test(result, nativeResult);
                }
                catch (Exception e)
                {
                    Helper.Suppress(e);
                    throw;
                }
            };
        }
        
        public static IMethodContext CreateMethodContext<TDelegate>(TDelegate method) where TDelegate : class
        {
            var resolver = CreateAssemblyResolver();
            var sourceMethod = GetMethodDefinitionFromLambda(method, resolver);

            return new MethodContext(sourceMethod);
        }

        private static void PrintInstructions(Instruction inst)
        {
            Trace.WriteLine("--------------------");
            //while (inst != null)
            //{
            //    Trace.WriteLine(inst);
            //    inst = inst.Next;
            //}
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
