namespace Dog_Books_BackEnd
{
    public class Settings
    {
        public string ApiKey { get; set; }

        //If we were working with a vendor/company, then we'd need these to be environment specific
        //(if they have dev/test/stg/prod... etc 
        //Since we're just using production we won't need these to be further specified in the env files
        public string DogApiBaseUrl { get; set; }
        public string BookApiBaseUrl { get; set; }

    }
}
