namespace SE1614_Group4_Project_API.DTOs
{
	public class UpdateUserProfile
	{
		public string UserName { get; set; }
		public string Avatar { get; set; }
		public string DisplayName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public DateTime Birth { get; set; }
		public bool Gender { get; set; }

	}
}
