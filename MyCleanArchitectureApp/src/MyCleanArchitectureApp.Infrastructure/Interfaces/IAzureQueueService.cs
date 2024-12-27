using System.Threading.Tasks;

namespace MyCleanArchitectureApp.Infrastructure.Interfaces
{
    public interface IAzureQueueService
    {
        Task SendMessageAsync(string message);
    }
}