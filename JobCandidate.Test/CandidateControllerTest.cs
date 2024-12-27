using JobCandidate.Controllers;
using JobCandidate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Data;

namespace JobCandidate.Test
{
	public class CandidateControllerTest
	{
		private readonly Mock<IConfiguration> _mockConfiguration;
		private readonly Mock<IDbConnection> _mockDbConnection;
		private readonly Mock<IDbCommand> _mockDbCommand;
		private readonly CandidateController _controller;  
		
        public CandidateControllerTest()
		{
			// Setup mock configuration
			_mockConfiguration = new Mock<IConfiguration>();

			var mockConfSection = new Mock<IConfigurationSection>();
			mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "DefaultConnection")]).Returns("Server=192.168.20.70; Database=CANDIDATE_DB; uid=sa; pwd=infodev; MultipleActiveResultSets=true; Trusted_Connection=True;TrustServerCertificate=True; Connection Timeout=300;Integrated Security=false");

			_mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(mockConfSection.Object);


			// Setup mock SQL Connection and Command
			_mockDbConnection = new Mock<IDbConnection>();

			_mockDbCommand = new Mock<IDbCommand>();

			// Since we're mocking IDbConnection, we mock the behavior of creating SqlCommand
			_mockDbConnection.Setup(conn => conn.CreateCommand()).Returns(_mockDbCommand.Object);

			// Ensure ExecuteNonQuery returns a successful result
			_mockDbCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1);

			// Instantiate the controller with the mocked dependencies
			_controller = new CandidateController(_mockConfiguration.Object);
		}
		[Fact]
		public async Task SaveOrEditCandidateInfo_ShouldReturnSuccess_WhenDataIsValid()
		{
			// Arrange: Create a dummy request object
			var requestData = new CandidateDetailsInfo
			{
				FirstName = "Nabin",
				LastName = "Lav",
				PhoneNumber = "98654878954",
				Email = "nabin@gmail.com",
				LinkedInProfile = "https://linkedin.com/in/nabin",
				GitHubProfile = "https://github.com/nabin",
				TimeToContact = "9am - 5pm",
				Remarks = "Great candidate"
			};
			
			// Act: Call the method under test
			var result = await _controller.SaveOrEditCandidateInfo(requestData);

			// Assert: Use IsAssignableFrom instead of IsType to check for base class (ObjectResult)
			Assert.IsAssignableFrom<ObjectResult>(result);

			// Optionally, you can verify the specific properties of the result
			var okResult = result as ObjectResult;
			Assert.Equal(200, okResult.StatusCode); // Verify status code
			Assert.Equal("Successful !!!", okResult.Value); // Verify returned value
		}
	}
}