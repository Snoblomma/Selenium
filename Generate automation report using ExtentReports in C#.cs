// Generate automation report using ExtentReports in C#

using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;

namespace ComparativeTests.Tests
{
    public abstract class Base
    {
        public static bool initialized = false;
        public static ExtentReports _extent;
        public static ExtentHtmlReporter htmlReporter;

        [OneTimeSetUp]
        protected void Setup()
        {
            var dir = TestContext.CurrentContext.TestDirectory + "\\";
            var fileName = this.GetType().ToString() + "ComparativeTestsReport.html";


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
            Console.Write("flush");
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
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
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

            _test.Log(logstatus, "Test ended with " + logstatus + stacktrace);
            _extent.Flush();
        }
    }
}
