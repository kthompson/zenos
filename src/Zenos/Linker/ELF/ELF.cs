using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Zenos.Linker.ELF
{
    class FileHeader
    {
        public int MagicNumber => 0x7f454c46;
        public bool Format64Bit { get; set; }
        public bool BigEndian => true;
        public byte IdentVersion => 1;
        public TargetOperatingSystem TargetOperatingSystem { get; set; }
        public byte ABIVersion { get; set; }
        public ElfFileType Type { get; set; }
        public MachineArchitecture Architecture { get; set; }
        public int Version => 1;
        public ulong EntryPoint { get; set; }
        public ulong ProgramHeaderOffset { get; set; }
        public ulong SectionHeaderOffset { get; set; }
        public uint Flags { get; set; }
        public int HeaderSize => Format64Bit ? 64 : 52;
        public int ProgramHeaderEntrySize { get; set; }
        public int ProgramHeaderEntries { get; set; }
        public int SectionHeaderEntrySize { get; set; }
        public int SectionHeaderEntries { get; set; }
        public int SectionHeaderNamesIndex { get; set; }
    }

    class ProgramHeader
    {
        public SegmentType Type { get; set; }
        public int SegmentFlags { get; set; }
        public ulong SegmentOffset { get; set; }
        public ulong VirtualAddress { get; set; }
        public ulong PhysicalAddress { get; set; }
        public ulong FileSegmentSize { get; set; }
        public ulong MemorySegmentSize { get; set; }
        public ulong Alignment { get; set; }
    }

    class SectionHeader
    {
        public uint NameOffset { get; set; }
        public SectionHeaderType Type { get; set; }
        public SectionHeaderFlags Flags { get; set; }
        public ulong VirtualAddress { get; set; }
        public ulong SectionOffset { get; set; }
        public ulong FileSectionSize { get; set; }
        public uint SectionLinkIndex { get; set; }
        public uint SectionInfo { get; set; }
        public ulong Alignment { get; set; }
        public ulong EntrySize { get; set; }
    }
}
