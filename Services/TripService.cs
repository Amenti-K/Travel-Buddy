using MongoDB.Driver;

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


    public class TripService : ITripService
    {
        private readonly IMongoCollection<Trip>? _tripCollection;

       public TripService(IConfiguration configuration)
{
    try
    {
        var connectionString = configuration["MongoDbSettings:ConnectionString"];
        var databaseName = configuration["MongoDbSettings:DatabaseName"];
        var collectionName = configuration["MongoDbSettings:CollectionName"];

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _tripCollection = database.GetCollection<Trip>(collectionName);

        Console.WriteLine($"Connected to MongoDB: {databaseName}, Collection: {collectionName}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"MongoDB Connection Error: {ex.Message}");
        throw;
    }
}

        public async Task<bool> AddTrip(Trip newTrip)
        {
            try
            {
                newTrip.TripId = null;
                await _tripCollection?.InsertOneAsync(newTrip)!;
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

        if (trips == null || trips.Count == 0)
        {
            throw new Exception("No trips found in the database.");
        }

        return trips;
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
                var trip = await _tripCollection.Find(t => t.TripId == tripId).FirstOrDefaultAsync();
                return trip ?? throw new Exception("Trip not found");
            }
            catch (Exception)
            {
                return null!;
            }
        }

  public async Task<bool> UpdateTrip(string tripId, Trip updatedTrip)
{
    try
    {
        if (_tripCollection == null)
        {
            throw new InvalidOperationException("Database collection is not initialized.");
        }

        var filter = Builders<Trip>.Filter.Eq(t => t.TripId, tripId);
        var update = Builders<Trip>.Update
            .Set(t => t.TripName, updatedTrip.TripName)
            .Set(t => t.TripDescription, updatedTrip.TripDescription)
            .Set(t => t.DepartureDate, updatedTrip.DepartureDate)
            .Set(t => t.ReturnDate, updatedTrip.ReturnDate)
            .Set(t => t.StartingLocation, updatedTrip.StartingLocation)
            .Set(t => t.Destination, updatedTrip.Destination);

        var result = await _tripCollection.UpdateOneAsync(filter, update);

        if (result.MatchedCount == 0)
        {
            Console.WriteLine($"No trip found with ID {tripId}.");
            return false;
        }

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
                var status = await _tripCollection?.DeleteOneAsync(t => t.TripId == tripId)!;
                return status.DeletedCount > 0;
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
