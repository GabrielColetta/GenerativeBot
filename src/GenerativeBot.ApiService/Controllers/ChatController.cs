using GenerativeBot.Domain.Interfaces;
using GenerativeBot.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeBot.Api.Controllers;

[Route("api/")]
public class ChatController : Controller
{
    private readonly IBusPublisher _busPublisher;

    public ChatController(IBusPublisher busPublisher)
    {
        _busPublisher = busPublisher;
    }

    [HttpGet("api/chat")]
    public async Task<IActionResult> Get(string message, CancellationToken cancellationToken)
    {
        await _busPublisher.PublishAsync(new ChatQuery(message), cancellationToken);

        return Accepted();
    } 
}
