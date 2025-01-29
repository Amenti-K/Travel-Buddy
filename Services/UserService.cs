using System;
using System.Collections.Generic;
using System.Linq;

public class UserService : IUserService
{
    private static List<User> users = new List<User>
    {
        new User { UserId = Guid.NewGuid(), Email = "john@example.com", UserName = "JohnDoe", PhoneNumber = "1234567890", Password = "pass123" },
        new User { UserId = Guid.NewGuid(), Email = "jane@example.com", UserName = "JaneDoe", PhoneNumber = "0987654321", Password = "pass456" },
        new User { UserId = Guid.NewGuid(), Email = "alex@example.com", UserName = "AlexSmith", PhoneNumber = "1122334455", Password = "pass789" }
    };

    public List<User> GetAllUsers()
    {
        return users;
    }

    public User GetUserByEmail(string email)
    {
        return users.FirstOrDefault(u => u.Email == email);
    }

    public bool AddUser(User newUser)
    {
        if (users.Any(u => u.Email == newUser.Email || u.PhoneNumber == newUser.PhoneNumber))
        {
            return false; // User already exists
        }

        newUser.UserId = Guid.NewGuid();
        users.Add(newUser);
        return true;
    }

    public bool UpdateUser(string email, User updatedUser)
    {
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null) return false;

        user.UserName = updatedUser.UserName;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.Password = updatedUser.Password;
        return true;
    }

    public bool DeleteUser(string email)
    {
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null) return false;

        users.Remove(user);
        return true;
    }
}
