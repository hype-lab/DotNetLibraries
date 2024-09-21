namespace HypeLab.MailEngine.Data.Models
{
    public interface IAttachment
    {
        string Name { get; set; }
        string? ContentId { get; set; }
    }
}
