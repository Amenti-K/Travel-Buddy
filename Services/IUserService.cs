using System.Collections.Generic;

public interface IUserService{
    Task<List<User>> GetAllUsers();
    Task<User> GetUserByEmail(string email);
    Task<bool> AddUser(User newUser);
    Task<bool> UpdateUser(string email, User updatedUser);
    Task<bool> DeleteUser(string email);
}