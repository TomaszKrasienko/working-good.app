using System.Net;
using Shouldly;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.activities.integration.tests;

[Collection("#1")]
public sealed class HomeControllerTests : BaseTestsController
{
    [Fact]
    public async Task Get_ShouldReturn200OkStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync("/activities-module");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}