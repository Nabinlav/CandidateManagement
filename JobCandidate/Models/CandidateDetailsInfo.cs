using System.ComponentModel.DataAnnotations;

namespace JobCandidate.Models
{
	public class CandidateDetailsInfo
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		[Required, DataType(DataType.EmailAddress)] 
		public string Email { get; set; }
		public string LinkedInProfile { get; set; }
		public string TimeToContact { get; set; }
		public string GitHubProfile { get; set; }
		[Required]
		public string Remarks { get; set; }
	}
}
