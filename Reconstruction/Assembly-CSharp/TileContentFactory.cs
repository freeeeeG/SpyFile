using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001D8 RID: 472
[CreateAssetMenu(menuName = "Factory/ContentFactory", fileName = "GameContentFactory")]
public class TileContentFactory : GameObjectFactory
{
	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06000C24 RID: 3108 RVA: 0x0001F827 File Offset: 0x0001DA27
	// (set) Token: 0x06000C25 RID: 3109 RVA: 0x0001F82F File Offset: 0x0001DA2F
	public List<TurretAttribute> Rare1Att { get; private set; }

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x06000C26 RID: 3110 RVA: 0x0001F838 File Offset: 0x0001DA38
	// (set) Token: 0x06000C27 RID: 3111 RVA: 0x0001F840 File Offset: 0x0001DA40
	public List<TurretAttribute> Rare2Att { get; private set; }

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x06000C28 RID: 3112 RVA: 0x0001F849 File Offset: 0x0001DA49
	// (set) Token: 0x06000C29 RID: 3113 RVA: 0x0001F851 File Offset: 0x0001DA51
	public List<TurretAttribute> Rare3Att { get; private set; }

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x06000C2A RID: 3114 RVA: 0x0001F85A File Offset: 0x0001DA5A
	// (set) Token: 0x06000C2B RID: 3115 RVA: 0x0001F862 File Offset: 0x0001DA62
	public List<TurretAttribute> Rare4Att { get; private set; }

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0001F86B File Offset: 0x0001DA6B
	// (set) Token: 0x06000C2D RID: 3117 RVA: 0x0001F873 File Offset: 0x0001DA73
	public List<TurretAttribute> Rare5Att { get; private set; }

	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0001F87C File Offset: 0x0001DA7C
	// (set) Token: 0x06000C2F RID: 3119 RVA: 0x0001F884 File Offset: 0x0001DA84
	public List<TurretAttribute> Rare6Att { get; private set; }

