using BattleShipGame.ApplicationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace BattleShipGameTest
{
    [TestClass]
    public class BattleShipBoardTest
    {
        private Mock<IBattleBoardService> battleBoardServiceMock;
        private Mock<IBattleShipFactory> battleShipFactoryMock;
        private Mock<IShip> _shipMock;
        private Mock<IShipCoordinate> _shipCoordinatesMock;
        private int _shipLength;

        [TestInitialize]
        public void TestSetup()
        {
            //service Mocks
            battleBoardServiceMock = new Mock<IBattleBoardService>();
            battleShipFactoryMock = new Mock<IBattleShipFactory>();

            _shipLength = 3;
        }

        [TestMethod]
        public void SetShipOnBattleBoardTest()
        {
            //arrange
            var battleBoardService = new BattleBoardService(battleShipFactoryMock.Object);

            MockShip();

            //act

            var result = battleBoardService.SetShipOnBattleBoard('A', 1, _shipLength, "U", _shipMock.Object);

            //assert

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ValidateShipLocationOnBoardTest_PositiveScenario()
        {
            var battleBoardService = new BattleBoardService(battleShipFactoryMock.Object);

            battleBoardService.BoardDimension = 5;

            var result = battleBoardService.ValidateShipLocationOnBoard("R", 'C', 2);
            Assert.AreEqual(true,result);
        }

        [TestMethod]
        public void ValidateShipLocationOnBoardTest_NegativeScenario()
        {
            var battleBoardService = new BattleBoardService(battleShipFactoryMock.Object);

            battleBoardService.BoardDimension = 5;

            var result = battleBoardService.ValidateShipLocationOnBoard("R",'E', 2);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestIfShipHitByMissile()
        {
            var battleBoardService = new BattleBoardService(battleShipFactoryMock.Object);

            MockShip();

            var result = battleBoardService.IsShipHitByMissile('A', 1, _shipMock.Object);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestIfMissileTargetIsMiss()
        {
            var battleBoardService = new BattleBoardService(battleShipFactoryMock.Object);

            MockShip();

            var result = battleBoardService.IsShipHitByMissile('D', 4, _shipMock.Object);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ShipNotSunkTest()
        {
            var battleBoardService = new BattleBoardService(battleShipFactoryMock.Object);

            MockShip();

            var result = battleBoardService.IsShipSunk(_shipMock.Object);
            Assert.AreEqual(false, result);
        }


        [TestMethod]
        public void ShipSunkTest()
        {
            var battleBoardService = new BattleBoardService(battleShipFactoryMock.Object);

            _shipMock = new Mock<IShip>();
            _shipMock.Setup(x => x.ShipId).Returns(1);
            _shipMock.Setup(x => x.ShipLength).Returns(_shipLength);
            _shipMock.Setup(x => x.ShipDirection).Returns(ShipDirection.U);
            var shipCoordinates = new List<IShipCoordinate>();
            _shipMock.Setup(x => x.ShipCoordinates).Returns(shipCoordinates);

            var result = battleBoardService.IsShipSunk(_shipMock.Object);
            Assert.AreEqual(true, result);
        }

        private void MockShip()
        {
            _shipMock = new Mock<IShip>();
            _shipMock.Setup(x => x.ShipId).Returns(1);
            _shipMock.Setup(x => x.ShipLength).Returns(_shipLength);
            _shipMock.Setup(x => x.ShipDirection).Returns(ShipDirection.U);
            var shipCoordinates = new List<IShipCoordinate>();

            for (int i = 1; i <= _shipLength; i++)
            {
                _shipCoordinatesMock = new Mock<IShipCoordinate>();
                _shipCoordinatesMock.Setup(x => x.ShipId).Returns(_shipMock.Object.ShipId);
                _shipCoordinatesMock.Setup(x => x.X).Returns('A');
                _shipCoordinatesMock.Setup(x => x.Y).Returns(i);
                shipCoordinates.Add(_shipCoordinatesMock.Object);
            }
            _shipMock.Setup(x => x.ShipCoordinates).Returns(shipCoordinates);
        }

    }
}

