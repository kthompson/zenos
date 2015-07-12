using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Mono.Cecil;
using Ninject;
using Zenos.Core;
using Zenos.Framework;
using SR = System.Reflection;
using Assert = Xunit.Assert;


namespace Zenos.Tests
{
    internal sealed class Test
    {
        #region Properties and Initialization

        static Test()
        {
            Container = new StandardKernel(new TestCompilerModule());
        }

        private static readonly IKernel Container;

        private static Compiler Compiler
        {
            get
            {
                return Container.Get<Compiler>();
            }
        }

        #endregion

        #region Test Runners

        public static Action<TDelegate> Runs<TDelegate, TResult>(Func<TDelegate, TResult> executor, Action<TResult, TResult> test, Action<IMethodContext> testMethodContext)
            where TDelegate : class
        {
            return method =>
            {
                //update arguments
                IAssemblyContext context = null;
                try
                {
                    var resolver = CreateAssemblyResolver();
                    var sourceMethod = GetMethodDefinitionFromLambda(method, resolver);

                    var mc = new MethodContext(sourceMethod);
                    context = new AssemblyContext
                    {
                        {
                            "main", new TypeContext
                            {
                                {"test_method", mc}
                            }
                        }
                    };

                    //compile 
                    Compiler.Compile(context);

                    Assert.NotEqual(0, mc.Code.Count);
                    Assert.Equal(0, mc.Code.Count % 16);

                    if(testMethodContext != null)
                        testMethodContext(mc);

                    PrintInstructions(mc.Instruction);

                    var compiled = Function.FromBytes<TDelegate>(mc.Code.ToArray());

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
                finally
                {
                    if (context != null)
                        context.Dispose();
                }
            };
        }

        private static void PrintInstructions(IInstruction inst)
        {
            Trace.WriteLine("--------------------");
            while (inst != null)
            {
                Trace.WriteLine(inst);
                inst = inst.Next;
            }
        }

        #endregion

        #region Private Static Helper Methods

        private static DefaultAssemblyResolver CreateAssemblyResolver()
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
