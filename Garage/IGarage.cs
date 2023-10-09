namespace Garage
{
    public interface IGarage<T> : IEquatable<IGarage<T>>, IEnumerable<T> where T : IVehicle
    {
        int Capacity { get; }
        int VehicleCount { get; }
        string Name { get; set; }
        T? GetVehicle(int parkingIndex);
        bool SetVehicle(int parkingIndex, T? vehicle);
        string ToString();
    }
}