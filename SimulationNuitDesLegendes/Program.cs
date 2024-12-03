using ExcelDataReader;
using SimulationNuitDesLegendes;

// Define limits
const int startRow = 2;
const int endRow = 14;
const int startColumn = 1;
const int endColumn = 8;

// Run the simulation
// Print the results
PrintResults(RunSimulation());

List<List<Game.Winner>> RunSimulation()
{
    var results = new List<List<Game.Winner>> ();
    
    // Run each variant 100 times and record the results
    foreach (var currentVariant in GetSetupList())
    {
        var winnerList = new List<Game.Winner>();
        for (int i = 0; i < 100; i++)
        {
            // Create a new game
            Game game = new Game(currentVariant[0], currentVariant[1], currentVariant[2], currentVariant[3], currentVariant[4], currentVariant[5], currentVariant[6], currentVariant[7]);
        
            // Run the game
            var winner = game.RunGame();
        
            // Record the results
            winnerList.Add(winner);
        }
        results.Add(winnerList);
    }

    return results;
}

void PrintResults(List<List<Game.Winner>> results)
{
    // Print the results
    for (int i = 0; i < results.Count; i++)
    {
        int totalGames = results[i].Count;
        int vouivreGameWins = results[i].Count(w => w == Game.Winner.Vouivre);
        int belierGameWins = results[i].Count(w => w == Game.Winner.Belier);
        int hogGameWins = results[i].Count(w => w == Game.Winner.Hog);
        
        int vouivrePercentage = (int)((double)vouivreGameWins / totalGames * 100);
        int belierPercentage = (int)((double)belierGameWins / totalGames * 100);
        int hogPercentage = (int)((double)hogGameWins / totalGames * 100);
        
        Console.WriteLine($"Variant {i + 1}");
        Console.WriteLine($"The Vouivre won {vouivreGameWins} out of {totalGames} games ({vouivrePercentage}%)");
        Console.WriteLine($"The Belier won {belierGameWins} out of {totalGames} games ({belierPercentage}%)");
        Console.WriteLine($"The Hog won {hogGameWins} out of {totalGames} games ({hogPercentage}%)");
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
