namespace VehicleRentalAPI.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public decimal DailyRate { get; set; }

        public bool Available { get; set; }
    }
}
