using System.Collections.Generic;
using System.Threading.Tasks;

 public interface ITripService
    {
        Task<bool> AddTrip(Trip trip);
        Task<List<Trip>> GetAllTrips();
        Task<Trip?> GetTripById(string tripId);
        Task<bool> UpdateTrip(string tripId, Trip trip);
        Task<bool> DeleteTrip(string tripId);
        Task<List<Trip>> GetTripsByDestination(string destination);
    }