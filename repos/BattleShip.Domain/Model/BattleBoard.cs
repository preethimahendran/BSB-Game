namespace BattleShipGame.ApplicationService
{
    public class BattleBoard : IBattleBoard
    {
        private static int nextBoardId = 0;
        public readonly IPlayer Player;
        public readonly IShip Ship;

        public BattleBoard(IPlayer player, IShip ship)
        {
            Player = player;
            Ship = ship;
        }

        //TODO: convert this to auto increment field using static counter
        public int BoardId { get; set; }
        public int BoardDimension { get; set; }
        public int BoardWidth { get; set; }

     



    }
}
