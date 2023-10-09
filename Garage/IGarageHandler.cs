namespace Garage
{
    public interface IGarageHandler
    {
        IGarage<IVehicle> CurrentGarage { get; }
        int CurrentGarageIndex { get; }
        int GarageCapacity { get; }
        int GarageCount { get; }
        int VehicleCount { get; }
        void AddGarage(IGarage<IVehicle> garage);
        bool AddVehicle(int parkingIndex, IVehicle vehicle);
        void CreateGarage(IEnumerable<IVehicle> vehicles, string name = "");
        void CreateGarage(int capacity, string name = "");
        void CreateGarage(int capacity, IEnumerable<IVehicle> vehicles, string name = "");
        int FindVehicleParkingIndex(string registrationNumber);
        IEnumerable<IVehicle> FindVehicles(string? registrationNumber, string? color, int? wheels);
        IEnumerable<int> GetAvailablePlaces();
        IGarage<IVehicle> GetGarage(int index);
        IEnumerable<IGarage<IVehicle>> GetGarages();
        IEnumerable<int> GetNotAvailablePlaces();
        IVehicle? GetVehicle(int parkingIndex);
        IEnumerable<IVehicle> GetVehicles();
        void RemoveAllGarages();
        void RemoveGarage(int index);
        bool RemoveVehicle(int parkingIndex);
        void SetCurrentGarage(int index);
    }
}