using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipGame.ApplicationService
{
    public class Player : IPlayer
    {
      

        public Player()
        {
           
        }
        
        public int PlayerId { get; set; }

        public Boolean IsFiringMissileCurrently { get; set; }
    }
}
