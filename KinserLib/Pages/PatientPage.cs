using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace KinserLib.Pages
{
	public class PatientPage : BasePage
	{

		public PatientPage(IWebDriver driver)
            : base(driver)
        {

		}


		public void GoToMyPatient(string url)
		{

			driver.Navigate().GoToUrl(url);

		}
	}
}
