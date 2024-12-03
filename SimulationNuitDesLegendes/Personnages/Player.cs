namespace SimulationNuitDesLegendes.Personnages;

public abstract class Player
{
    public enum DeathReason
    {
        Hogs,
        Vote,
        Curse,
        Caught
    }
    
    protected bool IsProtected = false;
    public bool IsAlive = true;
    
    public Player? LinkedPlayer = null;

    public abstract void Play(List<Player> alivePlayers);
    
    public void Kill(DeathReason deathReason)
    {
        // The hogs can't kill a protected player
        if (IsProtected && deathReason == DeathReason.Hogs)
        {
            return;
        }
        
        IsAlive = false;
        // If the player is linked to another player, kill the other player
        // Except if the other player is the Vouivre
        if (LinkedPlayer != null
            && LinkedPlayer.GetType() != typeof(Vouivre))
        {
            // Avoid infinite loop
            LinkedPlayer.LinkedPlayer = null;
        
            // Kill the linked player
            LinkedPlayer.Kill(DeathReason.Curse);
            LinkedPlayer = null;
        }
    }
}