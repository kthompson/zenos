using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Ast;
using ICSharpCode.NRefactory.CSharp;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    static class CecilExtensions
    {
        public static BlockStatement Decompile(this MethodDefinition method)
        {
            return AstMethodBodyBuilder.CreateMethodBody(method, new DecompilerContext(method.Module)
            {
                CurrentType = method.DeclaringType,
                CurrentMethod = method
            });
        }
    }
}
