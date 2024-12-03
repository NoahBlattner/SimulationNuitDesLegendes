﻿namespace SimulationNuitDesLegendes.Personnages;

public class Monk : Belier
{
    public override void Play(List<Player> alivePlayers)
    {
        // Protect a random player
        Random random = new Random();
        int randomIndex = random.Next(0, alivePlayers.Count);
        Player playerToProtect = alivePlayers[randomIndex];
        playerToProtect.Protect();
    }
}