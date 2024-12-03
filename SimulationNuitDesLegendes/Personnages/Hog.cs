namespace SimulationNuitDesLegendes.Personnages;

public class Hog : Player
{
    public static void AttackPlayer(List<Player> alivePlayers)
    {
        // Get only the non-hog and non-vouivre hog players
        List<Player> nonHogPlayers = new List<Player>();
        foreach (var player in alivePlayers)
        {
            if (player.GetType() != typeof(Hog)
                && player.GetType() != typeof(Vouivre))
            {
                nonHogPlayers.Add(player);
            }
        }
        
        // Get a random player
        int randomIndex = new Random().Next(0, nonHogPlayers.Count);
        Player attackedPlayer = nonHogPlayers[randomIndex];
        
        // Try to kill the player
        attackedPlayer.Kill(DeathReason.Hogs);
        
        Logger.Log($"The hogs attacked a {attackedPlayer.GetType().Name}\n");
    }
    
    public override void Play(List<Player> alivePlayers)
    {
        // Hogs play in group
        return;
    }
}