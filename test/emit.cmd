@echo off
SET PATH=%PATH%;c:\Program Files (x86)\LLVM\bin\
del test.s
clang -m64 -S -O0 -o test.s test.c
clang -cc1as -triple x86_64-pc-windows-gnu -filetype obj  -target-cpu x86-64 test.s | llvm-objdump -d -
del test.s
::type test.s | find /v ".seh_" | find /v ".Ltmp" | find /v ".def" | find /v ".scl" | find /v ".type" | find /v ".endef" | find /v ".globl" | find /v ".align" | find /v ".Leh_"
::https://en.wikibooks.org/wiki/X86_Assembly/GAS_Syntax
