namespace Northwind.Web.App.Tests
{
    using NRepository.EntityFramework;
    using NUnit.Framework;

    [SetUpFixture]
    public class NUnitSetupFixture
    {
        [SetUp]
        public void SetUp()
        {
            // This sets up the default interface extension methods for NRepository.EntityFramework as interface extension methods aren't that easy to mock
            EntityFrameworkRepositoryExtensions.SetDefaultImplementation(new TestsEntityFrameworkRepositoryExtensions());
        }
    }
}
