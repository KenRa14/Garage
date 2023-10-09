using Garage.UserInterface;

namespace Garage
{
    public class Menu
    {
        private IGarageHandler handler;
        private readonly IUI ui;
        private readonly IFiles files;
        private readonly string welcome = "Welcome to Garage 1.0";
        private readonly string dataFileName = "dt.json";
        private readonly string dataPath;

        public Menu(IGarageHandler handler, IUI ui, IFiles files)
        {
            this.handler = handler;
            this.ui = ui;
            this.files = files;
            dataPath = $@"{Environment.CurrentDirectory}\{dataFileName}";
        }

        public void Run()
        {
            bool firsTime = true;
            do
            {
                ui.Clear();
                ui.PrintTitle($"{welcome}");
                if (firsTime)
                {
                    Startup();
                    firsTime = false;
                }

                PrintGarageInfo();
                ui.PrintLine("\nPlease choose one of the next functions."
                    + "\n1. Garage management"
                    + "\n2. Parking"
                    + "\n3. Save data"
                    + "\n4. Load data"
                    + "\n0. Exit");

                int chosen = Util.GetNumericOption(ui, 0, 5);
                switch (chosen)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        OpenGarageManagementMenu();
                        break;
                    case 2:
                        OpenParkingMenu();
                        break;
                    case 3:
                        SaveData();
                        break;
                    case 4:
                        LoadData();
                        break;
                }
            } while (true);
        }

        protected void Startup()
        {
            if (File.Exists(dataPath))
            {
                ui.PrintLine("Saved data was found."
                    + "\n1. Load data"
                    + "\n2. Don't load data");

                int chosen = Util.GetNumericOption(ui, 1, 2);

                if (chosen == 1)
                {
                    if (!LoadData())
                    {
                        ui.PrintLine("");
                        CreateNewGarage(mustCreate: true);
                    }
                    return;
                }
                ui.PrintLine("");
                CreateNewGarage(mustCreate: true);
            }
            else
            {
                CreateNewGarage(mustCreate: true);
            }
        }

        protected void PrintGarageInfo()
        {
            if (handler.GarageCount == 0)
            {
                ui.PrintLine($"Garages count: {handler.GarageCount}");
                return;
            }
            ui.PrintLine($"Garages count: {handler.GarageCount}\nCurrent garage: {handler.CurrentGarage.Name}, capacity: {handler.GarageCapacity}, available space: {handler.GetAvailablePlaces().Count()}");
        }

