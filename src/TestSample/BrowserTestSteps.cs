namespace TestSample
{
    using System;
    using TechTalk.SpecFlow;

    [Binding]
    public class BrowserTestSteps
    {
        public static bool BackgroundRun;
        public static bool BackgroundHasBrowser;
        public BrowserTestSteps()
        {
            BackgroundRun = false;
            BackgroundHasBrowser = false;
        }

        [Given(@"I have a browser when running the background")]
        public void GivenIHaveABackground()
        {
            BackgroundRun = true;
            var browser = (string) ScenarioContext.Current["Browser"];
            if (string.IsNullOrWhiteSpace(browser))
            {
                throw new Exception("No browser when running background");
            }
            BackgroundHasBrowser = true;

        }


        [Then(@"the test method should throw a no browser exception")]
        public void ThenTheTestMethodShouldThrowANoBrowserException()
        {
            throw new Exception("Should never get here");
        }

        [Then(@"the block should pass")]
        [Then(@"the test method should have 1 testcase")]
        public void ThenTheBlockShouldPass()
        {
            
        }

        [Then(@"the test method should have (.*) testcases")]
        public void ThenTheBlockShouldPass(string p0)
        {

        }
    }
}
