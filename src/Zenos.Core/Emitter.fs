namespace Zenos.Framwork

open Zenos.Framework

module Emitter = 
    type Symbol = string

    type Offset = int64
    type Relocation =
    | Relative of Symbol * Offset
    | Absolute of Symbol * Offset

    type Data = byte ResizeArray
    type ObjectData = {Data: byte array; Alignment: int; Relocations: Relocation list; Symbols: Symbol list}

    type SymbolTable = Map<Symbol, Relocation>
    type Ref = Symbol
    type LocationCounter = int64
    type Reservation = Reservation of int

    type State = State of SymbolTable * ObjectData list * LocationCounter

    
    module Data =
        let rec zeroExtend count (data : Data) =
            for i = 0 to (count - 1) do
                data.Add(0uy)
        
        let emitByte b (data: Data) =
            data.Add(b)

        let emitInt16 (s: int16) (data: Data) =
            emitByte (byte(s &&& 0xffs)) data
            emitByte (byte(((s >>> 8) &&& 0xffs))) data

        let emitInt32 (i: int32) (data: Data) =
            emitByte (byte(i &&& 0xff)) data
            emitByte (byte(((i >>> 8) &&& 0xff))) data
            emitByte (byte(((i >>> 16) &&& 0xff))) data
            emitByte (byte(((i >>> 24) &&& 0xff))) data

        let emitUInt32 (i: uint32) (data: Data) =            
            emitByte (byte(i &&& 0xffu)) data
            emitByte (byte(((i >>> 8) &&& 0xffu))) data
            emitByte (byte(((i >>> 16) &&& 0xffu))) data
            emitByte (byte(((i >>> 24) &&& 0xffu))) data

        let emitInt64 (l: int64) (data: Data) =
            emitByte (byte(l &&& 0xffL)) data
            emitByte (byte(((l >>> 8) &&& 0xffL))) data
            emitByte (byte(((l >>> 16) &&& 0xffL))) data
            emitByte (byte(((l >>> 24) &&& 0xffL))) data
            emitByte (byte(((l >>> 32) &&& 0xffL))) data
            emitByte (byte(((l >>> 40) &&& 0xffL))) data
            emitByte (byte(((l >>> 48) &&& 0xffL))) data
            emitByte (byte(((l >>> 56) &&& 0xffL))) data

        let emitBytes (bytes: byte array) (data: Data) =
            data.AddRange(bytes)

        let emitZeroes count (data: Data) =
            zeroExtend count data
        
        let getReservation size (data: Data) =
            let ticket = Reservation data.Count
            zeroExtend size data
            ticket

        let reserveByte data = getReservation 1 data
        let reserveInt16 data = getReservation 2 data
        let reserveInt32 data = getReservation 4 data
        let reserveInt64 data = getReservation 8 data

        let emitReservedByte value reservation (data: Data) =
            let (Reservation index) = reservation
            data.[index] = value

        let emitReservedInt16 value reservation (data: Data) =
            let (Reservation index) = reservation
            data.[index] <- (byte(value &&& 0xffs))
            data.[index + 1] <- (byte(((value >>> 8) &&& 0xffs)))

        let emitReservedInt32 value reservation (data: Data) =
            let (Reservation index) = reservation
            data.[index] <- (byte(value &&& 0xff))
            data.[index + 1] <- (byte(((value >>> 8) &&& 0xff)))
            data.[index + 2] <- (byte(((value >>> 16) &&& 0xff)))
            data.[index + 3] <- (byte(((value >>> 24) &&& 0xff)))

        let emitReservedInt64 value reservation (data: Data) =
            let (Reservation index) = reservation
            data.[index] <- (byte(value &&& 0xffl))
            data.[index + 1] <- (byte(((value >>> 8) &&& 0xffl)))
            data.[index + 2] <- (byte(((value >>> 16) &&& 0xffl)))
            data.[index + 3] <- (byte(((value >>> 24) &&& 0xffl)))
            data.[index + 4] <- (byte(((value >>> 32) &&& 0xffl)))
            data.[index + 5] <- (byte(((value >>> 40) &&& 0xffl)))
            data.[index + 6] <- (byte(((value >>> 48) &&& 0xffl)))
            data.[index + 7] <- (byte(((value >>> 56) &&& 0xffl)))

        let toArray (data: Data) =
            data
            |> Array.ofSeq


    module SymbolTable =
        let add (table: SymbolTable) name symbolType =
            table.Add(name, symbolType)

        let find (table: SymbolTable) name =
            table.TryFind(name)

    let assembleSectionEntry state (entry: SectionEntry) =
        state

    let assembleDirective state directive =
        match directive with
        | Global name ->
            state
        | Extern name ->
            state
        | Section (name, entries) ->
            entries
            |> List.fold assembleSectionEntry state

    let assemble state (listing : Listing) =
        let (Listing directives) = listing
        directives
        |> List.fold assembleDirective state

