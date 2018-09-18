using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KinserLib.Data;

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

			driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);

			
		

		}


		public IDictionary<string, DerivedAnswers> GetContent()
		{

			IDictionary<string, DerivedAnswers> data = new Dictionary<string, DerivedAnswers>();




			var divs = driver.FindElements(By.XPath("//div[@class='oasisBox']/div/p/span[@class='bold'][1]"));

			if (divs != null && divs.Count > 0)
			{

				foreach (IWebElement div in divs)
				{
					bool answerFound = false;
					DerivedAnswers eachAnswer = new DerivedAnswers();

					var parent = div.FindElement(By.XPath("../.."));

					if (parent != null)
					{
						// get the selected radio buttons 

						var selectedOptions = parent.FindElements(By.XPath(".//img"));

						// get selected options 

						if (selectedOptions != null && selectedOptions.Count > 0)
						{
							int selectedOption = 0;
							foreach (IWebElement radio in selectedOptions)
							{

								if (radio.GetAttribute("src").Contains("radioBtn-selected.png"))
								{
									Log.Info(" " + div.Text + "Selected Option number is " + selectedOption);
									answerFound = true;
									eachAnswer.IsMultiChoise = false;
									eachAnswer.AnswerCaptured = true;
									eachAnswer.SelectedRadioButtonOption = selectedOption;
									eachAnswer.Question = div.Text;
									break;
								}
								else
								{
									selectedOption++;
								}
							}

							if (!answerFound)
							{
								List<int> selectedCheckox = new List<int>(); ;
								for (int i = 0; i < selectedOptions.Count; i++)
								{

									if (selectedOptions[i].GetAttribute("src").Contains("checkbox-selected.png"))
									{
										Log.Info(" " + div.Text + "Selected Checkbox is  number is " + i);
										answerFound = true;
										eachAnswer.IsMultiChoise = true;
										eachAnswer.AnswerCaptured = true;
										eachAnswer.SelectedCheckBoxes.Add(i);
										eachAnswer.Question = div.Text;
										selectedCheckox.Add(i);

									}

								}


							}


						}

					}

					if (!answerFound)
					{
						Log.Info("No Answer found for Question=>" + div.Text);
					}
					else
					{

						data.Add(eachAnswer.Question, eachAnswer);
					}

				}

			}


			Log.Info("Total answers found " + data.Count);



			return data;


		}


		public void EnterTime(string enterTime, string outTime, string date)
		{

			driver.FindElement(By.Id("cTO_timein")).Clear();
			driver.FindElement(By.Id("cTO_timein")).SendKeys(enterTime);

			driver.FindElement(By.Id("cTO_timeout")).Clear();
			driver.FindElement(By.Id("cTO_timeout")).SendKeys(outTime);


			driver.FindElement(By.Id("cTO_visitdate")).Clear();
			driver.FindElement(By.Id("cTO_visitdate")).SendKeys(date);

		}

		public bool NextPageAvailable()
		{

			var nextButton = driver.FindElement(By.Id("oasisSaveContinueButton"));

			if (nextButton != null)
			{
				nextButton.Click();
				return true;
			}
			return false;
		}

		public void FillContent(IDictionary<string, DerivedAnswers> data)
		{

			


			var divs = driver.FindElements(By.XPath("//div[contains(@id,'Section')]/div/p/span[@class='bold'][1]"));


			if (divs != null && divs.Count > 0)
			{

				foreach (IWebElement div in divs)
				{

					DerivedAnswers eachAnswer = new DerivedAnswers();

					// search data 

					if (data.ContainsKey(div.Text))
					{


						var answer = data[div.Text];
						var parent = div.FindElement(By.XPath("../.."));
						// select respective check box; 
						if (parent != null && answer.AnswerCaptured)
						{


							if (answer.IsMultiChoise)
							{

								// get the selected radio buttons 

								var selectedOptions = parent.FindElements(By.XPath(".//input[@type='checkbox']"));


								for (int i = 0; i < answer.SelectedCheckBoxes.Count; i++)
								{


									selectedOptions[answer.SelectedCheckBoxes[i]].Click();
								}



							}
							else
							{
								var selectRadioButton = parent.FindElements(By.XPath(".//input[@type='radio']"));


								selectRadioButton[answer.SelectedRadioButtonOption].Click();





							}
						}


					}


				}
			}

		

			}

		}
	}

