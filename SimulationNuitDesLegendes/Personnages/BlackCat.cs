namespace SimulationNuitDesLegendes.Personnages;

public class BlackCat : Belier
{
    
    public override void Play(List<Player> alivePlayers)
    {
        // Link two players 
        // Get two random different players
        int randomIndex1 = new Random().Next(0, alivePlayers.Count);
        int randomIndex2 = new Random().Next(0, alivePlayers.Count);
        while (randomIndex1 == randomIndex2)
        {
            randomIndex2 = new Random().Next(0, alivePlayers.Count);
        }
            
        // Get the two players
        Player player1 = alivePlayers[randomIndex1];
        Player player2 = alivePlayers[randomIndex2];
            
        // Link the two players
        player1.LinkedPlayer = player2;
        player2.LinkedPlayer = player1;
    }
}