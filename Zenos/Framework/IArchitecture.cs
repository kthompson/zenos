using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Zenos.Stages;

namespace Zenos.Framework
{
    public interface IArchitecture
    {
        string Name { get; }

        CallInfo get_call_info(object gsctx, MethodDefinition method);

        void mono_arch_emit_setret(IMethodContext context, MethodDefinition method, IInstruction instruction);
    }
}
