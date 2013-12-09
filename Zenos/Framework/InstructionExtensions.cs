using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zenos.Framework
{
    static class InstructionExtensions
    {
        public static IInstruction SetNext(this IInstruction @this, IInstruction next)
        {
            @this.Next = next;
            next.Previous = @this;
            return next;
        }

        public static IInstruction ReplaceWith(this IInstruction @this, IInstruction newInstr)
        {
            var prev = @this.Previous;
            var next = @this.Next;

            @this.Previous = null;
            @this.Next = null;

            if(prev != null)
                prev.SetNext(newInstr);

            newInstr.SetNext(next);

            return newInstr;
        }
    }
}