	// Token: 0x06000C30 RID: 3120 RVA: 0x0001F890 File Offset: 0x0001DA90
	public void Initialize()
	{
		this.ElementDIC = new Dictionary<ElementType, TurretAttribute>();
		this.TrapDIC = new Dictionary<string, TrapAttribute>();
		this.RefactorDIC = new Dictionary<string, TurretAttribute>();
		this.TechDIC = new Dictionary<TechnologyName, TechAttribute>();
		this.Rare1Att = new List<TurretAttribute>();
		this.Rare2Att = new List<TurretAttribute>();
		this.Rare3Att = new List<TurretAttribute>();
		this.Rare4Att = new List<TurretAttribute>();
		this.Rare5Att = new List<TurretAttribute>();
		this.Rare6Att = new List<TurretAttribute>();
		this.BattleTraps = new List<TrapAttribute>();
		ContentAttribute[] array = Resources.LoadAll<TurretAttribute>("SO/RefactorAttribute");
		this.returnAtts = array;
		foreach (TurretAttribute turretAttribute in this.returnAtts)
		{
			this.RefactorDIC.Add(turretAttribute.Name, turretAttribute);
		}
		this.RefactorTurretNames = this.RefactorDIC.Keys.ToList<string>();
		this.RefactorTurretNames.Remove("BOUNTY");
		this.RefactorTurretNames.Remove("TELEPORTOR");
		this.RefactorTurretNames.Remove("PRISM");
		this.RefactorTurretNames.Remove("AMPLIFIER");
		array = Resources.LoadAll<TrapAttribute>("SO/TrapAttribute");
		this.returnAtts = array;
		foreach (TrapAttribute trapAttribute in this.returnAtts)
		{
			if (!trapAttribute.isLock && !trapAttribute.SpecialTrap)
			{
				this.BattleTraps.Add(trapAttribute);
			}
			this.TrapDIC.Add(trapAttribute.Name, trapAttribute);
		}
		this.TrapNames = this.TrapDIC.Keys.ToList<string>();
		this.TrapNames.Remove("BONUSTRAP");
		array = Resources.LoadAll<TurretAttribute>("SO/ElementAttribute");
		this.returnAtts = array;
		foreach (TurretAttribute turretAttribute2 in this.returnAtts)
		{
			this.ElementDIC.Add(turretAttribute2.element, turretAttribute2);
		}
		array = Resources.LoadAll<TechAttribute>("SO/TechAttributes");
		this.returnAtts = array;
		foreach (TechAttribute techAttribute in this.returnAtts)
		{
			this.TechDIC.Add(techAttribute.TechName, techAttribute);
		}
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x0001FAC1 File Offset: 0x0001DCC1
	public void SetDefaultRecipes()
	{
		this.BattleRecipes = this.DefaultRecipes.ToList<TurretAttribute>();
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0001FAD4 File Offset: 0x0001DCD4
	public void LoadRareList()
	{
		this.Rare1Att.Clear();
		this.Rare2Att.Clear();
		this.Rare3Att.Clear();
		this.Rare4Att.Clear();
		this.Rare5Att.Clear();
		this.Rare6Att.Clear();
		this.BattleRecipes.Clear();
		foreach (string name in Singleton<LevelManager>.Instance.LastGameSave.SaveBattleRecipes)
		{
			TurretAttribute refactorByString = this.GetRefactorByString(name);
			this.BattleRecipes.Add(refactorByString);
			switch (refactorByString.Rare)
			{
			case 1:
				this.Rare1Att.Add(refactorByString);
				break;
			case 2:
				this.Rare2Att.Add(refactorByString);
				break;
			case 3:
				this.Rare3Att.Add(refactorByString);
				break;
			case 4:
				this.Rare4Att.Add(refactorByString);
				break;
			case 5:
				this.Rare5Att.Add(refactorByString);
				break;
			case 6:
				this.Rare6Att.Add(refactorByString);
				break;
			}
		}
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x0001FC0C File Offset: 0x0001DE0C
	public void SetRareLists()
	{
		this.Rare1Att.Clear();
		this.Rare2Att.Clear();
		this.Rare3Att.Clear();
		this.Rare4Att.Clear();
		this.Rare5Att.Clear();
		this.Rare6Att.Clear();
		foreach (TurretAttribute turretAttribute in this.BattleRecipes)
		{
			switch (turretAttribute.Rare)
			{
			case 1:
				this.Rare1Att.Add(turretAttribute);
				break;
			case 2:
				this.Rare2Att.Add(turretAttribute);
				break;
			case 3:
				this.Rare3Att.Add(turretAttribute);
				break;
			case 4:
				this.Rare4Att.Add(turretAttribute);
				break;
			case 5:
				this.Rare5Att.Add(turretAttribute);
				break;
			case 6:
				this.Rare6Att.Add(turretAttribute);
				break;
			}
		}
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0001FD1C File Offset: 0x0001DF1C
	public GameTileContent GetDestinationPoint()
	{
		TrapContent trapContent = this.Get(this.destinationAtt.Prefab) as TrapContent;
		trapContent.Initialize(this.destinationAtt);
		return trapContent;
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x0001FD40 File Offset: 0x0001DF40
	public GameTileContent GetSpawnPoint()
	{
		TrapContent trapContent = this.Get(this.spawnPointAtt.Prefab) as TrapContent;
		trapContent.Initialize(this.spawnPointAtt);
		return trapContent;
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x0001FD64 File Offset: 0x0001DF64
	public GameTileContent GetBasicContent(GameTileContentType type)
	{
		if (type == GameTileContentType.Empty)
		{
			return this.Get(this.emptyAtt.Prefab) as GameTileContent;
		}
		return null;
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x0001FD84 File Offset: 0x0001DF84
	public ElementTurret GetElementTurret(ElementType element, int quality)
	{
		TurretAttribute turretAttribute = this.ElementDIC[element];
		ElementTurret elementTurret = this.Get(turretAttribute.Prefab) as ElementTurret;
		elementTurret.InitializeTurret(new StrategyBase(turretAttribute, quality));
		return elementTurret;
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0001FDBC File Offset: 0x0001DFBC
	public TurretAttribute GetElementAttribute(ElementType element)
	{
		return this.ElementDIC[element];
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x0001FDCA File Offset: 0x0001DFCA
	public TurretAttribute GetRandomElementAttribute()
	{
		return this.ElementDIC[(ElementType)Random.Range(0, this.ElementDIC.Count)];
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0001FDE8 File Offset: 0x0001DFE8
	public RefactorTurret GetRefactorTurret(RefactorStrategy strategy)
	{
		RefactorTurret refactorTurret = this.Get(strategy.Attribute.Prefab) as RefactorTurret;
		refactorTurret.InitializeTurret(strategy);
		strategy.CompositeSkill();
		GameRes.TotalRefactor++;
		return refactorTurret;
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x0001FE19 File Offset: 0x0001E019
	public TurretAttribute GetRefactorByString(string name)
	{
		if (this.RefactorDIC.ContainsKey(name))
		{
			return this.RefactorDIC[name];
		}
		Debug.LogWarning("没有对应名字的合成塔");
		return null;
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0001FE41 File Offset: 0x0001E041
	public string GetRandomRefactorName()
	{
		return this.RefactorTurretNames[Random.Range(0, this.RefactorTurretNames.Count)];
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x0001FE5F File Offset: 0x0001E05F
	public string GetRandomTrapName()
	{
		return this.TrapNames[Random.Range(0, this.TrapNames.Count)];
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x0001FE80 File Offset: 0x0001E080
	public TurretAttribute GetRandomCompositeAtt()
	{
		float[] array = new float[6];
		for (int i = 0; i < 6; i++)
		{
			array[i] = StaticData.RareChances[GameRes.SystemLevel - 1, i];
		}
		int num = StaticData.RandomNumber(array) + 1;
		TurretAttribute result = null;
		switch (num)
		{
		case 1:
			result = this.Rare1Att[Random.Range(0, this.Rare1Att.Count)];
			break;
		case 2:
			result = this.Rare2Att[Random.Range(0, this.Rare2Att.Count)];
			break;
		case 3:
			result = this.Rare3Att[Random.Range(0, this.Rare3Att.Count)];
			break;
		case 4:
			result = this.Rare4Att[Random.Range(0, this.Rare4Att.Count)];
			break;
		case 5:
			result = this.Rare5Att[Random.Range(0, this.Rare5Att.Count)];
			break;
		case 6:
			result = this.Rare6Att[Random.Range(0, this.Rare6Att.Count)];
			break;
		}
		return result;
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x0001FFA4 File Offset: 0x0001E1A4
	public TrapContent GetTrapContentByName(string name, bool isReveal = false)
	{
		if (this.TrapDIC.ContainsKey(name))
		{
			TrapContent trapContent = this.Get(this.TrapDIC[name].Prefab) as TrapContent;
			trapContent.Initialize(this.TrapDIC[name]);
			if (isReveal)
			{
				trapContent.RevealTrap();
			}
			return trapContent;
		}
		Debug.LogWarning("没有对应名字的陷阱");
		return null;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00020004 File Offset: 0x0001E204
	public TrapContent GetRandomTrapContent()
	{
		TrapAttribute trapAttribute = this.BattleTraps[Random.Range(0, this.BattleTraps.Count)];
		TrapContent trapContent = this.Get(trapAttribute.Prefab) as TrapContent;
		trapContent.Initialize(trapAttribute);
		return trapContent;
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x00020046 File Offset: 0x0001E246
	public TrapAttribute GetTrapAtt(string trapName)
	{
		if (this.TrapDIC.ContainsKey(trapName))
		{
			return this.TrapDIC[trapName];
		}
		Debug.LogWarning("不包含该TRAP:" + trapName);
		return null;
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x00020074 File Offset: 0x0001E274
	private ReusableObject Get(ReusableObject prefab)
	{
		return base.CreateInstance(prefab);
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x0002007D File Offset: 0x0001E27D
	public TechAttribute GetTechAtt(TechnologyName techName)
	{
		if (this.TechDIC.ContainsKey(techName))
		{
			return this.TechDIC[techName];
		}
		Debug.LogWarning("没有对应的科技ATT");
		return null;
	}

	// Token: 0x0400060E RID: 1550
	[SerializeField]
	private TrapAttribute emptyAtt;

	// Token: 0x0400060F RID: 1551
	[SerializeField]
	private TrapAttribute spawnPointAtt;

	// Token: 0x04000610 RID: 1552
	[SerializeField]
	private TrapAttribute destinationAtt;

	// Token: 0x04000611 RID: 1553
	private Dictionary<ElementType, TurretAttribute> ElementDIC;

	// Token: 0x04000612 RID: 1554
	public Dictionary<string, TurretAttribute> RefactorDIC;

	// Token: 0x04000613 RID: 1555
	private Dictionary<string, TrapAttribute> TrapDIC;

	// Token: 0x04000614 RID: 1556
	private Dictionary<TechnologyName, TechAttribute> TechDIC;

	// Token: 0x04000615 RID: 1557
	public List<string> RefactorTurretNames;

	// Token: 0x04000616 RID: 1558
	public List<string> TrapNames;

	// Token: 0x0400061D RID: 1565
	[SerializeField]
	private List<TurretAttribute> DefaultRecipes;

	// Token: 0x0400061E RID: 1566
	public List<TrapAttribute> BattleTraps;

	// Token: 0x0400061F RID: 1567
	public List<TurretAttribute> BattleRecipes;

	// Token: 0x04000620 RID: 1568
	private ContentAttribute[] returnAtts;
}
