using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Entity
{
	public class Airplane : Vehicle
    {

        public Airplane(string registrationNumber, string color, int wheels, int numberOfEngines) : base(registrationNumber, color, wheels)
        {
            NumberOfEngines = numberOfEngines;
        }
        private int numberOfEngines;

        public int NumberOfEngines
        {
			get { return numberOfEngines; }
			set 
			{
				if (value < 1)
				{
					throw new ArgumentException($"{nameof(NumberOfEngines)} can't be less than 1");
				}
				numberOfEngines = value; 
			}
		}

        public override string ToString()
        {
			return $"{base.ToString()}, Number of engines: {NumberOfEngines}";
        }

        public override bool Equals(IVehicle? other)
        {

            if (!base.Equals(other))
            {
                return false;
            }
            var airplane = other as Airplane;
            return numberOfEngines == airplane!.NumberOfEngines;
        }
    }
}
