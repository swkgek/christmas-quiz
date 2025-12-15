namespace Quiz.Models;

public class QuizQuestion
{
    public int Id { get; set; }
    public string Category { get; set; } = "";
    public string Question { get; set; } = "";
    public List<string> Options { get; set; } = new();
    public int CorrectAnswer { get; set; }
    public int Points { get; set; }
}

public class Team
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public int Score { get; set; }
    public List<Player> Players { get; set; } = new();
}

public class Player
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public string TeamId { get; set; } = "";
}

public class GameState
{
    public int CurrentQuestionIndex { get; set; }
    public bool IsActive { get; set; }
    public Dictionary<string, int> TeamAnswers { get; set; } = new();
    public bool ShowResults { get; set; }
}