module Ledger

open System
open System.Globalization

type Entry = { dat: DateTime; des: string; chg: int }

let mkEntry (date: string) description change = { dat = DateTime.Parse(date, CultureInfo.InvariantCulture); des = description; chg = change }

let formatDate locale (dat:DateTime) =
    if locale = "nl-NL" then 
        dat.ToString("dd-MM-yyyy")
    elif locale = "en-US" then 
        dat.ToString("MM\/dd\/yyyy")
    else
        failwith "Unexpected locale"

let formatDesc (des:string) =
    if des.Length <= 25 then 
        des.PadRight(25)
    elif des.Length = 25 then 
        des
    else 
        des.[0..21] + "..."

let formatChange locale currency chg =
    let c = float chg / 100.0

    if c < 0.0 then 
        if locale = "nl-NL" then
            if currency = "USD" then
                ("$ " + c.ToString("#,#0.00", new CultureInfo("nl-NL"))).PadLeft(13) 
            elif currency = "EUR" then
                ("€ " + c.ToString("#,#0.00", new CultureInfo("nl-NL"))).PadLeft(13) 
            else
                failwith "Unexpected currency"
        elif locale = "en-US" then
            if currency = "USD" then
                ("($" + c.ToString("#,#0.00", new CultureInfo("en-US")).Substring(1) + ")").PadLeft(13) 
            elif currency = "EUR" then
                ("(€" + c.ToString("#,#0.00", new CultureInfo("en-US")).Substring(1) + ")").PadLeft(13) 
            else
                failwith "Unexpected currency"
        else
            failwith "Unexpected locale"
    else 
        if locale = "nl-NL" then
            if currency = "USD" then
                ("$ " + c.ToString("#,#0.00", new CultureInfo("nl-NL")) + " ").PadLeft(13) 
            elif currency = "EUR" then
                ("€ " + c.ToString("#,#0.00", new CultureInfo("nl-NL")) + " ").PadLeft(13) 
            else
                failwith "Unexpected currency"
        elif locale = "en-US" then
            if currency = "USD" then
                ("$" + c.ToString("#,#0.00", new CultureInfo("en-US")) + " ").PadLeft(13) 
            elif currency = "EUR" then
                ("€" + c.ToString("#,#0.00", new CultureInfo("en-US")) + " ").PadLeft(13) 
            else
                failwith "Unexpected currency"
        else
            failwith "Unexpected locale"

let formatLedger currency locale entries =
    
    let res =
        if locale = "en-US" then "Date       | Description               | Change       "
        elif locale = "nl-NL" then "Datum      | Omschrijving              | Verandering  "
        else failwith "Unexpected locale"
        
    let folder res x =
        res + Environment.NewLine + formatDate locale x.dat + " | " + formatDesc x.des + " | " + formatChange locale currency x.chg
    
    entries
    |> List.sortBy (fun x -> x.dat, x.des, x.chg)
    |> List.fold folder res
