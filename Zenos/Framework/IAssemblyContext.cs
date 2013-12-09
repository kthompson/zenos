using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface IAssemblyContext : IDisposable
    {
        bool IsDisposed { get; }
        string OutputFile { get; }

        ModuleDefinition Module { get; }

        ITypeContext[] Types { get; }

        ITypeContext GetTypeContext(TypeDefinition type);
        ITypeContext GetOrCreateTypeContext(TypeDefinition type);
        ITypeContext CreateTypeContext(TypeDefinition type);
    }

    //public interface IHasArchitecture
    //{
    //    IArchitecture Architecture { get; }
    //}


    //public interface IArchitecture
    //{
    //    bool SoftwareFloat { get; }
    //    int RegisterSize { get; }
    //}
}