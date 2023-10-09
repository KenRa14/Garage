using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Entity
{
    public class Bus : Vehicle
    {
        public Bus(string registrationNumber, string color, int wheels, int numberOfSeats) : base(registrationNumber, color, wheels)
        {
            NumberOfSeats = numberOfSeats;
        }

        private int numberOfSeats;

        public int NumberOfSeats
		{
			get { return numberOfSeats; }
			set 
			{ 
				if (value < 1)
				{
					throw new ArgumentException($"{nameof(NumberOfSeats)} can't be less than 1");
				}
				numberOfSeats = value; 
			}
		}
        public override string ToString()
        {
            return $"{base.ToString()}, Number of seats: {NumberOfSeats}";
        }

        public override bool Equals(IVehicle? other)
        {
            if (!base.Equals(other))
            {
                return false;
            }
            var bus = other as Bus;
            return NumberOfSeats == bus!.NumberOfSeats;
        }
    }
}
