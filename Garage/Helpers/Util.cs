using Garage.UserInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Garage.Helpers
{

    public static class Util
    {
        #region data saving helpers

        public static void SaveGaragehandler(IFiles files, IGarageHandler handler, string path)
        {
            JArray garages = new JArray();

            foreach (var g in handler.GetGarages())
            {
                JObject garage = new JObject
                {
                    ["Name"] = g.Name,
                    ["Capacity"] = g.Capacity
                };

                JArray vehicles = new JArray();

                foreach (var v in g)
                {
                    JObject vehicle;
                    if (v is not null)
                    {
                        vehicle = JObject.FromObject(v);
                        vehicle["Type"] = v.GetType().Name;
                    }
                    else
                    {
                        vehicle = new JObject();
                        vehicle["Type"] = "null";
                    }
                    
                    vehicles.Add(vehicle);
                }

                garage["vehicles"] = vehicles;
                garages.Add(garage);
            }

            JObject jHandler = new JObject
            {
                ["CurrentGarage"] = handler.CurrentGarageIndex,
                ["garages"] = garages
            };

            files.WriteAllText(path, jHandler.ToString(Formatting.None));
        }

        

        public static IGarageHandler LoadGarageHandler(IFiles files, string path)
        {
            string jsonString = files.ReadAllText(path);

            IGarageHandler handler = new GarageHandler();

            JObject jHandler = JObject.Parse(jsonString);

            int currentGarage = jHandler["CurrentGarage"].Value<int>();

            LoadGarages(handler, jHandler);

            handler.SetCurrentGarage(currentGarage);

            return handler;
        }

        private static void LoadGarages(IGarageHandler handler, JObject jHandler)
        {
            var garages = jHandler["garages"]!.Value<JArray>();
            foreach (JObject g in garages)
            {
                string name = g["Name"].Value<string>();
                int capacity = g["Capacity"].Value<int>();
                handler.CreateGarage(capacity, LoadVehicles(g), name);
            }
        }

        private static string[] types = new[] {
                nameof(Airplane),
                nameof(Motorcycle),
                nameof(Car),
                nameof(Bus),
                nameof(Boat),
                "null"
            };

        private static IEnumerable<IVehicle> LoadVehicles(JObject garage)
        {
            

            var vehicles = garage["vehicles"].Value<JArray>();
            foreach (JObject v in vehicles)
            {
                IVehicle vehicle = null;

                string type = v["Type"].Value<string>();
                //v.Remove("Type");

                if (type == types[0])
                {
                    vehicle = v.ToObject<Airplane>();
                }
                else if (type == types[1])
                {
                    vehicle = v.ToObject<Motorcycle>();
                }
                else if (type == types[2])
                {
                    vehicle = v.ToObject<Car>();
                }
                else if (type == types[3])
                {
                    vehicle = v.ToObject<Bus>();
                }
                else if (type == types[4])
                {
                    vehicle = v.ToObject<Boat>();
                }

                yield return vehicle;
            }
        }
        #endregion

        #region Formatter helpers
        public static string GetVehicleInfo(IVehicle vehicle)
        {
            return $"Type: {vehicle.GetType().Name}, {vehicle}";
        }

        public static string GetVehicleInfo(IGarageHandler handler, int parkingPlace)
        {
            IVehicle vehicle = handler.GetVehicle(parkingPlace)!;
            if (vehicle is null)
            {
                return "null";
            }
            return $"Place: {parkingPlace}, {GetVehicleInfo(vehicle)}";
        }
        #endregion

        #region UI effect helpers
        public static void Stop(IUI ui)
        {
            ui.Print("Press Enter to continue.");
            ui.GetInput();
            ui.PrintLine("");
        }
        #endregion

        #region Input acquisition helpers
        public static bool GetText(IUI ui, string field, out string result, bool acceptNothing = true)
        {
            bool firstTime = true;
            do
            {
                if (!firstTime)
                {
                    ui.PrintError("something must be entered\n");
                }
                firstTime = false;

                ui.Print(field);
                string input = ui.GetInput();

                if (string.IsNullOrWhiteSpace(input))
                {
                    if (acceptNothing)
                    {
                        result = "";
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    result = input.Trim();
                    return true;
                }
            } while (true);
        }

        public static bool GetInt(IUI ui, string field, out int result, bool acceptNothing = true)
        {
            bool firstTime = true;
            do
            {
                if (!firstTime)
                {
                    ui.PrintError("not a valid number format.\n");
                }
                firstTime = false;

                GetText(ui, field, out string input);

                if (string.IsNullOrEmpty(input))
                {
                    if (acceptNothing)
                    {
                        result = 0;
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (int.TryParse(input, out result))
                {
                    return true;
                }
            } while (true);
        }

        public static bool GetRangedInt(IUI ui, string field, out int result, int rangeStart, int rangeEnd, bool acceptNothing = true)
        {
            if (rangeStart >= rangeEnd)
            {
                throw new ArgumentException($"{nameof(rangeStart)} cannot be equal or grater than {nameof(rangeEnd)}");
            }
            bool firstTime = true;
            int tempResult = 0;
            do
            {

                if (!firstTime)
                {
                    ui.PrintError($"{tempResult} is outside range {rangeStart} - {rangeEnd}.\n");
                }
                firstTime = false;

                if (!GetInt(ui, field, out result, acceptNothing))
                {
                    return false;
                }

                if (result >= rangeStart && result <= rangeEnd)
                {
                    return true;
                }
                else
                {
                    tempResult = result;
                }

            } while (true);
        }

        public static bool GetRangedInt(IUI ui, string field, out int result, int start, bool equalOrGreater, bool acceptNothing = true)
        {
            bool firstTime = true;
            do
            {

                if (!firstTime)
                {
                    ui.PrintError($"Input should be equal or {(equalOrGreater ? "greater" : "less")} than {start}.\n");
                }
                firstTime = false;

                if (!GetInt(ui, field, out result, acceptNothing))
                {
                    return false;
                }

                if (!equalOrGreater)
                {
                    if (result <= start)
                    {
                        return true;
                    }
                }
                else
                {
                    if (result >= start)
                    {
                        return true;
                    }
                }

            } while (true);
        }

        public static int GetNumericOption(IUI ui, int rangeStart, int rangeEnd)
        {
            if (rangeStart >= rangeEnd)
            {
                throw new ArgumentException($"{nameof(rangeStart)} cannot be equal or grater than {nameof(rangeEnd)}");
            }
            do
            {
                GetInt(ui, "Selection: ", out int option, acceptNothing: false);

                if (option >= rangeStart && option <= rangeEnd)
                {
                    return option;
                }
                else
                {
                    ui.PrintError($"{option} is not an option. Please choose a number between {rangeStart} and {rangeEnd}.\n");
                }
            } while (true);
        }

        public static bool AskCancelIfNothing(IUI ui)
        {
            if (AskClosedQuestion(ui, "You've just entered nothing, would you like to cancel the operation?", "y", "n") == "y")
            {
                ui.PrintLine("\nThe operation has been canceled.");
                Stop(ui);
                return true;
            }
            return false;

        }

        public static string AskClosedQuestion(IUI ui, string question, params string[] answers)
        {
            bool firstTime = true;
            string answer = "";
            string formatedAnswers = string.Join("/", answers);
            do
            {
                if (!firstTime)
                {
                    ui.PrintError($"{answer} is not a possible answer.");
                }
                firstTime = false;

                ui.PrintLine($"\n{question}");
                ui.PrintLine($"(Possible answers: {formatedAnswers})");
                GetText(ui, "Answer: ", out answer);
                if (answers.Contains(answer))
                {
                    return answer;
                }
            } while (true);
        }
        #endregion

        #region Search helpers
        public static IEnumerable<int> SearchVehicle(IGarageHandler handler, string? registration, string? color, int? wheels)
        {
            return handler.GetNotAvailablePlaces().Where(i
                => {
                    var v = handler.GetVehicle(i)!;

                    return (registration is null || registration.ToUpper() == v.RegistrationNumber)
                    && (color is null || color.ToLower() == v.Color)
                    && (wheels is null || wheels == v.Wheels);
                }
            );
        }

        private static IEnumerable<int> SearchVehicle(IGarageHandler handler, Type type, string? registration, string? color, int? wheels)
        {
            return handler.GetNotAvailablePlaces().Where(i
                => {
                    var v = handler.GetVehicle(i)!;

                    return v.GetType() == type
                    && (registration is null || registration.ToUpper() == v.RegistrationNumber)
                    && (color is null || color.ToLower() == v.Color)
                    && (wheels is null || wheels == v.Wheels);
                }
            );
        }

        public static IEnumerable<int> SearchAirplane(IGarageHandler handler, string? registration, string? color, int? wheels, int? numberOfEngines)
        {
            return SearchVehicle(handler, typeof(Airplane), registration, color, wheels).Where(i 
                => numberOfEngines is null || numberOfEngines == (handler.GetVehicle(i) as Airplane)!.NumberOfEngines
            );
        }

        public static IEnumerable<int> SearchMotorcycle(IGarageHandler handler, string? registration, string? color, int? wheels, int? CylinderVolume)
        {
            return SearchVehicle(handler, typeof(Motorcycle), registration, color, wheels).Where(i
                => CylinderVolume is null || CylinderVolume == (handler.GetVehicle(i) as Motorcycle)!.CylinderVolume
            );
        }

        public static IEnumerable<int> SearchCar(IGarageHandler handler, string? registration, string? color, int? wheels, string? fuelType)
        {
            return SearchVehicle(handler, typeof(Car), registration, color, wheels).Where(i
                => fuelType is null || fuelType.ToLower() == (handler.GetVehicle(i) as Car)!.FuelType
            );
        }

        public static IEnumerable<int> SearchBus(IGarageHandler handler, string? registration, string? color, int? wheels, int? numberOfSeats)
        {
            return SearchVehicle(handler, typeof(Bus), registration, color, wheels).Where(i
        => numberOfSeats is null || numberOfSeats == (handler.GetVehicle(i) as Bus)!.NumberOfSeats
            );
        }

        public static IEnumerable<int> SearchBoat(IGarageHandler handler, string? registration, string? color, int? wheels, int? length)
        {
            return SearchVehicle(handler, typeof(Boat), registration, color, wheels).Where(i
                => length is null || length == (handler.GetVehicle(i) as Boat)!.Length
            );
        }
        #endregion

        #region listing helpers
        public static IEnumerable<(string vehicleTypeName, int count)> GetVehicleTypeCount(IGarageHandler handler)
        {
            return handler.GetVehicles().Where(v => v is not null).GroupBy(v => v.GetType().Name).Select(g => (vehicleTypeName: g.Key, count: g.Count()))!;
        }
        #endregion
    }
}
