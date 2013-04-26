using NUnit.Framework;
using Ninject;

namespace Yorganize.Test
{
    [SetUpFixture]
    public class SetUp
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
         
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            // nothing for now
        }
    }
}
