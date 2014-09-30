using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zenos.Framework
{
    static class InstructionExtensions
    {
        public static IInstruction Remove(this IInstruction @this)
        {
            if (@this == null)
                return null;

            var prev = @this.Previous;
            var next = @this.Next;

            @this.Detach();

            if (prev != null) 
                return prev.SetNext(next);

            next.Previous = null;
            return null;
        }

        public static void Detach(this IInstruction @this)
        {
            @this.Previous = null;
            @this.Next = null;
        }
        
        public static IInstruction SetNext(this IInstruction @this, IInstruction next)
        {
            @this.Next = next;
            
            if(next != null)
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
            
            if(next != null)
                newInstr.SetNext(next);

            return newInstr;
        }
    }
}
