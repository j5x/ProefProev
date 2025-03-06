using Platformer.Core;
using Platformer.Mechanics;
using Player;

namespace Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerLanded"></typeparam>
    public class PlayerLanded : Simulation.Event<PlayerLanded>
    {
        public PlayerController player;
        public Movement movement;
        

        public override void Execute()
        {

        }
    }
}