using System;
using System.Runtime.InteropServices;
using Xunit.Sdk;

namespace Zenos.Tests
{
    static class Function
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAlloc(IntPtr lpAddress, IntPtr dwSize, AllocationType allocationType, MemoryProtection protection);

        public static IntPtr VirtualAlloc(uint size, AllocationType allocationType, MemoryProtection protection)
        {
            return VirtualAlloc(IntPtr.Zero, new IntPtr(size), allocationType, protection);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool VirtualFree(IntPtr lpAddress, IntPtr dwSize, FreeType dwFreeType);

        public static bool VirtualFree(IntPtr lpAddress)
        {
            return VirtualFree(lpAddress, IntPtr.Zero, FreeType.Release);
        }

        public enum FreeType : uint
        {
            Decommit = 0x4000,
            Release = 0x8000
        }

        [Flags]
        public enum AllocationType : uint
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
        public enum MemoryProtection : uint
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

        public static Function<T> FromBytes<T>(byte[] code) where T : class
        {
            return new Function<T>(code);
        }
    }

    class Function<T> : IDisposable
    {
        private readonly IntPtr _addr;
        private readonly uint _size;

        bool _disposed;

        public Function(byte[] code)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            if (code.Length == 0)
                throw new ArgumentException("Code cannot be empty", nameof(code));

            _size = (uint)code.Length;
            _addr = Function.VirtualAlloc(_size, Function.AllocationType.Reserve | Function.AllocationType.Commit, Function.MemoryProtection.ExecuteReadwrite);
            GC.AddMemoryPressure(code.Length);

            Marshal.Copy(code, 0, _addr, code.Length);

            this.Instance = Marshal.GetDelegateForFunctionPointer<T>(_addr);
        }

        public T Instance { get; private set; }

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

            Function.VirtualFree(_addr);
            GC.RemoveMemoryPressure(_size);
        }
    }
}