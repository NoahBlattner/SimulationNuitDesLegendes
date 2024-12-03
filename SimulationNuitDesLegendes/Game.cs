using System.Collections;
using NPOI.SS.Formula.Functions;
using SimulationNuitDesLegendes.Personnages;

namespace SimulationNuitDesLegendes;

public class Game
{
    Vouivre _vouivre = null;
    BlackCat? _blackCat = null;
    Robber? _robber = null;
    Monk? _monk = null;
    Bishop? _bishop = null;
    Dinosaur? _dinosaur = null;
    
    private bool _isGameRunning = true;
    
    private Winner _winner = Winner.None;

    public enum Winner
    {
        None,
        Belier,
        Hog,
        Vouivre
    }
    
    // List of all players in the game
    List<Player> players = new List<Player>();
    
    public Game(int vouivreCount, int belierCount, int hogCount, int robberCount, int monkCount, int bishopCount, int blackCatCount, int dinosaurCount)
    {
        // Add all players to the game
        for (int i = 0; i < vouivreCount; i++)
        {
            AddPlayer(new Vouivre());
        }
        
        for (int i = 0; i < belierCount; i++)
        {
            AddPlayer(new Belier());
        }
        
        for (int i = 0; i < hogCount; i++)
        {
            AddPlayer(new Hog());
        }
        
        for (int i = 0; i < robberCount; i++)
        {
            AddPlayer(new Robber());
        }
        
        for (int i = 0; i < monkCount; i++)
        {
            AddPlayer(new Monk());
        }
        
        for (int i = 0; i < bishopCount; i++)
        {
            AddPlayer(new Bishop());
        }
        
        for (int i = 0; i < blackCatCount; i++)
        {
            AddPlayer(new BlackCat());
        }

        for (int i = 0; i < dinosaurCount; i++)
        {
            AddPlayer(new Dinosaur());
        }
        
        if (_vouivre == null)
        {
            throw new Exception("Vouivre is required to play the game");
        }
    }
    
    public Game(List<Player> players)
    {
        // Add all players to the game
        foreach (Player player in players)
        {
            AddPlayer(player);
        }
        
        if (_vouivre == null)
        {
            throw new Exception("Vouivre is required to play the game");
        }
    }
    
    public List<Player> AlivePlayers()
    {
        List<Player> alivePlayers = new List<Player>();
        foreach (var player in players)
        {
            if (player.IsAlive)
            {
                alivePlayers.Add(player);
            }
        }

        return alivePlayers;
    }
    
    public void AddPlayer(Player player)
    {
        players.Add(player);
        
        // switch on class of player
        switch (player)
        {
            case BlackCat blackCat:
                _blackCat = blackCat;
                break;
            case Vouivre vouivre:
                _vouivre = vouivre;
                break;
            case Robber robber:
                _robber = robber;
                break;
            case Monk monk:
                _monk = monk;
                break;
            case Bishop bishop:
                _bishop = bishop;
                break;
        }
    }
    
    public Winner RunGame()
    {
        // Start with night
        
        // First night, if the cat is alive, it can link two players
        if (_blackCat != null && _blackCat.IsAlive)
        {
            _blackCat.Play(players);
        }
        
        bool wasDinosaurAlive = _dinosaur?.IsAlive ?? false;

        do
        {
            // NIGHT
            // The robber plays
            _robber?.Play(AlivePlayers());
            
            // The monk plays
            _monk?.Play(AlivePlayers());
            
            // The hogs play
            Hog.AttackPlayer(AlivePlayers());
            
            // Check if someone won
            Winner winner = CheckForWin();
            if (winner != Winner.None)
            {
                return winner;
            }
            
            // If the dinosaur died, let him play
            if (wasDinosaurAlive && !_dinosaur!.IsAlive)
            {
                _dinosaur!.Play(AlivePlayers());
                wasDinosaurAlive = false;
            }
            
            // Check if someone won
            winner = CheckForWin();
            if (winner != Winner.None)
            {
                return winner;
            }
            
            // DAY
            Player votedPlayer = Vote();
            votedPlayer.Kill(Player.DeathReason.Democracy);
            
            // If the dinosaur died, let him play
            if (wasDinosaurAlive && !_dinosaur!.IsAlive)
            {
                _dinosaur!.Play(AlivePlayers());
                wasDinosaurAlive = false;
            }
            
            // Check if someone won
            winner = CheckForWin();
            if (winner != Winner.None)
            {
                return winner;
            }
            
        } while(_isGameRunning);

        return Winner.None;
    }
    
    private Player Vote()
    {
        // As this is jsut a simulation, the vote is random
        // This system does not consider a possible draw
        // Get a random player
        int randomIndex = new Random().Next(0, players.Count);
        return players[randomIndex];
    }

    // Checks if any win conditions are met
    private Winner CheckForWin()
    {
        int countBelier = 0;
        int countHog = 0;
        bool vouivreAlive = _vouivre.IsAlive;
        
        List<Player> alivePlayers = AlivePlayers();

        foreach (var player in alivePlayers)
        {
            // Get hog count
            if (player.GetType() == typeof(Hog))
            {
                countHog++;
            }
            else if (player.GetType() == typeof(Belier))
            {
                countBelier++;
            }
        }
        
        // Check if the Vouivre is alive
        if (vouivreAlive)
        {
            countHog++;
        }
        
        // Check win
        if (alivePlayers.Count == 2 && vouivreAlive)
        {
            return Winner.Vouivre;
        }
        
        // If there are more or equal Hogs than Beliers, Hogs win
        if (countBelier <= countHog)
        {
            return Winner.Hog;
        } else if (countHog == 0) // If there are no Hogs, Beliers win
        {
            return Winner.Belier;
        }

        return Winner.None;
    }
}