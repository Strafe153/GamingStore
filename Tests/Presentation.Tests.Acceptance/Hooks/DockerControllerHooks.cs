using Ductus.FluentDocker.Services;
using TechTalk.SpecFlow;

namespace Presentation.Tests.Acceptance.Hooks
{
    [Binding]
    public class DockerControllerHooks
    {
        //private ICompositeService _compositeService;

        [BeforeTestRun]
        public static void Up()
        {

        }

        [AfterTestRun]
        public static void Down()
        {

        }
    }
}
