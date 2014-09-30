using System;
using System.Collections.Generic;

namespace Zenos.Framework
{
    public class InstructionChain
    {
        private readonly int _size;
        private readonly List<IInstruction> _instructions;

        public InstructionChain(int size = 0)
        {
            _size = size;
            _instructions = new List<IInstruction>(size);
            
            this.Index = 0;

            EnsureBuffer();
        }

        public int Index { get; private set; }

        public static InstructionChain FromInstructions(IInstruction instruction)
        {
            var chain = new InstructionChain();
            while (instruction != null)
            {
                chain.AssignAndIncrement(instruction);

                instruction = instruction.Next;
            }
            chain.Reset();

            return chain;
        }

        public int Size
        {
            get
            {
                return this._instructions.Count - 1;
            }
        }


        public InstructionCode Code
        {
            get
            {
                return EnsureInstruction().Code;
            }
        }

        public int Offset
        {
            get
            {
                return EnsureInstruction().Offset;
            }
        }

        public IRegister Destination
        {
            get
            {
                return EnsureInstruction().Destination;
            }
        }
        

        public IInstruction FirstInstruction {
            get
            {
                if (_instructions.Count == 0)
                    return null;

                return _instructions[0];
            }
        }

        public IInstruction Instruction
        {
            get
            {
                return _instructions[Index];
            }
            set
            {
                UnlinkCurrent();

                _instructions[Index] = value;

                UpdatePrevAndNextLinks(value);
            }
        }

        public void AssignAndIncrement(IInstruction value)
        {
            this.Instruction = value;
            this.Increment();
        }

        public void Reset()
        {
            this.Index = 0;
        }

        public void Increment()
        {
            this.Index++;

            EnsureBuffer();
        }

        public void Decrement()
        {
            var i = this.Index - 1;
            if(i < 0)
                throw new InvalidOperationException("Cannot decrement past beginning.");

            this.Index = i;
        }

        private IInstruction EnsureInstruction()
        {
            var current = Instruction;
            if (current == null)
                throw new InvalidOperationException("No current instruction");

            return current;
        }

        private void UnlinkCurrent()
        {
            var old = _instructions[Index];
            if (old == null) 
                return;

            old.Next = null;
            old.Previous = null;
        }

        private void UpdatePrevAndNextLinks(IInstruction value)
        {
            if (value == null)
                return;

            var prevIndex = this.Index - 1;
            if (prevIndex >= 0)
            {
                var prev = _instructions[prevIndex];
                if (prev != null)
                    prev.SetNext(value);
            }

            var nextIndex = Index + 1;
            if (nextIndex >= _instructions.Count)
                return;

            var next = _instructions[nextIndex];
            if (next != null)
                value.SetNext(next);
        }

        private void EnsureBuffer()
        {
            while (this.Index >= _instructions.Count)
                _instructions.Add(null);
        }

        public static InstructionChain operator ++(InstructionChain c)
        {
            c.Increment();
            return c;
        }

        public static InstructionChain operator --(InstructionChain c)
        {
            c.Decrement();
            return c;
        }
    }
}