        public void OpenGarageManagementMenu()
        {
            ui.Clear();
            bool exit = false;
            do
            {
                ui.PrintTitle("Garage Management");

                PrintGarageInfo();

                ui.PrintLine("\nPlease choose one of the next functions."
                    + "\n1. Set current garage"
                    + "\n2. Add garage"
                    + "\n3. Remove garage"
                    + "\n4. Garages overview"
                    + "\n0. Exit to main menu");

                int chosen = Util.GetNumericOption(ui, 0, 4);
                ui.PrintLine("");
                switch (chosen)
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        SetCurrentGarage();
                        break;
                    case 2:
                        CreateNewGarage();
                        break;
                    case 3:
                        RemoveGarage();
                        break;
                    case 4:
                        GarageOverview();
                        break;
                }
            } while (!exit);
        }

        public void OpenParkingMenu()
        {
            if (handler.GarageCount == 0)
            {
                ui.PrintLine("\nThere are no garages where to park a vehicle. Exiting...");
                Util.Stop(ui);
                return;
            }

            ui.Clear();
            bool exit = false;
            do
            {
                ui.PrintTitle("Parking");

                PrintGarageInfo();

                ui.PrintLine("\nPlease choose one of the next functions."
                    + "\n1. Add vehicle"
                    + "\n2. Remove vehicle"
                    + "\n3. Search vehicle by registration number"
                    + "\n4. Search vehicles"
                    + "\n5. List vehicles"
                    + "\n6. List vehicles by type"
                    + "\n0. Exit to main menu");

                int chosen = Util.GetNumericOption(ui, 0, 6);
                ui.PrintLine("");
                switch (chosen)
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        AddVehicle();
                        break;
                    case 2:
                        RemoveVehicle();
                        break;
                    case 3:
                        SearchVehicleByRegistrationNumber();
                        break;
                    case 4:
                        SearchVehicles();
                        break;
                    case 5:
                        ListVehicles();
                        break;
                    case 6:
                        ListVehicleTypes();
                        break;
                }
            } while (!exit);
        }

        protected void SaveData()
        {
            if (handler.GarageCount == 0)
            {
                ui.PrintLine("\nThere is nothing to save.\nAt least add one garage.\n");
                Util.Stop(ui);
                return;
            }
            try
            {
                ui.PrintLine("");
                Util.SaveGaragehandler(files, handler, dataPath);
                ui.PrintLine("The data was saved successfully.");
                Util.Stop(ui);
            }
            catch (IOException ex)
            {
                ui.PrintError($"data could not be saved\n{ex.Message}");
                Util.Stop(ui);
            }
            catch (Exception ex)
            {
                ui.PrintError($"data could not be saved\n{ex.Message}");
                Util.Stop(ui);
            }
        }

        protected bool LoadData()
        {
            try
            {
                ui.PrintLine("");
                handler = Util.LoadGarageHandler(files, dataPath);
                ui.PrintLine("The data was loaded successfully.");
                Util.Stop(ui);
                return true;
            }
            catch (FileNotFoundException ex)
            {
                ui.PrintError($"data could not be loaded\n{ex.Message}");
                Util.Stop(ui);
                return false;
            }
            catch (IOException ex)
            {
                ui.PrintError($"data could not be loaded\n{ex.Message}");
                Util.Stop(ui);
                return false;
            }
            catch (Exception ex)
            {
                ui.PrintError($"data could not be loaded\n{ex.Message}");
                Util.Stop(ui);
                return false;
            }
        }

        #region Garage management menu methods

        protected void SetCurrentGarage()
        {
            if (handler.GarageCount == 0)
            {
                ui.PrintLine("There are no garages to set. Exiting...");
                Util.Stop(ui);
                return;
            }
            else if (handler.GarageCount == 1)
            {
                ui.PrintLine("There is only one garage. Exiting...");
                Util.Stop(ui);
                return;
            }

            ui.PrintLine("Please select the garage to manage from now on.");
            int i = 0;
            foreach (var garage in handler.GetGarages())
            {
                ui.PrintLine($"{++i}. {garage.Name}");
            }
            ui.PrintLine($"0. Go back");

            int chosen = Util.GetNumericOption(ui, 0, i);

            if (chosen == 0)
            {
                return;
            }
            int index = chosen - 1;
            handler.SetCurrentGarage(index);
            ui.PrintLine($"\nThe garage {handler.GetGarage(index).Name} has been set as current garage.");
            Util.Stop(ui);
        }

        protected void CreateNewGarage(bool mustCreate = false)
        {

            if (!mustCreate)
            {
                ui.PrintLine("If you change your mind just leave empty and press enter.\n");
            }
            ui.PrintLine("Please enter the capacity of the garage");
            int capacity;
            do
            {
                if (!Util.GetInt(ui, "Garage capacity: ", out capacity, acceptNothing: !mustCreate))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return;
                    }
                    continue;
                }
                break;
            } while(true);
            
            ui.PrintLine("\nPlease enter the name of the garage");
            string name;
            do
            {
                if (!Util.GetText(ui, "Garage name: ", out name, acceptNothing: !mustCreate))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return;
                    }
                    continue;
                }
                string upperName = name.ToUpper();
                if (handler.GetGarages().FirstOrDefault(g => g.Name.ToUpper() == upperName) is not null)
                {
                    ui.PrintError("a garage with this name already exists.\n");
                    continue;
                }

                break;
            } while (true);
            

            handler.CreateGarage(capacity, name);
            ui.PrintLine("\nA new garage has been created.");
            if (handler.GarageCount == 1)
            {
                ui.PrintLine("The new garage has been set as current garage to manage.");
            }
            ui.PrintLine("");
            Util.Stop(ui);
        }

        protected void RemoveGarage()
        {
            do
            {
                if (handler.GarageCount == 0)
                {
                    ui.PrintLine("There are no garages to remove. Exiting...");
                    Util.Stop(ui);
                    return;
                }

                ui.PrintLine("Please select the garage to remove.");
                int i = 0;
                foreach (var garage in handler.GetGarages())
                {
                    ui.PrintLine($"{++i}. {garage.Name}");
                }
                ui.PrintLine($"0. Go back");

                int chosen = Util.GetNumericOption(ui, 0, i);

                if (chosen == 0)
                {
                    return;
                }
                int index = chosen - 1;
                if (Util.AskClosedQuestion(ui, $"Are you sure you want to permanently remove the garage {handler.GetGarage(index).Name}?", "y", "n") == "y")
                {
                    var garage = handler.GetGarage(index);
                    if (handler.CurrentGarageIndex == index)
                    {
                        handler.RemoveGarage(index);
                        ui.PrintLine("The current garage was removed.");
                        if (handler.GarageCount > 0)
                        {
                            ui.PrintLine("Other garage was selected as current.");
                        }
                    }
                    else
                    {
                        handler.RemoveGarage(index);
                        ui.PrintLine($"The garage {garage.Name} was removed.");
                    }

                    if (handler.GarageCount == 0)
                    {
                        ui.PrintLine("All of the garages were removed.");
                    }
                }
                else
                {
                    ui.PrintLine("\nAction canceled.");
                    Util.Stop(ui);
                    return;
                }
                ui.PrintLine("");
                Util.Stop(ui);
            } while (true);
        }

        protected void GarageOverview()
        {
            if (handler.GarageCount == 0)
            {
                ui.PrintLine("\nNo garages.");
                Util.Stop(ui);
                return;
            }

            ui.PrintLine("Garages:");
            int i = 1;
            foreach (var garage in handler.GetGarages())
            {
                ui.PrintLine($"{i++}. {garage}");
            }
            ui.PrintLine("");
            Util.Stop(ui);
        }

        #endregion

        #region Parking menu methods
        protected void AddVehicle()
        {
            if (handler.VehicleCount == handler.GarageCapacity)
            {
                ui.PrintLine("\nThe garage is full. Exiting...");
                Util.Stop(ui);
                return;
            }

            IVehicle vehicle = CreateNewVehicle()!;
            if (vehicle is null)
            {
                return;
            }

            do
            {
                int[] availablePlaces = handler.GetAvailablePlaces().ToArray();

                if (availablePlaces.Length == 0)
                {
                    ui.PrintLine("\nThe garage is full. Exiting...");
                    Util.Stop(ui);
                    return;
                }

                ui.PrintLine("\nWhere would you like the vehicle to be parked?");
                ui.PrintLine($"Available spaces: {string.Join(", ", availablePlaces)}");
                if (!Util.GetInt(ui, "Parking place: ", out int parkingPlace))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return;
                    }
                }
                else
                {
                    if (availablePlaces.Contains(parkingPlace))
                    {
                        if (handler.AddVehicle(parkingPlace, vehicle))
                        {
                            ui.PrintLine("\nThe vehicle has been added to the garage.");
                        }
                        else
                        {
                            ui.PrintLine("\nThe vehicle could not be added to the garage.\nTry again.\n");
                        }

                        Util.Stop(ui);
                        return;
                    }
                    else
                    {
                        ui.PrintError($"{parkingPlace} is not a valid/available parking place.");
                    }
                }

            } while (true);

            
        }

        protected void RemoveVehicle()
        {
            if (handler.VehicleCount == 0)
            {
                ui.PrintLine("There are no vehicles to remove. Exiting...\n");
                Util.Stop(ui);
                return;
            }
            do
            {
                ui.PrintLine("Please select how to remove the vehicle"
                + "\n1. Remove by parking space"
                + "\n2. Remove by registration number"
                + "\n0. Go back");

                int chosen = Util.GetNumericOption(ui, 0, 2);

                IVehicle vehicle;
                int parkingPlace = 0;
                switch (chosen)
                {
                    case 0:
                        return;
                    case 1:
                        parkingPlace = GetParkingIndexWithAVehicle();
                        if (parkingPlace == -1)
                        {
                            continue;
                        }
                        
                        break;
                    case 2:
                        parkingPlace = GetVehicleParkingIndexByRegistrationNumber();
                        if (parkingPlace == -1)
                        {
                            continue;
                        }
                        break;
                }
                vehicle = handler.GetVehicle(parkingPlace)!;

                ui.PrintLine($"\nVehicle found:\n{Util.GetVehicleInfo(vehicle)}");
                if (Util.AskClosedQuestion(ui, "Are you sure you want to remove the vehicle?", "y", "n") == "y")
                {
                    handler.RemoveVehicle(parkingPlace);
                    ui.PrintLine("The vehicle has been removed.");
                    if (handler.VehicleCount == 0)
                    {
                        ui.PrintLine("\nThere are no more vehicle to remove. Exiting...\n");
                        Util.Stop(ui);
                        return;
                    }
                }
                else
                {
                    ui.PrintLine("\nAction canceled.");
                }
                ui.PrintLine("");
                Util.Stop(ui);
            } while (true);
            
        }

        private void SearchVehicleByRegistrationNumber()
        {
            if (handler.VehicleCount == 0)
            {
                ui.PrintLine("There are no vehicles to search. Exiting...\n");
                Util.Stop(ui);
                return;
            }
            int parkingIndex = GetVehicleParkingIndexByRegistrationNumber();

            if (parkingIndex == -1)
            {
                return;
            }

            ui.PrintLine($"\n{Util.GetVehicleInfo(handler, parkingIndex)}\n");
            Util.Stop(ui);
        }

        private void SearchVehicles()
        {
            if (handler.VehicleCount == 0)
            {
                ui.PrintLine("There are no vehicles to search. Exiting...\n");
                Util.Stop(ui);
                return;
            }
            ui.PrintLine("The fields to search by will appear sequentially."
                + "\nEnter the exact value you're looking for (letter case doesn't matter)."
                + "\nIf you don't whant to include a certain field in the search, just leave it empty and press enter."
                + "\n");

            ui.PrintLine("What type of vehicle will you search?"
                + "\n1. Any type"
                + "\n2. Airplane"
                + "\n3. Motorcycle"
                + "\n4. Car"
                + "\n5. Bus"
                + "\n6. Boat");

            Util.GetRangedInt(ui, "Type: ", out int type, 1, 6, acceptNothing: false);

            IEnumerable<int> parkingIndexs = null!;
            switch (type)
            {
                case 1:
                    parkingIndexs = SearchAnyVehicles();
                    break;
                case 2:
                    parkingIndexs = SearchAirplane();
                    break;
                case 3:
                    parkingIndexs = SearchMotorcycle();
                    break;
                case 4:
                    parkingIndexs = SearchCar();
                    break;
                case 5:
                    parkingIndexs = SearchBus();
                    break;
                case 6:
                    parkingIndexs = SearchBoat();
                    break;
            }

            if (!parkingIndexs.Any())
            {
                ui.PrintLine("\nNo vehicle found.\n");
                Util.Stop(ui);
                return;
            }

            int total = 0;
            ui.PrintLine("\nVehicle(s) found:\n");
            foreach (var parkingIndex in parkingIndexs)
            {
                ui.PrintLine(Util.GetVehicleInfo(handler, parkingIndex));
                total++;
            }

            ui.PrintLine($"\nTotal: {total}\n");
            Util.Stop(ui);

        }

        private void ListVehicles()
        {
            int total = 0;
            foreach (var parkingIndex in handler.GetNotAvailablePlaces())
            {
                ui.PrintLine(Util.GetVehicleInfo(handler, parkingIndex));
                total++;
            }
            ui.PrintLine($"\nTotal: {total}\n");
            Util.Stop(ui);
        }

        private void ListVehicleTypes()
        {
            int total = 0;
            foreach (var (vehicleType, count) in Util.GetVehicleTypeCount(handler))
            {
                ui.PrintLine($"{vehicleType}: {count}");
                total+=count;
            }
            ui.PrintLine($"\nTotal: {total}\n");
            Util.Stop(ui);
        }

        #region Add vehicle methods

        private IVehicle? CreateNewVehicle()
        {
            ui.PrintLine("Please enter the type of the vehicle.");
            ui.PrintLine("1. Airplane"
                + "\n2. Motorcycle"
                + "\n3. Car"
                + "\n4. Bus"
                + "\n5. Boat");

            int chosen;
            do
            {
                if (!Util.GetRangedInt(ui, "Type: ", out chosen, 1, 5, acceptNothing: true))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return null;
                    }
                }
                else
                {
                    break;
                }
            } while (true);

            switch (chosen)
            {
                case 1:
                    return CreateAirplane();
                case 2:
                    return CreateMotorcycle();
                case 3:
                    return CreateCar();
                case 4:
                    return CreateBus();
                case 5:
                    return CreateBoat();
                default:
                    return null;
            }
        }

        private IVehicle? CreateVehicleData()
        {
            string registrationNumber; 
            do
            {
                if (!Util.GetText(ui, "Registration number: ", out registrationNumber))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return null;
                    }
                }
                else
                {
                    if (handler.FindVehicleParkingIndex(registrationNumber) != -1)
                    {
                        ui.PrintError("A vehicle with this registration number already exists.\n");
                    }
                    else
                    {
                        break;
                    }
                }
            } while (true);

            string color;
            do
            {
                if (!Util.GetText(ui, "Color: ", out color))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return null;
                    }
                }
                else
                {
                    break;
                }
            } while (true);

            int wheels;
            do
            {
                if (!Util.GetRangedInt(ui, "Number of wheels: ", out wheels, start: 0, equalOrGreater: true))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return null;
                    }
                }
                else
                {
                    break;
                }
            } while (true);

            return new Vehicle(registrationNumber, color, wheels);
        }

        private IVehicle? CreateAirplane()
        {
            IVehicle data = CreateVehicleData()!;
            if (data is null)
            {
                return null;
            }

            int numberOfEngines;
            do
            {
                if (!Util.GetRangedInt(ui, "Number of engines: ", out numberOfEngines, start: 1, equalOrGreater: true))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return null;
                    }
                }
                else
                {
                    break;
                }
            } while (true);

            return new Airplane(data.RegistrationNumber, data.Color, data.Wheels, numberOfEngines);
        }

        private IVehicle? CreateMotorcycle()
        {
            IVehicle data = CreateVehicleData()!;
            if (data is null)
            {
                return null;
            }

            int cylinderVolume;
            do
            {
                if (!Util.GetRangedInt(ui, "Cylinder volume: ", out cylinderVolume, start: 1, equalOrGreater: true))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return null;
                    }
                }
                else
                {
                    break;
                }
            } while (true);

            return new Motorcycle(data.RegistrationNumber, data.Color, data.Wheels, cylinderVolume);
        }

        private IVehicle? CreateCar()
        {
            IVehicle data = CreateVehicleData()!;
            if (data is null)
            {
                return null;
            }

            string fuelType;
            do
            {
                if (!Util.GetText(ui, "Fuel type: ", out fuelType))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return null;
                    }
                }
                else
                {
                    break;
                }
            } while (true);

            return new Car(data.RegistrationNumber, data.Color, data.Wheels, fuelType);
        }

        private IVehicle? CreateBus()
        {
            IVehicle data = CreateVehicleData()!;
            if (data is null)
            {
                return null;
            }

            int numberOfSeats;
            do
            {
                if (!Util.GetRangedInt(ui, "Number of seats: ", out numberOfSeats, start: 1, equalOrGreater: true))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return null;
                    }
                }
                else
                {
                    break;
                }
            } while (true);

            return new Bus(data.RegistrationNumber, data.Color, data.Wheels, numberOfSeats);
        }

        private IVehicle? CreateBoat()
        {
            IVehicle data = CreateVehicleData()!;
            if (data is null)
            {
                return null;
            }

            int length;
            do
            {
                if (!Util.GetRangedInt(ui, "Length: ", out length, start: 1, equalOrGreater: true))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return null;
                    }
                }
                else
                {
                    break;
                }
            } while (true);

            return new Boat(data.RegistrationNumber, data.Color, data.Wheels, length);
        }

        #endregion

        #region Remove vehicle methods

        private int GetParkingIndexWithAVehicle()
        {
            do
            {
                if (!Util.GetRangedInt(ui, $"Parking place ({0} - {handler.GarageCapacity - 1}): ", out int parkingPlace, 0, handler.GarageCapacity - 1))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return -1;
                    }
                    continue;
                }
                IVehicle vehicle = handler.GetVehicle(parkingPlace)!;

                if (vehicle is null)
                {
                    ui.PrintError($"There is no vehicle at the parking place {parkingPlace}\n");
                }
                else
                {
                    return parkingPlace;
                }
            } while (true);
        }

        private int GetVehicleParkingIndexByRegistrationNumber()
        {
            do
            {
                if (!Util.GetText(ui, "Registration number: ", out string registrationNumber))
                {
                    if (Util.AskCancelIfNothing(ui))
                    {
                        return -1;
                    }
                }
                else
                {
                    int parkingIndex = handler.FindVehicleParkingIndex(registrationNumber);
                    if (parkingIndex == -1)
                    {
                        ui.PrintLine("\nNo vehicle found\n");
                    }
                    else
                    {
                        return parkingIndex;
                    }
                }


            } while (true);
            
        }

        #endregion

        #region Search vehicles methods

        private IEnumerable<int> SearchAnyVehicles()
        {
            var (registrationNumber, color, wheels) = GetSearchVehicleData();

            return Util.SearchVehicle(handler, registrationNumber, color, wheels);
        }

        private (string registrationNumber, string color, int? wheels) GetSearchVehicleData()
        {
            if (!Util.GetText(ui, "Registration number: ", out string registrationNumber))
            {
                registrationNumber = null!;
            }

            if (!Util.GetText(ui, "Color: ", out string color))
            {
                color = null!;
            }

            int? wheels;
            if (!Util.GetRangedInt(ui, "Number of wheels: ", out int tempInt, start: 0, equalOrGreater: true))
            {
                wheels = null!;
            }
            else
            {
                wheels = tempInt;
            }

            return (registrationNumber, color, wheels);
        }

        private IEnumerable<int> SearchAirplane()
        {
            var (registrationNumber, color, wheels) = GetSearchVehicleData();

            int? numberOfEngines;
            if (!Util.GetRangedInt(ui, "Number of engines: ", out int tempInt, start: 1, equalOrGreater: true))
            {
                numberOfEngines = null!;
            }
            else
            {
                numberOfEngines = tempInt;
            }

            return Util.SearchAirplane(handler, registrationNumber, color, wheels, numberOfEngines);
        }

        private IEnumerable<int> SearchMotorcycle()
        {
            var (registrationNumber, color, wheels) = GetSearchVehicleData();

            int? cylinderVolume;
            if (!Util.GetRangedInt(ui, "Cylinder volume: ", out int tempInt, start: 1, equalOrGreater: true))
            {
                cylinderVolume = null!;
            }
            else
            {
                cylinderVolume = tempInt;
            }


            return Util.SearchMotorcycle(handler, registrationNumber, color, wheels, cylinderVolume);
        }

        private IEnumerable<int> SearchCar()
        {
            var (registrationNumber, color, wheels) = GetSearchVehicleData();

            if (!Util.GetText(ui, "Fuel type: ", out string fuelType))
            {
                fuelType = null!;
            }


            return Util.SearchCar(handler, registrationNumber, color, wheels, fuelType);
        }

        private IEnumerable<int> SearchBus()
        {
            var (registrationNumber, color, wheels) = GetSearchVehicleData();

            int? numberOfSeats;
            if (!Util.GetRangedInt(ui, "Number of seats: ", out int tempInt, start: 1, equalOrGreater: true))
            {
                numberOfSeats = null!;
            }
            else
            {
                numberOfSeats = tempInt;
            }


            return Util.SearchBus(handler, registrationNumber, color, wheels, numberOfSeats);
        }

        private IEnumerable<int> SearchBoat()
        {
            var (registrationNumber, color, wheels) = GetSearchVehicleData();

            int? length;
            if (!Util.GetRangedInt(ui, "Length: ", out int tempInt, start: 1, equalOrGreater: true))
            {
                length = null!;
            }
            else
            {
                length = tempInt;
            }


            return Util.SearchBoat(handler, registrationNumber, color, wheels, length);
        }

        #endregion

        #endregion
    }
}
