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
using Newtonsoft.Json;

namespace KinserLib.Pages
{
	public class PatientPage : BasePage
	{

		string patientHeight = string.Empty;
		string patientWeigt = string.Empty;
		IDictionary<string, List<DerivedAnswers>> dictionary = new Dictionary<string, List<DerivedAnswers>>();
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


		public void GetVitalSign()
		{
			var trs = driver.FindElements(By.XPath("//div[@id='vitalsigns']//table[@id='mainTable']//tr[2]//td[2]//ul//li[2]"));
			if (trs.Count > 0)
			{
				patientHeight = trs[0].Text;
				patientWeigt = trs[1].Text;
			}

			var trs2 = driver.FindElements(By.XPath("//div[@id='vitalsigns']//table[@id='mainTable']//tr[3]//td[2]"));

			if (trs2.Count > 0)
			{

				var radioButtons = trs2[0].FindElements(By.XPath(".//img"));
			}

		}

		public void PutVitalSign()
		{
			try
			{
				var height = driver.FindElement(By.Id("cVS_height"));

				IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;

				if (height != null)
				{
					executor.ExecuteScript("arguments[0].value='" + patientHeight + "';", height);
				}

				var weight = driver.FindElement(By.Id("cVS_weight"));
				if (weight != null)
				{
					executor.ExecuteScript("arguments[0].value='" + patientWeigt + "';", weight);
				}
			}
			catch (Exception ex)
			{
				Log.Error("Vital Sign Failed " + ex.ToString());
			}

		}

