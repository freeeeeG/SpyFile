using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class TurretSkillFactory
{
	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000695 RID: 1685 RVA: 0x00011DA2 File Offset: 0x0000FFA2
	private static bool isInitialize
	{
		get
		{
			return TurretSkillFactory.TurretSkillDIC != null;
		}
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00011DAC File Offset: 0x0000FFAC
	public static void Initialize()
	{
		if (TurretSkillFactory.isInitialize)
		{
			return;
		}
		IEnumerable<Type> enumerable = from myType in Assembly.GetAssembly(typeof(InitialSkill)).GetTypes()
		where myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(InitialSkill))
		select myType;
		TurretSkillFactory.TurretSkillDIC = new Dictionary<int, Type>();
		foreach (Type type in enumerable)
		{
			InitialSkill initialSkill = Activator.CreateInstance(type) as InitialSkill;
			TurretSkillFactory.TurretSkillDIC.Add((int)initialSkill.EffectName, type);
		}
		TurretSkillFactory.InitialzieElementDIC();
		TurretSkillFactory.InitializeBuildingDIC();
		TurretSkillFactory.InitializeGlobalSkillDIC();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00011E64 File Offset: 0x00010064
	private static void InitializeGlobalSkillDIC()
	{
		IEnumerable<Type> enumerable = from myType in Assembly.GetAssembly(typeof(GlobalSkill)).GetTypes()
		where myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(GlobalSkill))
		select myType;
		TurretSkillFactory.GlobalSkillDIC = new Dictionary<int, Type>();
		foreach (Type type in enumerable)
		{
			GlobalSkill globalSkill = Activator.CreateInstance(type) as GlobalSkill;
			TurretSkillFactory.GlobalSkillDIC.Add((int)globalSkill.GlobalSkillName, type);
		}
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x00011F04 File Offset: 0x00010104
	private static void InitialzieElementDIC()
	{
		IEnumerable<Type> enumerable = from myType in Assembly.GetAssembly(typeof(ElementSkill)).GetTypes()
		where myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ElementSkill))
		select myType;
		TurretSkillFactory.ElementSkillDIC = new Dictionary<List<int>, Type>();
		foreach (Type type in enumerable)
		{
			ElementSkill elementSkill = Activator.CreateInstance(type) as ElementSkill;
			if (!TurretSkillFactory.ElementSkillDIC.ContainsKey(elementSkill.InitElements))
			{
				TurretSkillFactory.ElementSkillDIC.Add(elementSkill.InitElements, type);
			}
			else
			{
				Debug.LogWarning("重复的元素搭配：" + elementSkill.InitElements[0].ToString() + elementSkill.InitElements[1].ToString() + elementSkill.InitElements[2].ToString());
			}
		}
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00012008 File Offset: 0x00010208
	private static void InitializeBuildingDIC()
	{
		IEnumerable<Type> enumerable = from myType in Assembly.GetAssembly(typeof(BuildingSkill)).GetTypes()
		where myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BuildingSkill))
		select myType;
		TurretSkillFactory.BuildingSkillDIC = new Dictionary<int, Type>();
		foreach (Type type in enumerable)
		{
			BuildingSkill buildingSkill = Activator.CreateInstance(type) as BuildingSkill;
			TurretSkillFactory.BuildingSkillDIC.Add((int)buildingSkill.BuildingSkillName, type);
		}
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x000120A8 File Offset: 0x000102A8
	public static InitialSkill GetInitialSkill(int id)
	{
		if (TurretSkillFactory.TurretSkillDIC.ContainsKey(id))
		{
			return Activator.CreateInstance(TurretSkillFactory.TurretSkillDIC[id]) as InitialSkill;
		}
		return null;
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x000120D0 File Offset: 0x000102D0
	public static GlobalSkill GetGlobalSkill(GlobalSkillInfo info)
	{
		if (TurretSkillFactory.GlobalSkillDIC.ContainsKey((int)info.SkillName))
		{
			GlobalSkill globalSkill = Activator.CreateInstance(TurretSkillFactory.GlobalSkillDIC[(int)info.SkillName]) as GlobalSkill;
			globalSkill.IsAbnormal = info.IsAbnormal;
			return globalSkill;
		}
		Debug.LogWarning("不存在该全局技能");
		return null;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00012121 File Offset: 0x00010321
	public static BuildingSkill GetBuidlingSkill(int id, bool isAbnormal)
	{
		if (TurretSkillFactory.BuildingSkillDIC.ContainsKey(id))
		{
			BuildingSkill buildingSkill = Activator.CreateInstance(TurretSkillFactory.BuildingSkillDIC[id]) as BuildingSkill;
			buildingSkill.IsAbnormalBuilding = isAbnormal;
			return buildingSkill;
		}
		return null;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00012150 File Offset: 0x00010350
	public static ElementSkill GetElementSkill(List<int> elements)
	{
		foreach (List<int> list in TurretSkillFactory.ElementSkillDIC.Keys)
		{
			List<int> list2 = list.ToList<int>();
			foreach (int num in elements)
			{
				if (!list2.Contains(num))
				{
					break;
				}
				list2.Remove(num);
				if (list2.Count == 0)
				{
					ElementSkill elementSkill = Activator.CreateInstance(TurretSkillFactory.ElementSkillDIC[list]) as ElementSkill;
					elementSkill.Elements = elementSkill.InitElements;
					int num2 = num / 10;
					string text = "";
					foreach (int num3 in elementSkill.Elements)
					{
						text += StaticData.ElementDIC[(ElementType)(num3 % 10)].GetElementName;
					}
					text += ((num2 > 0) ? num2.ToString() : "");
					elementSkill.SkillDescription = GameMultiLang.GetTraduction(text + "INFO");
					elementSkill.SkillName = GameMultiLang.GetTraduction(text);
					return elementSkill;
				}
			}
		}
		Debug.LogWarning("没有这个元素技能" + elements[0].ToString() + elements[1].ToString() + elements[2].ToString());
		return null;
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x00012344 File Offset: 0x00010544
	public static void AddGlobalSkill(GlobalSkillInfo skillInfo)
	{
		TurretSkillFactory.GetGlobalSkills.Add(skillInfo);
		foreach (IGameBehavior gameBehavior in Singleton<GameManager>.Instance.refactorTurrets.behaviors)
		{
			GlobalSkill globalSkill = TurretSkillFactory.GetGlobalSkill(skillInfo);
			((TurretContent)gameBehavior).Strategy.AddGlobalSkill(globalSkill);
		}
		foreach (BluePrintGrid bluePrintGrid in BluePrintShopUI.ShopBluePrints)
		{
			GlobalSkill globalSkill2 = TurretSkillFactory.GetGlobalSkill(skillInfo);
			bluePrintGrid.Strategy.AddGlobalSkill(globalSkill2);
		}
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x00012408 File Offset: 0x00010608
	public static void AddGlobalSkillToStrategy(StrategyBase strategy)
	{
		foreach (GlobalSkillInfo info in TurretSkillFactory.GetGlobalSkills)
		{
			GlobalSkill globalSkill = TurretSkillFactory.GetGlobalSkill(info);
			strategy.AddGlobalSkill(globalSkill);
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00012460 File Offset: 0x00010660
	public static void Release()
	{
		TurretSkillFactory.GetGlobalSkills.Clear();
	}

	// Token: 0x0400030D RID: 781
	public static Dictionary<int, Type> TurretSkillDIC;

	// Token: 0x0400030E RID: 782
	public static Dictionary<List<int>, Type> ElementSkillDIC;

	// Token: 0x0400030F RID: 783
	public static Dictionary<int, Type> BuildingSkillDIC;

	// Token: 0x04000310 RID: 784
	public static Dictionary<int, Type> GlobalSkillDIC;

	// Token: 0x04000311 RID: 785
	public static List<GlobalSkillInfo> GetGlobalSkills = new List<GlobalSkillInfo>();
}
