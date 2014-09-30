using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Zenos.Framework;

namespace Zenos.Tests
{
    public class InstructionChainTests
    {

        [Fact]
        public void InstructionChain_OnInstructionAssignment_SetsFirstInstruction()
        {
            var chain = new InstructionChain();
            var inst = new IrInstruction();
            Assert.Null(chain.Instruction);

            chain.Instruction = inst;

            Assert.Equal(chain.Instruction, inst);
            Assert.Equal(chain.FirstInstruction, inst);
        }

        [Fact]
        public void InstructionChain__OnInstructionAssignment_InstructionIsReplaced()
        {
            var chain = new InstructionChain();
            var inst = new IrInstruction();
            var inst2 = new IrInstruction();

            chain.Instruction = inst;
            chain.Instruction = inst2;

            Assert.Equal(chain.Instruction, inst2);
            Assert.Equal(chain.FirstInstruction, inst2);
        }



        [Fact]
        public void InstructionChain_OnIncrement_InstructionIsNull()
        {
            var chain = new InstructionChain
            {
                Instruction = new IrInstruction()
            };

            chain.Increment();

            Assert.Null(chain.Instruction);
        }

        [Fact]
        public void InstructionChain_ReplacingInstruction_UpdatesNextAndPreviousLinks()
        {
            var chain = new InstructionChain();
            var first = new IrInstruction();
            var second = new IrInstruction();

            chain.Instruction = first;
            chain.Increment();

            chain.Instruction = second;

            Assert.Equal(chain.Instruction, second);
            Assert.Equal(second.Previous, first);
            Assert.Equal(first.Next, second);
        }

        [Fact]
        public void InstructionChain_OnDecrement_InstructionIsReplaced()
        {
            var chain = new InstructionChain();
            var first = new IrInstruction();
            var newFirst = new IrInstruction();
            var second = new IrInstruction();

            chain.Instruction = first;
            chain.Increment();

            chain.Instruction = second;
            chain.Decrement();

            Assert.Equal(chain.Instruction, first);

            chain.Instruction = newFirst;

            Assert.Equal(chain.Instruction, newFirst);
            Assert.Equal(second.Previous, newFirst);
            Assert.Equal(newFirst.Next, second);
            Assert.Null(first.Next);
        }

        [Fact]
        public void InstructionChain_SizeIncrements_OnIncrement()
        {
            var chain = new InstructionChain();

            chain.Increment();

            Assert.Equal(1, chain.Size);
        }


        [Fact]
        public void InstructionChain_SizeDoesntChange_OnDecrement()
        {
            var chain = new InstructionChain();

            chain.Increment();
            chain.Increment();
            chain.Increment();
            chain.Increment();

            Assert.Equal(4, chain.Size);
            chain.Decrement();
            Assert.Equal(4, chain.Size);
            chain.Decrement();
            Assert.Equal(4, chain.Size);
            chain.Decrement();
            Assert.Equal(4, chain.Size);
        }

        [Fact]
        public void InstructionChain_SizeDefaultsToZero()
        {
            var chain = new InstructionChain();

            Assert.Equal(0, chain.Size);
        }

        [Fact]
        public void InstructionChain_Throws_OnDecrement()
        {
            var chain = new InstructionChain();

            var exception = Assert.Throws<InvalidOperationException>(() => chain.Decrement());
            Assert.NotNull(exception);
        }
    }
}
