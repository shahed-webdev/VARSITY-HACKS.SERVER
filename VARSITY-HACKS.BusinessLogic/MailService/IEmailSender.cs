namespace VARSITY_HACKS.BusinessLogic;

public interface IEmailSender
{
    void SendEmail(Message message);
    Task SendEmailAsync(Message message);
}