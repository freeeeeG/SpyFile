using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class RuleFactory
{
	// Token: 0x1700035E RID: 862
	// (get) Token: 0x06000962 RID: 2402 RVA: 0x00018D35 File Offset: 0x00016F35
	private static bool isInitialize
	{
		get
		{
			return RuleFactory.RuleDIC != null;
		}
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00018D40 File Offset: 0x00016F40
	public static void Initialize()
	{
		if (RuleFactory.isInitialize)
		{
			return;
		}
		RuleFactory.BattleRules = new List<Rule>();
		IEnumerable<Type> enumerable = from myType in Assembly.GetAssembly(typeof(Rule)).GetTypes()
		where myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Rule))
		select myType;
		RuleFactory.RuleDIC = new Dictionary<int, Rule>();
		foreach (Type type in enumerable)
		{
			Rule rule = Activator.CreateInstance(type) as Rule;
			if (rule.Add)
			{
				RuleFactory.RuleDIC.Add((int)rule.RuleName, rule);
			}
		}
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00018DF8 File Offset: 0x00016FF8
	public static Rule GetRule(int ruleName)
	{
		if (RuleFactory.RuleDIC.ContainsKey(ruleName))
		{
			return RuleFactory.RuleDIC[ruleName];
		}
		Debug.LogWarning("没有对应的规则" + ruleName.ToString());
		return null;
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00018E2C File Offset: 0x0001702C
	public static void BeforeRules()
	{
		foreach (Rule rule in RuleFactory.BattleRules)
		{
			rule.BeforeGameLoad();
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00018E7C File Offset: 0x0001707C
	public static void LoadRules()
	{
		foreach (Rule rule in RuleFactory.BattleRules)
		{
			rule.OnGameLoad();
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00018ECC File Offset: 0x000170CC
	public static void InitRules()
	{
		foreach (Rule rule in RuleFactory.BattleRules)
		{
			rule.OnGameInit();
		}
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00018F1C File Offset: 0x0001711C
	public static void LoadSaveRules()
	{
		RuleFactory.BattleRules.Clear();
		foreach (int ruleName in Singleton<LevelManager>.Instance.LastGameSave.SaveRules)
		{
			RuleFactory.BattleRules.Add(RuleFactory.GetRule(ruleName));
		}
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00018F8C File Offset: 0x0001718C
	public static void Release()
	{
		RuleFactory.BattleRules.Clear();
	}

	// Token: 0x040004CD RID: 1229
	public static Dictionary<int, Rule> RuleDIC;

	// Token: 0x040004CE RID: 1230
	public static List<Rule> BattleRules;
}
