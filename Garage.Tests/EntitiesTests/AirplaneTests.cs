using Garage.Entity;

namespace Garage.Tests.EntitiesTests
{
    public class AirplaneTests
    {
        private static Airplane GetAirplane() => new Airplane(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.NumberOfEngines);

        [Fact]
        public void SetPropNumberOfEngines_LessThan1_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => GetAirplane().NumberOfEngines = 0);
        }

        [Fact]
        public void Equals_EqualAirplane_ReturnsTrue()
        {
            Assert.True(GetAirplane().Equals(GetAirplane()));
        }

        [Theory]
        [InlineData(Vehicles.DifferentRegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.NumberOfEngines)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.DifferentColor, Vehicles.Wheels, Vehicles.NumberOfEngines)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels + 1, Vehicles.NumberOfEngines)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.NumberOfEngines + 1)]
        public void Equals_NotEqualAirplane_ReturnsFalse(string registrationNumber, string color, int wheels, int numberofEngines)
        {
            Assert.False(GetAirplane().Equals(new Airplane(registrationNumber, color, wheels, numberofEngines)));
        }
    }
}
