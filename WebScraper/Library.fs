module WebScraper
open System
open System.IO
open System.Net.Http
open FSharp.Data
open System.Globalization

let asinPath = @"C:\Users\fisch\Desktop\projects\ebay\AmzPriceNotifier\WebScraper\acab.txt"

[<Literal>]
let ResolutionFolder = __SOURCE_DIRECTORY__

type PriceFinder = HtmlProvider<"sample.html", ResolutionFolder=ResolutionFolder>

let doReq asin =
    use req = new HttpClient()
    let baseUrl = "https://api.webscrapingapi.com/v1?api_key="
    let apiKey = Environment.GetEnvironmentVariable("SCRAPE_APIKEY")
    let amzUrl = "https://amazon.de/d/"

    let data = PriceFinder.Load(baseUrl + apiKey + "&url=" + amzUrl + asin)
    let price = data.Html.CssSelect ".a-offscreen" |> List.head
    let p = price.InnerText()
    Decimal.Parse(p, NumberStyles.Currency)
    

let getAsins path = 
    let asins = File.ReadAllLines(path)
    Array.take 5 asins


let workFlow () =
    asinPath
    |> getAsins
    |> Array.map doReq
    |> Array.iter (printfn "%A")

workFlow() |> ignore