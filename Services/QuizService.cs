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
            // Christmas Questions (25)
            ("Christmas Movies", "In 'Home Alone', where does Kevin's family go on vacation?", new[] {"Paris", "London", "Rome", "Madrid"}, 0, 1),
            ("Christmas Movies", "What's the name of the Grinch's dog?", new[] {"Max", "Rex", "Buddy", "Charlie"}, 0, 1),
            ("Christmas Movies", "In 'Elf', what are the four main food groups?", new[] {"Candy, candy canes, candy corns, syrup", "Sugar, sweets, chocolate, gum", "Cookies, cake, ice cream, candy", "Chocolate, caramel, fudge, toffee"}, 0, 1),
            ("Christmas Movies", "What does George Bailey want to do at the beginning of 'It's a Wonderful Life'?", new[] {"Travel the world", "Get married", "Start a business", "Move to the city"}, 0, 1),
            ("Christmas Movies", "In 'The Polar Express', what is the first gift of Christmas?", new[] {"A bell from Santa's sleigh", "A train ticket", "Hot chocolate", "A golden ticket"}, 0, 1),
            ("Christmas Songs", "Which song contains the lyric 'Chestnuts roasting on an open fire'?", new[] {"The Christmas Song", "White Christmas", "Silver Bells", "Let it Snow"}, 0, 1),
            ("Christmas Songs", "In 'The Twelve Days of Christmas', what is given on the 5th day?", new[] {"Five golden rings", "Five calling birds", "Five French hens", "Five geese a-laying"}, 0, 1),
            ("Christmas Songs", "Who originally sang 'Blue Christmas'?", new[] {"Elvis Presley", "Bing Crosby", "Frank Sinatra", "Dean Martin"}, 0, 1),
            ("Christmas Songs", "Complete the lyric: 'I'm dreaming of a white Christmas, just like...'", new[] {"the ones I used to know", "the movies I have seen", "my childhood memories", "the stories I've been told"}, 0, 1),
            ("Christmas Songs", "What Christmas song was originally written for Thanksgiving?", new[] {"Jingle Bells", "White Christmas", "Silent Night", "Deck the Halls"}, 0, 1),
            ("Christmas Traditions", "Which country started the tradition of putting up a Christmas tree?", new[] {"Germany", "England", "France", "Norway"}, 0, 1),
            ("Christmas Traditions", "What do people traditionally put on top of a Christmas tree?", new[] {"Star or Angel", "Cross", "Bell", "Crown"}, 0, 1),
            ("Christmas Traditions", "In which country did the tradition of Christmas stockings originate?", new[] {"Turkey", "Italy", "Greece", "Spain"}, 0, 1),
            ("Christmas Traditions", "What plant is known as the Christmas flower?", new[] {"Poinsettia", "Holly", "Mistletoe", "Pine"}, 0, 1),
            ("Christmas Traditions", "Boxing Day is celebrated on which date?", new[] {"December 26th", "December 27th", "January 1st", "December 24th"}, 0, 1),
            ("Christmas Food", "What spice is used to flavor traditional Christmas eggnog?", new[] {"Nutmeg", "Cinnamon", "Cloves", "Ginger"}, 0, 1),
            ("Christmas Food", "What type of cake is traditionally eaten at Christmas in Britain?", new[] {"Christmas Pudding", "Fruitcake", "Sponge Cake", "Chocolate Cake"}, 0, 1),
            ("Christmas Food", "Which country is famous for its Christmas stollen?", new[] {"Germany", "Austria", "Switzerland", "Netherlands"}, 0, 1),
            ("Christmas Food", "What are Christmas cookies called in Britain?", new[] {"Christmas biscuits", "Holiday treats", "Festive cakes", "Yuletide sweets"}, 0, 1),
            ("Christmas Food", "What meat is traditionally served at Christmas dinner in the UK?", new[] {"Turkey", "Ham", "Beef", "Goose"}, 0, 1),
            ("Santa & Reindeer", "How many reindeer pull Santa's sleigh (including Rudolph)?", new[] {"9", "8", "10", "7"}, 0, 1),
            ("Santa & Reindeer", "What is the name of Rudolph's father?", new[] {"Donner", "Dasher", "Comet", "Blitzen"}, 0, 1),
            ("Santa & Reindeer", "In what year was Rudolph the Red-Nosed Reindeer created?", new[] {"1939", "1935", "1942", "1940"}, 0, 1),
            ("Christmas Around World", "In which country is it traditional to eat KFC for Christmas dinner?", new[] {"Japan", "China", "Korea", "Thailand"}, 0, 1),
            ("Christmas History", "In what year was Christmas declared a federal holiday in the US?", new[] {"1870", "1850", "1890", "1900"}, 0, 1),
            
            // General Knowledge Questions (25)
            ("Geography", "What is the capital of Australia?", new[] {"Sydney", "Melbourne", "Canberra", "Perth"}, 2, 1),
            ("Geography", "Which river is the longest in the world?", new[] {"Amazon", "Nile", "Mississippi", "Yangtze"}, 1, 1),
            ("Geography", "What is the smallest country in the world?", new[] {"Monaco", "Vatican City", "San Marino", "Liechtenstein"}, 1, 1),
            ("Geography", "Which mountain range contains Mount Everest?", new[] {"Andes", "Himalayas", "Alps", "Rockies"}, 1, 1),
            ("Geography", "What is the largest desert in the world?", new[] {"Sahara", "Antarctica", "Gobi", "Arabian"}, 1, 1),
            ("Science", "What is the chemical symbol for gold?", new[] {"Go", "Gd", "Au", "Ag"}, 2, 1),
            ("Science", "How many bones are in the adult human body?", new[] {"196", "206", "216", "186"}, 1, 1),
            ("Science", "What gas makes up about 78% of Earth's atmosphere?", new[] {"Oxygen", "Nitrogen", "Carbon Dioxide", "Hydrogen"}, 1, 1),
            ("Science", "What is the hardest natural substance on Earth?", new[] {"Gold", "Iron", "Diamond", "Platinum"}, 2, 1),
            ("Science", "What planet is known as the Red Planet?", new[] {"Venus", "Mars", "Jupiter", "Saturn"}, 1, 1),
            ("History", "In which year did World War II end?", new[] {"1944", "1945", "1946", "1947"}, 1, 1),
            ("History", "Who was the first person to walk on the moon?", new[] {"Buzz Aldrin", "Neil Armstrong", "John Glenn", "Alan Shepard"}, 1, 1),
            ("History", "In which year did the Berlin Wall fall?", new[] {"1987", "1988", "1989", "1990"}, 2, 1),
            ("History", "Which ancient wonder of the world was located in Alexandria?", new[] {"Hanging Gardens", "Lighthouse", "Colossus", "Mausoleum"}, 1, 1),
            ("History", "Who painted the ceiling of the Sistine Chapel?", new[] {"Leonardo da Vinci", "Michelangelo", "Raphael", "Donatello"}, 1, 1),
            ("Sports", "How many players are on a basketball team on court at one time?", new[] {"4", "5", "6", "7"}, 1, 1),
            ("Sports", "In which sport would you perform a slam dunk?", new[] {"Volleyball", "Basketball", "Tennis", "Baseball"}, 1, 1),
            ("Sports", "How often are the Summer Olympic Games held?", new[] {"Every 2 years", "Every 3 years", "Every 4 years", "Every 5 years"}, 2, 1),
            ("Sports", "What is the maximum score possible in ten-pin bowling?", new[] {"250", "300", "350", "400"}, 1, 1),
            ("Sports", "In golf, what is one stroke under par called?", new[] {"Eagle", "Birdie", "Bogey", "Albatross"}, 1, 1),
            ("Literature", "Who wrote 'Romeo and Juliet'?", new[] {"Charles Dickens", "William Shakespeare", "Jane Austen", "Mark Twain"}, 1, 1),
            ("Literature", "Which book begins with 'It was the best of times, it was the worst of times'?", new[] {"Great Expectations", "A Tale of Two Cities", "Oliver Twist", "David Copperfield"}, 1, 1),
            ("Movies", "Which movie won the Academy Award for Best Picture in 2020?", new[] {"1917", "Joker", "Parasite", "Once Upon a Time in Hollywood"}, 2, 1),
            ("Music", "Which instrument has 88 keys?", new[] {"Organ", "Piano", "Harpsichord", "Accordion"}, 1, 1),
            ("Nature", "What is the largest mammal in the world?", new[] {"African Elephant", "Blue Whale", "Giraffe", "Polar Bear"}, 1, 1)
        };

        // Shuffle all 50 unique questions
        var random = new Random();
        allQuestions = allQuestions.OrderBy(x => random.Next()).ToList();
        
        for (int i = 0; i < 50; i++)
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