using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface ICompilerStage
    {
        void Compile(ICompilerContext context, AssemblyDefinition assembly);
        void Compile(ICompilerContext context, AssemblyNameDefinition assemblyName);
        void Compile(ICompilerContext context, ModuleDefinition module);
    }
}
