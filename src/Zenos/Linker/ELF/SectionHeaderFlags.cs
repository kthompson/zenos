using System;

namespace Zenos.Linker.ELF
{
    [Flags]
    enum SectionHeaderFlags : ulong
    {
        WRITE = 0x1,               // Writable
        ALLOC = 0x2,               // Occupies memory during execution
        EXECINSTR = 0x4,           // Executable
        MERGE = 0x10,              // Might be merged
        STRINGS = 0x20,            // Contains nul-terminated strings
        INFO_LINK = 0x40,          // 'sh_info' contains SHT index
        LINK_ORDER = 0x80,         // Preserve order after combining
        OS_NONCONFORMING = 0x100,  // Non-standard OS specific handling required
        GROUP = 0x200,             // Section is member of a group
        TLS = 0x400,               // Section hold thread-local data
        MASKOS = 0x0ff00000,       // OS-specific
        MASKPROC = 0xf0000000,     // Processor-specific
        ORDERED = 0x4000000,       // Special ordering requirement (Solaris)
        EXCLUDE = 0x8000000,       // Section is excluded unless referenced or allocated (Solaris)
    }
}