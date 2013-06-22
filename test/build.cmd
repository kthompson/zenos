@echo off
gcc -Wall ctest.c runtime.c -shared -o test
::test
::del test.exe