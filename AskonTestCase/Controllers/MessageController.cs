using AskonTestCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AskonTestCase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        //Использовал static для сохранения данных внутри проекта из-зи отсутствия БД
        private static readonly Queue<Message> MessageQueue = new Queue<Message>();

        [HttpPost]
        public IActionResult PostMessage([FromBody] MessageCreate request)
        {
            if (request == null || string.IsNullOrEmpty(request.Subject) || string.IsNullOrEmpty(request.Body) || request.Recipients == null || request.Recipients.Count == 0)
            {
                return BadRequest("Invalid request. Please provide subject, body, and recipients.");
            }

            foreach (var recipient in request.Recipients)
            {
                MessageQueue.Enqueue(new Message
                {
                    Subject = request.Subject,
                    Body = request.Body,
                    Recipient = recipient
                });
            }

            return Created("", "Message(s) added to the queue.");
        }

        [HttpGet]
        public IActionResult GetMessages([FromQuery] int rcpt)
        {
            if (MessageQueue.Count == 0)
            {
                return NotFound("No messages in the queue.");
            }

            var messages = new List<Message>();
            foreach (var message in MessageQueue)
            {
                if (message.Recipient == rcpt)
                {
                    messages.Add(new Message
                    {
                        Subject = message.Subject,
                        Body = message.Body
                    });
                }
            }

            if (messages.Count == 0)
            {
                return NotFound("No messages found for the specified recipient.");
            }

            return Ok(messages);
        }
    }
}
