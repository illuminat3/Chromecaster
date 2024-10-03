namespace Chromecaster.Datatypes
{
    public class Message
    {
        public string Type { get; set; } = "LOAD";
        public string Url;
        public string ApplicationID;
        public string NamespaceName;
    }
}