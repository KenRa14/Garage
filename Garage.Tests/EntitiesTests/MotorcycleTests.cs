using Garage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Tests.EntitiesTests
{
    public class MotorcycleTests
    {
        private static Motorcycle GetMotorcycle() => new Motorcycle(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.CylinderVolume);

        [Fact]
        public void SetPropCylinderVolume_LessThan1_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => GetMotorcycle().CylinderVolume = 0);
        }

        [Fact]
        public void Equals_EqualMotorcycle_ReturnsTrue()
        {
            Assert.True(GetMotorcycle().Equals(GetMotorcycle()));
        }

        [Theory]
        [InlineData(Vehicles.DifferentRegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.CylinderVolume)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.DifferentColor, Vehicles.Wheels, Vehicles.CylinderVolume)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels + 1, Vehicles.CylinderVolume)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.CylinderVolume + 1)]
        public void Equals_NotEqualMotorcycle_ReturnsFalse(string registrationNumber, string color, int wheels, int cylinderVolume)
        {
            Assert.False(GetMotorcycle().Equals(new Motorcycle(registrationNumber, color, wheels, cylinderVolume)));
        }
    }
}
