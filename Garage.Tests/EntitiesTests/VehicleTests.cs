using Garage.Entity;

namespace Garage.Tests.EntitiesTests
{
    public class VehicleTests
    {
        private static Vehicle GetVehicle() => new Vehicle(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels);


        [Fact]
        public void SetPropRegistrationNumber_Null_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => GetVehicle().RegistrationNumber = null!);
        }

        [Fact]
        public void SetPropRegistrationNumber_EmptyString_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => GetVehicle().RegistrationNumber = "");
        }

        [Fact]
        public void SetPropRegistrationNumber_OnlyWhiteSpaces_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => GetVehicle().RegistrationNumber = " ");
        }

        [Fact]
        public void GetPropRegistrationNumber_NotEmptyString_ReturnsUpperString()
        {
            string registrationNumber = "abc123";
            string expected = registrationNumber.ToUpper();
            Vehicle vehicle = GetVehicle();
            vehicle.RegistrationNumber = registrationNumber;

            string actual = vehicle.RegistrationNumber;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropColor_Null_ReturnsEmptyString()
        {
            string color = null!;
            string expected = "";
            Vehicle vehicle = GetVehicle();
            vehicle.Color = color;

            string actual = vehicle.Color;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropColor_OnlyWhiteSpaces_ReturnsEmptyString()
        {
            string color = " ";
            string expected = "";
            Vehicle vehicle = GetVehicle();
            vehicle.Color = color;

            string actual = vehicle.Color;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropColor_NotEmptyString_ReturnsLowerString()
        {
            string color = "MAGENTA";
            string expected = color.ToLower();
            Vehicle vehicle = GetVehicle();
            vehicle.Color = color;

            string actual = vehicle.Color;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetPropWheels_NegativeNumber_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => GetVehicle().Wheels = -1);
        }

        [Fact]
        public void Equals_EqualVehicle_ReturnsTrue()
        {
            Assert.True(GetVehicle().Equals(GetVehicle()));
        }

        [Fact]
        public void Equals_EqualVehicleButSubClass_ReturnsFalse()
        {
            var tempVehicle = GetVehicle();
            var otherVehicle = new Car(tempVehicle.RegistrationNumber, tempVehicle.Color, tempVehicle.Wheels, "Gas");

            Assert.False(GetVehicle().Equals(otherVehicle));
        }

        [Theory]
        [InlineData(Vehicles.DifferentRegistrationNumber, Vehicles.Color, Vehicles.Wheels)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.DifferentColor, Vehicles.Wheels)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels + 1)]
        public void Equals_NotEqualVehicle_ReturnsFalse(string registrationNumber, string color, int wheels)
        {
            Assert.False(GetVehicle().Equals(new Vehicle(registrationNumber, color, wheels)));
        }

        [Fact]
        public void Equals_EqualVehicleAsObject_ReturnsTrue()
        {
            Assert.True(GetVehicle().Equals((object)GetVehicle()));
        }

        [Theory]
        [InlineData(Vehicles.DifferentRegistrationNumber, Vehicles.Color, Vehicles.Wheels)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.DifferentColor, Vehicles.Wheels)]
        [InlineData(Vehicles.RegistrationNumber, Vehicles.Color, Vehicles.Wheels + 1)]
        public void Equals_NotEqualVehicleAsObjects_ReturnsFalse(string registrationNumber, string color, int wheels)
        {
            Assert.False(GetVehicle().Equals((object)new Vehicle(registrationNumber, color, wheels)));
        }
    }
}
