using Presentation.Tests.Acceptance.Contracts.Companies;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Presentation.Tests.Acceptance.StepDefinitions;

[Binding]
public class CompaniesSteps
{
    [When("I create companies with the following details")]
    public void WhenICreateCompaniesWithTheFollowingDetails(Table table)
    {
        var createCompanyRequests = table.CreateSet<CreateCompanyRequest>();
        var createdProductsList = new List<GetCompanyResponse>();
    }

    [Then("the companies are created successfully")]
    public void ThenTheCompaniesAreCreatedSuccessfully()
    {
        ScenarioContext.StepIsPending();
    }

    [Given("Company gets deleted successfully")]
    public void GivenCompanyGetsDeletedSuccessfully(Table table)
    {
        var createCompanyRequests = table.CreateSet<CreateCompanyRequest>();
    }

    [When("I delete the companies")]
    public void WhenIDeleteTheCompanies()
    {
        ScenarioContext.StepIsPending();
    }

    [Then("the companies are deleted")]
    public void ThenTheCompaniesAreDeleted()
    {
        ScenarioContext.StepIsPending();
    }
}