		public void InsertGenericContent(string key)
		{
			try
			{
				List<DerivedAnswers> data;
				if (dictionary.TryGetValue(key, out data))
				{
					var checkboxes = driver.FindElements(By.XPath("//*[@type='checkbox' or @type='radio']"));

					if (checkboxes.Count > 0)
					{

						foreach (DerivedAnswers answer in data)
						{
							if (!answer.IsTextArea)
							{

								if (!checkboxes[answer.SelectedOption].Selected)
								{
									checkboxes[answer.SelectedOption].Click();
								}
							}
						}
					}


					var textboxes = driver.FindElements(By.XPath("//table[@id='mainTable']//*[contains(@name,'isOasis') and not(@type='hidden' or @type='checkbox' or @type='radio')]"));

					if (textboxes.Count > 0)
					{
						IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
						var textData = data.Where(x => x.IsTextArea == true).ToList();
			
						foreach (DerivedAnswers answer in textData)
						{
								
									executor.ExecuteScript("arguments[0].value='" + answer.Text + "';", textboxes[answer.SelectedOption]);
								

						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception in page " + key, ex);
			}
			

		}

		public List<DerivedAnswers> GetText(string xPath,string key)
		{
			var trs = driver.FindElements(By.XPath(xPath));
			List<DerivedAnswers> data = new List<DerivedAnswers>();

			if (trs.Count > 0)
			{
				int selectedTextArea = 0;
				foreach (IWebElement elem in trs)
				{

					// get the text 
					var textElements = elem.FindElements(By.XPath(".//*[contains(translate(@class, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'printdata') and not(descendant::span)]"));
					
					foreach (IWebElement text in textElements)
					{
						var eachAnswer = new DerivedAnswers();

						if (!string.IsNullOrWhiteSpace(text.Text))
						{
							eachAnswer.Text = text.Text.Trim();
							eachAnswer.IsTextArea = true;
							eachAnswer.SelectedOption = selectedTextArea;
							data.Add(eachAnswer);
						}

						selectedTextArea++;


					}
				}

				
			}

			dictionary.Add(key, data);

			return data;

		}

		public void GetContentByPage()
		{
			foreach (KeyValuePair<string, string> keyValue in Constants.TitleMap())
			{

				GetConentent2(keyValue.Value, keyValue.Key);	

			}

		}

		public void GetConentent2(string xPath, string key)
		{
			var trs = driver.FindElements(By.XPath(xPath));
			List<DerivedAnswers> data = new List<DerivedAnswers>();
			if (trs.Count > 0)
			{

				foreach (IWebElement elem in trs)
				{
					// checkboxes and radio buttons
					var radioButtons = elem.FindElements(By.XPath(".//img"));

					//get selected options


					if (radioButtons != null && radioButtons.Count > 0)
					{
						int selectedOption = 0;
						foreach (IWebElement radio in radioButtons)
						{
							var eachAnswer = new DerivedAnswers();
							bool answerFound = false;

							if (radio.GetAttribute("src").Contains("radioBtn-selected.png"))
							{
								//Log.Info(" " + hidden.Text + "Selected Option number is " + selectedOption);
								answerFound = true;
								eachAnswer.IsMultiChoise = false;
								eachAnswer.AnswerCaptured = true;
								eachAnswer.SelectedOption = selectedOption;

								data.Add(eachAnswer);
							}
							else if (radio.GetAttribute("src").Contains("checkbox-selected.png"))
							{
								//Log.Info(" " + hidden.Text + "Selected Checkbox is  number is " + i);
								answerFound = true;
								eachAnswer.IsMultiChoise = true;
								eachAnswer.AnswerCaptured = true;
								eachAnswer.SelectedOption = selectedOption;

								data.Add(eachAnswer);

							}

							selectedOption++;

						}
					}

					
				}

				var data2 = GetText(xPath,key);
				data.AddRange(data2);
				if (!dictionary.Keys.Contains(key))
				{
					dictionary.Add(key, data);
				}

			}


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


		public IDictionary<string, DerivedAnswers> GetPLaneCare()
		{
			var trs = driver.FindElements(By.XPath("//div[@class='OasisHeading'  and contains(text(), 'Therapy Need and Plan of Care')]//parent::div//following::table[@id='mainTable'][1]//tr"));

			IDictionary<string, DerivedAnswers> data = new Dictionary<string, DerivedAnswers>();

			var mappings = Constants.HiddenMap();


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
					hidden = mappings.ContainsKey(hidden) ? mappings[hidden] : hidden;

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
			var element = driver.FindElement(By.Id("cTO_visitdate"));

			IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
			executor.ExecuteScript("arguments[0].value='" + date + "';", element);

			//
			driver.FindElement(By.Id("M0090_INFO_COMPLETED_DT")).Clear();
			var element2 = driver.FindElement(By.Id("M0090_INFO_COMPLETED_DT"));
			executor.ExecuteScript("arguments[0].value='" + date + "';", element2);


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

		public void FillContent(IDictionary<string, DerivedAnswers> data, IDictionary<string, DerivedAnswers> careManagementData, IDictionary<string, DerivedAnswers> plancareData)
		{

			
			Log.Info("JSON " + JsonConvert.SerializeObject(data));
			Log.Info("JSON " + JsonConvert.SerializeObject(careManagementData));
			Log.Info("JSON " + JsonConvert.SerializeObject(plancareData));
			Log.Info("JSON " + JsonConvert.SerializeObject(dictionary));
			var pageTitle =driver.FindElement(By.XPath("//li[@class='span-12']"));
			
			switch (pageTitle?.Text)
			{

				case "Care Management":
					FillCareManagement(careManagementData);

					break;
				case "Data Items Collected at Inpatient Facility Admission or Agency Discharge Only":
				case "Therapy Need and Plan of Care":
					FillCareManagement(plancareData);
					break;
				case "Patient History and Diagnoses":
					PutVitalSign();
					break;

				default:

					if (Constants.TitleMap().ContainsKey(pageTitle.Text))
					{
						InsertGenericContent(pageTitle.Text);
					}
				

					break;

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

								if (selectedOptions.Count > 0)
								{
									for (int i = 0; i < answer.SelectedCheckBoxes.Count; i++)
									{

										if (!selectedOptions[answer.SelectedCheckBoxes[i]].Selected)
										{

											selectedOptions[answer.SelectedCheckBoxes[i]].Click();
										}
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

