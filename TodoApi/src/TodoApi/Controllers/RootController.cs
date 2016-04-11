using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
	[Route("api")]
	public class RootController : Controller
	{
		[FromServices]
		public DatabaseService Database { get; set; }

		[HttpPost("register")]
		public Response Register([FromBody] UserRequest request)
		{
			if (string.IsNullOrEmpty(request?.Login) || string.IsNullOrEmpty(request.Password))
			{
				return new Response
				{
					Ok = false,
					Error = "Login ou mot de passe vide"
				};
			}

			User result = Database.AddUser(request.Login, request.Password);

			if (result == null)
			{
				return new Response
				{
					Ok = false,
					Error = "Un utilisateur existe déjà avec ce login"
				};
			}
			return new Response
			{
				Ok = true
			};
		}

		private bool TryLogin(UserRequest request)
		{
			if (string.IsNullOrEmpty(request?.Password) || string.IsNullOrEmpty(request.Login))
			{
				return false;
			}

			User user = Database.GetUser(request.Login);
			if (request.Password != user?.Password)
			{
				return false;
			}
			return true;
		}

		[HttpPost("login")]
		public Response Login([FromBody] UserRequest request)
		{
			if (TryLogin(request))
			{
				return new Response
				{
					Ok = true
				};
			}
			return new Response
			{
				Ok = false,
				Error = "Login ou mot de passe invalide"
			};
		}

		[HttpPost("list")]
		public Response<List<Todo>> List([FromBody] UserRequest request)
		{
			if (!TryLogin(request))
			{
				return new Response<List<Todo>>
				{
					Ok = false,
					Error = "Login ou mot de passe invalide"
				};
			}
			User user = Database.GetUser(request.Login);

			return new Response<List<Todo>>
			{
				Ok = true,
				Resource = Database.GetTodos(user)
			};
		}
		
		[HttpPost("add")]
		public Response<Todo> Add([FromBody] AddTodoRequest request)
		{
			if (!TryLogin(request))
			{
				return new Response<Todo>
				{
					Ok = false,
					Error = "Login ou mot de passe invalide"
				};
			}
			User user = Database.GetUser(request.Login);

			if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
			{
				return new Response<Todo>
				{
					Ok = false,
					Error = "Champs Title ou Description vide"
				};
			}

			Todo newTodo = Database.AddTodo(user, request.Title, request.Description);
			if (newTodo != null)
			{
				return new Response<Todo>
				{
					Ok = true,
					Resource = newTodo
				};
			}
			return new Response<Todo>
			{
				Ok = false,
				Error = "Hmm... something went wrong !"
			};
		}

		[HttpPost("edit")]
		public Response<Todo> Edit([FromBody] UpdateTodoRequest request)
		{
			if (!TryLogin(request))
			{
				return new Response<Todo>
				{
					Ok = false,
					Error = "Login ou mot de passe invalide"
				};
			}
			User user = Database.GetUser(request.Login);

			if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
			{
				return new Response<Todo>
				{
					Ok = false,
					Error = "Champs Title ou Description vide"
				};
			}

			Todo newTodo = Database.UpdateTodo(user, request.Id, request.Title, request.Description);
			if (newTodo != null)
			{
				return new Response<Todo>
				{
					Ok = true,
					Resource = newTodo
				};
			}
			return new Response<Todo>
			{
				Ok = false,
				Error = "Cet id n'existe pas il me semble"
			};
		}

		[HttpPost("delete")]
		public Response Delete([FromBody] DeleteTodoRequest request)
		{
			if (!TryLogin(request))
			{
				return new Response
				{
					Ok = false,
					Error = "Login ou mot de passe invalide"
				};
			}
			User user = Database.GetUser(request.Login);

			bool result = Database.DeleteTodo(user, request.Id);
			if (result)
			{
				return new Response
				{
					Ok = true,
				};
			}
			return new Response
			{
				Ok = false,
				Error = "Cet id n'existe pas il me semble"
			};
		}
	}
}
