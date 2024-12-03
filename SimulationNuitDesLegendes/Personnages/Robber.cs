namespace SimulationNuitDesLegendes.Personnages;

public class Robber : Belier
{
    public override void Play(List<Player> alivePlayers)
    {
        // Get a random player
        Player player;
        do
        {
            int randomIndex = new Random().Next(0, alivePlayers.Count);
            player = alivePlayers[randomIndex];
        } while (player == this);
        
        Logger.Log($"The robber chose to see {player.GetType().Name}'s identity\n");
        
        // If the chosen player is the Vouivre, the Robber dies
        if (player.GetType() == typeof(Vouivre))
        {
            Kill(DeathReason.Caught);
        }
    }
}