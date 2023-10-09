using Garage.Entity;

namespace Garage.Tests.Helpers
{
    public static class Vehicles
    {
        public const string RegistrationNumber = "ANY";
        public const string Color = "any"; 
        public const string DifferentRegistrationNumber = "Ha";
        public const string DifferentColor = "mhe";
        public const int Wheels = 0;
        public const int NumberOfEngines = 1;
        public const int CylinderVolume = 1;
        public const string FuelType = "hu";
        public const string DifferentFuelType = "hustle";
        public const int NumberOfSeats = 1;
        public const int Length = 1;


        private static readonly IVehicle[] vehicles10 = new IVehicle[]
        {
            new Car("MCV453", "Awesome", 7, "Diesel"),
            new Car("XOI707", "Cream", 3, "Gasoline"),
            new Car("UVC514", "Blue", 0, "Diesel"),
            new Airplane("QIU142", "blue", 7, 2),
            new Motorcycle("GUV034", "green", 6, 112),
            new Boat("MKA317", "blue", 3, 14),
            new Motorcycle("FIK710", "Linen", 6, 72),
            new Bus("MWW835", "green", 7, 58),
            new Car("KVX601", "Buff", 5, "Gasoline"),
            new Boat("YUJ867", "Bright ube", 4, 56),
        };

        public static IEnumerable<(string, int)> Vehicles10TypeCount { get; } = new List<(string, int)>(5)
        {
            (nameof(Car), 4),
            (nameof(Airplane), 1),
            (nameof(Motorcycle), 2),
            (nameof(Boat), 2),
            (nameof(Bus), 1)

        }.AsEnumerable();

        private static readonly IVehicle[] vehicles10WithNulls = new IVehicle[vehicles10.Length];

        public static IEnumerable<(string vehicleTypeName, int count)> Vehicles10WithNullsTypeCount { get; private set; }

        static Vehicles()
        {
            vehicles10.CopyTo(vehicles10WithNulls, 0);
            vehicles10WithNulls[1] = null!;
            vehicles10WithNulls[7] = null!;

            Vehicles10WithNullsTypeCount = new List<(string, int)>(5)
            {
                (nameof(Car), 3),
                (nameof(Airplane), 1),
                (nameof(Motorcycle), 2),
                (nameof(Boat), 2)

            }.AsEnumerable();
        }

        public static IVehicle GetAVehicleOf10(int index)
        {
            return vehicles10[index];
        }

        public static IEnumerable<IVehicle> GetVehicles10()
        {
            return vehicles10.AsEnumerable();
        }

        public static IVehicle GetAVehicleOf10WithNulls(int index)
        {
            return vehicles10WithNulls[index];
        }

        public static IEnumerable<IVehicle> GetVehicles10WithNulls()
        {
            return vehicles10WithNulls.AsEnumerable();
        }
    }
}
