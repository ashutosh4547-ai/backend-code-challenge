using CodeChallenge.Api.Logic.Results;
using CodeChallenge.Models;

namespace CodeChallenge.Api.Logic
{
    public interface IMessageLogic
    {
        Task<LogicResult<IEnumerable<Message>>> GetAllMessagesAsync(Guid organizationId);

        Task<LogicResult<Message>> GetMessageAsync(Guid organizationId, Guid id);

        Task<LogicResult<Message>> CreateMessageAsync(Guid organizationId, Message message);

        Task<LogicResult<Message>> UpdateMessageAsync(Guid organizationId, Message message);

        Task<LogicResult<bool>> DeleteMessageAsync(Guid organizationId, Guid id);
    }
}
