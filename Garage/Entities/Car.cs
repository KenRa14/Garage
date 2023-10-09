using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Entity
{
    public class Car : Vehicle
    {
        public Car(string registrationNumber, string color, int wheels, string fuelType) : base(registrationNumber, color, wheels)
        {
            FuelType = fuelType;
        }

        private string fuelType = string.Empty;

        public string FuelType
		{
			get { return fuelType; }
			set 
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					fuelType = string.Empty;
					return;
				}
				fuelType = value.Trim().ToLower(); 
			}
		}
        public override string ToString()
        {
            return $"{base.ToString()}, Fuel type: {FuelType}";
        }

        public override bool Equals(IVehicle? other)
        {

            if (!base.Equals(other))
            {
                return false;
            }
            var car = other as Car;
            return FuelType == car!.FuelType;
        }
    }
}
