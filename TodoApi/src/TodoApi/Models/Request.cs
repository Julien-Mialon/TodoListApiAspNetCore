using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class UserRequest
    {
		public string Login { get; set; }

		public string Password { get; set; }
    }

	public class DeleteTodoRequest : UserRequest
	{
		public int Id { get; set; }
	}

	public class AddTodoRequest : UserRequest
	{
		public string Title { get; set; }

		public string Description { get; set; }
	}

	public class UpdateTodoRequest : AddTodoRequest
	{
		public int Id { get; set; }
	}
}
