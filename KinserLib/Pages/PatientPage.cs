using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KinserLib.Data;
using OpenQA.Selenium.Support.UI;
using KinserLib.Helpers;

namespace KinserLib.Pages
{
	public class PatientPage : BasePage
	{

		public PatientPage(IWebDriver driver)
			: base(driver)
		{

		}


		public void NavigateToURL(string url)
		{

			driver.Navigate().GoToUrl(url);

			driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);




		}


		public void OpenPatient(string patientName, string TaskName)
		{

			// select 100 items on the page 

			var ddl = driver.FindElement(By.Name("resultsTable_length"));
			if (ddl != null)
			{
				SelectElement drp = new SelectElement(ddl);
				drp.SelectByText("100");
			}

			var links = driver.FindElements(By.LinkText(patientName));
			// make sure this link has name 
			if (links != null && links.Count > 0)
			{

				foreach (var link in links)
				{
					var tr = link.FindElement(By.XPath("../.."));
					if (tr.Text.Contains(TaskName))
					{
						link.Click();
						break;
					}
				}

			}


		}

		public IDictionary<string, DerivedAnswers> GetTableContent()
		{
			IDictionary<string, DerivedAnswers> data = new Dictionary<string, DerivedAnswers>();
			var trs = driver.FindElements(By.XPath("//table[@id='mainTable']/tbody/tr[position()>1]"));

			if (trs.Count > 0)
			{


				foreach (var tr in trs)
				{

					DerivedAnswers eachAnswer = new DerivedAnswers();
					bool answerFound = false;
					string hidden = string.Empty;
					Log.Info(tr.Text);
					if (tr.Exists(By.XPath("./input[@type='hidden']")))
					{

						hidden = tr.FindElement(By.XPath("./ input[@type='hidden']")).GetAttribute("name");

					}
					if (!string.IsNullOrEmpty(hidden))
					{
						var selectedOptions = tr.FindElements(By.XPath(".//img"));

						if (selectedOptions != null)
						{
							//get selected options

							if (selectedOptions != null && selectedOptions.Count > 0)
							{
								int selectedOption = 0;
								foreach (IWebElement radio in selectedOptions)
								{

									if (radio.GetAttribute("src").Contains("radioBtn-selected.png"))
									{
										//Log.Info(" " + hidden.Text + "Selected Option number is " + selectedOption);
										answerFound = true;
										eachAnswer.IsMultiChoise = false;
										eachAnswer.AnswerCaptured = true;
										eachAnswer.SelectedRadioButtonOption = selectedOption;
										eachAnswer.Question = hidden;
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
											//Log.Info(" " + hidden.Text + "Selected Checkbox is  number is " + i);
											answerFound = true;
											eachAnswer.IsMultiChoise = true;
											eachAnswer.AnswerCaptured = true;
											eachAnswer.SelectedCheckBoxes.Add(i);
											eachAnswer.Question = hidden;
											selectedCheckox.Add(i);

										}

									}


								}
							}


						}

						if (!answerFound)
						{
							Log.Info("No Answer found for Question=>" + hidden);
						}
						else
						{
							if (!data.ContainsKey(eachAnswer.Question))
							{
								data.Add(eachAnswer.Question, eachAnswer);
							}
							else
							{
								Log.Info("Duplicate Key " + eachAnswer.Question);
							}
						}

					}

				}
			}
			return data;

		}

		public IDictionary<string, DerivedAnswers> GetCareManagement()
		{
			var trs = driver.FindElements(By.XPath("//div[@class='OasisHeading'  and contains(text(), 'Care Management')]//parent::div//following::table[@id='mainTable'][1]//tr"));

			IDictionary<string, DerivedAnswers> data = new Dictionary<string, DerivedAnswers>();



			foreach (IWebElement tr in trs)
			{

				DerivedAnswers eachAnswer = new DerivedAnswers();
				bool answerFound = false;
				string hidden = string.Empty;

				if (tr.Exists(By.XPath("./input[@type='hidden']")))
				{

					hidden = tr.FindElement(By.XPath("./ input[@type='hidden']")).GetAttribute("name");

				}
				var selectedOptions = tr.FindElements(By.XPath(".//img"));

				if (!string.IsNullOrEmpty(hidden))
				{
					//get selected options

					if (selectedOptions != null && selectedOptions.Count > 0)
					{
						int selectedOption = 0;
						foreach (IWebElement radio in selectedOptions)
						{

							if (radio.GetAttribute("src").Contains("radioBtn-selected.png"))
							{
								//Log.Info(" " + hidden.Text + "Selected Option number is " + selectedOption);
								answerFound = true;
								eachAnswer.IsMultiChoise = false;
								eachAnswer.AnswerCaptured = true;
								eachAnswer.SelectedRadioButtonOption = selectedOption;
								eachAnswer.Question = hidden;
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
									//Log.Info(" " + hidden.Text + "Selected Checkbox is  number is " + i);
									answerFound = true;
									eachAnswer.IsMultiChoise = true;
									eachAnswer.AnswerCaptured = true;
									eachAnswer.SelectedCheckBoxes.Add(i);
									eachAnswer.Question = hidden;
									selectedCheckox.Add(i);

								}

							}


						}
					}


				}

				if (!answerFound)
				{
					Log.Info("No Answer found for Question=>" + hidden);
				}
				else
				{
					if (!data.ContainsKey(eachAnswer.Question))
					{
						data.Add(eachAnswer.Question, eachAnswer);
					}
					else
					{
						Log.Info("Duplicate Key " + eachAnswer.Question);
					}
				}


			}

			return data;
		}

		public IDictionary<string, DerivedAnswers> GetContent()
		{

			IDictionary<string, DerivedAnswers> data = new Dictionary<string, DerivedAnswers>();
			var divs1 = driver.FindElements(By.XPath("//div[@class='oasisBox']//p/span[@class='bold'][1]"));
			//var divs2 = driver.FindElements(By.XPath("//div[@class='oasisBox']/p/span[@class='bold'][1]"));
			//IEnumerable<IWebElement> divs = divs1.Concat<IWebElement>(divs2);

			if (divs1 != null && divs1.Count<IWebElement>() > 0)
			{

				foreach (IWebElement div in divs1)
				{
					bool answerFound = false;
					DerivedAnswers eachAnswer = new DerivedAnswers();

					Log.Info(div.Text);
					if (div.Text != "(Mark all that apply)")
					{
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
							if (!data.ContainsKey(eachAnswer.Question))
							{
								data.Add(eachAnswer.Question, eachAnswer);
							}
							else
							{
								Log.Info("Duplicate Key " + eachAnswer.Question);
							}
						}
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

		public void FillCareManagement(IDictionary<string, DerivedAnswers> data)
		{

			foreach (var val in data.Values)
			{

				var elmts = driver.FindElements(By.Name(val.Question));

				if (elmts != null)
				{

					foreach (var elm in elmts)
					{

						var tr = elm.FindElement(By.XPath(".."));

						if (tr != null)
						{


							var selectRadioButton = tr.FindElements(By.XPath(".//input[@type='radio']"));

							if (!selectRadioButton[val.SelectedRadioButtonOption].Selected)
							{
								selectRadioButton[val.SelectedRadioButtonOption].Click();
							}

						}
					}

				}
			}




		}

		public void FillContent(IDictionary<string, DerivedAnswers> data, IDictionary<string, DerivedAnswers> careManagementData)
		{

			var carmanagement = driver.FindElements(By.XPath("//li[@class='span-12' and contains(text(), 'Care Management')]"));

			if (carmanagement.Count >0)
			{
				FillCareManagement(careManagementData);
			}
			

			var divs1 = driver.FindElements(By.XPath("//div[contains(@id,'Section')]/div/p/span[@class='bold'][1]"));

			var divs2 = driver.FindElements(By.XPath("//div[contains(@id,'Section')]/p/span[@class='bold'][1]"));

			var divs3 = driver.FindElements(By.XPath("//div[contains(@id,'Section')]/p/span[@class='bold'][1]"));

			IEnumerable<IWebElement> divs = divs1.Concat<IWebElement>(divs2);

			if (divs != null && divs.Count<IWebElement>() > 0)
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

									if (!selectedOptions[answer.SelectedCheckBoxes[i]].Selected)
									{

										selectedOptions[answer.SelectedCheckBoxes[i]].Click();
									}
								}



							}
							else
							{
								var selectRadioButton = parent.FindElements(By.XPath(".//input[@type='radio']"));

								if (!selectRadioButton[answer.SelectedRadioButtonOption].Selected)
								{
									selectRadioButton[answer.SelectedRadioButtonOption].Click();
								}





							}
						}


					}


				}
			}



		}

	}
}

