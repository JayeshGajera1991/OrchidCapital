namespace Orchid.DataModels
{
    public class EmailRequestModel
    {
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string EmailBCC { get; set; }
        public string EmailCC { get; set; }
        public string EmailFrom { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string FilePath { get; set; }
        public List<string> MultipleToEmails { get; set; }
        public List<string> MultipleCCEmails { get; set; }
        public List<string> MultipleBCCEmails { get; set; }
        public string ImpersonatedUserId { get; set; }
        public string ReplyToEmail { get; set; }
        public string FileAttachmentName { get; set; }
        public byte[] FileAttachment { get; set; }
        public int MaxSize { get; set; } = 5;
        public string PublicFolderEmailId { get; set; }

    }
    public class EmailConfiguration
    {
        public bool SSL { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string SmtpServer { get; set; }
        public string Username { get; set; }
        public string ReplyToEmail { get; set; }
        public string FileAttachmentName { get; set; }
        public string FileAttachment { get; set; }
        public string OrgURL { get; set; }
    }        

    public enum ParticipationTypeMask
    {
        From = 1,
        To = 2,
        Cc = 3,
        Bcc = 4
    }
}