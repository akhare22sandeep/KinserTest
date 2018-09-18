using KinserLib.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;

namespace KinserTest
{
	[TestFixture]
	public class KinserFixture : BaseFixture
	{
		[Test]
		public void KinserLogin()
		{

			var login = new LoginPage(Driver);

			login.LogMeIn("kchatarkar.ihhc", "Sandeep2020", applicationUrl);

			var patient = new PatientPage(Driver);

			patient.GoToMyPatient("https://kinnser.net/am/printwrapper.cfm?PatientTaskKey=334174574");

			var data =patient.GetContent();

			patient.GoToMyPatient("https://kinnser.net/am/hotbox.cfm");

			patient.GoToMyPatient("https://kinnser.net/am/OASIS/OASISC/index.cfm?PatientTaskKey=498385830");

			patient.GoToMyPatient("https://kinnser.net/am/OASIS/OASISC/index.cfm?p=1#/index.cfm?p=1");

			patient.EnterTime("00:00", "00:45", "09/14/2018");

			do
			{
				patient.FillContent(data);
			} while (patient.NextPageAvailable());


		}

		[Test]
		public void KinserTest()
		{

			

			var patient = new PatientPage(Driver);

			patient.GoToMyPatient("file:///C:/Personal/Kanchan/Selenium/Page/Print%20Preview.html");

			var data = patient.GetContent();


			patient.GoToMyPatient("file:///C:/Personal/Kanchan/Selenium/Page/OASIS-C2%20Start%20of%20Care%20(PT)%20_%20Kinnser%20Software4.html");

			patient.FillContent(data);


		}

		[Test]
		public void Test()
		{

			using (IWebDriver driver = new FirefoxDriver())
			{
				//Notice navigation is slightly different than the Java version
				//This is because 'get' is a keyword in C#
				driver.Navigate().GoToUrl("http://www.google.com/");

				// Find the text input element by its name
				IWebElement query = driver.FindElement(By.Name("q"));

				// Enter something to search for
				query.SendKeys("Cheese");

				// Now submit the form. WebDriver will find the form for us from the element
				query.Submit();

				// Google's search is rendered dynamically with JavaScript.
				// Wait for the page to load, timeout after 10 seconds
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				wait.Until(d => d.Title.StartsWith("cheese", StringComparison.OrdinalIgnoreCase));

				// Should see: "Cheese - Google Search" (for an English locale)
				Console.WriteLine("Page title is: " + driver.Title);
			}

		}
	}
}
