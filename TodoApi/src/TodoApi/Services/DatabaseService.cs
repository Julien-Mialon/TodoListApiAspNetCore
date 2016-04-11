using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class DatabaseService
    {
		private DatabaseContext _context { get; }

	    public DatabaseService(DatabaseContext context)
	    {
		    _context = context;
	    }

	    public User AddUser(string login, string password)
	    {
		    User user = GetUser(login);

		    if (user != null)
		    {
			    return null;
		    }

		    User result = _context.Users.Add(new User
		    {
			    Login = login,
			    Password = password
		    }).Entity;

		    _context.SaveChanges();

		    return result;
	    }

	    public User GetUser(string login)
	    {
		    return _context.Users.FirstOrDefault(x => x.Login == login);
	    }

	    public Todo AddTodo(User user, string title, string description)
	    {
		    Todo result = _context.Todos.Add(new Todo
		    {
			    UserId = user.Id,
			    Description = description,
			    Title = title
		    }).Entity;

		    _context.SaveChanges();

		    return result;
	    }

	    public Todo UpdateTodo(User user, int id, string title, string description)
	    {
		    Todo todo = GetTodo(user, id);

		    if (todo == null)
		    {
			    return null;
		    }

		    todo.Description = description;
		    todo.Title = title;
		    Todo result = _context.Todos.Update(todo).Entity;
		    _context.SaveChanges();

		    return result;
	    }

	    public bool DeleteTodo(User user, int id)
	    {
		    Todo todo = GetTodo(user, id);

		    if (todo == null)
		    {
			    return false;
		    }

		    _context.Todos.Remove(todo);
		    _context.SaveChanges();
		    return true;
	    }

	    public Todo GetTodo(User user, int id)
	    {
		    return _context.Todos.FirstOrDefault(x => x.UserId == user.Id && x.Id == id);
	    }

	    public List<Todo> GetTodos(User user)
	    {
		    return _context.Todos.Where(x => x.UserId == user.Id).ToList();
	    } 
    }
}
