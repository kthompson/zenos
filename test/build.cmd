@echo off
::SET PATH=C:\Program Files\mingw-w64\x86_64-5.1.0-win32-seh-rt_v4-rev0\mingw64\bin;%PATH%
del test.exe
gcc -m64 -Wall -O0 -S -masm=intel test.c
gcc -m64 -Wall test.s runtime.c -o test
test.exe
objdump -d --disassembler-options=intel test.exe | more
