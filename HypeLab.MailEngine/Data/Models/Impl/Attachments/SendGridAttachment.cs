namespace HypeLab.MailEngine.Data.Models.Impl.Attachments
{
    public class SendGridAttachment : IAttachment
    {
        public SendGridAttachment() { }

        public SendGridAttachment(string name, string type, byte[] content, string disposition)
        {
            Name = name;
            Type = type;
            Content = content;
            Disposition = disposition;
        }

        public SendGridAttachment(string name, string type, byte[] content, string disposition, string contentId)
        {
            Name = name;
            Type = type;
            Content = content;
            Disposition = disposition;
            ContentId = contentId;
        }

        public string Name { get; set; }
        public string? ContentId { get; set; }
        public string Type { get; set; }
        public byte[] Content { get; set; }
        public string Disposition { get; set; }
    }
}
