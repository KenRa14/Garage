using Garage.Entity;
using Garage.Helpers;
using Garage.IO;
using Garage.Tests.Helpers;
using Garage.UserInterface;
using Moq;

namespace Garage.Tests.HelpersTests
{
    public class UtilTests
    {
        #region Input acquisition helpers
        [Fact]
        public void GetText_NotEmptyString_ReturnsTrimmedString()
        {
            string expected = "text";

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(" text ");


            Util.GetText(mockIui.Object, "Text: ", out string actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetText_NotEmptyString_ReturnsTrue()
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("text");


            bool actual = Util.GetText(mockIui.Object, "Text: ", out string result);

            Assert.True(actual);
        }

        [Fact]
        public void GetText_EmptyStringWhenAccpetNothingIsTrue_ReturnsFalse()
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("");

            bool actual = Util.GetText(mockIui.Object, "Text: ", out string result, acceptNothing: true);

            Assert.False(actual);
        }

        [Fact]
        public void GetText_EmptyStringWhenAcceptNothingIsFalse_PrintsError()
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? "" : "text").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);


            Util.GetText(mockIui.Object, "Text: ", out string result, acceptNothing: false);

            Assert.NotNull(error);
        }

        [Fact]
        public void GetInt_NotParseableString_PrintsError()
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? "A" : "0").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);

            Util.GetInt(mockIui.Object, "Number: ", out int result);

            Assert.NotNull(error);
        }

        [Fact]
        public void GetInt_ParseableString_DoesNotPrintError()
        {
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("0");
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);

            Util.GetInt(mockIui.Object, "Number: ", out int result);

            Assert.Null(error);
        }

        [Fact]
        public void GetInt_ParseableString_ReturnsParsedInt()
        {
            int expected = 0;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("0");


            Util.GetInt(mockIui.Object, "Number: ", out int actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetInt_ParseableString_ReturnsTrue()
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("0");


            bool actual = Util.GetInt(mockIui.Object, "Number: ", out int result);

            Assert.True(actual);
        }

        [Fact]
        public void GetInt_EmptyStringWhenAccpetNothingIsTrue_ReturnsFalse()
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("");


            bool actual = Util.GetInt(mockIui.Object, "Number: ", out int result, acceptNothing: true);

            Assert.False(actual);
        }

        [Fact]
        public void GetInt_EmptyStringWhenAccpetNothingIsFalse_PrintsError()
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? "" : "0").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);


            Util.GetInt(mockIui.Object, "Number: ", out int result, acceptNothing: false);

            Assert.NotNull(error);
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("2")]
        public void GetRangedInt_2LimitsOutOfRange_PrintsError(string input)
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? input : "0").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);


            Util.GetRangedInt(mockIui.Object, "Number: ", out int result, rangeStart: 0, rangeEnd: 1);

            Assert.NotNull(error);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        public void GetRangedInt_2LimitsInRange_ReturnsParsedInt(string input, int expected)
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(input);

            Util.GetRangedInt(mockIui.Object, "Number: ", out int actual, rangeStart: 0, rangeEnd: 1);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        public void GetRangedInt_2LimitsInRange_ReturnsTrue(string input)
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(input);

            bool actual = Util.GetRangedInt(mockIui.Object, "Number: ", out int result, rangeStart: 0, rangeEnd: 1);

            Assert.True(actual);
        }

        [Fact]
        public void GetRangedInt_2LimitsEmptyStringWhenAcceptNothingIsTrue_ReturnsFalse()
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("");

            bool actual = Util.GetRangedInt(mockIui.Object, "Number: ", out int result, rangeStart: 0, rangeEnd: 1, acceptNothing: true);

            Assert.False(actual);
        }

        [Fact]
        public void GetRangedInt_2LimitsEmptyStringWhenAcceptNothingIsFalse_PrintsError()
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? "" : "0").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);

            Util.GetRangedInt(mockIui.Object, "Number: ", out int result, rangeStart: 0, rangeEnd: 1, acceptNothing: false);

            Assert.NotNull(error);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        public void GetRangedInt_2LimitsRangeStartIsEqualOrGreatherThanRangeEnd_ThrowsException(int rangeStart, int rangeEnd)
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("0");

            Assert.Throws<ArgumentException>(()
                => Util.GetRangedInt(mockIui.Object, "Number: ", out int result,
                rangeStart: rangeStart, rangeEnd: rangeEnd));
        }

        [Fact]
        public void GetRangedInt_1LimitOutOfRangeWhenArgumenIsLessThanStart_PrintsError()
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? "-1" : "0").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);


            Util.GetRangedInt(mockIui.Object, "Number: ", out int result, start: 0, equalOrGreater: true);

            Assert.NotNull(error);
        }

        [Fact]
        public void GetRangedInt_1LimitOutOfRangeWhenArgumenIsGreaterThanStart_PrintsError()
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? "1" : "0").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);


            Util.GetRangedInt(mockIui.Object, "Number: ", out int result, start: 0, equalOrGreater: false);

            Assert.NotNull(error);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        public void GetRangedInt_1LimitWhenSeekingEqualOrGreater_ReturnsParsedInt(string input, int expected)
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(input);

            Util.GetRangedInt(mockIui.Object, "Number: ", out int actual, start: 0, equalOrGreater: true);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("-1", -1)]
        public void GetRangedInt_1LimitWhenSeekingEqualOrLess_ReturnsParsedInt(string input, int expected)
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(input);

            Util.GetRangedInt(mockIui.Object, "Number: ", out int actual, start: 0, equalOrGreater: false);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        public void GetRangedInt_1LimitWhenSeekingEqualOrGreater_ReturnsTrue(string input)
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(input);

            bool actual = Util.GetRangedInt(mockIui.Object, "Number: ", out int result, start: 0, equalOrGreater: true);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        public void GetRangedInt_1LimitWhenSeekingEqualOrLess_ReturnsTrue(string input)
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(input);

            bool actual = Util.GetRangedInt(mockIui.Object, "Number: ", out int result, start: 0, equalOrGreater: false);

            Assert.True(actual);
        }

        [Fact]
        public void GetRangedInt_1LimitEmptyStringWhenAcceptNothingIsTrue_ReturnsFalse()
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("");

            bool actual = Util.GetRangedInt(mockIui.Object, "Number: ", out int result,
                start: 0, equalOrGreater: true, acceptNothing: true);

            Assert.False(actual);
        }

        [Fact]
        public void GetRangedInt_EmptyStringWhenAcceptNothingIsFalse_PrintsError()
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? "" : "0").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);

            Util.GetRangedInt(mockIui.Object, "Number: ", out int result,
                start: 0, equalOrGreater: true, acceptNothing: false);

            Assert.NotNull(error);
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("2")]
        public void GetNumericOption_OutOfRange_PrintsError(string input)
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? input : "0").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);


            int result = Util.GetNumericOption(mockIui.Object, rangeStart: 0, rangeEnd: 1);

            Assert.NotNull(error);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        public void GetNumericOption_InRange_ReturnsParsedInt(string input, int expected)
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(input);

            int actual = Util.GetNumericOption(mockIui.Object, rangeStart: 0, rangeEnd: 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericOption_EmptyString_PrintsError()
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? "" : "0").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);

            Util.GetNumericOption(mockIui.Object, rangeStart: 0, rangeEnd: 1);

            Assert.NotNull(error);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        public void GetNumericOption_RangeStartIsEqualOrGreatherThanRangeEnd_ThrowsException(int rangeStart, int rangeEnd)
        {
            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns("0");

            Assert.Throws<ArgumentException>(()
                => Util.GetNumericOption(mockIui.Object,
                rangeStart: rangeStart, rangeEnd: rangeEnd));
        }

        [Fact]
        public void AskClosedQuestion_InputIsNotAnAnswer_PrintsError()
        {
            bool firstTime = true;
            string error = null!;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(() => firstTime ? "b" : "y").Callback(() => firstTime = false);
            mockIui.Setup(ui => ui.PrintError(It.IsAny<string>())).Callback<string>(m => error = m);

            Util.AskClosedQuestion(mockIui.Object, "Yes or No?", "y", "n");

            Assert.NotNull(error);
        }

        [Theory]
        [InlineData("y")]
        [InlineData("n")]
        public void AskClosedQuestion_InputIsOneOfTheAnswers_ReturnsTheAnswer(string input)
        {
            string expected = input;

            var mockIui = new Mock<IUI>();
            mockIui.Setup(ui => ui.GetInput()).Returns(input);

            string actual = Util.AskClosedQuestion(mockIui.Object, "Yes or No?", "y", "n");

            Assert.Equal(expected, actual);
        }

        #endregion

        #region listing helpers
        [Fact]
        public void GetVehicleTypeCount_NoNulls_ReturnsCount()
        {
            IEnumerable<(string, int)> expected = Vehicles.Vehicles10TypeCount;
            GarageHandler handler = new GarageHandler();
            handler.CreateGarage(vehicles: Vehicles.GetVehicles10());

            IEnumerable<(string, int)> actual = Util.GetVehicleTypeCount(handler);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetVehicleTypeCount_WithNulls_ReturnsCountSkipingNulls()
        {
            IEnumerable<(string, int)> expected = Vehicles.Vehicles10WithNullsTypeCount;
            GarageHandler handler = new GarageHandler();
            handler.CreateGarage(vehicles: Vehicles.GetVehicles10WithNulls());

            IEnumerable<(string, int)> actual = Util.GetVehicleTypeCount(handler);

            Assert.Equal(expected, actual);
        }
        #endregion

        #region data saving helpers
        [Fact]
        public void SaveFunction_GarageHandler_SavesAndLoadsGarageHandlerSuccessfully()
        {
            string savedText = null!;

            var mockFiles = new Mock<IFiles>();

            mockFiles.Setup(files => files.WriteAllText("", It.IsAny<string>()))
                .Callback<string, string>((path, contents) => savedText = contents);

            mockFiles.Setup(files => files.ReadAllText("")).Returns(() => savedText);

            var files = mockFiles.Object;

            var handlerToSave = GetGarageHandlerToSave();
            Util.SaveGaragehandler(files, handlerToSave, path: "");

            var loadedHandler = Util.LoadGarageHandler(files, path: "");

            Assert.Equal(handlerToSave.CurrentGarageIndex, loadedHandler.CurrentGarageIndex);
            Assert.Equal<IEnumerable<IGarage<IVehicle>>>(handlerToSave.GetGarages(), loadedHandler.GetGarages());
        }
        

        private static GarageHandler GetGarageHandlerToSave()
        {
            var handler = new GarageHandler();
            //A Garage with 10 vehicles
            handler.CreateGarage(name: "Garage 1", vehicles: new IVehicle[]
            {
                new Motorcycle("AMQ701", "Red", 4, 22),
                new Boat("XRE309", "gold", 5, 65),
                new Boat("STP669", "pink", 7, 23),
                new Boat("VFW031", "Fawn", 2, 74),
                new Airplane("NZT779", "green", 6, 5),
                new Motorcycle("IZE345", "Ultramarine", 5, 62),
                new Boat("QHF614", "blue", 5, 107),
                new Boat("ADY180", "Spiro Disco Ball", 1, 108),
                new Motorcycle("KMO482", "red", 0, 111),
                new Airplane("VTH260", "Blond", 4, 4)
            });

            //Another garage with capacity of 10
            handler.CreateGarage(name: "Garage 2", vehicles: new IVehicle[]
            {
                new Car("ZGW076", "Bole", 3, "Diesel") ,
                new Boat("IMN023", "Corn", 0, 20) ,
                new Boat("NKI016", "green", 5, 118) ,
                new Airplane("GFN064", "green", 1, 5) ,
                null!,
                new Car("AJJ315", "Canary", 4, "Gasoline") ,
                null!,
                new Car("EWA868", "orange", 2, "Gasoline") ,
                new Car("VAH406", "brown", 2, "Gasoline") ,
                new Bus("JSB685", "rose", 6, 59)
            });


            //Last garage with capacity of 10
            handler.CreateGarage(name: "Garage 3", vehicles: new IVehicle[]
            {
                new Car("YSM685", "grey", 7, "Gasoline") ,
                new Bus("OAL977", "Isabelline", 5, 56) ,
                null!,
                new Motorcycle("NEZ576", "Dollar bill", 4, 27) ,
                null!,
                new Airplane("SEE987", "Pale taupe", 2, 2) ,
                null!,
                null!,
                new Car("ZMB866", "red", 4, "Diesel") ,
                new Car("AXF314", "Field drab", 2, "Diesel")
            });

            handler.SetCurrentGarage(1);

            return handler;
        }
        #endregion

        #region Search helpers
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void SearchVehicle_ExistentValues_ReturnsIntIEnumerable(int index)
        {
            var handler = new GarageHandler();
            handler.CreateGarage(Searches.GetSearchVehicles());
            var expected = Searches.GetAnExpectedOF4_SearchVehicle(index);
            var (registrationNumber, color, wheels) = Searches.GetASearchOF4_SearchVehicle(index);

            var actual = Util.SearchVehicle(handler, registrationNumber, color, wheels);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void SearchAirplane_ExistentValues_ReturnsIntIEnumerable(int index)
        {
            var handler = new GarageHandler();
            handler.CreateGarage(Searches.GetSearchVehicles());
            var expected = Searches.GetAnExpectedOF5_SearchAirplane(index);
            var (registrationNumber, color, wheels, numberOfEngines) = Searches.GetASearchOF5_SearchAirplane(index);

            var actual = Util.SearchAirplane(handler, registrationNumber, color, wheels, numberOfEngines);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void SearchMotorcycle_ExistentValues_ReturnsIntIEnumerable(int index)
        {
            var handler = new GarageHandler();
            handler.CreateGarage(Searches.GetSearchVehicles());
            var expected = Searches.GetAnExpectedOF5_SearchMotorcycle(index);
            var (registrationNumber, color, wheels, volumeCylinder) = Searches.GetASearchOF5_SearchMotorcycle(index);

            var actual = Util.SearchMotorcycle(handler, registrationNumber, color, wheels, volumeCylinder);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void SearchCar_ExistentValues_ReturnsIntIEnumerable(int index)
        {
            var handler = new GarageHandler();
            handler.CreateGarage(Searches.GetSearchVehicles());
            var expected = Searches.GetAnExpectedOF5_SearchCar(index);
            var (registrationNumber, color, wheels, fuelType) = Searches.GetASearchOF5_SearchCar(index);

            var actual = Util.SearchCar(handler, registrationNumber, color, wheels, fuelType);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void SearchBus_ExistentValues_ReturnsIntIEnumerable(int index)
        {
            var handler = new GarageHandler();
            handler.CreateGarage(Searches.GetSearchVehicles());
            var expected = Searches.GetAnExpectedOF5_SearchBus(index);
            var (registrationNumber, color, wheels, numberOfSeats) = Searches.GetASearchOF5_SearchBus(index);

            var actual = Util.SearchBus(handler, registrationNumber, color, wheels, numberOfSeats);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void SearchBoat_ExistentValues_ReturnsIntIEnumerable(int index)
        {
            var handler = new GarageHandler();
            handler.CreateGarage(Searches.GetSearchVehicles());
            var expected = Searches.GetAnExpectedOF5_SearchBoat(index);
            var (registrationNumber, color, wheels, Length) = Searches.GetASearchOF5_SearchBoat(index);

            var actual = Util.SearchBoat(handler, registrationNumber, color, wheels, Length);

            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
