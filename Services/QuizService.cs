using Quiz.Models;

namespace Quiz.Services;

public class QuizService
{
    private readonly List<QuizQuestion> _questions = new();
    private readonly Dictionary<string, Team> _teams = new();
    private readonly GameState _gameState = new();

    public QuizService()
    {
        InitializeQuestions();
    }

    public List<QuizQuestion> GetQuestions() => _questions;
    public QuizQuestion? GetCurrentQuestion() => _gameState.CurrentQuestionIndex < _questions.Count ? _questions[_gameState.CurrentQuestionIndex] : null;
    public GameState GetGameState() => _gameState;
    public List<Team> GetTeams() => _teams.Values.OrderByDescending(t => t.Score).ToList();

    public Team CreateTeam(string teamName)
    {
        var team = new Team { Name = teamName };
        _teams[team.Id] = team;
        return team;
    }

    public Team JoinTeam(string teamName, string playerName)
    {
        var existingTeam = _teams.Values.FirstOrDefault(t => t.Name == teamName);
        if (existingTeam != null)
        {
            var player = new Player { Name = playerName, TeamId = existingTeam.Id };
            existingTeam.Players.Add(player);
            return existingTeam;
        }
        
        var newTeam = CreateTeam(teamName);
        var newPlayer = new Player { Name = playerName, TeamId = newTeam.Id };
        newTeam.Players.Add(newPlayer);
        return newTeam;
    }

    public void SubmitAnswer(string teamId, int answer)
    {
        if (_gameState.IsActive && !_gameState.TeamAnswers.ContainsKey(teamId))
        {
            _gameState.TeamAnswers[teamId] = answer;
            
            var currentQuestion = GetCurrentQuestion();
            if (currentQuestion != null && answer == currentQuestion.CorrectAnswer)
            {
                _teams[teamId].Score += currentQuestion.Points;
            }
        }
    }

    public void NextQuestion()
    {
        _gameState.CurrentQuestionIndex++;
        _gameState.TeamAnswers.Clear();
        _gameState.ShowResults = false;
        _gameState.IsActive = _gameState.CurrentQuestionIndex < _questions.Count;
    }

    public void ShowResults() => _gameState.ShowResults = true;
    public void StartGame() => _gameState.IsActive = true;
    
    public void ResetGame()
    {
        _teams.Clear();
        _gameState.CurrentQuestionIndex = 0;
        _gameState.IsActive = false;
        _gameState.ShowResults = false;
        _gameState.TeamAnswers.Clear();
    }
    
    public void EndQuiz()
    {
        _gameState.IsActive = false;
        _gameState.ShowResults = true;
    }

