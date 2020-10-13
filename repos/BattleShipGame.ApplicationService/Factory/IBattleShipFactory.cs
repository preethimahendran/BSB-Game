using BattleShipGame.ApplicationService;
using System.Collections.Generic;

namespace BattleShipGame.ApplicationService
{
    public interface IBattleShipFactory
    {
        IEnumerable<BattleBoard> CreateBattleBoards(int noOfPlayers, int boardDimension);
        void AddShipCoordinate(char x, int y, IShip ship);
    }
}