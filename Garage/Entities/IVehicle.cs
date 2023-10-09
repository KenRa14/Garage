namespace Garage.Entity
{
    public interface IVehicle: IEquatable<IVehicle>
    {
        string Color { get; set; }
        string RegistrationNumber { get; set; }
        int Wheels { get; set; }
    }
}