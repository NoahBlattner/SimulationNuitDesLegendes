namespace SimulationNuitDesLegendes.Personnages;

public class Monk : Belier
{
    List<Player> _protectedPlayers = new List<Player>();
    
    public override void Play(List<Player> alivePlayers)
    {
        // Remove already protected players
        foreach (var protectedPlayer in _protectedPlayers)
        {
            alivePlayers.Remove(protectedPlayer);
        }
        
        if (alivePlayers.Count == 0)
        {
            Logger.Log("The monk can no longer protect anyone\n");
            return;
        }
        
        // Protect a random player
        Random random = new Random();
        int randomIndex = random.Next(0, alivePlayers.Count);
        Player playerToProtect = alivePlayers[randomIndex];
        playerToProtect.Protect();
        
        // Remember the protected player
        _protectedPlayers.Add(playerToProtect);
        
        Logger.Log($"The monk protected a {playerToProtect.GetType().Name}\n");
    }
}