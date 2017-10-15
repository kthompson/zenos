namespace Zenos.Core

open System.Threading.Tasks

module Result =
    let succeed x = Ok x
    let fail x = Error x
    let getOrElse errValue = function
    | Ok x -> x
    | Error _ -> errValue

    let orElse other = function
    | Ok x -> Ok x
    | Error _ -> other

    let ofBool errValue = function
    | true -> Ok ()
    | false -> Error errValue

    let ofOption err = function
    | Some some -> Ok some
    | None -> Error err

    let ofOptionUnit err = function
    | Some _ -> Ok ()
    | None -> Error err

    let ofTask<'a> (task: Task<'a>) =
        let continuation (t : Task<'a>) =
            if t.IsFaulted then
                Error t.Exception
            else Ok t.Result

        task.ContinueWith continuation |> Async.AwaitTask

    let apply fResult xResult =
        match (fResult, xResult) with
        | Ok f, Ok x -> Ok (f x)
        | Error err1, Ok x -> Error err1
        | Ok f, Error err2 -> Error err2
        | Error err1, Error err2 -> Error (err1 @ err2)

    let (<*>) = apply
    let (<!>) = Result.map

    let mapErrorToList res =
        res
        |> Result.mapError (fun err -> [err])

    let sequence aListOfResults = 
        let cons head tail = head :: tail
        let consR head tail = cons <!> head <*> tail
        let initialValue = Ok []

        List.foldBack consR aListOfResults initialValue

    /// appy either a success function or failure function
    let either successFunc failureFunc twoTrackInput =
        match twoTrackInput with
        | Ok s -> successFunc s
        | Error f -> failureFunc f

    /// convert two one-track functions into a two-track function
    let doubleMap successFunc failureFunc =
        either (successFunc >> succeed) (failureFunc >> fail)

    type ResultBuilder() =
        member this.Return(x) = Ok x
        member this.Bind(x, f) = Result.bind f x

    let result = ResultBuilder()