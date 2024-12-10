using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class PeopleControllerTests
{
    private readonly HttpClient _client;

    public PeopleControllerTests()
    {
        _client = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:8082/")
        };
    }

    [Fact]
    public async Task TestGetPeople_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("api/people");

        // Assert
        response.EnsureSuccessStatusCode(); // Перевіряємо, чи запит був успішним
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content)); // Перевіряємо, що відповідь не порожня
    }

    [Fact]
    public async Task TestGetPersonById_ReturnsCorrectPerson()
    {
        // Arrange
        var testId = 1; // Замініть на ID, який існує у вашій базі даних

        // Act
        var response = await _client.GetAsync($"api/people/{testId}");

        // Assert
        response.EnsureSuccessStatusCode(); // Перевіряємо, чи запит був успішним
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content)); // Перевіряємо, що відповідь не порожня
        Assert.Contains($"\"id\":{testId}", content); // Перевіряємо, що відповідь містить правильний ID
    }

    [Fact]
    public async Task TestGetPersonById_ReturnsNotFoundForInvalidId()
    {
        // Arrange
        var invalidId = 9999; // ID, який точно не існує у вашій базі даних

        // Act
        var response = await _client.GetAsync($"api/people/{invalidId}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode); // Перевіряємо, чи повертається 404
    }
}

