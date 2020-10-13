namespace BattleShipGame.ApplicationService
{
    public interface IPlayer
    {
        bool IsFiringMissileCurrently { get; set; }
        int PlayerId { get; set; }
    }
}