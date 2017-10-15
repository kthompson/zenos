using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zenos.Framework
{
    static class InstructionExtensions
    {
        ///// <summary>
        ///// Removes the instruction, patching up the surrounding instructions.
        ///// </summary>
        ///// <param name="this"></param>
        ///// <returns>The next instruction</returns>
        //public static Instruction Remove(this Instruction @this)
        //{
        //    if (@this == null)
        //        return null;

        //    var prev = @this.Previous;
        //    var next = @this.Next;

        //    @this.Detach();

        //    if (prev != null) 
        //        return prev.SetNext(next);

        //    next.Previous = null;
        //    return null;
        //}

        ///// <summary>
        ///// Removes previous and next instructions. (Does not modify other instructions)
        ///// </summary>
        ///// <param name="this"></param>
        //public static void Detach(this Instruction @this)
        //{
        //    @this.Previous = null;
        //    @this.Next = null;
        //}

        ///// <summary>
        ///// Sets the following instruction, returning that instruction.
        ///// </summary>
        ///// <param name="this"></param>
        ///// <param name="next"></param>
        ///// <returns></returns>
        //public static Instruction SetNext(this Instruction @this, Instruction next)
        //{
        //    @this.Next = next;
            
        //    if(next != null)
        //        next.Previous = @this;

        //    return next;
        //}

        ///// <summary>
        ///// Sets the following instruction, returning that instruction.
        ///// </summary>
        ///// <param name="this"></param>
        ///// <param name="next"></param>
        ///// <returns></returns>
        //public static Instruction InsertAfter(this Instruction @this, Instruction next)
        //{
        //    next.SetNext(@this.Next);
        //    @this.SetNext(next);

        //    return next;
        //}

        ///// <summary>
        ///// Detaches the following instruction and returns that instruction.
        ///// </summary>
        ///// <param name="this"></param>
        ///// <returns></returns>
        //public static Instruction DetachNext(this Instruction @this)
        //{
        //    var next = @this.Next;
        //    @this.Next = null;

        //    if (next != null)
        //        next.Previous = null;

        //    return next;
        //}

        //public static Instruction ReplaceWith(this Instruction @this, Instruction newInstr)
        //{
        //    var prev = @this.Previous;
        //    var next = @this.Next;

        //    @this.Previous = null;
        //    @this.Next = null;

        //    prev?.SetNext(newInstr);

        //    if(next != null)
        //        newInstr.SetNext(next);

        //    return newInstr;
        //}
    }
}
