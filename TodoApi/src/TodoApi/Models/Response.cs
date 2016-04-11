namespace TodoApi.Models
{
    public class Response
    {
		public bool Ok { get; set; }

		public string Error { get; set; }
    }

	public class Response<T> : Response
	{
		public T Resource { get; set; }
	}
}
