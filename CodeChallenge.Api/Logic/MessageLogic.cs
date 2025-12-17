using CodeChallenge.Api.Repositories;
using CodeChallenge.Api.Logic.Results;
using CodeChallenge.Models;

namespace CodeChallenge.Api.Logic
{
    public class MessageLogic : IMessageLogic
    {
        private readonly IMessageRepository _repository;

        public MessageLogic(IMessageRepository repository)
        {
            _repository = repository;
        }

        public async Task<LogicResult<IEnumerable<Message>>> GetAllMessagesAsync(Guid organizationId)
        {
            var messages = await _repository.GetAllByOrganizationAsync(organizationId);
            return LogicResult<IEnumerable<Message>>.Success(messages);
        }

        public async Task<LogicResult<Message>> GetMessageAsync(Guid organizationId, Guid id)
        {
            var message = await _repository.GetByIdAsync(organizationId, id);

            if (message == null)
                return LogicResult<Message>.NotFound("Message not found");

            return LogicResult<Message>.Success(message);
        }

        public async Task<LogicResult<Message>> CreateMessageAsync(Guid organizationId, Message message)
        {
            if (string.IsNullOrWhiteSpace(message.Title) ||
                message.Title.Length < 3 || message.Title.Length > 200)
                return LogicResult<Message>.Invalid("Title must be between 3 and 200 characters");

            if (string.IsNullOrWhiteSpace(message.Content) ||
                message.Content.Length < 10 || message.Content.Length > 1000)
                return LogicResult<Message>.Invalid("Content must be between 10 and 1000 characters");

            var existing = await _repository.GetByTitleAsync(organizationId, message.Title);
            if (existing != null)
                return LogicResult<Message>.Invalid("Title must be unique per organization");

            message.OrganizationId = organizationId;
            message.CreatedAt = DateTime.UtcNow;
            message.UpdatedAt = DateTime.UtcNow;
            message.IsActive = true;

            var created = await _repository.CreateAsync(message);
            return LogicResult<Message>.Success(created);
        }

        public async Task<LogicResult<Message>> UpdateMessageAsync(Guid organizationId, Message message)
        {
            var existing = await _repository.GetByIdAsync(organizationId, message.Id);

            if (existing == null)
                return LogicResult<Message>.NotFound("Message not found");

            if (!existing.IsActive)
                return LogicResult<Message>.Invalid("Inactive messages cannot be updated");

            if (string.IsNullOrWhiteSpace(message.Title) ||
                message.Title.Length < 3 || message.Title.Length > 200)
                return LogicResult<Message>.Invalid("Title must be between 3 and 200 characters");

            if (string.IsNullOrWhiteSpace(message.Content) ||
                message.Content.Length < 10 || message.Content.Length > 1000)
                return LogicResult<Message>.Invalid("Content must be between 10 and 1000 characters");

            var duplicate = await _repository.GetByTitleAsync(organizationId, message.Title);
            if (duplicate != null && duplicate.Id != message.Id)
                return LogicResult<Message>.Invalid("Title must be unique per organization");

            existing.Title = message.Title;
            existing.Content = message.Content;
            existing.IsActive = message.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);

            return LogicResult<Message>.Success(existing);
        }

        public async Task<LogicResult<bool>> DeleteMessageAsync(Guid organizationId, Guid id)
        {
            var existing = await _repository.GetByIdAsync(organizationId, id);

            if (existing == null)
                return LogicResult<bool>.NotFound("Message not found");

            if (!existing.IsActive)
                return LogicResult<bool>.Invalid("Inactive messages cannot be deleted");

            await _repository.DeleteAsync(organizationId, id);
            return LogicResult<bool>.Success(true);
        }
    }
}
