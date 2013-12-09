﻿using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public class MethodContext : IMethodContext
    {
        public ITypeContext Context { get; private set; }
        public Sections Sections { get; private set; }
        public string OutputFile { get; set; }
        public bool IsDisposed { get; private set; }

        public MethodContext(ITypeContext context, MethodDefinition method)
        {
            this.Context = context;
            this.Method = method;
            this.Sections = new Sections();
        }

        private int _lastLabel = 1;

        public Section Text
        {
            get { return this.Sections["text"]; }
        }

        public Section Data
        {
            get { return this.Sections["rdata"]; }
        }

        public int StackSize { get; private set; }

        public IInstruction[] Variables { get; set; }
        public VariableDefinition[] VariableDefinitions { get; set; }

        public IInstruction ReturnType { get; set; }
        public IInstruction[] Parameters { get; set; }
        public ParameterDefinition[] ParameterDefinitions { get; set; }

        public string CreateLabel(string prefix = null)
        {
            return (prefix ?? "L") + (_lastLabel++).ToString("D4");
        }

        public string CreateTemporary()
        {
            this.StackSize += 4;
            return string.Format("{0}(%ebp)", this.StackSize);
        }

        public IInstruction Instruction { get; set; }
        public MethodDefinition Method { get; private set; }
        public BasicBlock CurrentBasicBlock { get; set; }
        public int next_vreg { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (!disposing)
                return;

            foreach (var section in this.Sections)
            {
                section.Dispose();
            }

            this.Context = null;
            this.IsDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}