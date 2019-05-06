using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinserLib
{
	public class Constants
	{


		public static IDictionary<string, string> TitleMap()
		{
			Dictionary<string, string> TitleAndPathDic = new Dictionary<string, string>
		{

			{"Risk Assessment", "//div[@id='oasisImmunizations']//table[@id='mainTable']" },
			{"Prognosis", "//div[@id='advancedCarePlan']"},
			{"Sensory Status", "//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Sensory Status')]]"},
			{"Pain", "//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Pain Scale')]]"},
			{"Integumentary Status", "//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Integumentary Status')]]"},
			{"Respiratory Status", "//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Respiratory')]]"},
			{"Endocrine", "//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Endocrine')]]"},
			{"Cardiac Status", "//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Cardiovascular')]]"},
			{"Elimination Status", "//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'GU')]]"},
			{ "Nutrition","//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Nutrition')]]"},
			{ "Neuro/Emotional/Behavioral Status","//div[@id='oasisNeuroemostatus']//table[@id='mainTable']"},
			{ "ADL/IADLs","//table[@id='mainTable' and tbody//tr/th[@class='textCenter bold' and contains(text(), 'Musculoskeletal')]]"}

		};

			return TitleAndPathDic;


		}

		public static IDictionary<string, string> HiddenMap()
		{
			IDictionary<string, string> HiddenMap = new Dictionary<string, string>();

			HiddenMap.Add("M2250_PLAN_SMRY_DBTS_FT_CARE", "M2401_INTRVTN_SMRY_DBTS_FT");
			HiddenMap.Add("M2250_PLAN_SMRY_FALL_PRVNT", "M2401_INTRVTN_SMRY_FALL_PRVNT");
			HiddenMap.Add("M2250_PLAN_SMRY_DPRSN_INTRVTN", "M2401_INTRVTN_SMRY_DPRSN");
			HiddenMap.Add("M2250_PLAN_SMRY_PAIN_INTRVTN", "M2401_INTRVTN_SMRY_PAIN_MNTR");
			HiddenMap.Add("M2250_PLAN_SMRY_PRSULC_PRVNT", "M2401_INTRVTN_SMRY_PRSULC_PRVN");
			HiddenMap.Add("M2250_PLAN_SMRY_PRSULC_TRTMT", "M2401_INTRVTN_SMRY_PRSULC_WET");


			return HiddenMap;

		}



	}
}
