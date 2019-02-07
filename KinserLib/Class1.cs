using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinserLib
{
    public class Constants
    {


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
