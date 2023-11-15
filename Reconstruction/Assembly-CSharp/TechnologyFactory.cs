using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class TechnologyFactory
{
	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001182E File Offset: 0x0000FA2E
	private static bool isInitialize
	{
		get
		{
			return TechnologyFactory.TechDIC != null;
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x00011838 File Offset: 0x0000FA38
	public static void Initialize()
	{
		if (TechnologyFactory.isInitialize)
		{
			return;
		}
		IEnumerable<Type> enumerable = from myType in Assembly.GetAssembly(typeof(Technology)).GetTypes()
		where myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Technology))
		select myType;
		TechnologyFactory.TechDIC = new Dictionary<int, Technology>();
		foreach (Type type in enumerable)
		{
			Technology technology = Activator.CreateInstance(type) as Technology;
			technology.InitializeTech();
			TechnologyFactory.TechDIC.Add((int)technology.TechnologyName, technology);
		}
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x000118E4 File Offset: 0x0000FAE4
	public static void SetBattleTechs()
	{
		TechnologyFactory.BattleTechs = new List<Technology>();
		foreach (Technology technology in TechnologyFactory.TechDIC.Values)
		{
			if (technology.Add && technology.RefactorBinding == RefactorTurretName.None)
			{
				TechnologyFactory.BattleTechs.Add(technology);
			}
		}
		TechnologyFactory.ResetAllTech();
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00011960 File Offset: 0x0000FB60
	public static void SetRecipeTechs()
	{
		foreach (TurretAttribute turretAttribute in Singleton<StaticData>.Instance.ContentFactory.BattleRecipes)
		{
			foreach (Technology technology in TechnologyFactory.TechDIC.Values)
			{
				if (turretAttribute.RefactorName == technology.RefactorBinding)
				{
					TechnologyFactory.BattleTechs.Add(technology);
					break;
				}
			}
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00011A10 File Offset: 0x0000FC10
	public static void ResetAllTech()
	{
		foreach (Technology technology in TechnologyFactory.TechDIC.Values)
		{
			technology.IsAbnormal = false;
			technology.SaveValue = 0f;
		}
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00011A70 File Offset: 0x0000FC70
	public static Technology GetTech(int techName)
	{
		if (TechnologyFactory.TechDIC.ContainsKey(techName))
		{
			return TechnologyFactory.TechDIC[techName];
		}
		Debug.LogWarning("没有对应的科技" + techName.ToString());
		return null;
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00011AA4 File Offset: 0x0000FCA4
	public static Technology GetBattleTech(int techName)
	{
		foreach (Technology technology in TechnologyFactory.BattleTechs)
		{
			if (technology.TechnologyName == (TechnologyName)techName)
			{
				return technology;
			}
		}
		string str = "没有对应的科技";
		TechnologyName technologyName = (TechnologyName)techName;
		Debug.LogWarning(str + technologyName.ToString());
		return null;
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00011B20 File Offset: 0x0000FD20
	public static List<Technology> GetRandomTechs(int count)
	{
		List<int> list = StaticData.SelectNoRepeat(TechnologyFactory.BattleTechs.Count, count);
		List<Technology> list2 = new List<Technology>();
		foreach (int index in list)
		{
			list2.Add(TechnologyFactory.BattleTechs[index]);
		}
		return list2;
	}

	// Token: 0x040002FA RID: 762
	public static Dictionary<int, Technology> TechDIC;

	// Token: 0x040002FB RID: 763
	public static List<Technology> BattleTechs;
}
