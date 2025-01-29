using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



    [ApiController]
    [Route("[Controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripService? _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
public async Task<ActionResult> GetAllTrips()
{
    try
    {
        if (_tripService == null)
        {
            return StatusCode(500, "Trip service is unavailable.");
        }

        var trips = await _tripService.GetAllTrips();

        return Ok(trips);
    }
    catch (Exception e)
    {
        return NotFound(new { message = e.Message });
    }
}


        [HttpGet("{id}")]
 public async Task<ActionResult> GetTripById(string id)
{
    try
    {
        var trip = await _tripService?.GetTripById(id)!;
        if (trip == null)
        {
            return NotFound(new { Message = $"No trip found with ID: {id}" });
        }
        return Ok(trip);
    }
    catch (Exception e)
    {
        return StatusCode(500, new { Message = "An error occurred", Details = e.Message });
    }
}


        [HttpPost]
        public async Task<ActionResult> AddTrip(Trip newTrip)
        {
            try
            {
                if (await _tripService?.AddTrip(newTrip)! == true)
                {
                    return Ok("Trip added successfully");
                }
                throw new Exception("Failed to add trip!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

       [HttpPut("{id}")]
public async Task<ActionResult> UpdateTrip(string id, Trip updatedTrip)
{
    try
    {
        if (_tripService == null)
        {
            return StatusCode(500, "Trip service is unavailable.");
        }

        bool updated = await _tripService.UpdateTrip(id, updatedTrip);
        
        if (updated)
        {
            return Ok(new { message = "Trip updated successfully" });
        }

        return NotFound(new { message = $"No trip found with id {id} to update!" });
    }
    catch (Exception e)
    {
        return StatusCode(500, new { message = $"An error occurred: {e.Message}" });
    }
}


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrip(string id)
        {
            try
            {
                if (await _tripService?.DeleteTrip(id)! == true)
                {
                    return Ok("Trip deleted successfully");
                }
                throw new Exception($"No trip found with id => {id} to delete!");
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("destination/{destination}")]
        public async Task<ActionResult> GetTripsByDestination(string destination)
        {
            try
            {
                var trips = await _tripService?.GetTripsByDestination(destination)!;
                if (trips != null && trips.Count > 0)
                {
                    return Ok(trips);
                }
                throw new Exception($"No trips found for destination => {destination}");
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }

