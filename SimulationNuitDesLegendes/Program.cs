using ExcelDataReader;
using ExcelDataReader.Log;
using SimulationNuitDesLegendes;

// Define excel limits
const int startRow = 2;
const int endRow = 14;
const int startColumn = 1;
const int endColumn = 8;

// Define simulation parameters
const int variantIterations = 100;
const bool saveLogFile = true;

// Run the simulation
// Print the results
PrintResults(RunSimulation());

List<List<Game.GameStats>> RunSimulation()
{
    var results = new List<List<Game.GameStats>> ();
    
    // Run each variant 100 times and record the results
    var setupList = GetSetupList();
    for (int i = 0; i < setupList.Count; i++)
    {
        var currentVariant = setupList[i];
        
        var statsList = new List<Game.GameStats>();
        
        // Run the variant 100 times
        for (int j = 0; j < variantIterations; j++)
        {
            // Create a new game
            Logger.Log($"Start of {i + 5} player variation, iteration {j + 1}");
            Game game = new Game(currentVariant[0], currentVariant[1], currentVariant[2], currentVariant[3], currentVariant[4], currentVariant[5], currentVariant[6], currentVariant[7]);
        
            // Run the game
            var stats = game.RunGame();
        
            // Record the results
            statsList.Add(stats);
            Logger.Log($"The {stats.winner} won in {stats.rounds} rounds\n");
            Logger.Log($"End of {i + 5} player variation, iteration {j + 1}");
            Logger.SplitLog();
        }
        // Add the stats list to the results
        results.Add(statsList);

        if (saveLogFile)
        {
            // Save the log file of the last variant
            Logger.SaveLogFile($"{i + 5} PlayerVariant");
            Logger.ClearLog();
        }
    }

    return results;
}

void PrintResults(List<List<Game.GameStats>> results)
{
    // Print the results
    for (int i = 0; i < results.Count; i++)
    {
        // Compute the win percentages
        int totalGames = results[i].Count;
        int vouivreGameWins = results[i].Count(stat => stat.winner == Game.Winner.Vouivre);
        int belierGameWins = results[i].Count(stat => stat.winner == Game.Winner.Belier);
        int hogGameWins = results[i].Count(stat => stat.winner == Game.Winner.Hog);
        
        int vouivrePercentage = (int)((double)vouivreGameWins / totalGames * 100);
        int belierPercentage = (int)((double)belierGameWins / totalGames * 100);
        int hogPercentage = (int)((double)hogGameWins / totalGames * 100);
        
        // Compute the average number of rounds
        double totalRounds = results[i].Sum(stat => stat.rounds);
        double averageRounds = totalRounds / totalGames;
        
        Console.WriteLine($"For the {i + 5} player variation:");
        Console.WriteLine($"The Vouivre won {vouivreGameWins} out of {totalGames} games ({vouivrePercentage}%)");
        Console.WriteLine($"The Belier won {belierGameWins} out of {totalGames} games ({belierPercentage}%)");
        Console.WriteLine($"The Hog won {hogGameWins} out of {totalGames} games ({hogPercentage}%)");
        Console.WriteLine($"The average number of rounds was {averageRounds}");
        Console.WriteLine();
    }
}

List<List<int>> GetSetupList()
{
    List<List<int>> setupList = new List<List<int>>();
    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

    // Get project directory
    string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

    using (var fs = File.Open(projectDirectory + "/Excel/RepartitionRoles.xlsx", FileMode.Open, FileAccess.Read))
    {
        // Read the Excel file from startRow to endRow
        using (var reader = ExcelReaderFactory.CreateReader(fs))
        {
            // Skip forward to the startRow
            for (int i = 0; i < startRow; i++)
            {
                reader.Read();
            }
            do
            {
                while (reader.Read())
                {
                    var lineData = new List<int>();
                    for (int i = startColumn; i <= endColumn; i++)
                    {
                        lineData.Add(Int32.TryParse(reader.GetValue(i).ToString(), out int result) ? result : 0);
                    }
                    setupList.Add(lineData);
                }
            } while (reader.NextResult() && reader.Depth <= endRow);
        }
    }
    return setupList;
}
