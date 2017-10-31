using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Zenos.Linker.Common
{
    interface IByteWriter<T>
    {
        byte[] Write(T @object);
    }

    interface IByteReader<T>
    {
        T Read(byte[] input);
    }

    interface IByteFormat<T> : IByteReader<T>, IByteWriter<T>
    {
    }


    internal struct ArrayBuilder<T>
    {
        private T[] _items;
        private int _count;

        public T[] ToArray()
        {
            if (_items == null)
                return Array.Empty<T>();
            if (_count != _items.Length)
                Array.Resize(ref _items, _count);
            return _items;
        }

        public void Add(T item)
        {
            if (_items == null || _count == _items.Length)
                Array.Resize(ref _items, 2 * _count + 1);
            _items[_count++] = item;
        }


        public void Append(T[] newItems)
        {
            Append(newItems, 0, newItems.Length);
        }

        public void Append(T[] newItems, int offset, int length)
        {
            if (length == 0)
                return;

            Debug.Assert(length > 0);
            Debug.Assert(newItems.Length >= offset + length);

            EnsureCapacity(_count + length);
            Array.Copy(newItems, offset, _items, _count, length);
            _count += length;
        }

        public void Append(ArrayBuilder<T> newItems)
        {
            if (newItems.Count == 0)
                return;
            EnsureCapacity(_count + newItems.Count);
            Array.Copy(newItems._items, 0, _items, _count, newItems.Count);
            _count += newItems.Count;
        }

        public void ZeroExtend(int numItems)
        {
            EnsureCapacity(_count + numItems);
            _count += numItems;
        }

        public void EnsureCapacity(int requestedCapacity)
        {
            if (requestedCapacity > (_items?.Length ?? 0))
            {
                int newCount = Math.Max(2 * _count + 1, requestedCapacity);
                Array.Resize(ref _items, newCount);
            }
        }

        public int Count => _count;

        public T this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
            }
        }

        public bool Contains(T t)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_items[i].Equals(t))
                {
                    return true;
                }
            }

            return false;
        }
    }

    class ObjectData
    {
        public ObjectData(byte[] data, int alignment, Relocation[] relocations, Symbol[] symbols)
        {
            Data = data;
            Alignment = alignment;
            Relocations = relocations;
            Symbols = symbols;
        }

        public byte[] Data { get; }
        public int Alignment { get; }
        public Relocation[] Relocations { get; }
        public Symbol[] Symbols { get; }
    }

    public enum RelocType
    {
        IMAGE_REL_BASED_ABSOLUTE = 0x00,     // No relocation required
        IMAGE_REL_BASED_ADDR32NB = 0x02,     // The 32-bit address without an image base (RVA)
        IMAGE_REL_BASED_HIGHLOW = 0x03,     // 32 bit address base
        IMAGE_REL_BASED_THUMB_MOV32 = 0x07,     // Thumb2: based MOVW/MOVT
        IMAGE_REL_BASED_DIR64 = 0x0A,     // 64 bit address base
        IMAGE_REL_BASED_REL32 = 0x10,     // 32-bit relative address from byte following reloc
        IMAGE_REL_BASED_THUMB_BRANCH24 = 0x13,     // Thumb2: based B, BL
        IMAGE_REL_BASED_ARM64_BRANCH26 = 0x14,     // Arm64: B, BL
        IMAGE_REL_BASED_RELPTR32 = 0x7C,     // 32-bit relative address from byte starting reloc
        // This is a special NGEN-specific relocation type 
        // for relative pointer (used to make NGen relocation 
        // section smaller)    
        IMAGE_REL_SECREL = 0x80,     // 32 bit offset from base of section containing target
    }

    class Relocation
    {
        public Relocation(RelocType relocType, Symbol symbol, int offset)
        {
            Symbol = symbol;
            Offset = offset;
            RelocType = relocType;
        }

        public Symbol Symbol { get; }
        public int Offset { get; }
        public RelocType RelocType { get; }

    }

    class Symbol { }

    public struct ObjectDataBuilder
    {
        private ArrayBuilder<byte> _data;
        //private ArrayBuilder<Relocation> _relocs;

        public int Alignment { get; private set; }

        public ObjectDataBuilder(bool relocsOnly)
        {
            //_target = factory.Target;
            _data = new ArrayBuilder<byte>();
            //_relocs = new ArrayBuilder<Relocation>();
            Alignment = 1;
            //_definedSymbols = new ArrayBuilder<ISymbolDefinitionNode>();
            //#if DEBUG
            //_numReservations = 0;
            //_checkAllSymbolDependenciesMustBeMarked = !relocsOnly;
            //#endif
        }

        /// <summary>
        /// Raise the alignment requirement of this object to <paramref name="align"/>. This has no effect
        /// if the alignment requirement is already larger than <paramref name="align"/>.
        /// </summary>
        public void RequireInitialAlignment(int align)
        {
            this.Alignment = Math.Max(align, Alignment);
        }

        public int CountBytes
        {
            get
            {
                return _data.Count;
            }
        }

        public void EmitByte(byte emit)
        {
            _data.Add(emit);
        }

        public void EmitShort(short emit)
        {
            EmitByte((byte)(emit & 0xFF));
            EmitByte((byte)((emit >> 8) & 0xFF));
        }

        public void EmitInt(int emit)
        {
            EmitByte((byte)(emit & 0xFF));
            EmitByte((byte)((emit >> 8) & 0xFF));
            EmitByte((byte)((emit >> 16) & 0xFF));
            EmitByte((byte)((emit >> 24) & 0xFF));
        }

        public void EmitUInt(uint emit)
        {
            EmitByte((byte)(emit & 0xFF));
            EmitByte((byte)((emit >> 8) & 0xFF));
            EmitByte((byte)((emit >> 16) & 0xFF));
            EmitByte((byte)((emit >> 24) & 0xFF));
        }

        public void EmitLong(long emit)
        {
            EmitByte((byte)(emit & 0xFF));
            EmitByte((byte)((emit >> 8) & 0xFF));
            EmitByte((byte)((emit >> 16) & 0xFF));
            EmitByte((byte)((emit >> 24) & 0xFF));
            EmitByte((byte)((emit >> 32) & 0xFF));
            EmitByte((byte)((emit >> 40) & 0xFF));
            EmitByte((byte)((emit >> 48) & 0xFF));
            EmitByte((byte)((emit >> 56) & 0xFF));
        }


        public void EmitBytes(byte[] bytes)
        {
            _data.Append(bytes);
        }

        public void EmitBytes(byte[] bytes, int offset, int length)
        {
            _data.Append(bytes, offset, length);
        }

        public void EmitZeros(int numBytes)
        {
            _data.ZeroExtend(numBytes);
        }

        private Reservation GetReservationTicket(int size)
        {
            var ticket = (Reservation)_data.Count;
            _data.ZeroExtend(size);
            return ticket;
        }

        private int ReturnReservationTicket(Reservation reservation)
        {
            return (int)reservation;
        }

        public Reservation ReserveByte()
        {
            return GetReservationTicket(1);
        }

        public void EmitByte(Reservation reservation, byte emit)
        {
            int offset = ReturnReservationTicket(reservation);
            _data[offset] = emit;
        }

        public Reservation ReserveShort()
        {
            return GetReservationTicket(2);
        }

        public void EmitShort(Reservation reservation, short emit)
        {
            int offset = ReturnReservationTicket(reservation);
            _data[offset] = (byte)(emit & 0xFF);
            _data[offset + 1] = (byte)((emit >> 8) & 0xFF);
        }

        public Reservation ReserveInt()
        {
            return GetReservationTicket(4);
        }

        public void EmitInt(Reservation reservation, int emit)
        {
            int offset = ReturnReservationTicket(reservation);
            _data[offset] = (byte)(emit & 0xFF);
            _data[offset + 1] = (byte)((emit >> 8) & 0xFF);
            _data[offset + 2] = (byte)((emit >> 16) & 0xFF);
            _data[offset + 3] = (byte)((emit >> 24) & 0xFF);
        }
        public enum Reservation { }
    }
}
