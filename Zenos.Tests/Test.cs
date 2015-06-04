using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Mono.Cecil;
using Ninject;
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
            get { return Container.Get<Compiler>(); }
        }

        #endregion

        #region Test Runners

        public static Action<TDelegate> Runs<TDelegate, TResult>(Func<TDelegate, TResult> executor)
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
                    var compiled = Function.FromBytes<TDelegate>(mc.Code.ToArray());

                    //run the compiled exe and return output
                    var result = executor(method);
                    var nativeResult = executor(compiled.Instance);

                    Assert.Equal(result, nativeResult);
                }
                //catch (Exception e)
                //{
                //    Helper.Suppress(e);
                //    throw;
                //}
                finally
                {
                    if (context != null)
                        context.Dispose();
                }

            };
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

        private static MethodDefinition GetMethodDefinitionFromLambda<TDelegate>(TDelegate action, DefaultAssemblyResolver resolver)
        {
            var delegateAction = action as Delegate;
            if (delegateAction == null)
                throw new InvalidOperationException("action must be a delegate type");

            var lambda = delegateAction.Method;
            var lambdaParent = lambda.DeclaringType;

            var sourceModule = ModuleDefinition.ReadModule(lambdaParent.Assembly.Location, new ReaderParameters{AssemblyResolver = resolver});
            var sourceType = sourceModule.Types.First(t => t.FullName == lambdaParent.FullName);
            return sourceType.Methods.First(m => m.Name == lambda.Name);
        }

        #endregion
    }
}
