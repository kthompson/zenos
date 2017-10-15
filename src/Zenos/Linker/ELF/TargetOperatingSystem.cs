namespace Zenos.Linker.ELF
{
    enum TargetOperatingSystem : byte
    {
        SystemV = 0x00,
        HPUX = 0x01,
        NetBSD = 0x02,
        Linux = 0x03,
        GNUHurd = 0x04,
        Solaris = 0x06,
        AIX = 0x07,
        IRIX = 0x08,
        FreeBSD = 0x09,
        Tru64 = 0x0A,
        NovellModesto = 0x0B,
        OpenBSD = 0x0C,
        OpenVMS = 0x0D,
        NonStopKernel = 0x0E,
        AROS = 0x0F,
        FenixOS = 0x10,
        CloudABI = 0x11,
        Sortix = 0x53
    }
}