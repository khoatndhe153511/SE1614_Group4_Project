namespace SE1614_Group4_Project_API.DTOs
{
	public class UserDTO
	{
		public string Id { get; set; } = null!;
		public string? Avatar { get; set; }
		public string? DisplayName { get; set; }
		public string? Gravatar { get; set; }
		public string Name { get; set; }
		public int? Role { get; set; }
		public string? Password { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
		public DateTime? Birth { get; set; }
		public bool? Gender { get; set; }
		public int? TotalPost { get; set; }
		public int? TotalComment { get; set; }
	}
}
