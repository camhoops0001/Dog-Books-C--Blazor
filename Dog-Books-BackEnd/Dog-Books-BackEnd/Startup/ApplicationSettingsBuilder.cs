namespace Dog_Books_BackEnd.Startup
{
    public static class ApplicationSettingsBuilder
    {   //If I wanted to secure the API I would require this api key. I would NOT normally secure it in appsetting.env.json
        //I would secure it within the Azure Key Vault and 

        //This is what I would include if I intended on adding environmental app settings.

        //Would set ASPNETCORE_ENVIRONMENT variable within the relevant Azure Portal Service

        //You also could set these variable within the Azure Portal Configuration values, and do away with
        //setting alot of the variables in the appsettings.env.json up.

        //Since we are not spinning up an Azure environment for this project I'll just include the Proof of Concept for a working
        // api_key implementation in the appsettings.env.json files.
        
        //Again, I would NOT recommend having an API key ANYWHERE in your codebase, this is just for a proof of concept.
        public static IHostBuilder ConfigureAppSettings(this IHostBuilder host)
        {
            var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            host.ConfigureAppConfiguration((ctx, builder) =>
            {
                builder.AddJsonFile("appsettings.json", false, true);
                builder.AddJsonFile($"appsettings.{enviroment}.json", true, true);
                builder.AddEnvironmentVariables();
            });

            return host;
        }
    }
}
