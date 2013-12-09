namespace Zenos.Framework
{
    public abstract class InstructionCompilerStage : CodeCompilerStage
    {
        public override void Compile(ITypeContext context)
        {
            foreach (var cc in context.MethodContexts)
                this.Compile(cc);
        }

        public override void Compile(IMethodContext context)
        {
            if(context.CurrentBasicBlock == null)
                context.CurrentBasicBlock = new BasicBlock();

            var instruction = context.Instruction;
            while (instruction != null)
            {
                instruction = Compile(context, instruction);

                // if this has no Previous instruction then it should become our first instruction
                if (instruction.Previous == null)
                    context.Instruction = instruction;

                instruction = instruction.Next;
            }
        }

        public abstract IInstruction Compile(IMethodContext context, IInstruction instruction);
    }
}