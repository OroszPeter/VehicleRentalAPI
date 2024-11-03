namespace SimplePizzaRentalAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VehicleRentalAPI.Context;
    using VehicleRentalAPI.DTO;
    using VehicleRentalAPI.Entities;

    namespace YourNamespace.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class RentalsController(VehicleRentalContext context) : ControllerBase
        {

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
            {
                var rentals = await context.Rentals
                    .Include(r => r.Customer)
                    .Include(r => r.Vehicle)
                    .ToListAsync();

                return Ok(rentals);
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Rental>> GetRental(int id)
            {
                var rental = await context.Rentals
                    .Include(r => r.Customer)
                    .Include(r => r.Vehicle)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (rental == null)
                {
                    return NotFound("A kölcsönzés nem található.");
                }

                return Ok(rental);
            }

            [HttpPost]
            public async Task<ActionResult<Rental>> CreateRental(RentalDto rentalDto)
            {
                // Ellenőrzés és rental létrehozása
                var customer = await context.Customers.FindAsync(rentalDto.CustomerId);
                var vehicle = await context.Vehicles.FindAsync(rentalDto.VehicleId);

                if (customer == null || vehicle == null)
                {
                    return BadRequest("Invalid CustomerId or VehicleId");
                }

                var rental = new Rental
                {
                    CustomerId = rentalDto.CustomerId,
                    VehicleId = rentalDto.VehicleId,
                    RentalDate = rentalDto.RentalDate,
                    ReturnDate = rentalDto.ReturnDate,
                    Customer = customer,
                    Vehicle = vehicle
                };

                context.Rentals.Add(rental);
                await context.SaveChangesAsync();

                // Az újonnan létrehozott Rental újra lekérése Include-dal
                var createdRental = await context.Rentals
                    .Include(r => r.Customer)
                    .Include(r => r.Vehicle)
                    .FirstOrDefaultAsync(r => r.Id == rental.Id);

                return CreatedAtAction(nameof(GetRental), new { id = createdRental.Id }, createdRental);
            }


            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateRental(int id, Rental updatedRental)
            {
                var rentalToUpdate = await context.Rentals.FirstOrDefaultAsync(o => o.Id == id);

                if (rentalToUpdate is null)
                {
                    return NotFound("A kölcsönzés nem található.");
                }

                rentalToUpdate.RentalDate = updatedRental.RentalDate;
                rentalToUpdate.ReturnDate = updatedRental.ReturnDate;
                await context.SaveChangesAsync();

                return Ok("Sikeres módosítás!");
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteRental(int id)
            {
                var rental = await context.Rentals.FindAsync(id);
                if (rental == null)
                {
                    return NotFound("A kölcsönzés nem található.");
                }

                context.Rentals.Remove(rental);
                await context.SaveChangesAsync();

                return Ok("Sikeres törlés.");
            }
        }
    }
}

