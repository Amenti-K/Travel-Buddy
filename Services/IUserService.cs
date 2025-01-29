using System.Collections.Generic;

public interface IUserService{
    List<User> GetAllUsers();
    User GetUserByEmail(string email);
    bool AddUser(User newUser);
    bool UpdateUser(string email, User updatedUser);
    bool DeleteUser(string email);
}