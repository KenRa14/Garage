using Garage.Entity;
using Garage.Tests.Helpers;

namespace Garage.Tests
{
    public class GarageTests
    {
        private static IEnumerable<IVehicle> CreateVehicles()
        {
            return new IVehicle[]
                {
                    new Boat("DLA138", "pink", 5, 25) ,
                    new Motorcycle("PPX154", "pink", 4, 64),
                    new Car("BMR676", "Orange", 3, "Diesel")
                };
        }
        [Fact]
        public void SetVehicle_VehicleWhereIsNull_ReturnsTrue()
        {
            Garage<IVehicle> garage = new Garage<IVehicle>(1);

            bool actual = garage.SetVehicle(0, Vehicles.GetAVehicleOf10(0));

            Assert.True(actual);
        }

        [Fact]
        public void SetVehicle_VehicleWhereIsNull_VehicleCountIncreases()
        {
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            int expected = 1;
            
            garage.SetVehicle(0, Vehicles.GetAVehicleOf10(0));
            int actual = garage.VehicleCount;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetVehicle_VehicleWhereIsNotNull_ReturnsFalse()
        {
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            garage.SetVehicle(0, Vehicles.GetAVehicleOf10(0));

            bool actual = garage.SetVehicle(0, Vehicles.GetAVehicleOf10(0));

            Assert.False(actual);
        }

        [Fact]
        public void SetVehicle_VehicleWhereIsNotNull_VehicleCountStaysTheSame()
        {
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            garage.SetVehicle(0, Vehicles.GetAVehicleOf10(0));
            int expected = garage.VehicleCount;

            garage.SetVehicle(0, Vehicles.GetAVehicleOf10(0));
            int actual = garage.VehicleCount;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetVehicle_NullWhereIsNull_ReturnsFalse()
        {
            Garage<IVehicle> garage = new Garage<IVehicle>(1);

            bool actual = garage.SetVehicle(0, null);

            Assert.False(actual);
        }

        [Fact]
        public void SetVehicle_NullWhereIsNull_VehicleCountStaysTheSame()
        {
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            int expected = 0;

            garage.SetVehicle(0, null);
            int actual = garage.VehicleCount;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetVehicle_NullWhereIsNotNull_ReturnsTrue()
        {
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            garage.SetVehicle(0, Vehicles.GetAVehicleOf10(0));

            bool actual = garage.SetVehicle(0, null);

            Assert.True(actual, null);
        }

        [Fact]
        public void SetVehicle_NullWhereIsNotNull_VehicleCountDecreases()
        {
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            garage.SetVehicle(0, Vehicles.GetAVehicleOf10(0));
            int expected = garage.VehicleCount - 1;

            garage.SetVehicle(0, null);
            int actual = garage.VehicleCount;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_CapacityLessThan1_ThrowsException()
        {
            int capacity = 0;

            Assert.Throws<ArgumentException>(() => new Garage<IVehicle>(capacity));
        }

        [Fact]
        public void Constructor_CapacityLessThanVehicles_ThrowsException()
        {
            IEnumerable<IVehicle> vehicles = Vehicles.GetVehicles10();

            Assert.Throws<ArgumentException>(() => new Garage<IVehicle>(1, vehicles));
        }

        [Fact]
        public void Constructor_CapacityEqualToVehicles_AsEnumerableIsEqualToVehicles()
        {
            IEnumerable<IVehicle> vehicles = Vehicles.GetVehicles10();

            IEnumerable<IVehicle> actual = new Garage<IVehicle>(10, vehicles);

            Assert.Equal(vehicles, actual);
        }

        [Fact]
        public void Constructor_OnlyVehicles_AsEnumerableIsEqualToVehicles()
        {
            IEnumerable<IVehicle> vehicles = Vehicles.GetVehicles10();

            IEnumerable<IVehicle> actual = new Garage<IVehicle>(vehicles);

            Assert.Equal(vehicles, actual);
        }

        [Fact]
        public void Equals_EqualGarage_ReturnsTrue()
        {

            var garage1 = new Garage<IVehicle>(CreateVehicles(), "garage 1");
            var garage2 = new Garage<IVehicle>(CreateVehicles(), "garage 1");

            Assert.True(garage1.Equals(garage2));
        }

        [Fact]
        public void Equals_EqualGarageAsObject_ReturnsTrue()
        {

            var garage1 = new Garage<IVehicle>(CreateVehicles(), "garage 1");
            object objectGarage2 = new Garage<IVehicle>(CreateVehicles(), "garage 1");

            Assert.True(garage1.Equals(objectGarage2));
        }

        [Fact]
        public void Equals_GarageWithDifferentName_ReturnsFalse()
        {

            var garage1 = new Garage<IVehicle>(CreateVehicles(), "garage 1");
            var garage2 = new Garage<IVehicle>(CreateVehicles(), "garage 2");

            Assert.False(garage1.Equals(garage2));
        }

        [Fact]
        public void Equals_GarageWithDifferentVehicles_ReturnsFalse()
        {

            var garage1 = new Garage<IVehicle>(CreateVehicles(), "garage 1");
            var garage2 = new Garage<IVehicle>(CreateVehicles(), "garage 1");
            garage2.SetVehicle(1, null);
            garage2.SetVehicle(1, new Airplane("DVI742", "Dark blue", 7, 1));

            Assert.False(garage1.Equals(garage2));
        }
    }
}