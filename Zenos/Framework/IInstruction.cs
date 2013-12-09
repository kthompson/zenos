using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public interface IInstruction
    {
        InstructionCode Code { get; set; }
        StackType StackType { get; set; }
        byte Flags { get; set; }

        long Offset { get; set; }

        IRegister Destination { get; set; }
        IRegister Source1 { get; set; }
        IRegister Source2 { get; set; }
        IRegister Source3 { get; set; }
    
        IInstruction Previous { get; set; }
        IInstruction Next { get; set; }

        object Operand { get; set; }
        Instruction SourceInstruction { get; set; }
        TypeDefinition klass { get; set; }
    }
}