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

let formatChange (locale:Locale) (currency:Currency) chg =
    let c = float chg / 100.0

    let currencyStr =
        match locale.Type with
        | Dutch -> currency.Sign + " "
        | American -> currency.Sign
    
    let changeStr =
        let res = c.ToString("#,#0.00", new CultureInfo(locale.Name))
        if c < 0.0 then res else res + " "

    let res =
        if c < 0.0 && locale.Type = American then
            "(" + currencyStr + changeStr.Substring(1) + ")"
        else
            currencyStr + changeStr
    
    res.PadLeft(13)

let formatLedger currency locale entries =
    let currency = Currency.create currency
    let locale = Locale.create locale
    
    let res =
        match locale.Type with
        | American -> "Date       | Description               | Change       "
        | Dutch -> "Datum      | Omschrijving              | Verandering  "
        
    let folder res x =
        res + Environment.NewLine + formatDate locale x.dat + " | " + formatDesc x.des + " | " + formatChange locale currency x.chg
    
    entries
    |> List.sortBy (fun x -> x.dat, x.des, x.chg)
    |> List.fold folder res
