namespace Zenos.Linker.ELF
{
    enum MachineArchitecture : ushort
    {
        Any = 0x00,
        SPARC = 0x02,
        x86 = 0x03,
        MIPS = 0x08,
        PowerPC = 0x14,
        S390 = 0x16,
        ARM = 0x28,
        SuperH = 0x2A,
        IA_64 = 0x32,
        x86_64 = 0x3E,
        AArch64 = 0xB7,
        RISC_V = 0xF3,
    }
}