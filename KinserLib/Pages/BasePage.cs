using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using KinserLib.Helpers;

namespace KinserLib.Pages
{
	public class BasePage
	{

		public readonly string applicationUrl = ConfigurationSettings.AppSettings["ApplicationURL"];
			public IWebDriver driver;
			private bool acceptNextAlert = true;
			public readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			protected int TimeOutInSecs = 10;

			public BasePage(IWebDriver dr)
			{
				this.driver = dr;
				PageFactory.InitElements(dr, this);
			}

			public string CloseAlertAndGetItsText()
			{
				try
				{
					IAlert alert = driver.SwitchTo().Alert();
					string alertText = alert.Text;
					if (acceptNextAlert)
					{
						alert.Accept();
					}
					else
					{
						alert.Dismiss();
					}
					return alertText;
				}
				finally
				{
					acceptNextAlert = true;
				}
			}

			public bool IsTextPresent(string text)
			{
				var bodyTag = driver.WaitUntil(x => x.FindElement(By.TagName("body")));
				if (bodyTag.Text.Contains(text))
				{
					return true;

				}
				else
				{
					Log.Info("(" + text + ") text is not present for the page title " + driver.SwitchTo());
				}
				return false;
			}

			public bool Exists(By by)
			{
				if (driver.FindElements(by).Count != 0)
				{
					return true;
				}
				else
				{
					return false;
				}

			}


		}
	}

