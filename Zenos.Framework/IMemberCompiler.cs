using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface IMemberCompiler
    {
        IMemberContext Compile(IMemberContext context, IMemberDefinition member);
        IMemberContext Compile(IMemberContext context, EventDefinition @event);
        IMemberContext Compile(IMemberContext context, FieldDefinition field);
        IMemberContext Compile(IMemberContext context, MethodDefinition method);
        IMemberContext Compile(IMemberContext context, PropertyDefinition property);
        IMemberContext Compile(IMemberContext context, TypeDefinition type);
    }
}
