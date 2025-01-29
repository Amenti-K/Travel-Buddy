using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TripService : ITripService
{
    private readonly IMongoCollection<Trip> _tripCollection;

    public TripService(MongoDbService dbService)
    {
        _tripCollection = dbService.Trips;
    }

    public async Task<bool> AddTrip(Trip newTrip)
    {
        try
        {
            newTrip.TripId = null;
            await _tripCollection.InsertOneAsync(newTrip);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<Trip>> GetAllTrips()
    {
        try
        {
            var trips = await _tripCollection.Find(_ => true).ToListAsync();
            return trips ?? new List<Trip>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching trips: {ex.Message}");
            return new List<Trip>();
        }
    }

    public async Task<Trip?> GetTripById(string tripId)
    {
        try
        {
            return await _tripCollection.Find(t => t.TripId == tripId).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateTrip(string tripId, Trip updatedTrip)
    {
        try
        {
            var filter = Builders<Trip>.Filter.Eq(t => t.TripId, tripId);
            var update = Builders<Trip>.Update
                .Set(t => t.TripName, updatedTrip.TripName)
                .Set(t => t.TripDescription, updatedTrip.TripDescription)
                .Set(t => t.DepartureDate, updatedTrip.DepartureDate)
                .Set(t => t.ReturnDate, updatedTrip.ReturnDate)
                .Set(t => t.StartingLocation, updatedTrip.StartingLocation)
                .Set(t => t.Destination, updatedTrip.Destination);

            var result = await _tripCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating trip: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteTrip(string tripId)
    {
        try
        {
            var result = await _tripCollection.DeleteOneAsync(t => t.TripId == tripId);
            return result.DeletedCount > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<Trip>> GetTripsByDestination(string destination)
    {
        try
        {
            return await _tripCollection.Find(t => t.Destination == destination).ToListAsync();
        }
        catch (Exception)
        {
            return new List<Trip>();
        }
    }
}
