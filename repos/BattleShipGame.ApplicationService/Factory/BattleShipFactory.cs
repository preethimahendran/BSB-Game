using System;
using System.Collections.Generic;
using BattleShipGame.ApplicationService;

namespace BattleShipGame.ApplicationService
{
    public class BattleShipFactory : IBattleShipFactory
    {
        /// <summary>
        /// Creates Board and assigns players to their board
        /// </summary>
        /// <param name="noOfPlayers"></param>
        /// <param name="boardDimension"></param>
        /// <returns>List of created boards</returns>
        public IEnumerable<BattleBoard> CreateBattleBoards(int noOfPlayers, int boardDimension)
        {
            try
            {
                List<BattleBoard> battleBoards = new List<BattleBoard>();
                for (int i = 1; i <= noOfPlayers; i++)
                {
                    Player player = new Player
                    {
                        PlayerId = i
                    };
                    Ship ship = new Ship();
                    
                    BattleBoard battleBoard = new BattleBoard(player, ship)
                    {
                        BoardId = i,
                        BoardDimension = boardDimension
                    };
                    battleBoards.Add(battleBoard);
                }

                return battleBoards;
            }
            catch (Exception)
            {
                //TODO Handle this
                throw;
            }
        }
        /// <summary>
        /// Add Ship Coordinates to ship
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ship"></param>
        public void AddShipCoordinate(char x, int y, IShip ship)
        {
            ship.ShipCoordinates.Add(new ShipCoordinate() { X = x, Y = y, ShipId = ship.ShipId });
        }
    }
}
