module LedgerTypes

type CurrencyType = USD | EUR
type LocaleType = American | Dutch

type Currency =
    { Type: CurrencyType; Name: string; Sign: string }

    static member create name =
        match name with
        | "USD" -> { Type = USD; Name = name; Sign = "$" }
        | "EUR" -> { Type = EUR; Name = name; Sign = "€" }
        | _ -> failwith "Unexpected currency name"

type Locale =
    { Type: LocaleType; Name: string; Format: string }

    static member create name =
        match name with
        | "en-US" -> { Type = American; Name = name; Format = "MM\/dd\/yyyy"}
        | "nl-NL" -> { Type = Dutch; Name = name; Format = "dd-MM-yyyy"}
        | _ -> failwith "Unexpected locale name"