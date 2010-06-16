@echo off
del ctest.s
gcc  -S -O0 -fverbose-asm ctest.c
TYPE ctest.s 