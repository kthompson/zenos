using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    class ZenosInstruction : IInstruction
    {
        public InstructionCode Code { get; set; }
        public StackType StackType { get; set; }
        public InstructionFlags Flags { get; set; }
        public int Offset { get; set; }
        public IRegister Destination { get; set; }
        public IRegister Source1 { get; set; }
        public IRegister Source2 { get; set; }
        public IRegister Source3 { get; set; }
        public IInstruction Previous { get; set; }
        public IInstruction Next { get; set; }
        public Instruction SourceInstruction { get; set; }
        public TypeDefinition klass { get; set; }

        public object Operand0 { get; set; }
        public object Operand1 { get; set; }
        public object Operand2 { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this.Code);
            
            sb.Append(" (");
            if (this.Source1 != null)
            {
                sb.Append(this.Source1);
                if(this.Source2 != null)
                    sb.AppendFormat(", {0}", Source2);
                if(this.Source3 != null)
                    sb.AppendFormat(", {0}", Source3);
            } 
            else if (this.Operand0 != null)
            {
                sb.Append(this.Operand0);
            }
            
            sb.Append(")");
            if (this.Destination != null)
                sb.AppendFormat(" => {0}", this.Destination);
            
            return sb.ToString();
        }
    }
}