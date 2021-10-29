namespace InterfaceAdapters
{
    public class AdConf
    {
        public readonly string AdId;
        public string GameId { get; set; }
        public bool TestMode { get; set; }
        
        public AdConf(string adId)
        {
            AdId = adId;
        }

    }
}