using System.Data.SqlClient;
using Code.Model;
using Microsoft.Extensions.Configuration;

namespace Code;

class Program
{
    static void Main()
    {   
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();
        string connectionString = config.GetConnectionString("DefaultConnection");

        Console.WriteLine("Using connection: " + connectionString);

// Use AppDbContext
        using var context = new AppDbContext(config);
        context.Database.EnsureCreated(); // Optional: creates DB if not exists

        Console.WriteLine("✅ Using EF DbContext!");
        UserCrud(config);
    }

    private static void UserCrud(IConfiguration configuration)
    {
        using var context = new AppDbContext(configuration);
        var newPerson = new User() { Name = "Alice", Age = new Random().Next(21,65)};
        context.Users.Add(newPerson);
        context.SaveChanges();
        Console.WriteLine($"Added: {newPerson.Name}");

        // READ
        var people = context.Users.ToList();
        Console.WriteLine("People in DB:");
        foreach (var person in people)
        {
            Console.WriteLine($"ID: {person.Id}, Name: {person.Name}, Age: {person.Age}");
        }

        // UPDATE
        var personToUpdate = context.Users.FirstOrDefault(p => p.Name == "Alice");
        if (personToUpdate != null)
        {
            personToUpdate.Age = 31;
            context.SaveChanges();
            Console.WriteLine("Updated Alice's age.");
        }

        // DELETE : All User
        var personToDelete = context.Users.Where(u => u.Name == "Alice").ToList();
        foreach (var person in personToDelete)
        {
            // context.Users.Remove(person);
            Console.WriteLine("Deleted Alice."+person.Id);
        }
        context.SaveChanges();
    }
}