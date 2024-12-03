namespace SimulationNuitDesLegendes.Personnages;

public abstract class Player
{
    public enum DeathReason
    {
        Hogs,
        Democracy,
        Curse,
        Caught,
        Eaten
    }

    private bool _isProtected = false;
    public bool IsAlive = true;
    
    public Player? LinkedPlayer = null;

    public abstract void Play(List<Player> alivePlayers);
    
    public void Protect()
    {
        _isProtected = true;
    }
    
    public void Kill(DeathReason deathReason)
    {
        // The hogs can't kill a protected player
        if (_isProtected && deathReason == DeathReason.Hogs)
        {
            Logger.Log($"{this.GetType().Name} is protected and survived the hogs\n");
            return;
        }

        if (!IsAlive)
        {
            throw new InvalidOperationException("Can't kill a dead player");
        }
        
        IsAlive = false;
        
        // If the player is linked to another player, kill the other player
        // Except if the other player is the Vouivre
        if (LinkedPlayer != null)
        {
            if (LinkedPlayer is Vouivre)
            {
                // If the linked player is the Vouivre
                // She will not be killed and her link to this player is severed
                LinkedPlayer.LinkedPlayer = null;

                Logger.Log($"As {GetType().Name} was killed, the Vouivre survived the black magic linking them");
            }
            else // Else, kill the linked player
            {
                // Avoid infinite loop
                LinkedPlayer.LinkedPlayer = null;

                // Kill the linked player
                LinkedPlayer.Kill(DeathReason.Curse);
                Logger.Log(
                    $"As {GetType().Name} was killed, a {LinkedPlayer.GetType().Name} died by the black magic linking them"); 
                LinkedPlayer = null;
            }
        }
    }
}