namespace Zenos.Linker.ELF
{
    enum SegmentType : uint
    {
        NULL = 0x00000000,
        LOAD = 0x00000001,
        DYNAMIC = 0x00000002,
        INTERP = 0x00000003,
        NOTE = 0x00000004,
        SHLIB = 0x00000005,
        PHDR = 0x00000006,
        LOOS = 0x60000000,
        HIOS = 0x6FFFFFFF,
        LOPROC = 0x70000000,
        HIPROC = 0x7FFFFFFF,
    }
}