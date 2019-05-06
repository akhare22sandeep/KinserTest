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

			login.LogMeIn(username, password, applicationUrl);

			var patient = new PatientPage(Driver);

			patient.NavigateToURL(copyFromUrl);

			var data =patient.GetContent();
			patient.GetVitalSign();
			patient.GetContentByPage();
			//var tableData = patient.GetTableContent();

			var careManagementData = patient.GetCareManagement();

			var plancareData = patient.GetPLaneCare();

			patient.NavigateToURL("https://kinnser.net/am/hotbox.cfm");

			Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

			patient.OpenPatient(patientName,taskName);

			patient.NavigateToURL("https://kinnser.net/AM/OASIS/OASISD/index.cfm?p=1#/index.cfm?p=1");

			patient.EnterTime("16:00", "17:15", "04/29/2019");	

			Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

			do
			{
				
					patient.FillContent(data, careManagementData, plancareData);
				

			} while (patient.NextPageAvailable());


		}		

		[Test]
		public void KinserTest()
		{

			

			var patient = new PatientPage(Driver);

			try
			{

				patient.NavigateToURL("file:///C:/Personal/Kanchan%20Documents/Kinser/Pages/Print%20Preview.html");
			}
			catch (Exception ex)
			{

			}

			

			//patient.GetContenent2("//div[@id='oasisImmunizations']//table[@id='mainTable']");
			//patient.GetConentent2("//div[@id='advancedCarePlan']");
			//patient.GetConentent2("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Sensory Status')]]");
			patient.GetText("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Pain Scale')]]", "Pain Scale");
			//patient.GetConentent2("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Integumentary Status')]]");
			//patient.getText("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Integumentary Status')]]");
			//patient.GetConentent2("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Respiratory')]]");
			//patient.getText("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Respiratory')]]");
			//patient.GetConentent2("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Endocrine')]]");
			//patient.getText("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Endocrine')]]");
			//patient.GetConentent2("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Cardiovascular')]]");
			//patient.getText("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Cardiovascular')]]");
			//patient.GetConentent2("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'GU')]]");
			//patient.getText("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'GU')]]");
			//patient.GetConentent2("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Nutrition')]]");
			//patient.getText("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Nutrition')]]");
			//patient.GetConentent2("//div[@id='oasisNeuroemostatus']//table[@id='mainTable']");
			//patient.getText("//div[@id='oasisNeuroemostatus']//table[@id='mainTable']");
			//patient.GetConentent2("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Musculoskeletal')]]");
			//patient.getText("//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Musculoskeletal')]]");



			patient.NavigateToURL("file:///C:/Personal/Kanchan%20Documents/Kinser/Pages/Pain.html#/Pain.html");

			patient.InsertGenericContent("Pain Scale");



		}

		//[Test]
		//public void Test()
		//{

		//	using (IWebDriver driver = new FirefoxDriver())
		//	{
		//		//Notice navigation is slightly different than the Java version
		//		//This is because 'get' is a keyword in C#
		//		driver.Navigate().GoToUrl("http://www.google.com/");

		//		// Find the text input element by its name
		//		IWebElement query = driver.FindElement(By.Name("q"));

		//		// Enter something to search for
		//		query.SendKeys("Cheese");

		//		// Now submit the form. WebDriver will find the form for us from the element
		//		query.Submit();

		//		// Google's search is rendered dynamically with JavaScript.
		//		// Wait for the page to load, timeout after 10 seconds
		//		var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
		//		wait.Until(d => d.Title.StartsWith("cheese", StringComparison.OrdinalIgnoreCase));

		//		// Should see: "Cheese - Google Search" (for an English locale)
		//		Console.WriteLine("Page title is: " + driver.Title);
		//	}

		//}
	}
}
