using CodeChallenge.Api.Logic;
using CodeChallenge.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/v1/organizations/{organizationId}/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageLogic _logic;

        public MessagesController(IMessageLogic logic)
        {
            _logic = logic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid organizationId)
        {
            var result = await _logic.GetAllMessagesAsync(organizationId);
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid organizationId, Guid id)
        {
            var result = await _logic.GetMessageAsync(organizationId, id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid organizationId, [FromBody] Message message)
        {
            var result = await _logic.CreateMessageAsync(organizationId, message);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid organizationId, Guid id, [FromBody] Message message)
        {
            message.Id = id;
            var result = await _logic.UpdateMessageAsync(organizationId, message);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid organizationId, Guid id)
        {
            var result = await _logic.DeleteMessageAsync(organizationId, id);
            return result.ToActionResult();
        }
    }
}
