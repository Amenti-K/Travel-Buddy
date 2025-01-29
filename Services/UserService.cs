using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(MongoDbService dbService)
    {
        _users = dbService.Users;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _users.Find(user => true).ToListAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
    }

    public async Task<bool> AddUser(User newUser)
    {
        var existingUser = await GetUserByEmail(newUser.Email);
        if (existingUser != null) return false; // Prevent duplicate emails

        await _users.InsertOneAsync(newUser);
        return true;
    }

    public async Task<bool> UpdateUser(string email, User updatedUser)
    {
        var result = await _users.ReplaceOneAsync(u => u.Email == email, updatedUser);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteUser(string email)
    {
        var result = await _users.DeleteOneAsync(u => u.Email == email);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
