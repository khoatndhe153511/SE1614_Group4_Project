namespace SE1614_Group4_Project_API.Utils.Interfaces
{
    public interface ILogicHandler
    {
        Task<bool> SendEmailAsync(string recipient, string subject, string body);

        string GeneratePassword(int length);
        public string GetFirstTag(string html);
        public string GetNode(string url, string type, string element);
    }
}