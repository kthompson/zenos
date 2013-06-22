	.file	"ctest.c"
 # GNU C (GCC) version 4.6.2 (mingw32)
 #	compiled by GNU C version 4.6.2, GMP version 5.0.1, MPFR version 2.4.1, MPC version 0.8.1
 # GGC heuristics: --param ggc-min-expand=100 --param ggc-min-heapsize=131072
 # options passed:  -iprefix c:\mingw\bin\../lib/gcc/mingw32/4.6.2/ ctest.c
 # -mtune=i386 -march=i386 -O0 -fverbose-asm
 # options enabled:  -fasynchronous-unwind-tables -fauto-inc-dec
 # -fbranch-count-reg -fcommon -fdelete-null-pointer-checks
 # -fdwarf2-cfi-asm -fearly-inlining -feliminate-unused-debug-types
 # -ffunction-cse -fgcse-lm -fident -finline-functions-called-once
 # -fira-share-save-slots -fira-share-spill-slots -fivopts
 # -fkeep-inline-dllexport -fkeep-static-consts -fleading-underscore
 # -fmath-errno -fmerge-debug-strings -fmove-loop-invariants -fpeephole
 # -fprefetch-loop-arrays -freg-struct-return
 # -fsched-critical-path-heuristic -fsched-dep-count-heuristic
 # -fsched-group-heuristic -fsched-interblock -fsched-last-insn-heuristic
 # -fsched-rank-heuristic -fsched-spec -fsched-spec-insn-heuristic
 # -fsched-stalled-insns-dep -fset-stack-executable -fshow-column
 # -fsigned-zeros -fsplit-ivs-in-unroller -fstrict-volatile-bitfields
 # -ftrapping-math -ftree-cselim -ftree-forwprop -ftree-loop-if-convert
 # -ftree-loop-im -ftree-loop-ivcanon -ftree-loop-optimize
 # -ftree-parallelize-loops= -ftree-phiprop -ftree-pta -ftree-reassoc
 # -ftree-scev-cprop -ftree-slp-vectorize -ftree-vect-loop-version
 # -funit-at-a-time -funwind-tables -fvect-cost-model -fverbose-asm
 # -fzero-initialized-in-bss -m32 -m80387 -m96bit-long-double
 # -maccumulate-outgoing-args -malign-double -malign-stringops
 # -mfancy-math-387 -mfp-ret-in-387 -mieee-fp -mno-red-zone -mno-sse4
 # -mpush-args -msahf -mstack-arg-probe

 # Compiler executable checksum: c20aed7c018482d7b62efcd5dcab2a9d

	.text
	.globl	_MyFunction3
	.def	_MyFunction3;	.scl	2;	.type	32;	.endef
_MyFunction3:
LFB6:
	.cfi_startproc
	pushl	%ebp	 #
	.cfi_def_cfa_offset 8
	.cfi_offset 5, -8
	movl	%esp, %ebp	 #,
	.cfi_def_cfa_register 5
	subl	$16, %esp	 #,
	movl	$-1877855146, -8(%ebp)	 #, fieldT
	movl	$305419896, -4(%ebp)	 #, fieldT
	movl	$19088743, -12(%ebp)	 #, fieldF
	movl	-8(%ebp), %eax	 # fieldT, D.2540
	leave
	.cfi_restore 5
	.cfi_def_cfa 4, 4
	ret
	.cfi_endproc
LFE6:
