using Domain.Models;

namespace Domain.IRepositories
{
    public interface IMessagesRepository
    {
        void DeleteAllMessages();
        bool InsertMessages(List<Messages> messages);
        bool InsertMessage(Messages messages);
    }
}
