using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KinserLib.Pages
{
	public class LoginPage : BasePage
	{

		public LoginPage(IWebDriver driver)
            : base(driver)
        {

		}

		public void LogMeIn(string username, string password, string appUrl)
		{

			driver.Navigate().GoToUrl(appUrl);
			driver.FindElement(By.Id("username")).Clear();
			driver.FindElement(By.Id("username")).SendKeys(username);
			driver.FindElement(By.Id("password")).Clear();
			driver.FindElement(By.Id("password")).SendKeys(password);
		

			driver.FindElement(By.Id("login_btn")).Click();

			IAlert alert = driver.SwitchTo().Alert();
			alert.Accept();


			Thread.Sleep(2000);

		}


	}
}
