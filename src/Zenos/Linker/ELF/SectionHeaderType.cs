namespace Zenos.Linker.ELF
{
    enum SectionHeaderType : uint
    {
        NULL = 0x0,           // Section header table entry unused
        PROGBITS = 0x1,       // Program data
        SYMTAB = 0x2,         // Symbol table
        STRTAB = 0x3,         // String table
        RELA = 0x4,           // Relocation entries with addends
        HASH = 0x5,           // Symbol hash table
        DYNAMIC = 0x6,        // Dynamic linking information
        NOTE = 0x7,           // Notes
        NOBITS = 0x8,         // Program space with no data (bss)
        REL = 0x9,            // Relocation entries, no addends
        SHLIB = 0x0A,         // Reserved
        DYNSYM = 0x0B,        // Dynamic linker symbol table
        INIT_ARRAY = 0x0E,    // Array of constructors
        FINI_ARRAY = 0x0F,    // Array of destructors
        PREINIT_ARRAY = 0x10, // Array of pre-constructors
        GROUP = 0x11,         // Section group
        SYMTAB_SHNDX = 0x12,  // Extended section indeces
        NUM = 0x13,           // Number of defined types.
        LOOS = 0x60000000,    // Start OS-specific.
    }
}