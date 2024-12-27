using JobCandidate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobCandidate.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class CandidateController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		public CandidateController(IConfiguration configuration)
		{
			_configuration = configuration;

		}
		[HttpPost]
		public async Task<IActionResult> SaveOrEditCandidateInfo([FromBody] CandidateDetailsInfo requestData)
		{
			try
			{			

				using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
				{
					con.Open();
					using (SqlCommand cmd = new SqlCommand())
					{
						cmd.Connection = con;
						cmd.CommandText = "USP_INSERT_CANDIDATE_INFORMATION";
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.Add("@FIRST_NAME", SqlDbType.VarChar).Value = requestData.FirstName;
						cmd.Parameters.Add("@LAST_NAME", SqlDbType.VarChar).Value = requestData.LastName;
						cmd.Parameters.Add("@PHONE_NUMBER", SqlDbType.VarChar).Value = requestData.PhoneNumber;
						cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = requestData.Email;
						cmd.Parameters.Add("@LINKEDIN_PROFILE", SqlDbType.VarChar).Value = requestData.LinkedInProfile;
						cmd.Parameters.Add("@GITHUB_PROFILE", SqlDbType.VarChar).Value = requestData.GitHubProfile;
						cmd.Parameters.Add("@TIME_TO_CONTACT", SqlDbType.VarChar).Value = requestData.TimeToContact;
						cmd.Parameters.Add("@REMARKS", SqlDbType.VarChar).Value = requestData.Remarks;
						cmd.CommandType = CommandType.StoredProcedure;
						await cmd.ExecuteNonQueryAsync();

					}
					con.Close();
					return Ok("Successful !!!");

				}
			}
			catch (Exception ex)
			{

				return StatusCode(500, "Error: " + ex.Message);
			}
		}
	}
}
