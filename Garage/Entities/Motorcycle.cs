using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Entity
{
    public class Motorcycle: Vehicle
    {
        public Motorcycle(string registrationNumber, string color, int wheels, int cylinderVolume) : base(registrationNumber, color, wheels)
        {
            CylinderVolume = cylinderVolume;
        }

        private int cylinderVolume;

        public int CylinderVolume
        {
            get { return cylinderVolume; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException($"{nameof(CylinderVolume)} can't be less than 1");
                }
                cylinderVolume = value;
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Cylinder volume: {CylinderVolume}";
        }

        public override bool Equals(IVehicle? other)
        {
            if (!base.Equals(other))
            {
                return false;
            }
            var motorcycle = other as Motorcycle;
            return CylinderVolume == motorcycle!.CylinderVolume;
        }
    }
}
