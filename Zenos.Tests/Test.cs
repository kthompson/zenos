using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Ninject;
using Zenos.Core;
using Zenos.Framework;
using SR = System.Reflection;
using TypeAttributes = Mono.Cecil.TypeAttributes;
using Assert = Xunit.Assert;


namespace Zenos.Tests
{
    sealed class Test
    {

        class Library : IDisposable
        {
            [DllImport("kernel32.dll")]
            private static extern IntPtr LoadLibrary(string dllToLoad);

            [DllImport("kernel32.dll")]
            private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);


            [DllImport("kernel32.dll")]
            private static extern bool FreeLibrary(IntPtr hModule);


            private IntPtr _module;
            private bool _disposed;

            public Library(string libraryPath)
            {
                this._module = LoadLibrary(libraryPath);
                if (_module != IntPtr.Zero) 
                    return;

                var error = Marshal.GetLastWin32Error();
                throw new ArgumentException(string.Format("Could not load library at '{0}' due to error code {1}", libraryPath, error), "libraryPath");
            }

            ~Library()
            {
                Dispose(false);
            }

            private Delegate GetFunction<TDelegate>(string name)
                where TDelegate : class
            {
                var address = GetProcAddress(_module, name);
                if (address == IntPtr.Zero)
                    return null;

                return Marshal.GetDelegateForFunctionPointer(address, typeof(TDelegate));
            }

            public object Call<TDelegate>(string name, params object[] arguments)
                where TDelegate : class
            {
                var nativeDelegate = GetFunction<TDelegate>(name);
                return nativeDelegate.DynamicInvoke(arguments);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed && _module != IntPtr.Zero)
                {
                    FreeLibrary(this._module);
                    _module = IntPtr.Zero;
                }
                _disposed = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }


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

        public static void Runs<TDelegate>(TDelegate method, params object[] arguments)
            where TDelegate : class
        {
            //update arguments
            TestContext context = null;
            try
            {
                var del = method as Delegate;
                Assert.NotNull(del);
                string name;
                var resolver = CreateAssemblyResolver();
                var sourceMethod = GetMethodDefinitionFromLambda(method, resolver);
                var assembly = CreateAssemblyFromMethod(sourceMethod, resolver, out name);
                assembly.Write(name + ".dll");

                var nativeDllName = name + "N.dll";
                context = new TestContext(assembly, nativeDllName, arguments);

                //compile 
                Compiler.Compile(context);

                Assert.NotNull(context);

                //run the compiled exe and return output
                var result = del.DynamicInvoke(arguments);

                using (var library = new Library(nativeDllName))
                {
                    var nativeResult = library.Call<TDelegate>(name);

                    Assert.Equal(result, nativeResult);    
                }
            }
            //catch (Exception e)
            //{
            //    Helper.Suppress(e);
            //    throw;
            //}
            finally
            {
                if(context != null)
                    context.Dispose();
            }
        }

        #endregion

        #region Private Static Helper Methods
        
        private static ModuleDefinition CreateAssemblyFromMethod(MethodDefinition sourceMethod, IAssemblyResolver resolver, out string name)
        {
            //create an assembly we can use
            var module = ModuleDefinition.CreateModule("TempAssembly".AppendRandom(), new ModuleParameters
            {
                Kind = ModuleKind.Dll, 
                Runtime = TargetRuntime.Net_4_0,
                AssemblyResolver = resolver
            });
            
            var name1 = typeof (object).Assembly.FullName;
            var assemblyNameReference = AssemblyNameReference.Parse(name1);
            
            module.AssemblyReferences.Add(assemblyNameReference);

            var type = new TypeDefinition("SingleTest", "Tests", TypeAttributes.Public | TypeAttributes.Class, module.Import(typeof(object)));
            module.Types.Add(type);

            var method = new MethodDefinition("TestMethod".AppendRandom(), MethodAttributes.Static | MethodAttributes.Public, sourceMethod.ReturnType);
            type.Methods.Add(method);

            foreach (var param in sourceMethod.Parameters)
            {
                var parameterType = param.ParameterType.Module.Assembly.FullName;
                var p = new ParameterDefinition(param.ParameterType);
                method.Parameters.Add(p);
            }
            

            CloneMethodBody(sourceMethod.Body, method);
            //method.Body = new MethodBody(method);
            //method.Body.Method = method;

            name = method.Name;
            return module;
        }

        private static DefaultAssemblyResolver CreateAssemblyResolver()
        {
            var resolver = new DefaultAssemblyResolver();
            var corlibPath = Path.GetDirectoryName(typeof (object).Assembly.Location);
            resolver.AddSearchDirectory(corlibPath);
            return resolver;
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
