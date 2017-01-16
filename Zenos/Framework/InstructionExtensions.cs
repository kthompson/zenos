using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zenos.Framework
{
    static class InstructionExtensions
    {
        /// <summary>
        /// Removes the instruction, patching up the surrounding instructions.
        /// </summary>
        /// <param name="this"></param>
        /// <returns>The next instruction</returns>
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

        /// <summary>
        /// Removes previous and next instructions. (Does not modify other instructions)
        /// </summary>
        /// <param name="this"></param>
        public static void Detach(this IInstruction @this)
        {
            @this.Previous = null;
            @this.Next = null;
        }

        /// <summary>
        /// Sets the following instruction, returning that instruction.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static IInstruction SetNext(this IInstruction @this, IInstruction next)
        {
            @this.Next = next;
            
            if(next != null)
                next.Previous = @this;

            return next;
        }

        /// <summary>
        /// Sets the following instruction, returning that instruction.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static IInstruction InsertAfter(this IInstruction @this, IInstruction next)
        {
            next.SetNext(@this.Next);
            @this.SetNext(next);

            return next;
        }

        /// <summary>
        /// Detaches the following instruction and returns that instruction.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IInstruction DetachNext(this IInstruction @this)
        {
            var next = @this.Next;
            @this.Next = null;

            if (next != null)
                next.Previous = null;

            return next;
        }

        public static IInstruction ReplaceWith(this IInstruction @this, IInstruction newInstr)
        {
            var prev = @this.Previous;
            var next = @this.Next;

            @this.Previous = null;
            @this.Next = null;

            prev?.SetNext(newInstr);

            if(next != null)
                newInstr.SetNext(next);

            return newInstr;
        }
    }
}
