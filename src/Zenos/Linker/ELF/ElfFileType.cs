namespace Zenos.Linker.ELF
{
    enum ElfFileType : byte
    {
        Relocatable = 1,
        Executable = 2,
        Shared = 3,
        Core = 4
    }
}