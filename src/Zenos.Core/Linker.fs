namespace Zenos.Framework

open System

module Linker =
    type SectionHeaderType =
     | NULL = 0x0           // Section header table entry unused
     | PROGBITS = 0x1       // Program data
     | SYMTAB = 0x2         // Symbol table
     | STRTAB = 0x3         // String table
     | RELA = 0x4           // Relocation entries with addends
     | HASH = 0x5           // Symbol hash table
     | DYNAMIC = 0x6        // Dynamic linking information
     | NOTE = 0x7           // Notes
     | NOBITS = 0x8         // Program space with no data (bss)
     | REL = 0x9            // Relocation entries no addends
     | SHLIB = 0x0A         // Reserved
     | DYNSYM = 0x0B        // Dynamic linker symbol table
     | INIT_ARRAY = 0x0E    // Array of constructors
     | FINI_ARRAY = 0x0F    // Array of destructors
     | PREINIT_ARRAY = 0x10 // Array of pre-constructors
     | GROUP = 0x11         // Section group
     | SYMTAB_SHNDX = 0x12  // Extended section indeces
     | NUM = 0x13           // Number of defined types.
     | LOOS = 0x60000000    // Start OS-specific.

    [<Flags>]
    type SectionHeaderFlags =
    | WRITE = 0x1               // Writable
    | ALLOC = 0x2               // Occupies memory during execution
    | EXECINSTR = 0x4           // Executable
    | MERGE = 0x10              // Might be merged
    | STRINGS = 0x20            // Contains nul-terminated strings
    | INFO_LINK = 0x40          // 'sh_info' contains SHT index
    | LINK_ORDER = 0x80         // Preserve order after combining
    | OS_NONCONFORMING = 0x100  // Non-standard OS specific handling required
    | GROUP = 0x200             // Section is member of a group
    | TLS = 0x400               // Section hold thread-local data
    | MASKOS = 0x0ff00000       // OS-specific
    | MASKPROC = 0xf0000000     // Processor-specific
    | ORDERED = 0x4000000       // Special ordering requirement (Solaris)
    | EXCLUDE = 0x8000000       // Section is excluded unless referenced or allocated (Solaris)
    
    type MachineArchitecture =
    | Any = 0x00
    | SPARC = 0x02
    | x86 = 0x03
    | MIPS = 0x08
    | PowerPC = 0x14
    | S390 = 0x16
    | ARM = 0x28
    | SuperH = 0x2A
    | IA_64 = 0x32
    | x86_64 = 0x3E
    | AArch64 = 0xB7
    | RISC_V = 0xF3

    type SegmentType =
    | NULL = 0x00000000
    | LOAD = 0x00000001
    | DYNAMIC = 0x00000002
    | INTERP = 0x00000003
    | NOTE = 0x00000004
    | SHLIB = 0x00000005
    | PHDR = 0x00000006
    | LOOS = 0x60000000
    | HIOS = 0x6FFFFFFF
    | LOPROC = 0x70000000
    | HIPROC = 0x7FFFFFFF

    type ElfFileType =
    | Relocatable
    | Executable
    | Shared
    | Core

    type TargetOperatingSystem =
    | SystemV = 0x00
    | HPUX = 0x01
    | NetBSD = 0x02
    | Linux = 0x03
    | GNUHurd = 0x04
    | Solaris = 0x06
    | AIX = 0x07
    | IRIX = 0x08
    | FreeBSD = 0x09
    | Tru64 = 0x0A
    | NovellModesto = 0x0B
    | OpenBSD = 0x0C
    | OpenVMS = 0x0D
    | NonStopKernel = 0x0E
    | AROS = 0x0F
    | FenixOS = 0x10
    | CloudABI = 0x11
    | Sortix = 0x53

    type TargetArchitecture =
    | TargetArchitecture of MachineArchitecture * uint32

    type ABI =
    | ABI of TargetOperatingSystem * byte

    type EntryPoint = EntryPoint of uint64

    type ProgramHeader = {
        Type: SegmentType 
        SegmentFlags: int 
        SegmentOffset: uint64 
        VirtualAddress: uint64 
        PhysicalAddress: uint64 
        FileSegmentSize: uint64
        MemorySegmentSize: uint64
        Alignment: uint64
    }

    type SectionHeader = {
        NameOffset: uint32
        Type: SectionHeaderType 
        Flags: SectionHeaderFlags 
        VirtualAddress: uint64 
        SectionOffset: uint64 
        FileSectionSize: uint64 
        SectionLinkIndex: uint32
        SectionInfo: uint32
        Alignment: uint64 
        EntrySize: uint64
    }
    
    type FileHeader =
    | FileHeader32 of ABI * ElfFileType * TargetArchitecture * EntryPoint * ProgramHeader * SectionHeader
    | FileHeader64 of ABI * ElfFileType * TargetArchitecture * EntryPoint * ProgramHeader * SectionHeader