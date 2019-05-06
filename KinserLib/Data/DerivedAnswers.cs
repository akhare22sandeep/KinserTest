using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinserLib.Data
{

	public class DerivedAnswersList
	{

		public DerivedAnswersList()
		{
			DerivedAnswers = new List<Data.DerivedAnswers>();
		}
		public List<DerivedAnswers> DerivedAnswers { get; set; }
	}
	public class DerivedAnswers
	{

		public DerivedAnswers()
		{
			SelectedCheckBoxes = new List<int>();
		}
		public bool IsMultiChoise { get; set; }

		public bool IsTextArea { get; set; }

		public string Text { get; set; }
		public bool AnswerCaptured { get; set; }

		public int SelectedRadioButtonOption { get; set; }

		public int SelectedOption { get; set; }
		public string Question { get; set; }

		public List<int> SelectedCheckBoxes { get; set; }



	}
}
