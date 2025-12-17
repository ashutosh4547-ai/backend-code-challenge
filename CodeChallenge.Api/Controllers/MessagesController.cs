using CodeChallenge.Models;
using CodeChallenge.Repositories;
//using CodeChallenge.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/v1/organizations/{organizationId}/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessagesController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        // GET: /api/v1/organizations/{organizationId}/messages
        [HttpGet]
        public async Task<IActionResult> GetMessages(Guid organizationId)
        {
            var messages = await _messageRepository.GetAllByOrganizationAsync(organizationId);
            return Ok(messages);
        }

        // GET: /api/v1/organizations/{organizationId}/messages/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(Guid organizationId, Guid id)
        {
            var message = await _messageRepository.GetByIdAsync(organizationId, id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }

        // POST: /api/v1/organizations/{organizationId}/messages
        [HttpPost]
        public async Task<IActionResult> CreateMessage(
            Guid organizationId,
            [FromBody] Message message)
        {
            message.OrganizationId = organizationId;

            var createdMessage = await _messageRepository.CreateAsync(message);

            return CreatedAtAction(
                nameof(GetMessageById),
                new { organizationId = organizationId, id = createdMessage.Id },
                createdMessage);
        }

        // PUT: /api/v1/organizations/{organizationId}/messages/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(
            Guid organizationId,
            Guid id,
            [FromBody] Message message)
        {
            if (id != message.Id)
                return BadRequest("Message ID mismatch");

            message.OrganizationId = organizationId;

            var updated = await _messageRepository.UpdateAsync(message);

            if (updated == null)
                return NotFound();

            return Ok(message);
        }

        // DELETE: /api/v1/organizations/{organizationId}/messages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid organizationId, Guid id)
        {
            var deleted = await _messageRepository.DeleteAsync(organizationId, id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
