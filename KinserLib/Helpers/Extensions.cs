using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KinserLib.Helpers
{
	public static class BrowserExtensions
	{
		public static T WaitUntil<T>(this IWebDriver browser, Func<IWebDriver, T> condition, int timeout = 5)
		{
			var wait = new WebDriverWait(browser, new TimeSpan(0, 0, timeout));
			return wait.Until(condition);
		}
	}

	public static class WebElementExtensions
	{


		public static IWebElement GetParent(this IWebElement e)
		{
			return e.FindElement(By.XPath(".."));
		}

		public static bool Exists(this IWebElement e, By by)
		{
			if (e.FindElements(by).Count != 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
		{
			if (timeoutInSeconds > 0)
			{
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
				return wait.Until(drv => drv.FindElement(by));
			}
			return driver.FindElement(by);
		}
	}
}
