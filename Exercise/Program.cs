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

// Pass connection string to DbContext
        // using var context = new AppDbContext(connectionString);

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("✅ Connected to SQL Server!");

            var command = new SqlCommand("SELECT @@VERSION", connection);
            var version = command.ExecuteScalar();
            Console.WriteLine($"SQL Server version: {version}");
            // for (int i = 0; i < 100000000; i++)
            {
                UserCrud();
            }
        }
    }

    private static void UserCrud()
    {
        using var context = new AppDbContext();
        var newPerson = new User() { Name = "Alice", Age = 30 };
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
            context.Users.Remove(person);
            Console.WriteLine("Deleted Alice."+person.Id);
        }
        context.SaveChanges();
    }
}