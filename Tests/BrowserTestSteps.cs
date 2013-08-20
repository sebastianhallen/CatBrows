using System;
using TechTalk.SpecFlow;

namespace Tests
{
    [Binding]
    public class BrowserTestSteps
    {
        [Then(@"the test method should throw a no browser exception")]
        public void ThenTheTestMethodShouldThrowANoBrowserException()
        {
            throw new Exception("Should never get here");
        }
        
        [Then(@"the test method should have 1 testcase")]
        public void ThenTheTestMethodShouldHaveTestcase()
        {

        }
        
        [Then(@"the test method should have (.*) testcases")]
        public void ThenTheTestMethodShouldHaveTestcases(int p0)
        {

        }
    }
}
