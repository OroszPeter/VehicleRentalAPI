using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalAPI.Context;
using VehicleRentalAPI.Entities;

namespace SimpleVehicleOrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController(VehicleRentalContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            var vehicles = await context.Vehicles.ToListAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            var vehicle = await context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound("A jármű nem található.");
            }

            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, Vehicle updatedVehicle)
        {
            var vehicleToUpdate = await context.Vehicles.FirstOrDefaultAsync(p => p.Id == id);

            if (vehicleToUpdate is null)
            {
                return NotFound("A vehicle nem található.");
            }

            vehicleToUpdate.Model = updatedVehicle.Model;
            vehicleToUpdate.LicensePlate = updatedVehicle.LicensePlate;
            vehicleToUpdate.DailyRate = updatedVehicle.DailyRate;
            vehicleToUpdate.Available = updatedVehicle.Available;
            await context.SaveChangesAsync();

            return Ok("Sikeres módosítás!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound("A jármű nem található.");
            }

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();

            return Ok("Sikeres törlés.");
        }
    }
}
