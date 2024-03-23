module Ledger

open System
open System.Globalization
open LedgerTypes

type Entry = { dat: DateTime; des: string; chg: int }

let mkEntry (date: string) description change = { dat = DateTime.Parse(date, CultureInfo.InvariantCulture); des = description; chg = change }

let formatDate (locale:Locale) (dat:DateTime) =
    dat.ToString(locale.Format)

let formatDesc (des:string) =
    if des.Length <= 25 then 
        des.PadRight(25)
    elif des.Length = 25 then 
        des
    else 
        des.[0..21] + "..."

let formatChange locale (currency:Currency) chg =
    let c = float chg / 100.0

    if c < 0.0 then 
        if locale = "nl-NL" then
            ($"{currency.Sign} " + c.ToString("#,#0.00", new CultureInfo("nl-NL"))).PadLeft(13)
        elif locale = "en-US" then
            ($"({currency.Sign}" + c.ToString("#,#0.00", new CultureInfo("en-US")).Substring(1) + ")").PadLeft(13)
        else
            failwith "Unexpected locale"
    else 
        if locale = "nl-NL" then
            ($"{currency.Sign} " + c.ToString("#,#0.00", new CultureInfo("nl-NL")) + " ").PadLeft(13)
        elif locale = "en-US" then
            ($"{currency.Sign}" + c.ToString("#,#0.00", new CultureInfo("en-US")) + " ").PadLeft(13)
        else
            failwith "Unexpected locale"

let formatLedger currency locale entries =
    let currency = Currency.create currency
    let locale = Locale.create locale
    
    let res =
        if locale.Name = "en-US" then "Date       | Description               | Change       "
        elif locale.Name = "nl-NL" then "Datum      | Omschrijving              | Verandering  "
        else failwith "Unexpected locale"
        
    let folder res x =
        res + Environment.NewLine + formatDate locale x.dat + " | " + formatDesc x.des + " | " + formatChange locale.Name currency x.chg
    
    entries
    |> List.sortBy (fun x -> x.dat, x.des, x.chg)
    |> List.fold folder res
