using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player performs a Jump.
    /// </summary>
    /// <typeparam name="PlayerJumped"></typeparam>
    public class PlayerJumped : Simulation.Event<PlayerJumped>
    {
        public PlayerController player;
        public Movement movement;

        public override void Execute()
        {
            if (player.audioSource && player.jumpAudio)
                player.audioSource.PlayOneShot(player.jumpAudio);
            if (movement.audioSource && movement.jumpAudio)
                movement.audioSource.PlayOneShot(movement.jumpAudio);
        }
        
    }
}