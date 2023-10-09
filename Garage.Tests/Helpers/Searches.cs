using Garage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Tests.Helpers
{
    public static class Searches
    {
        private static readonly IVehicle[] searchVehicles = new IVehicle[]
        {
            new Car("HAP477", "black", 4, "Gasoline") ,
            new Car("IRT690", "Blue", 2, "Diesel") ,
            new Airplane("QOF972", "Black", 0, 1) ,
            new Bus("MCC612", "Green", 0, 8) ,
            new Bus("PKP658", "Green", 6, 6) ,
            /*new Boat("CCD513", "Green", 7, 94)*/  null!,
            new Car("BVW385", "Green", 6, "Gasoline") ,
            new Car("PGY959", "green", 0, "Diesel") ,
            new Airplane("TVB046", "Blue", 6, 5) ,
            new Motorcycle("ZBT794", "black", 1, 40) ,
            new Motorcycle("JEL680", "black", 4, 24) ,
            new Motorcycle("BNU418", "Blue", 5, 42) ,
            new Airplane("CSQ888", "green", 7, 1) ,
            /*new Airplane("OFL024", "Black", 6, 4)*/  null!,
            new Boat("XOY807", "Blue", 3, 72) ,
            new Boat("LSL768", "Black", 7, 42) ,
            /*new Car("FNQ179", "Green", 7, "Diesel")*/  null!,
            new Motorcycle("RAE405", "Blue", 3, 38) ,
            new Bus("MON429", "black", 1, 53) ,
            /*new Car("ZNP827", "Blue", 1, "Diesel")*/  null!,
            new Motorcycle("APG823", "blue", 6, 19) ,
            new Motorcycle("ORM005", "Black", 3, 104) ,
            new Bus("CMF710", "Black", 2, 42) ,
            new Boat("RHM449", "Blue", 4, 56) ,
            new Airplane("AZL192", "Black", 1, 3) ,
            new Bus("QUW552", "green", 5, 18) ,
            new Boat("GYX522", "green", 6, 48),
            /*new Airplane("KCV317", "Black", 7, 5)*/  null!,
            new Boat("LAZ605", "green", 0, 41) ,
            new Airplane("IEU683", "green", 7, 2)
        };

        public static IEnumerable<IVehicle> GetSearchVehicles()
        {
            return searchVehicles.AsEnumerable();
        }

        #region Util search methods

        private static readonly List<(string?, string?, int?, int?)> searchBoat_Searches = new()
        {
            new ("RHM449", null, null, null),
            new (null, "Blue", null, null),
            new (null, null, 6, null),
            new (null, null, null, 56),
            new ("LSL768", "Black", 7, 42)
        };

        private static readonly int[][] searchBoat_Expected = new int[][]
        {
            new int[] { 23 },
            new int[] { 14, 23 },
            new int[] { 26 },
            new int[] { 23 },
            new int[] { 15 }
        };

        public static (string?, string?, int?, int?) GetASearchOF5_SearchBoat(int index)
        {
            return searchBoat_Searches[index];
        }

        public static IEnumerable<int> GetAnExpectedOF5_SearchBoat(int index)
        {
            return searchBoat_Expected[index].AsEnumerable();
        }

        private static readonly List<(string?, string?, int?, int?)> searchBus_Searches = new()
        {
            new ("CMF710", null, null, null),
            new (null, "Green", null, null),
            new (null, null, 1, null),
            new (null, null, null, 8),
            new ("MCC612", "Green", 0, 8)
        };

        private static readonly int[][] searchBus_Expected = new int[][]
        {
            new int[] { 22 },
            new int[] { 3, 4, 25 },
            new int[] { 18 },
            new int[] { 3 },
            new int[] { 3 }
        };

        public static (string?, string?, int?, int?) GetASearchOF5_SearchBus(int index)
        {
            return searchBus_Searches[index];
        }

        public static IEnumerable<int> GetAnExpectedOF5_SearchBus(int index)
        {
            return searchBus_Expected[index].AsEnumerable();
        }

        private static readonly List<(string?, string?, int?, string?)> searchCar_Searches = new()
        {
            new ("BVW385", null, null, null),
            new (null, "green", null, null),
            new (null, null, 2, null),
            new (null, null, null, "Diesel"),
            new ("HAP477", "black", 4, "Gasoline")
        };

        private static readonly int[][] searchCar_Expected = new int[][]
        {
            new int[] { 6 },
            new int[] { 6, 7 },
            new int[] { 1 },
            new int[] { 1, 7 },
            new int[] { 0 }
        };

        public static (string?, string?, int?, string?) GetASearchOF5_SearchCar(int index)
        {
            return searchCar_Searches[index];
        }

        public static IEnumerable<int> GetAnExpectedOF5_SearchCar(int index)
        {
            return searchCar_Expected[index].AsEnumerable();
        }

        private static readonly List<(string?, string?, int?, int?)> searchMotorcycle_Searches = new()
        {
            new ("ORM005", null, null, null),
            new (null, "blue", null, null),
            new (null, null, 4, null),
            new (null, null, null, 38),
            new ("JEL680", "black", 4, 24)
        };

        private static readonly int[][] searchMotorcycle_Expected = new int[][]
        {
            new int[] { 21 },
            new int[] { 11, 17, 20 },
            new int[] { 10 },
            new int[] { 17 },
            new int[] { 10 }
        };

        public static (string?, string?, int?, int?) GetASearchOF5_SearchMotorcycle(int index)
        {
            return searchMotorcycle_Searches[index];
        }

        public static IEnumerable<int> GetAnExpectedOF5_SearchMotorcycle(int index)
        {
            return searchMotorcycle_Expected[index].AsEnumerable();
        }

        private static readonly List<(string?, string?, int?, int?)> searchAirplane_Searches = new()
        {
            new ("AZL192", null, null, null),
            new (null, "black", null, null),
            new (null, null, 7, null),
            new (null, null, null, 5),
            new ("CSQ888", "green", 7, 1)
        };

        private static readonly int[][] searchAirplane_Expected = new int[][]
        {
            new int[] { 24 },
            new int[] { 2, 24 },
            new int[] { 12, 29 },
            new int[] { 8 },
            new int[] { 12 }
        };

        public static (string?, string?, int?, int?) GetASearchOF5_SearchAirplane(int index)
        {
            return searchAirplane_Searches[index];
        }

        public static IEnumerable<int> GetAnExpectedOF5_SearchAirplane(int index)
        {
            return searchAirplane_Expected[index].AsEnumerable();
        }

        private static readonly List<(string?, string?, int?)> searchVehicle_Searches = new()
        {
            new ("IRT690", null, null),
            new (null, "Blue", null),
            new (null, null, 0),
            new ("TVB046", "Blue", 6)
        };

        private static readonly int[][] searchVehicle_Expected = new int[][]
        {
            new int[] { 1 },
            new int[] { 1, 8, 11, 14, 17, 20, 23 },
            new int[] { 2, 3, 7, 28 },
            new int[] { 8 },
        };
        public static (string?, string?, int?) GetASearchOF4_SearchVehicle(int index)
        {
            return searchVehicle_Searches[index];
        }

        public static IEnumerable<int> GetAnExpectedOF4_SearchVehicle(int index)
        {
            return searchVehicle_Expected[index].AsEnumerable();
        }

        #endregion

        #region GarageHandler find vehicles method

        private static readonly List<(string?, string?, int?)> findVehicles_Searches = new()
        {
            new ("IRT690", null, null),
            new (null, "Blue", null),
            new (null, null, 0),
            new ("TVB046", "Blue", 6)
        };

        private static readonly IVehicle[][] findVehicles_Expected = new IVehicle[][]
        {
            new IVehicle[] { searchVehicles[1] },

            new IVehicle[] { searchVehicles[1], searchVehicles[8], searchVehicles[11],
                searchVehicles[14], searchVehicles[17], searchVehicles[20], searchVehicles[23] },

            new IVehicle[] { searchVehicles[2], searchVehicles[3], searchVehicles[7], searchVehicles[28] },

            new IVehicle[] { searchVehicles[8] }
        };
        public static (string?, string?, int?) GetASearchOF4_FindVehicles(int index)
        {
            return findVehicles_Searches[index];
        }

        public static IEnumerable<IVehicle> GetAnExpectedOF4_FindVehicles(int index)
        {
            return findVehicles_Expected[index].AsEnumerable();
        }

        #endregion

    }
}
