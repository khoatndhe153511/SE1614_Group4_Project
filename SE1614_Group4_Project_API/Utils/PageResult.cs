namespace SE1614_Group4_Project_API.Utils
{
	public class PageResult<T>
	{
		public int TotalCount { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public IEnumerable<T> Results { get; set; }
	}
}
