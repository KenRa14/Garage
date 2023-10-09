using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class GarageHandler : IGarageHandler
    {
        private List<IGarage<IVehicle>> garages = new();

        public IGarage<IVehicle> CurrentGarage
        {
            get
            {
                if (GarageCount == 0)
                    throw new ArgumentNullException("garage", "There are no garages. At least one garage should've been added/created.");
                return garages[CurrentGarageIndex];
            }
        }

        public GarageHandler()
        {
        }

        public GarageHandler(IGarage<IVehicle> garage)
        {
            AddGarage(garage);
        }

        public int GarageCount { get => garages.Count; }

        public int GarageCapacity { get => CurrentGarage.Capacity; }

        public int VehicleCount { get => CurrentGarage.VehicleCount; }

        public int CurrentGarageIndex { get; protected set; }


        public void CreateGarage(int capacity, string name = "")
        {
            AddGarage(new Garage<IVehicle>(capacity, name));
        }

        public void CreateGarage(int capacity, IEnumerable<IVehicle> vehicles, string name = "")
        {
            AddGarage(new Garage<IVehicle>(capacity, vehicles, name));
        }

        public void CreateGarage(IEnumerable<IVehicle> vehicles, string name = "")
        {
            AddGarage(new Garage<IVehicle>(vehicles, name));
        }

        public void AddGarage(IGarage<IVehicle> garage)
        {
            ArgumentNullException.ThrowIfNull(garage, nameof(garage));
            garages.Add(garage);
            if (GarageCount == 1)
            {
                SetCurrentGarage(0);
            }
        }

        public void RemoveGarage(int index)
        {
            garages.RemoveAt(index);

            if (CurrentGarageIndex == index)
            {
                if (index >= GarageCount)
                {
                    if (index > 0)
                    {
                        SetCurrentGarage(index - 1);
                        return;
                    }
                    SetCurrentGarage(0);
                }
                else
                {
                    SetCurrentGarage(index);
                }
            }
            else if (index < CurrentGarageIndex)
            {
                SetCurrentGarage(CurrentGarageIndex - 1);
            }
        }

        public void SetCurrentGarage(int index)
        {
            if (GarageCount > 0)
            {
                CurrentGarageIndex = index;
                return;
            }
            else
            {
                CurrentGarageIndex = 0;
            }
        }

        public IEnumerable<IGarage<IVehicle>> GetGarages()
        {
            return garages.AsEnumerable();
        }

        public IEnumerable<IVehicle> GetVehicles()
        {
            return CurrentGarage.AsEnumerable();
        }

        public bool AddVehicle(int parkingIndex, IVehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
            return CurrentGarage.SetVehicle(parkingIndex, vehicle);
        }

        public IVehicle? GetVehicle(int parkingIndex)
        {
            return CurrentGarage.GetVehicle(parkingIndex);
        }

        public bool RemoveVehicle(int parkingIndex)
        {
            return CurrentGarage.SetVehicle(parkingIndex, null);
        }

        public int FindVehicleParkingIndex(string registrationNumber)
        {
            registrationNumber = registrationNumber.ToUpper();
            return GetNotAvailablePlaces().Where(i => GetVehicle(i)!.RegistrationNumber == registrationNumber).FirstOrDefault(-1);
        }

        public IEnumerable<IVehicle> FindVehicles(string? registrationNumber, string? color, int? wheels)
        {
            return CurrentGarage.Where(v
                => v is not null
                && (registrationNumber is null || v.RegistrationNumber == registrationNumber.ToUpper())
                && (color is null || v.Color == color.ToLower())
                && (wheels is null || v.Wheels == wheels)

                );
        }

        public IEnumerable<int> GetAvailablePlaces()
        {
            return CurrentGarage.Select((v, i) => v == null ? i : -1).Where(v => v != -1);
        }

        public IEnumerable<int> GetNotAvailablePlaces()
        {
            return CurrentGarage.Select((v, i) => v != null ? i : -1).Where(v => v != -1);
        }

        public void RemoveAllGarages()
        {
            garages.Clear();
            SetCurrentGarage(0);
        }

        public IGarage<IVehicle> GetGarage(int index)
        {
            return garages[index];
        }
    }
}
