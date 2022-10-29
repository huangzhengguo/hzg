namespace Hzg.Services;

public interface IFcmService
{
    Task<string> PushNotification(string deviceToken, NotificationType type, string title, string subtitle, string body);
}