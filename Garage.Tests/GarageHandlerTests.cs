using Garage.Entity;
using Garage.Helpers;
using Garage.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Tests
{
    public class GarageHandlerTests
    {
        private static GarageHandler GetGarageHandler(int garageCapacity = 1)
        {
            GarageHandler handler = new();
            handler.CreateGarage(garageCapacity);
            return handler;
        }

        private static GarageHandler GetGarageHandler(IEnumerable<IVehicle> vehicles)
        {
            GarageHandler handler = new();
            handler.CreateGarage(vehicles);
            return handler;
        }

        private static readonly GarageHandler findHandler = new GarageHandler();
        private static GarageHandler GetFindGarageHandler10Vehicles()
        {
            if (findHandler.GarageCount == 0)
            {
                findHandler.CreateGarage(Vehicles.GetVehicles10());
            }

            return findHandler;
        }

        [Fact]
        public void AddGarage_UponFirstTime_GarageShouldBeSetAsCurrentGarage()
        {
            var handler = new GarageHandler();
            IGarage<IVehicle> garage = new Garage<IVehicle>(1);
            IGarage<IVehicle> expected = garage;

            handler.AddGarage(garage);
            IGarage<IVehicle> actual = handler.CurrentGarage;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void AddGarage_AfterFirsTime_GarageShouldNotBeSetAsCurrentGarage()
        {
            var handler = new GarageHandler();
            IGarage<IVehicle> firstGarage = new Garage<IVehicle>(1);
            IGarage<IVehicle> secondGarage = new Garage<IVehicle>(1);
            handler.AddGarage(firstGarage);
            IGarage<IVehicle> expected = firstGarage;

            handler.AddGarage(secondGarage);
            IGarage<IVehicle> actual = handler.CurrentGarage;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void AddGarage_NullArgument_ThrowsException()
        {
            var handler = new GarageHandler();

            Assert.Throws<ArgumentNullException>(() => handler.AddGarage(null!));
        }

        [Fact]
        public void AddVehicle_IndexWhereNull_ReturnsTrue()
        {
            var handler = GetGarageHandler(garageCapacity: 1);
            IVehicle vehicle = Vehicles.GetAVehicleOf10(0);

            bool actual = handler.AddVehicle(0, vehicle);

            Assert.True(actual);
        }

        [Fact]
        public void AddVehicle_IndexWhereNotNull_ReturnsFalse()
        {
            var handler = GetGarageHandler(garageCapacity: 1);
            handler.AddVehicle(0, Vehicles.GetAVehicleOf10(0));

            bool actual = handler.AddVehicle(0, Vehicles.GetAVehicleOf10(0));

            Assert.False(actual);
        }

        [Fact]
        public void AddVehicle_NullVehicle_ThrowsException()
        {
            var handler = GetGarageHandler(garageCapacity: 1);

            Assert.Throws<ArgumentNullException>(() => handler.AddVehicle(0, null!));
        }

        [Theory]
        [InlineData("MCV453", 0)]
        [InlineData("YUJ867", 9)]
        [InlineData("MWW835", 7)]
        [InlineData("GUV034", 4)]
        [InlineData("UVC514", 2)]
        public void FindVehicleParkingIndex_VehicleFound_ReturnsCorrectIndex(string registrationNumber, int expected)
        {
            var handler = GetFindGarageHandler10Vehicles();

            int actual = handler.FindVehicleParkingIndex(registrationNumber);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FindVehicleParkingIndex_VehicleNotFound_ReturnsNegative1()
        {
            var handler = GetFindGarageHandler10Vehicles();
            int expected = -1;

            int actual = handler.FindVehicleParkingIndex("Craps");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAvailablePlaces_WhenGarageNotFull_ReturnsIndexEnumerableWhereNull()
        {
            GarageHandler garageHandler = GetGarageHandler(Vehicles.GetVehicles10());
            garageHandler.RemoveVehicle(3);
            garageHandler.RemoveVehicle(5);
            IEnumerable<int> expected = new int[]{ 3, 5}.AsEnumerable();

            IEnumerable<int> actual = garageHandler.GetAvailablePlaces();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAvailablePlaces_WhenGarageIsFull_ReturnsEmptyIndexEnumerable()
        {
            GarageHandler garageHandler = GetGarageHandler(Vehicles.GetVehicles10());

            IEnumerable<int> actual = garageHandler.GetAvailablePlaces();

            Assert.Empty(actual);
        }

        [Fact]
        public void GetNotAvailablePlaces_WhenGarageNotFull_ReturnsIndexEnumerableWhereNotNull()
        {
            GarageHandler garageHandler = GetGarageHandler(Vehicles.GetVehicles10());
            garageHandler.RemoveVehicle(3);
            garageHandler.RemoveVehicle(5);
            IEnumerable<int> expected = new int[] { 0, 1, 2, 4, 6, 7, 8, 9 };

            IEnumerable<int> actual = garageHandler.GetNotAvailablePlaces();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNotAvailablePlaces_WhenGarageIsFull_ReturnsIndexEnumerableWithEveryIndex()
        {
            GarageHandler garageHandler = GetGarageHandler(Vehicles.GetVehicles10());
            IEnumerable<int> expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            IEnumerable<int> actual = garageHandler.GetNotAvailablePlaces();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetCurrentGarage_IndexOfOtherGarage_CurrentGarageShouldBeOtherGarage()
        {
            GarageHandler garageHandler = new GarageHandler();
            garageHandler.CreateGarage(capacity: 1);
            garageHandler.CreateGarage(capacity: 1);
            IGarage<IVehicle> expected = garageHandler.GetGarage(1);
            
            garageHandler.SetCurrentGarage(1);
            IGarage<IVehicle> actual = garageHandler.CurrentGarage;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RemoveGarage_WhenAllOfTheGarageRemoved_CurrentGarageShouldBeNull()
        {
            GarageHandler garageHandler = new GarageHandler();
            garageHandler.CreateGarage(capacity: 1);
            garageHandler.RemoveGarage(0);

            Assert.Throws<ArgumentNullException>(() => garageHandler.CurrentGarage);
        }

        [Fact]
        public void RemoveGarage_WhenCurrentGarageRemovedAndIsNotTheLastOne_CurrentGarageShouldBeTheNextOne()
        {
            GarageHandler garageHandler = new GarageHandler();
            garageHandler.CreateGarage(capacity: 1);
            garageHandler.CreateGarage(capacity: 1);
            IGarage<IVehicle> expected = garageHandler.GetGarage(1);

            garageHandler.RemoveGarage(0);
            IGarage<IVehicle> actual = garageHandler.CurrentGarage;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RemoveGarage_WhenCurrentGarageRemovedAndIsNotTheLastOne_CurrentGarageIndexShouldStayTheSame()
        {
            GarageHandler garageHandler = new GarageHandler();
            garageHandler.CreateGarage(capacity: 1);
            garageHandler.CreateGarage(capacity: 1);
            int expected = 0;

            garageHandler.RemoveGarage(0);
            int actual = garageHandler.CurrentGarageIndex;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RemoveGarage_WhenCurrentGarageRemovedAndIsTheLastOne_CurrentGarageShouldBeThePreviousOne()
        {
            GarageHandler garageHandler = new GarageHandler();
            garageHandler.CreateGarage(capacity: 1);
            garageHandler.CreateGarage(capacity: 1);
            garageHandler.SetCurrentGarage(1);
            IGarage<IVehicle> expected = garageHandler.GetGarage(0);

            garageHandler.RemoveGarage(1);
            IGarage<IVehicle> actual = garageHandler.CurrentGarage;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RemoveGarage_WhenCurrentGarageRemovedAndIsTheLastOne_CurrentGarageIndexShouldDecrease()
        {
            GarageHandler garageHandler = new GarageHandler();
            garageHandler.CreateGarage(capacity: 1);
            garageHandler.CreateGarage(capacity: 1);
            garageHandler.SetCurrentGarage(1);
            garageHandler.RemoveGarage(1);
            int expected = 0;

            int actual = garageHandler.CurrentGarageIndex;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RemoveVehicle_IndexWhereIsNull_ReturnsFalse()
        {
            GarageHandler garageHandler = new GarageHandler();
            garageHandler.CreateGarage(capacity: 1);

            bool actual = garageHandler.RemoveVehicle(0);

            Assert.False(actual);
        }

        [Fact]
        public void RemoveVehicle_IndexWhereIsNotNull_ReturnsTrue()
        {
            GarageHandler garageHandler = new GarageHandler();
            garageHandler.CreateGarage(capacity: 1);
            garageHandler.AddVehicle(0, Vehicles.GetAVehicleOf10(0));

            bool actual = garageHandler.RemoveVehicle(0);

            Assert.True(actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void FindhVehicles_ExistentValues_ReturnsIVehicleIEnumerable(int index)
        {
            var handler = new GarageHandler();
            handler.CreateGarage(Searches.GetSearchVehicles());
            var expected = Searches.GetAnExpectedOF4_FindVehicles(index);
            var (registrationNumber, color, wheels) = Searches.GetASearchOF4_FindVehicles(index);

            var actual = handler.FindVehicles(registrationNumber, color, wheels);

            Assert.Equal(expected, actual);
        }

    }
}
