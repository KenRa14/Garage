using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Garage.Entity
{
    public class Vehicle : IVehicle
    {

        public Vehicle(string registrationNumber, string color, int wheels)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            Wheels = wheels;
        }

        private string registrationNumber = string.Empty;

        public string RegistrationNumber
        {
            get => registrationNumber;
            set
            {
                string toSet = (value is null ? null : value.Trim())!;
                ArgumentException.ThrowIfNullOrEmpty(toSet,nameof(RegistrationNumber));
                registrationNumber = toSet.ToUpper();
            }
        }

        private string color = string.Empty;

        public string Color
        {
            get => color;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    color = string.Empty;
                    return;
                }
                color = value.Trim().ToLower();
            }
        }

        private int wheels;


        public int Wheels
        {
            get => wheels;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"{nameof(Wheels)} can't be a negative number");
                }
                wheels = value;
            }
        }

        public override string ToString()
        {
            return $"Registration number: {RegistrationNumber}, Color: {color}, Wheels: {Wheels}";
        }

        public virtual bool Equals(IVehicle? other)
        {
            
            if (other is not null)
            {
                return GetType() == other.GetType()
                    && RegistrationNumber == other.RegistrationNumber
                    && Color == other.Color
                    && Wheels == other.Wheels;
            }

            return false;
        }

        public override bool Equals(object? obj)
        {

            if (obj is IVehicle vehicle)
            {
                return Equals(vehicle);
            }

            return false;
        }
    }
}
