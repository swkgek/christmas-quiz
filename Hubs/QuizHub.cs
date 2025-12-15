using Microsoft.AspNetCore.SignalR;

namespace Quiz.Hubs;

public class QuizHub : Hub
{
    public async Task UpdateGame()
    {
        await Clients.Others.SendAsync("UpdateGame");
    }

    public async Task NextQuestion()
    {
        await Clients.Others.SendAsync("NextQuestion");
    }

    public async Task GameStarted()
    {
        await Clients.Others.SendAsync("GameStarted");
    }

    public async Task GameReset()
    {
        await Clients.Others.SendAsync("GameReset");
    }

    public async Task QuizEnded()
    {
        await Clients.Others.SendAsync("QuizEnded");
    }
}