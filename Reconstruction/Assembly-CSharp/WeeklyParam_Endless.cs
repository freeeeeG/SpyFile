using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000170 RID: 368
[CreateAssetMenu(menuName = "Param/WeeklyParam_Endless", fileName = "WeeklyParam_Endless")]
public class WeeklyParam_Endless : ScriptableObject
{
	// Token: 0x06000971 RID: 2417 RVA: 0x00018FF4 File Offset: 0x000171F4
	private void GenerateParam()
	{
		this.EndlessParams = new List<EndlessParam>();
		for (int i = 0; i < this.generateCount; i++)
		{
			EndlessParam endlessParam = new EndlessParam();
			endlessParam.Version = i;
			List<int> list = RuleFactory.RuleDIC.Keys.ToList<int>();
			List<int> list2 = StaticData.SelectNoRepeat(list.Count, this.ruleCount);
			List<RuleName> list3 = new List<RuleName>();
			for (int j = 0; j < this.ruleCount; j++)
			{
				list3.Add((RuleName)list[list2[j]]);
			}
			endlessParam.RuleNames = list3;
			List<TurretAttribute> list4 = new List<TurretAttribute>();
			for (int k = 1; k < 7; k++)
			{
				List<TurretAttribute> list5 = new List<TurretAttribute>();
				foreach (TurretAttribute turretAttribute in Singleton<StaticData>.Instance.ContentFactory.RefactorDIC.Values)
				{
					if (turretAttribute.Rare == k)
					{
						list5.Add(turretAttribute);
					}
				}
				list4.Add(list5[Random.Range(0, list5.Count)]);
			}
			endlessParam.Recipes = list4;
			this.EndlessParams.Add(endlessParam);
		}
	}

	// Token: 0x040004D6 RID: 1238
	public List<EndlessParam> EndlessParams;

	// Token: 0x040004D7 RID: 1239
	[SerializeField]
	private int generateCount;

	// Token: 0x040004D8 RID: 1240
	[SerializeField]
	private int ruleCount;
}
