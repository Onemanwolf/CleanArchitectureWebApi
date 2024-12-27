using System.Threading.Tasks;
using Azure.Storage.Queues;
using MyCleanArchitectureApp.Infrastructure.Interfaces;

namespace MyCleanArchitectureApp.Infrastructure.Messaging
{
    public class AzureQueueService : IAzureQueueService
    {
        private readonly QueueClient _queueClient;

        public AzureQueueService(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }
    }
}