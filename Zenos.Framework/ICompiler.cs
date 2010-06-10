using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface ICompiler
    {
        ICompilerContext Compile(ICompilerContext context);

        ICompilerContext Compile(ICompilerContext context, AssemblyDefinition assembly);
        ICompilerContext Compile(ICompilerContext context, AssemblyNameDefinition assemblyName);
        ICompilerContext Compile(ICompilerContext context, ModuleDefinition module);
    }
}
