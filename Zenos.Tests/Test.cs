using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

    class Function
    {
        public static Function<T> FromBytes<T>(byte[] code)
            where T : class
        {
            return new Function<T>(code);
        }
    }

    class Function<T> : IDisposable
    {
        private readonly T _func;
        private readonly IntPtr _addr; 
        
        bool _disposed;

        public Function(byte[] code)
        {
            _addr = VirtualAlloc((uint)code.Length, AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ExecuteReadwrite);

            Write(_addr, 0, code);

            _func = Marshal.GetDelegateForFunctionPointer<T>(_addr);
        }

        public T Instance { get { return _func; } }

        ~Function()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;

            VirtualFree(_addr);
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, IntPtr dwSize, AllocationType allocationType, MemoryProtection protection);

        static IntPtr VirtualAlloc(uint size, AllocationType allocationType, MemoryProtection protection)
        {
            return VirtualAlloc(IntPtr.Zero, new IntPtr(size), allocationType, protection);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualFree(IntPtr lpAddress, IntPtr dwSize, FreeType dwFreeType);

        static bool VirtualFree(IntPtr lpAddress)
        {
            return VirtualFree(lpAddress, IntPtr.Zero, FreeType.Release);
        }

        enum FreeType : uint
        {
            Decommit = 0x4000,
            Release = 0x8000
        }

        [Flags]
        enum AllocationType : uint
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Reset = 0x80000,
            LargePages = 0x20000000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000
        }

        [Flags]
        enum MemoryProtection : uint
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadwrite = 0x40,
            ExecuteWritecopy = 0x80,
            Noaccess = 0x01,
            Readonly = 0x02,
            Readwrite = 0x04,
            Writecopy = 0x08,
            GuardModifierflag = 0x100,
            NocacheModifierflag = 0x200,
            WritecombineModifierflag = 0x400
        }

        static void Write(IntPtr addr, int offset, byte[] array)
        {
            for (var i = 0; i < array.Length; i++)
                Marshal.WriteByte(addr, offset + i, array[i]);
        }
    }
}
