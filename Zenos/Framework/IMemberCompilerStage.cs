﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public interface IMemberCompilerStage : ICompilerStage
    {
        void Compile(IMemberContext context, EventDefinition @event);
        void Compile(IMemberContext context, FieldDefinition field);
        void Compile(IMemberContext context, MethodDefinition method);
        void Compile(IMemberContext context, PropertyDefinition property);
        void Compile(IMemberContext context, TypeDefinition type);
    }
}
