using Microsoft.AspNetCore.SignalR;

namespace Quiz.Hubs;

public class QuizHub : Hub
{
    public async Task UpdateGame()
    {
        await Clients.All.SendAsync("UpdateGame");
    }

    public async Task NextQuestion()
    {
        await Clients.All.SendAsync("NextQuestion");
    }

    public async Task GameStarted()
    {
        await Clients.All.SendAsync("GameStarted");
    }

    public async Task GameReset()
    {
        await Clients.All.SendAsync("GameReset");
    }

    public async Task QuizEnded()
    {
        await Clients.All.SendAsync("QuizEnded");
    }
}