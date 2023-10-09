using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class Garage<T> : IGarage<T> where T : IVehicle
    {
        private readonly T[] vehicles;

        public Garage(int capacity, string name = "")
        {
            if (capacity < 1)
            {
                throw new ArgumentException($"{nameof(capacity)} cannot be less than 1");
            }
            vehicles = new T[capacity];
            Name = name;
        }

        public Garage(int capacity, IEnumerable<T> vehicles, string name = "")
        {
            if (capacity < 1)
            {
                throw new ArgumentException($"{nameof(capacity)} cannot be less than 1");
            }

            T[] vehiclesArray = vehicles.ToArray();
            if (capacity < vehiclesArray.Length)
            {
                throw new ArgumentException($"{nameof(capacity)} must be equal or greater than the amount of vehicles and nulls");
            }
            this.vehicles = new T[capacity];
            this.vehicles = vehiclesArray;
            VehicleCount = vehiclesArray.Where(v => v is not null).Count();
            Name = name;
        }

        public Garage(IEnumerable<T> vehicles, string name = "")
        {
            if (!vehicles.Any())
            {
                throw new ArgumentException($"{nameof(vehicles)} can't be empty");
            }
            this.vehicles = vehicles.ToArray();
            VehicleCount = this.vehicles.Where(v => v is not null).Count();
            Name = name;
        }

        public int VehicleCount { get; protected set; }

        public int Capacity { get => vehicles.Length; }

        public string Name { get; set; }

        public bool SetVehicle(int parkingIndex, T? vehicle)
        {
            if (vehicle is null)
            {
                if (vehicles[parkingIndex] != null)
                {
                    vehicles[parkingIndex] = vehicle!;
                    VehicleCount--;
                    return true;
                }
            }
            else if (vehicles[parkingIndex] == null)
            {
                vehicles[parkingIndex] = vehicle;
                VehicleCount++;
                return true;
            }
            return false;
        }

        public T? GetVehicle(int parkingIndex)
        {
            return vehicles[parkingIndex];
        }



        public IEnumerator<T> GetEnumerator()
        {
            foreach (var vehicle in vehicles)
            {
                yield return vehicle;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return $"Name: {Name}, capacity: {Capacity}, Vehicles: {VehicleCount}, Available space: {Capacity - VehicleCount}";
        }

        public virtual bool Equals(IGarage<T>? other)
        {
            if (other is null 
                || other.GetType() != GetType()
                || other.Name != Name 
                || other.Capacity != Capacity 
                || other.VehicleCount != VehicleCount)
            {
                return false;
            }

            return this.SequenceEqual(other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is IGarage<T> garage)
                return Equals(garage);
            return false;
        }
    }
}
