using Garage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Tests.EntitiesTests
{
    public class CarTests
    {
        private static Car GetCar() => new Car(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.FuelType);

        [Fact]
        public void GetPropFuelType_Null_ReturnsEmptyString()
        {
            string fuelType = null!;
            string expected = "";
            Car car = GetCar();
            car.FuelType = fuelType;

            string actual = car.FuelType;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropFuelType_OnlyWhiteSpaces_ReturnsEmptyString()
        {
            string fuelType = " ";
            string expected = "";
            Car car = GetCar();
            car.FuelType = fuelType;

            string actual = car.FuelType;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropFuelType_NotEmptyString_ReturnsLowerString()
        {
            string fuelType = "DIESEL";
            string expected = fuelType.ToLower();
            Car car = GetCar();
            car.FuelType = fuelType;

            string actual = car.FuelType;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Equals_EqualCar_ReturnsTrue()
        {
            Assert.True(GetCar().Equals(GetCar()));
        }

        [Theory]
        [InlineData(Vehicles.DifferentRegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.FuelType)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.DifferentColor, Vehicles.Wheels, Vehicles.FuelType)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels + 1, Vehicles.FuelType)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels, Vehicles.DifferentFuelType)]
        public void Equals_NotEqualCar_ReturnsFalse(string registrationNumber, string color, int wheels, string fuelType)
        {
            Assert.False(GetCar().Equals(new Car(registrationNumber, color, wheels, fuelType)));
        }
    }
}
