using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Autofac;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using Zenos.Core;
using Zenos.Framework;
using SR = System.Reflection;
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
            where TDelegate : class
        {
            //update arguments
            try
            {
                var assembly = AssemblyFromMethod(method);

                var context = new TestContext("test_".AppendRandom(20, ".exe"), arguments);
                
                //compile 
                using (var output = Compiler.Compile(context, assembly))
                {
                    Assert.IsNotNull(output);
                    //run the compiled exe and return output
                    Assert.AreEqual(expected.ToString(), Helper.Execute(output.OutputFile));    
                }
            }
            catch (AssertionException)
            {
                throw;
            }
            catch (Exception e)
            {
                Helper.Suppress(e);
                throw;
            }
        }

        private static ModuleDefinition AssemblyFromMethod<TDelegate>(TDelegate action)
            where TDelegate : class
        {
            //create an assembly we can use
            var module = CreateAssembly();
            var sourceMethod = GetMethodDefinitionFromLambda(action);

            var type = new TypeDefinition("SingleTest", "Tests", TypeAttributes.Public | TypeAttributes.Class, null);
            module.Types.Add(type);

            var method = new MethodDefinition("TestMethod".AppendRandom(), MethodAttributes.Static| MethodAttributes.Public, sourceMethod.ReturnType);

            CloneMethodBody(sourceMethod.Body, method);
            //method.Body = new MethodBody(method);
            //method.Body.Method = method;
            type.Methods.Add(method);

            return module;
        }

        private static void CloneMethodBody(MethodBody src, MethodDefinition dest)
        {
            var db = dest.Body = new MethodBody(dest);
            
            foreach (var handler in src.ExceptionHandlers)
                db.ExceptionHandlers.Add(handler);

            db.InitLocals = src.InitLocals;
            var il = db.GetILProcessor();
            foreach (var instruction in src.Instructions)
                il.Append(instruction);

            db.LocalVarToken = src.LocalVarToken;
            db.MaxStackSize = src.MaxStackSize;
            db.Scope = src.Scope;

            foreach (var variable in src.Variables)
                db.Variables.Add(variable);
        }

        private static MethodDefinition GetMethodDefinitionFromLambda<TDelegate>(TDelegate action)
        {
            var delegateAction = action as Delegate;
            if (delegateAction == null)
                throw new InvalidOperationException("action must be a delegate type");
            var lambda = delegateAction.Method;
            var lambdaParent = lambda.DeclaringType;

            var sourceModule = ModuleDefinition.ReadModule(lambdaParent.Assembly.Location);
            var sourceType = sourceModule.Types.Where(t => t.FullName == lambdaParent.FullName).First();
            return sourceType.Methods.Where(m => m.Name == lambda.Name).First();
        }

        private static TDelegate Compile<TDelegate>(Emitter emitter)
            where TDelegate : class
        {
            var name = GetTestCaseName();

            var module = CreateTestModule<TDelegate>(name, emitter);
            var assembly = LoadTestModule(module);

            return CreateRunDelegate<TDelegate>(GetTestCase(name, assembly));
        }

        static SR.Assembly LoadTestModule(ModuleDefinition module)
        {
            using (var stream = new MemoryStream())
            {
                module.Write(stream);
                File.WriteAllBytes(Path.Combine(Path.Combine(Path.GetTempPath(), "zenos"), module.Name + ".dll"), stream.ToArray());
                return SR.Assembly.Load(stream.ToArray());
            }
        }

        static Type GetTestCase(string name, SR.Assembly assembly)
        {
            return assembly.GetType(name);
        }

        static TDelegate CreateRunDelegate<TDelegate>(Type type)
            where TDelegate : class
        {
            return (TDelegate) (object) Delegate.CreateDelegate(typeof (TDelegate), type.GetMethod("Run"));
        }

        delegate void Emitter(ModuleDefinition module, MethodBody body);
        static ModuleDefinition CreateTestModule<TDelegate>(string name, Emitter emitter)
        {
            var module = CreateModule(name);

            var type = new TypeDefinition(
                "",
                name,
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract,
                module.Import(typeof(object)));

            module.Types.Add(type);

            var method = CreateMethod(type, typeof(TDelegate).GetMethod("Invoke"));

            emitter(module, method.Body);

            return module;
        }

        static MethodDefinition CreateMethod(TypeDefinition type, SR.MethodInfo pattern)
        {
            var module = type.Module;
            var method = new MethodDefinition("Run", MethodAttributes.Public | MethodAttributes.Static,
                                              module.Import(pattern.ReturnType));

            type.Methods.Add(method);

            foreach (var parameterPattern in pattern.GetParameters())
                method.Parameters.Add(new ParameterDefinition(module.Import(parameterPattern.ParameterType)));

            return method;
        }


        static ModuleDefinition CreateModule(string name)
        {
            return ModuleDefinition.CreateModule(name, ModuleKind.Dll);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static string GetTestCaseName()
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(2);

            return "ImportCecil_" + stackFrame.GetMethod().Name;
        }

        private static ModuleDefinition CreateAssembly()
        {
            return ModuleDefinition.CreateModule("TempAssembly".AppendRandom(), ModuleKind.Dll);
        }
    }
}
