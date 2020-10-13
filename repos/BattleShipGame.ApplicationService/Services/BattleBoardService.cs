using BattleShipGame.ApplicationService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShipGame.ApplicationService
{
    public class BattleBoardService : IBattleBoardService
    {
        private readonly IBattleShipFactory _battleShipFactory;

        public BattleBoardService(IBattleShipFactory battleShipFactory)
        {
            _battleShipFactory = battleShipFactory;
        }

        List<BattleBoard> _battleBoardCollection = new List<BattleBoard>();

        public int BoardDimension { get; set; }

        /// <summary>
        /// Validates Game Board dimension against game limits
        /// </summary>
        /// <param name="boarddimension">User Input</param>
        /// <param name="gameBoardSizeLowerLimit">set in UI layer</param>
        /// <param name="gameBoardSizeUpperLimit">set in UI layer</param>
        /// <returns></returns>
        public Boolean IsBoardSizeValid(int boarddimension, int gameBoardSizeLowerLimit, int gameBoardSizeUpperLimit)
        {
            var isBoardSizeValid = boarddimension >= gameBoardSizeLowerLimit && boarddimension <= gameBoardSizeUpperLimit;
            return isBoardSizeValid;
        }

        /// <summary>
        /// Creates battle boards from factory 
        /// </summary>
        /// <param name="boardDimension">User Input</param>
        /// <param name="noOfPlayers">must be set from the UI layer</param>
        public void CreateBoard( int boardDimension, int noOfPlayers)
        {
            //write unit test
            _battleBoardCollection = _battleShipFactory.CreateBattleBoards(noOfPlayers, boardDimension).ToList();
        }

        /// <summary>
        /// Set current boards ship for service
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="boardId"></param>
        public IShip GetCurrentPlayerShipOnBattleBoard(int playerId)
        {
            //write an extension method for the string conv
            //write an unit test for this
            return _battleBoardCollection.Where(x=>x.Player.PlayerId == playerId).Select(x => x.Ship).FirstOrDefault();
        }

        /// <summary>
        /// Set Ship on Battle Board 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool SetShipOnBattleBoard(char x, int y, int shipLength, string shipDirection, IShip currentShip)
        {
            var isShipSetupSuccessfull = false;
            try
            {
                currentShip.ShipLength = shipLength;

                if (Enum.TryParse(shipDirection, out ShipDirection shipDirectionTemp))
                {
                    currentShip.ShipDirection = shipDirectionTemp;
                }
                else return isShipSetupSuccessfull;

                if (currentShip.ShipLength != 0)
                {   
                    _battleShipFactory.AddShipCoordinate(x, y, currentShip);
                    var yCoordinate = y;
                    var xCoordinate = x;

                    while (currentShip.ShipCoordinates.Count() < currentShip.ShipLength)
                    {
                        //vertical
                        if (currentShip.ShipDirection == ShipDirection.U || currentShip.ShipDirection == ShipDirection.D)
                        {
                            if (currentShip.ShipDirection == ShipDirection.U)
                            {   
                                _battleShipFactory.AddShipCoordinate(x, ++yCoordinate, currentShip);
                            }
                            else
                            {
                                _battleShipFactory.AddShipCoordinate(x, --yCoordinate, currentShip);
                            }
                        }
                        //horizontal
                        else
                        {
                            if (currentShip.ShipDirection == ShipDirection.R)
                            {
                                xCoordinate = xCoordinate.NextAlphabet();
                                _battleShipFactory.AddShipCoordinate(xCoordinate, y, currentShip);
                            }
                            else
                            {
                                xCoordinate = xCoordinate.PreviousAlphabet();
                                _battleShipFactory.AddShipCoordinate(xCoordinate, y, currentShip);
                            }
                        }
                    }
                    isShipSetupSuccessfull = true;
                }
            }
            catch (Exception)
            {
                //TODO: Handle this - Add log
                isShipSetupSuccessfull = false;
                
            }
            return isShipSetupSuccessfull;
        }

        public bool ValidateShipDirection(string userInput)
        {
            var isShipDirectionValid = Enum.IsDefined(typeof(ShipDirection), userInput);
            return isShipDirectionValid;
        }
        public bool ValidateShipLocationOnBoard(string shipDirection, char xCoordinate, int yCoordinate)
        {
            var isShipLocationValid = false;
            var boardOrigin = 1;

            try
            {
                var isShipCoordinateValid = yCoordinate <= BoardDimension && xCoordinate.ConvertCharToNumber() <= BoardDimension;

                if (isShipCoordinateValid)
                {
                    if (shipDirection == ShipDirection.R.ToString())
                    {
                        isShipLocationValid = xCoordinate.ConvertCharToNumber() + 2 <= BoardDimension;
                    }
                    else if (shipDirection == ShipDirection.L.ToString())
                    {
                        isShipLocationValid = xCoordinate.ConvertCharToNumber() - 2 >= boardOrigin;
                    }
                    else if (shipDirection == ShipDirection.U.ToString())
                    {
                        isShipLocationValid = yCoordinate + 2 <= BoardDimension;
                    }
                    else if (shipDirection == ShipDirection.D.ToString())
                    {
                        isShipLocationValid = yCoordinate - 2 >= boardOrigin;
                    }
                }
                else isShipLocationValid = isShipCoordinateValid;
            }
            catch (Exception)
            {
                isShipLocationValid = false;
                //TODO: Handle this better with logging
            }

            return isShipLocationValid;
        }

        public bool ValidateMissileCoordinates(char x, int y)
        {
            var xCoordinateNumber = x.ConvertCharToNumber();
            return xCoordinateNumber <= BoardDimension && y <= BoardDimension;
        }

        public bool IsShipHitByMissile(char x, int y, IShip currentShip)
        {
            var isShipHitByMissile = false;
            try
            {
                var ShipHitByMissile = currentShip.ShipCoordinates.SingleOrDefault(shipCoordinate => shipCoordinate.X == x && shipCoordinate.Y == y);
                if (ShipHitByMissile != null)
                {
                    currentShip.ShipCoordinates.Remove(ShipHitByMissile);
                    isShipHitByMissile = true;
                }
                return isShipHitByMissile;
            }
            catch (Exception)
            {
                //TODO: Handle this better with logging
                isShipHitByMissile = false;
            }

            return isShipHitByMissile;
        }

        public bool IsShipSunk(IShip currentShip)
        {
            return currentShip.ShipCoordinates.Count() == 0;
        }


    }
}
