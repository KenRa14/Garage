using Garage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Tests.EntitiesTests
{
    public class BoatTests
    {
        private static Boat GetBoat() => new Boat(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.Length);

        [Fact]
        public void SetPropLength_LessThan1_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => GetBoat().Length = 0);
        }

        [Fact]
        public void Equals_EqualBoat_ReturnsTrue()
        {
            Assert.True(GetBoat().Equals(GetBoat()));
        }

        [Theory]
        [InlineData(Vehicles.DifferentRegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.Length)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.DifferentColor, Vehicles.Wheels, Vehicles.Length)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels + 1, Vehicles.Length)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.Length + 1)]
        public void Equals_NotEqualBoat_ReturnsFalse(string registrationNumber, string color, int wheels, int length)
        {
            Assert.False(GetBoat().Equals(new Boat(registrationNumber, color, wheels, length)));
        }
    }
}
