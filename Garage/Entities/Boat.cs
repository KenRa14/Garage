using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Entity
{
    public class Boat : Vehicle
    {
        public Boat(string registrationNumber, string color, int wheels, int length) : base(registrationNumber, color, wheels)
        {
            Length = length;
        }

        private int length;

        public int Length
		{
			get { return length; }
			set 
			{ 
				if (value < 1)
				{
					throw new ArgumentException($"{nameof(Length)} can't be less than 1");
				}
				length = value; 
			}
		}

        public override string ToString()
        {
            return $"{base.ToString()}, Length: {Length}";
        }

        public override bool Equals(IVehicle? other)
        {

            if (!base.Equals(other))
            {
                return false;
            }
            var boat = other as Boat;
            return Length == boat!.Length;
        }
    }
}