    private void InitializeQuestions()
    {
        var categories = new[] { "Christmas Movies", "Christmas Songs", "Christmas Traditions", "Christmas Food", "Santa & Reindeer", 
                               "Christmas Around World", "Christmas History", "Christmas Literature", "Winter Weather", "Holiday Decorations" };

        var allQuestions = new List<(string category, string question, string[] options, int correct, int points)>
        {
            // Christmas Questions (10)
            ("Christmas Movies", "In 'Home Alone', where does Kevin's family go on vacation?", new[] {"Paris", "London", "Rome", "Madrid"}, 0, 1),
            ("Christmas Movies", "What's the name of the Grinch's dog?", new[] {"Max", "Rex", "Buddy", "Charlie"}, 0, 1),
            ("Christmas Movies", "In 'Elf', what are the four main food groups?", new[] {"Candy, candy canes, candy corns, syrup", "Sugar, sweets, chocolate, gum", "Cookies, cake, ice cream, candy", "Chocolate, caramel, fudge, toffee"}, 0, 1),
            ("Christmas Songs", "Which song contains the lyric 'Chestnuts roasting on an open fire'?", new[] {"The Christmas Song", "White Christmas", "Silver Bells", "Let it Snow"}, 0, 1),
            ("Christmas Songs", "In 'The Twelve Days of Christmas', what is given on the 5th day?", new[] {"Five golden rings", "Five calling birds", "Five French hens", "Five geese a-laying"}, 0, 1),
            ("Christmas Songs", "Who originally sang 'Blue Christmas'?", new[] {"Elvis Presley", "Bing Crosby", "Frank Sinatra", "Dean Martin"}, 0, 1),
            ("Christmas Traditions", "Which country started the tradition of putting up a Christmas tree?", new[] {"Germany", "England", "France", "Norway"}, 0, 1),
            ("Christmas Food", "What spice is used to flavor traditional Christmas eggnog?", new[] {"Nutmeg", "Cinnamon", "Cloves", "Ginger"}, 0, 1),
            ("Santa & Reindeer", "How many reindeer pull Santa's sleigh (including Rudolph)?", new[] {"9", "8", "10", "7"}, 0, 1),
            ("Christmas Around World", "In which country is it traditional to eat KFC for Christmas dinner?", new[] {"Japan", "China", "Korea", "Thailand"}, 0, 1),
            
            // Music Questions (5)
            ("Music", "Which instrument has 88 keys?", new[] {"Organ", "Piano", "Harpsichord", "Accordion"}, 1, 1),
            ("Music", "What does 'forte' mean in music?", new[] {"Soft", "Loud", "Fast", "Slow"}, 1, 1),
            ("Music", "Which band released the album 'Abbey Road'?", new[] {"The Rolling Stones", "The Beatles", "Led Zeppelin", "Pink Floyd"}, 1, 1),
            ("Music", "What is the highest female singing voice?", new[] {"Alto", "Soprano", "Mezzo-soprano", "Contralto"}, 1, 1),
            ("Music", "How many strings does a standard guitar have?", new[] {"5", "6", "7", "8"}, 1, 1),
            
            // General Knowledge Questions (5)
            ("Geography", "What is the capital of Australia?", new[] {"Sydney", "Melbourne", "Canberra", "Perth"}, 2, 1),
            ("Science", "What is the chemical symbol for gold?", new[] {"Go", "Gd", "Au", "Ag"}, 2, 1),
            ("History", "In which year did World War II end?", new[] {"1944", "1945", "1946", "1947"}, 1, 1),
            ("Sports", "How many players are on a basketball team on court at one time?", new[] {"4", "5", "6", "7"}, 1, 1),
            ("Nature", "What is the largest mammal in the world?", new[] {"African Elephant", "Blue Whale", "Giraffe", "Polar Bear"}, 1, 1),
            
            // Events in 2025 (5)
            ("2025 Events", "Which major sporting event is scheduled for 2025?", new[] {"FIFA Women's World Cup", "Summer Olympics", "Winter Olympics", "UEFA European Championship"}, 0, 1),
            ("2025 Events", "What significant anniversary does the United Nations celebrate in 2025?", new[] {"75th", "80th", "85th", "90th"}, 1, 1),
            ("2025 Events", "Which country is hosting the World Expo 2025?", new[] {"UAE", "Japan", "France", "Germany"}, 1, 1),
            ("2025 Events", "What major tech event typically happens annually and will occur in 2025?", new[] {"Apple WWDC", "Google I/O", "CES", "All of the above"}, 3, 1),
            ("2025 Events", "Which planet will have a notable astronomical event visible from Earth in 2025?", new[] {"Mars", "Jupiter", "Saturn", "Venus"}, 2, 1)
        };

        // Shuffle all 25 unique questions
        var random = new Random();
        allQuestions = allQuestions.OrderBy(x => random.Next()).ToList();
        
        for (int i = 0; i < 25; i++)
        {
            var q = allQuestions[i];
            _questions.Add(new QuizQuestion
            {
                Id = i + 1,
                Category = q.category,
                Question = $"{q.question} (Q{i + 1})",
                Options = q.options.ToList(),
                CorrectAnswer = q.correct,
                Points = q.points
            });
        }
    }
}