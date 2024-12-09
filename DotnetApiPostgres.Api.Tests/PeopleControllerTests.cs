using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class PeopleControllerTests
{
    private readonly HttpClient _client;

    public PeopleControllerTests()
    {
        _client = new HttpClient();
        _client.BaseAddress = new System.Uri("http://localhost:8081/");
    }

    [Fact]
    public async Task TestGetPeople_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("api/people");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content));
    }
}
