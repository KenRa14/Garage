using Garage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Tests.EntitiesTests
{
    public class BusTests
    {
        private static Bus GetBus() => new Bus(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.NumberOfSeats);

        [Fact]
        public void SetPropNumberOfSeats_LessThan1_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => GetBus().NumberOfSeats = 0);
        }

        [Fact]
        public void Equals_EqualBus_ReturnsTrue()
        {
            Assert.True(GetBus().Equals(GetBus()));
        }

        [Theory]
        [InlineData(Vehicles.DifferentRegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.NumberOfSeats)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.DifferentColor, Vehicles.Wheels, Vehicles.NumberOfSeats)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels + 1, Vehicles.NumberOfSeats)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.NumberOfSeats + 1)]
        public void Equals_NotEqualBus_ReturnsFalse(string registrationNumber, string color, int wheels, int numberOfSeats)
        {
            Assert.False(GetBus().Equals(new Bus(registrationNumber, color, wheels, numberOfSeats)));
        }
    }
}
