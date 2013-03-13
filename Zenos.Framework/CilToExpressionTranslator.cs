using Cecil.Decompiler;
using Mono.Cecil;

namespace Zenos.Framework
{
    public class CilToExpressionTranslator : MemberCompilerStage
    {
        public override void Compile(IMemberContext context, MethodDefinition method)
        {
            var blocks = method.Body.Decompile();
            blocks.ToString();
        }

    }
}