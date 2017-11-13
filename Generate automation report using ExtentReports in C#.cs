// Generate automation report using ExtentReports in C#

using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Drawing.Imaging;

namespace ComparativeTests.Tests
{
    public abstract class Base
    {
        public static bool initialized = false;
        public static ExtentReports _extent;
        public static ExtentHtmlReporter htmlReporter;
        protected ExtentTest _test;
        public string dir;
        public string fileName;

        [OneTimeSetUp]
        protected void Setup()
        {
            dir = TestContext.CurrentContext.TestDirectory + "\\";
            fileName = "ComparativeTestsReport.html";


            if (initialized == false) {
                htmlReporter = new ExtentHtmlReporter(dir+fileName);
                _extent = new ExtentReports();
                _extent.AttachReporter(htmlReporter);
                initialized = true;
            }
            
        }

        [OneTimeTearDown]
        protected void TearDown()
        {
            _extent.Flush();
        }

        [SetUp]
        public void BeforeTest()
        {
            _test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void AfterTest()
        {
            _test.Log(Status.Debug, "Old website loading time: " + oldTime.Elapsed);
            _test.Log(Status.Debug, "New website loading time: " + newTime.Elapsed);
            
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.Message)
                    ? ""
                    : string.Format("{0}", TestContext.CurrentContext.Result.Message);
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    string oldScreenShotPath = GetScreenShot.Capture(OldSiteDriver, this.GetType().ToString() + "OldSiteScreenShot");
                    string newScreenShotPath = GetScreenShot.Capture(NewSiteDriver, this.GetType().ToString() + "NewSiteScreenShot");
                    _test.Log(Status.Debug, "Old website snapshot below: " + _test.AddScreenCaptureFromPath(oldScreenShotPath));
                    _test.Log(Status.Debug, "New website snapshot below: " + _test.AddScreenCaptureFromPath(newScreenShotPath));
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
            }
            _test.Log(logstatus, "Test ended with " + logstatus + Environment.NewLine + stacktrace);
        }
    }

    public class GetScreenShot
    {
        public static string Capture(IWebDriver driver, string screenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            string localpath = TestContext.CurrentContext.TestDirectory + "\\ReportScreenShots\\" + screenShotName + ".png"; ;
            screenshot.SaveAsFile(localpath, ScreenshotImageFormat.Png);
            return localpath;
        }
    }
}
