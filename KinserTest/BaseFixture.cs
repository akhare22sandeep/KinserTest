using log4net;
using log4net.Config;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KinserTest
{
	[TestFixture]
	public class BaseFixture
	{
		public readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public readonly string applicationUrl = ConfigurationSettings.AppSettings["ApplicationURL"];
		public readonly string username = ConfigurationSettings.AppSettings["Username"];
		public readonly string password = ConfigurationSettings.AppSettings["Password"];
				public readonly string copyFromUrl = ConfigurationSettings.AppSettings["PatientCopyFromUrl"];

		public readonly string patientName = ConfigurationSettings.AppSettings["PatientName"];
		public readonly string taskName = ConfigurationSettings.AppSettings["TaskName"];
		public IWebDriver Driver;
		

		//[TestFixture]
		//public void Init()
		//{
			
		//}

		[SetUp]
		public void SetupTest()
		{
			DOMConfigurator.Configure();
			//FirefoxProfile fxProfile = new FirefoxProfile();

			//fxProfile.SetPreference("browser.download.folderList", 2);
			////fxProfile.SetPreference("network.http.phishy-userpass-length", 255);
			//fxProfile.SetPreference("browser.download.dir", "C:\\Temp");
			////  fxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "text/csv,application/x-msexcel,application/excel,application/x-excel,application/vnd.ms-excel,image/png,image/jpeg,text/html,text/plain,application/msword,application/xml");
			//fxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octet-stream doc xlsx pdf txt");


			FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
			service.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
			


			Driver = new FirefoxDriver();
			//Driver = new PhantomJSDriver();

			

			Log.Info("SetupTest Has been completed");
		}


		

		[TearDown]
		public virtual void TearDown()
		{
			//Utility.SaveAsImage(Driver, "SomeName");
			////  Log.Info(TestContext.CurrentContext.Test.Name + " Test has been executed with result =>" + TestContext.CurrentContext.Result.Outcome);
			//Log.Info("##################################################################");
			//if (Driver != null)
			//{
			//    Driver.Quit();
			//    Driver.Dispose();
			//}

		}

		//protected IWebDriver SetBrowser()
		//{
		//    //string browser = ConfigurationSettings.AppSettings["Browser"]; //Get browser name from the config
		//    //switch (browser)
		//    //{
		//    //    case "Chrome":
		//    //        return new ChromeDriver();
		//    //        break;
		//    //    case "Firefox":
		//    //        return new FirefoxDriver(); ;
		//    //        break;
		//    //    case "InternetExplorer":
		//    //        return new InternetExplorerDriver();
		//    //        break;

		//    //    default:
		//    //        return new FirefoxDriver();
		//    //}

		//}


	}

}

