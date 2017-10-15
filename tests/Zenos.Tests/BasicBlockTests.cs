using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Zenos.Framework;
using Zenos.Stages;

namespace Zenos.Tests
{
    public class BasicBlockTests
    {
        //private readonly ICompiler<IMethodContext> _compiler;

        //public BasicBlockTests()
        //{
        //    var arch = new AMD64();
        //    _compiler = Compiler.CreateStagedWithAfter(
        //        CodePrinter.Value,
        //        new CecilToZenos(),
        //        new AllocateStorageForVariables(arch),
        //        new CilSimplifier(),
        //        new BasicBlockAssignment()
        //    );
        //}

        //[Fact]
        //public void SimpleCodeMakesOneBlock()
        //{
        //    var context = Test.CreateMethodContext<Func<int>>(() => 4);
        //    var method = _compiler.Compile(context);
            
        //    Assert.Equal(1, method.BasicBlocks.Count);
        //    Assert.Collection(method.BasicBlocks,
        //        bb => Assert.Empty(bb.OutBasicBlocks));
        //}

        //[Fact]
        //public void SimpleLotsOfBlocks()
        //{
        //    var context = Test.CreateMethodContext<Func<int, int>>(x =>
        //    {
        //        if (x == 2)
        //            return 3;

        //        return 0;
        //    });

        //    var method = _compiler.Compile(context);
            
        //    Assert.Equal(4, method.BasicBlocks.Count);
        //    Assert.Collection(OrderedBasicBlocks(method),
        //        bb => Assert.Equal(2, bb.OutBasicBlocks.Count),
        //        bb => Assert.Single(bb.OutBasicBlocks),
        //        bb => Assert.Single(bb.OutBasicBlocks),
        //        bb => Assert.Empty(bb.OutBasicBlocks));
        //}

        //private static IEnumerable<BasicBlock> OrderedBasicBlocks(IMethodContext method)
        //{
        //    return method.BasicBlocks.OrderBy(x => x.Instruction.Offset);
        //}

        //[Fact]
        //public void LoopHasOneBlock()
        //{
        //    // ReSharper disable once RedundantAssignment
        //    var context = Test.CreateMethodContext<Func<int, int>>(x =>
        //    {
        //        // ReSharper disable once RedundantAssignment
        //        x = 2;

        //        while (true)
        //        {
        //            // do nothing
        //        }

        //        // ReSharper disable once FunctionNeverReturns
        //    });

        //    var method = _compiler.Compile(context);

        //    Assert.Equal(3, method.BasicBlocks.Count);
        //    Assert.Collection(OrderedBasicBlocks(method),
        //        bb => Assert.Single(bb.OutBasicBlocks),
        //        bb => Assert.Single(bb.OutBasicBlocks),
        //        bb => Assert.Single(bb.OutBasicBlocks));
        //}

        //[Fact]
        //public void BasicBlockAssignmentHandlesBreaks()
        //{
        //    // ReSharper disable once RedundantAssignment
        //    var context = Test.CreateMethodContext<Func<int, int>>(x =>
        //    {
        //        // ReSharper disable once RedundantAssignment
        //        x = 2;

        //        while (true)
        //        {
        //            // do nothing

        //            if(x == 5)
        //                break;

        //            x++;
        //        }

        //        return 0;
        //        // ReSharper disable once FunctionNeverReturns
        //    });

        //    var method = _compiler.Compile(context);

        //    Assert.Equal(7, method.BasicBlocks.Count);
        //    Assert.Collection(OrderedBasicBlocks(method),
        //        bb => Assert.Single(bb.OutBasicBlocks),
        //        bb => Assert.Equal(2, bb.OutBasicBlocks.Count),
        //        bb => Assert.Single(bb.OutBasicBlocks),
        //        bb => Assert.Single(bb.OutBasicBlocks),
        //        bb => Assert.Single(bb.OutBasicBlocks),
        //        bb => Assert.Single(bb.OutBasicBlocks),
        //        bb => Assert.Empty(bb.OutBasicBlocks));
        //}
    }
}