open System


let installDeps () =
    let exitCode = Microsoft.Playwright.Program.Main([| "install-deps" |])

    if (exitCode <> 0) then
        Console.WriteLine("Failed to install browsers dependencies")
        Environment.Exit(exitCode)

let installBrowsers () =
    let exitCode = Microsoft.Playwright.Program.Main([| "install" |])

    if (exitCode <> 0) then
        Console.WriteLine("Failed to install browsers")
        Environment.Exit(exitCode)

installDeps ()
installBrowsers ()
