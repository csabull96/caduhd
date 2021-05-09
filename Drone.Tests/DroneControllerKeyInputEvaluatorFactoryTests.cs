using Caduhd.Controller;
using Caduhd.Controller.Command;
using Caduhd.Controller.InputEvaluator;
using Caduhd.Drone;
using Caduhd.Drone.Dji;
using System;
using Xunit;

namespace Drone.Tests
{
    public class DroneControllerKeyInputEvaluatorFactoryTests
    {
        class FakeDrone : IControllableDrone
        {
            public void Control(DroneCommand droneCommant)
            {
                throw new NotImplementedException();
            }
        }

        private DroneControllerKeyInputEvaluatorFactory _factory;
        public DroneControllerKeyInputEvaluatorFactoryTests()
        {
            _factory = new DroneControllerKeyInputEvaluatorFactory();
        }

        [Fact]
        public void GetDroneControllerKeyInputEvaluator_InputIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _factory.GetDroneControllerKeyInputEvaluator(null));
        }

        [Fact]
        public void GetDroneControllerKeyInputEvaluator_InputIsTello_ReturnsTelloKeyInputEvaluator()
        {
            Assert.IsType<TelloKeyInputEvaluator>(_factory.GetDroneControllerKeyInputEvaluator(new Tello()));
        }

        [Fact]
        public void GetDroneControllerKeyInputEvaluator_InputIsNotTello_ReturnsGeneralDroneKeyInputEvaluator()
        {
            Assert.IsType<GeneralDroneKeyInputEvaluator>(_factory.GetDroneControllerKeyInputEvaluator(new FakeDrone()));
        }
    }
}
