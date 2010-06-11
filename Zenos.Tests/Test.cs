using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Mono.Cecil;
using NUnit.Framework;
using Zenos.Core;
using Zenos.Framework;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace Zenos.Tests
{
    sealed class Test
    {
        static Test()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TestCompilerModule());
            Container = builder.Build();
        }

        private static readonly IContainer Container;
        private static Compiler Compiler
        {
            get
            {
                return Container.Resolve<Compiler>();
            }
        }

        public static void AreEqual<T1, T2, T3, T4, TResult>(TResult expected, Func<T1, T2, T3, T4, TResult> method, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            AreEqual(expected, method, new object[] { param1, param2, param3, param4 });
        }

        public static void AreEqual<T1, T2, T3, TResult>(TResult expected, Func<T1, T2, T3, TResult> method, T1 param1, T2 param2, T3 param3)
        {
            AreEqual(expected, method, new object[] { param1, param2, param3 });
        }

        public static void AreEqual<T1, T2, TResult>(TResult expected, Func<T1, T2, TResult> method, T1 param1, T2 param2)
        {
            AreEqual(expected, method, new object[] { param1, param2 });
        }

        public static void AreEqual<T1, TResult>(TResult expected, Func<T1, TResult> method, T1 param)
        {
            AreEqual(expected, method, new object[] { param });
        }

        public static void AreEqual<TResult>(TResult expected, Func<TResult> method)
        {
            AreEqual(expected, method, new string[] { });
        }

        public static void AreEqual<TDelegate>(object expected, TDelegate method, object[] arguments)
        {
            //update arguments
            try
            {
                var assembly = AssemblyFromMethod(method);

                var context = new TestContext("test_".AppendRandom(20, ".exe"), arguments);
                
                //compile 
                using (var output = Compiler.Compile(context, assembly))
                {
                    //run the compiled exe and return output
                    Assert.AreEqual(expected.ToString(), Helper.Execute(output.OutputFile));    
                }
            }
            catch (Exception e)
            {
                Helper.Suppress(e);
                throw;
            }
        }

        private static AssemblyDefinition AssemblyFromMethod<TDelegate>(TDelegate action)
        {
            //create an assembly we can use
            var asm = CreateAssembly();

            var delegateAction = action as Delegate;
            if (delegateAction == null)
                throw new InvalidOperationException("action must be a delegate type");

            var type = new TypeDefinition("SingleTest", "Tests", TypeAttributes.Public | TypeAttributes.Class, null);
            
            var method = asm.MainModule.Import(delegateAction.Method).Resolve();
            method.Name = "TestMethod".AppendRandom();
            type.Methods.Add(method);

            return asm;
        }

        private static AssemblyDefinition CreateAssembly()
        {
            var name = new AssemblyNameDefinition("TempAssembly".AppendRandom() , new Version(0, 0, 0, 0));
            return AssemblyDefinition.CreateAssembly(name, name.Name, ModuleKind.Dll);
        }
    }
}
