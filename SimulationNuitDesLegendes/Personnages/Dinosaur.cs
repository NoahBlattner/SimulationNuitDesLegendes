namespace SimulationNuitDesLegendes.Personnages;

public class Dinosaur : Belier
{
    public override void Play(List<Player> alivePlayers)
    {
        // Kill a random player
        Random random = new Random();
        int randomIndex = random.Next(0, alivePlayers.Count);
        Player playerToKill = alivePlayers[randomIndex];
        playerToKill.Kill(DeathReason.Eaten);
        
        Logger.Log($"As the dinosaur was killed, he ate a {playerToKill.GetType().Name}\n");
    }
}