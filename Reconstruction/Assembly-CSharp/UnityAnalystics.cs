using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.Analytics;

// Token: 0x020001CD RID: 461
public class UnityAnalystics
{
	// Token: 0x06000BB1 RID: 2993 RVA: 0x0001E832 File Offset: 0x0001CA32
	public static void EnableAnalysitics()
	{
		Analytics.enabled = true;
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x0001E83C File Offset: 0x0001CA3C
	public static void UploadModeData(ModeData modeData)
	{
		try
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["UserName"] = SteamFriends.GetPersonaName();
			dictionary["Wave"] = modeData.Wave;
			if (modeData.BeforeLost != 999)
			{
				dictionary["BeforeLost"] = modeData.BeforeLost;
			}
			if (modeData.HighestTurretStrategy != null)
			{
				dictionary[modeData.HighestTurretStrategy.Attribute.Name] = modeData.HighestTurretStrategy.TotalDamage;
				for (int i = 1; i < modeData.HighestTurretStrategy.TurretSkills.Count; i++)
				{
					ElementSkill elementSkill = modeData.HighestTurretStrategy.TurretSkills[i] as ElementSkill;
					string text = "";
					foreach (int num in elementSkill.InitElements)
					{
						text += num.ToString();
					}
					dictionary[text] = 1;
				}
			}
			Debug.Log("ModeData-Upload:" + Analytics.CustomEvent("Mode" + modeData.ModeID.ToString(), dictionary).ToString() + "ModeID:" + modeData.ModeID.ToString());
		}
		catch
		{
			Debug.Log("Uploaddata Unsucessful");
		}
	}
}
