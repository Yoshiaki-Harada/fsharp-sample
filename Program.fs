open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open ResponseJson
open Microsoft.AspNetCore.Http

let getUserHandler (id: int) =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let user = { name = "test" }
            return! json user next ctx
        }

let webApp =
    choose
        [ routef "/users/%i" getUserHandler
          route "/" >=> htmlFile "/pages/index.html" ]

let configureApp (app: IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe webApp

let configureServices (services: IServiceCollection) =
    // Add Giraffe dependencies
    services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder.Configure(configureApp).ConfigureServices(configureServices)
            |> ignore)
        .Build()
        .Run()

    0
