namespace SimulationNuitDesLegendes.Personnages;

public class Robber : Belier
{
    public override void Play(List<Player> alivePlayers)
    {
        // Get a random player
        int randomIndex = new Random().Next(0, alivePlayers.Count);
        Player player = alivePlayers[randomIndex];
        
        // If the chosen player is the Vouivre, the Robber dies
        if (player.GetType() == typeof(Vouivre))
        {
            Kill(DeathReason.Caught);
        }
    }
}