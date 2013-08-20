﻿using System;
using TechTalk.SpecFlow;

namespace Tests
{
    [Binding]
    public class BrowserTestSteps
    {
        public static bool BackgroundRun;
        public BrowserTestSteps()
        {
            BackgroundRun = false;
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

        }


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
