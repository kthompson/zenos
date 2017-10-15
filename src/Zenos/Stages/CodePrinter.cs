using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Zenos.Framework;

namespace Zenos.Stages
{
    public static class CodePrinter
    {
        public static readonly Action<ICompiler<IMethodContext>, IMethodContext> Value = (compiler, context) =>
        {
            Trace.WriteLine($"---- After {compiler.GetType().Name} ----");

            //if (context.BasicBlocks.Count == 0)
            //{
            //    foreach (var inst in context.Instruction)
            //    {
            //        Trace.WriteLine(inst);
            //    }
            //}
            //else
            //{
            //    foreach (var block in context.BasicBlocks)
            //    {
            //        Trace.WriteLine(block);
            //        foreach (var inst in block.Instruction)
            //        {
            //            Trace.WriteLine("    " + inst);
            //        }

            //        foreach (var outBasicBlock in block.OutBasicBlocks)
            //        {
            //            Trace.WriteLine("    " + outBasicBlock);
            //        }

            //        Trace.WriteLine("");
            //    }
            //}
        };
    }
}