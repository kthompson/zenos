	.file	"ctest.c"
 # GNU C version 3.4.5 (mingw-vista special r3) (mingw32)
 #	compiled by GNU C version 3.4.5 (mingw-vista special r3).
 # GGC heuristics: --param ggc-min-expand=100 --param ggc-min-heapsize=131072
 # options passed:  -iprefix -auxbase -O0 -fomit-frame-pointer
 # -fverbose-asm
 # options enabled:  -feliminate-unused-debug-types -fomit-frame-pointer
 # -fpeephole -ffunction-cse -fkeep-static-consts -freg-struct-return
 # -fgcse-lm -fgcse-sm -fgcse-las -fsched-interblock -fsched-spec
 # -fsched-stalled-insns -fsched-stalled-insns-dep -fbranch-count-reg
 # -fcommon -fverbose-asm -fargument-alias -fzero-initialized-in-bss
 # -fident -fmath-errno -ftrapping-math -m80387 -mhard-float
 # -mno-soft-float -malign-double -mieee-fp -mfp-ret-in-387
 # -mstack-arg-probe -maccumulate-outgoing-args -mno-red-zone
 # -mtune=pentiumpro -march=i386

	.text
.globl _test_entryInt64
	.def	_test_entryInt64;	.scl	2;	.type	32;	.endef
_test_entryInt64:
	subl	$12, %esp	 #,
	
	movl	$15, 8(%esp)	 #, a
	movl	$5, 4(%esp)	 #, b
	movl	$8, (%esp)	 #, c
	movl	(%esp), %eax	 # c, c
	
	addl	$12, %esp	 #,
	ret
