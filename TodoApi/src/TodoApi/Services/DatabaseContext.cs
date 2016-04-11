using Microsoft.Data.Entity;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class DatabaseContext : DbContext
    {
		public DbSet<Todo> Todos { get; set; } 

		public DbSet<User> Users { get; set; }

	    public DatabaseContext()
	    {
		    Database.EnsureCreated();
	    }
    }
}
