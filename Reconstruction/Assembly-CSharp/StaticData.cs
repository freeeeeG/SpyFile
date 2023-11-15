using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

// Token: 0x02000142 RID: 322
public class StaticData : Singleton<StaticData>
{
	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06000853 RID: 2131 RVA: 0x00015C23 File Offset: 0x00013E23
	public TileFactory TileFactory
	{
		get
		{
			return this._tileFactory;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06000854 RID: 2132 RVA: 0x00015C2B File Offset: 0x00013E2B
	public TileContentFactory ContentFactory
	{
		get
		{
			return this._contentFactory;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06000855 RID: 2133 RVA: 0x00015C33 File Offset: 0x00013E33
	public TileShapeFactory ShapeFactory
	{
		get
		{
			return this._shapeFactory;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06000856 RID: 2134 RVA: 0x00015C3B File Offset: 0x00013E3B
	public EnemyFactory EnemyFactory
	{
		get
		{
			return this._enemyFactory;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06000857 RID: 2135 RVA: 0x00015C43 File Offset: 0x00013E43
	public TurretStrategyFactory StrategyFactory
	{
		get
		{
			return this._strategyFactory;
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06000858 RID: 2136 RVA: 0x00015C4B File Offset: 0x00013E4B
	public NonEnemyFactory NonEnemyFactory
	{
		get
		{
			return this._nonEnemyFactory;
		}
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06000859 RID: 2137 RVA: 0x00015C53 File Offset: 0x00013E53
	// (set) Token: 0x0600085A RID: 2138 RVA: 0x00015C5A File Offset: 0x00013E5A
	public static bool ShowDamage
	{
		get
		{
			return StaticData.showDamage;
		}
		set
		{
			StaticData.showDamage = value;
			PlayerPrefs.SetInt("UI_ShowDamage", StaticData.showDamage ? 0 : 1);
		}
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x0600085B RID: 2139 RVA: 0x00015C77 File Offset: 0x00013E77
	// (set) Token: 0x0600085C RID: 2140 RVA: 0x00015C7E File Offset: 0x00013E7E
	public static bool ShowIntensify
	{
		get
		{
			return StaticData.showIntensify;
		}
		set
		{
			StaticData.showIntensify = value;
			PlayerPrefs.SetInt("UI_ShowIntensify", StaticData.showIntensify ? 0 : 1);
			Singleton<GameEvents>.Instance.ShowDamageIntensify(StaticData.showIntensify);
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x0600085D RID: 2141 RVA: 0x00015CAA File Offset: 0x00013EAA
	public static Collider2D[] AllBuffer
	{
		get
		{
			return StaticData.allBuffer;
		}
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x0600085E RID: 2142 RVA: 0x00015CB1 File Offset: 0x00013EB1
	// (set) Token: 0x0600085F RID: 2143 RVA: 0x00015CB8 File Offset: 0x00013EB8
	public static int BufferCount { get; private set; }

	// Token: 0x06000860 RID: 2144 RVA: 0x00015CC0 File Offset: 0x00013EC0
	public void Initialize()
	{
		this.InitializeData();
		this.TileFactory.Initialize();
		this.ContentFactory.Initialize();
		this.ShapeFactory.Initialize();
		this.EnemyFactory.InitializeFactory();
		StaticData.SetTipsPos();
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00015CFC File Offset: 0x00013EFC
	public static void SetTipsPos()
	{
		StaticData.MidUpPos = new Vector2((float)(Screen.width / 2), (float)Screen.height - 100f);
		StaticData.MidTipsPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		StaticData.LeftTipsPos = new Vector2((float)(Screen.width / 5), (float)(Screen.height / 2));
		StaticData.LeftMidTipsPos = new Vector2((float)Screen.width / 2.5f, (float)(Screen.height / 2));
		StaticData.RightTipsPos = new Vector2((float)(Screen.width - Screen.width / 5), (float)(Screen.height / 2));
		StaticData.RightMidTipsPos = new Vector2((float)Screen.width - (float)Screen.width / 2.5f, (float)(Screen.height / 2));
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00015DC0 File Offset: 0x00013FC0
	private void InitializeData()
	{
		StaticData.ElementDIC = new Dictionary<ElementType, Element>();
		StaticData.ElementDIC.Add(ElementType.None, new None());
		StaticData.ElementDIC.Add(ElementType.GOLD, new Gold());
		StaticData.ElementDIC.Add(ElementType.WOOD, new Wood());
		StaticData.ElementDIC.Add(ElementType.WATER, new Water());
		StaticData.ElementDIC.Add(ElementType.FIRE, new Fire());
		StaticData.ElementDIC.Add(ElementType.DUST, new Dust());
		StaticData.showDamage = (PlayerPrefs.GetInt("UI_ShowDamage", 0) == 0);
		StaticData.showIntensify = (PlayerPrefs.GetInt("UI_ShowIntensify", 1) == 0);
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00015E63 File Offset: 0x00014063
	public static bool FillBuffer(Vector2 pos, float range, int layerMask)
	{
		StaticData.BufferCount = Physics2D.OverlapCircleNonAlloc(pos, range, StaticData.allBuffer, layerMask);
		return StaticData.BufferCount > 0;
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00015E7F File Offset: 0x0001407F
	public static Collider2D GetBuffer(int index)
	{
		return StaticData.allBuffer[index];
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00015E88 File Offset: 0x00014088
	public static List<T> RandomSort<T>(List<T> list)
	{
		Random random = new Random();
		List<T> list2 = new List<T>();
		foreach (T item in list)
		{
			list2.Insert(random.Next(list2.Count), item);
		}
		return list2;
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00015EF0 File Offset: 0x000140F0
	public static int RandomNumber(float[] pros)
	{
		float num = 0f;
		foreach (float num2 in pros)
		{
			num += num2;
		}
		float num3 = Random.value * num;
		for (int j = 0; j < pros.Length; j++)
		{
			if (num3 < pros[j])
			{
				return j;
			}
			num3 -= pros[j];
		}
		return pros.Length - 1;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00015F50 File Offset: 0x00014150
	public static int[] GetRandomSequence(int total, int count)
	{
		int[] array = new int[total];
		int[] array2 = new int[count];
		for (int i = 0; i < total; i++)
		{
			array[i] = i;
		}
		int num = total - 1;
		for (int j = 0; j < count; j++)
		{
			int num2 = Random.Range(0, num + 1);
			array2[j] = array[num2];
			array[num2] = array[num];
			num--;
		}
		return array2;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00015FB0 File Offset: 0x000141B0
	public static List<Vector2Int> GetCirclePoints(int range, int forbidRange = 0)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = -range; i <= range; i++)
		{
			for (int j = -(range - Mathf.Abs(i)); j <= range - Mathf.Abs(i); j++)
			{
				if (i != 0 || j != 0)
				{
					Vector2Int item = new Vector2Int(i, j);
					list.Add(item);
				}
			}
		}
		if (forbidRange > 0)
		{
			List<Vector2Int> list2 = new List<Vector2Int>();
			for (int k = -forbidRange; k <= forbidRange; k++)
			{
				for (int l = -(forbidRange - Mathf.Abs(k)); l <= forbidRange - Mathf.Abs(k); l++)
				{
					if (k != 0 || l != 0)
					{
						Vector2Int item2 = new Vector2Int(k, l);
						list2.Add(item2);
					}
				}
			}
			return list.Except(list2).ToList<Vector2Int>();
		}
		return list;
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0001606C File Offset: 0x0001426C
	public static List<Vector2Int> GetHalfCirclePoints(int range, int forbidRange = 0)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = -range; i <= range; i++)
		{
			for (int j = 0; j <= range - Mathf.Abs(i); j++)
			{
				if (i != 0 || j != 0)
				{
					Vector2Int item = new Vector2Int(i, j);
					list.Add(item);
				}
			}
		}
		if (forbidRange > 0)
		{
			List<Vector2Int> list2 = new List<Vector2Int>();
			for (int k = -forbidRange; k <= forbidRange; k++)
			{
				for (int l = 0; l <= forbidRange - Mathf.Abs(k); l++)
				{
					if (k != 0 || l != 0)
					{
						Vector2Int item2 = new Vector2Int(k, l);
						list2.Add(item2);
					}
				}
			}
			return list.Except(list2).ToList<Vector2Int>();
		}
		return list;
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00016114 File Offset: 0x00014314
	public static List<Vector2Int> GetLinePoints(int range, int forbidRange = 0)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 1; i <= range; i++)
		{
			Vector2Int item = new Vector2Int(0, i);
			list.Add(item);
		}
		if (forbidRange > 0)
		{
			List<Vector2Int> list2 = new List<Vector2Int>();
			for (int j = 1; j <= forbidRange; j++)
			{
				Vector2Int item2 = new Vector2Int(0, j);
				list2.Add(item2);
			}
			return list.Except(list2).ToList<Vector2Int>();
		}
		return list;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00016180 File Offset: 0x00014380
	public static int[] GetSomeRandoms(int levelMin, int levelMax, int totalLevel, int number)
	{
		if (totalLevel / number > levelMax)
		{
			Debug.LogWarning("配方等级输错了，菜鸡");
			int[] array = new int[number];
			for (int i = 0; i < number; i++)
			{
				array[i] = levelMax;
			}
			return array;
		}
		int[] array2 = new int[number];
		while (number > 1)
		{
			if (number == 2)
			{
				int num = levelMin;
				while (totalLevel - num > levelMax)
				{
					num++;
				}
				array2[0] = Random.Range(num, totalLevel - num + 1);
				array2[1] = totalLevel - array2[0];
				number--;
			}
			else
			{
				if (number < 2)
				{
					Debug.LogWarning("刷随机等级的算法不支持！");
					return null;
				}
				int num2 = Mathf.Min(totalLevel - (number - 1), levelMax);
				int num3 = levelMin;
				while (totalLevel - num3 > levelMax * (number - 1))
				{
					num3++;
				}
				int num4 = Random.Range(num3, num2 + 1);
				totalLevel -= num4;
				array2[number - 1] = num4;
				number--;
			}
		}
		return array2;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00016250 File Offset: 0x00014450
	public static List<int> SelectNoRepeat(int total, int number)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < total; i++)
		{
			list.Add(i);
		}
		if (list.Count < number)
		{
			return list;
		}
		List<int> list2 = new List<int>();
		for (int j = 0; j < number; j++)
		{
			int index = Random.Range(0, list.Count);
			list2.Add(list[index]);
			list.RemoveAt(index);
		}
		return list2;
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x000162B8 File Offset: 0x000144B8
	public static Collider2D RaycastCollider(Vector2 pos, LayerMask layer)
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(pos, Vector3.forward, float.PositiveInfinity, layer);
		if (raycastHit2D.collider != null)
		{
			return raycastHit2D.collider;
		}
		return null;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x000162FC File Offset: 0x000144FC
	public static string SetElementSkillInfo(ElementSkill skill)
	{
		string text = GameMultiLang.GetTraduction("ELEMENTSKILL") + ":";
		foreach (int key in skill.InitElements)
		{
			text += StaticData.ElementDIC[(ElementType)key].GetSkillText;
		}
		return text;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x00016378 File Offset: 0x00014578
	public static string GetTurretDes(TurretSkill skill)
	{
		return string.Format(GameMultiLang.GetTraduction(skill.strategy.Attribute.Name + "SKILL"), new object[]
		{
			"<b>" + skill.DisplayValue + "</b>",
			"<b>" + skill.DisplayValue2 + "</b>",
			"<b>" + skill.DisplayValue3 + "</b>",
			"<b>" + skill.DisplayValue4 + "</b>",
			"<b>" + skill.DisplayValue5 + "</b>"
		}) + StaticData.ElementDIC[skill.IntensifyElement].GetExtraInfo;
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x00016444 File Offset: 0x00014644
	public static string GetBuildingDes(BuildingSkill skill)
	{
		return string.Format(skill.IsAbnormalBuilding ? (StaticData.ElementDIC[ElementType.GOLD].Colorized("<sprite=8>" + GameMultiLang.GetTraduction(skill.BuildingSkillName.ToString() + "INFO")) + "\n" + StaticData.ElementDIC[ElementType.FIRE].Colorized("<sprite=9>" + GameMultiLang.GetTraduction(skill.BuildingSkillName.ToString() + "INFO2"))) : GameMultiLang.GetTraduction(skill.BuildingSkillName.ToString() + "INFO"), new object[]
		{
			"<b>" + skill.DisplayValue + "</b>",
			"<b>" + skill.DisplayValue2 + "</b>",
			"<b>" + skill.DisplayValue3 + "</b>",
			"<b>" + skill.DisplayValue4 + "</b>",
			"<b>" + skill.DisplayValue5 + "</b>"
		});
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00016588 File Offset: 0x00014788
	public static string GetLevelInfo(int level)
	{
		float[] array = new float[5];
		for (int i = 0; i < 5; i++)
		{
			array[i] = StaticData.QualityChances[level - 1, i];
		}
		string text = "";
		text = text + GameMultiLang.GetTraduction("MODULELEVELINFO1") + ":\n";
		for (int j = 0; j < 5; j++)
		{
			text = string.Concat(new string[]
			{
				text,
				GameMultiLang.GetTraduction("MODULELEVELINFO2"),
				(j + 1).ToString(),
				": ",
				(array[j] * 100f).ToString(),
				"%\n"
			});
		}
		return text + GameMultiLang.GetTraduction("MODULELEVELINFO3");
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x00016644 File Offset: 0x00014844
	public static string GetEnergyInfo()
	{
		return GameMultiLang.GetTraduction("ENERGYINFO");
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x00016650 File Offset: 0x00014850
	public static void CorrectTileCoord(TileBase tile)
	{
		Vector2Int vector2Int = new Vector2Int(Convert.ToInt32(tile.transform.position.x), Convert.ToInt32(tile.transform.position.y));
		int x = vector2Int.x + StaticData.BoardOffset.x;
		int y = vector2Int.y + StaticData.BoardOffset.y;
		tile.OffsetCoord = new Vector2Int(x, y);
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x000166C4 File Offset: 0x000148C4
	public static void SetNodeWalkable(TileBase tile, bool walkable, bool changeAble = true)
	{
		GridGraph gridGraph = AstarPath.active.data.gridGraph;
		int x = tile.OffsetCoord.x;
		int y = tile.OffsetCoord.y;
		GridNode gridNode = gridGraph.nodes[y * gridGraph.width + x];
		gridNode.Walkable = walkable;
		gridNode.ChangeAbleNode = changeAble;
		gridGraph.CalculateConnectionsForCellAndNeighbours(x, y);
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00016728 File Offset: 0x00014928
	public static bool GetNodeWalkable(TileBase tile)
	{
		GridGraph gridGraph = AstarPath.active.data.gridGraph;
		int x = tile.OffsetCoord.x;
		int y = tile.OffsetCoord.y;
		return gridGraph.nodes[y * gridGraph.width + x].Walkable;
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00016779 File Offset: 0x00014979
	public void ShowJumpDamage(Vector2 pos, long amount, bool isCritical)
	{
		if (!StaticData.ShowDamage)
		{
			return;
		}
		(Singleton<ObjectPool>.Instance.Spawn(this.JumpDamagePrefab) as JumpDamage).Jump(amount, pos, isCritical);
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x000167A0 File Offset: 0x000149A0
	public static void C(List<int> lsArray, int selectCount)
	{
		int count = lsArray.Count;
		int[] array = new int[selectCount];
		int num = selectCount - 1;
		for (int i = 0; i < selectCount; i++)
		{
			array[i] = i;
		}
		for (;;)
		{
			for (int j = 0; j < selectCount; j++)
			{
				Debug.Log(lsArray[array[j]]);
			}
			Debug.Log("");
			if (array[num] < count - 1)
			{
				array[num]++;
			}
			else
			{
				int num2 = num;
				while (num2 > 0 && array[num2 - 1] == array[num2] - 1)
				{
					num2--;
				}
				if (num2 == 0)
				{
					break;
				}
				array[num2 - 1]++;
				for (int k = num2; k < selectCount; k++)
				{
					array[k] = array[k - 1] + 1;
				}
			}
		}
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x00016868 File Offset: 0x00014A68
	public static List<List<int>> GetAllC(List<int> dataList, int n, List<int> value = null)
	{
		List<List<int>> list = new List<List<int>>();
		int i = 0;
		int count = dataList.Count;
		while (i < count)
		{
			List<int> list2 = new List<int>();
			if (value != null && value.Count > 0)
			{
				list2.AddRange(value);
			}
			list2.Add(dataList[i]);
			if (list2.Count == n)
			{
				list.Add(list2);
			}
			else
			{
				list.AddRange(StaticData.GetAllC(dataList, n, list2));
			}
			i++;
		}
		return list;
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x000168D8 File Offset: 0x00014AD8
	public static List<List<int>> GetAllCC(List<int> dataList)
	{
		List<List<int>> list = new List<List<int>>();
		for (int i = 0; i < dataList.Count; i++)
		{
			for (int j = i; j < dataList.Count; j++)
			{
				for (int k = j; k < dataList.Count; k++)
				{
					list.Add(new List<int>
					{
						dataList[i],
						dataList[j],
						dataList[k]
					});
				}
			}
		}
		return list;
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x00016958 File Offset: 0x00014B58
	public static List<List<int>> GetAllCC2(List<int> dataList)
	{
		List<List<int>> list = new List<List<int>>();
		for (int i = 0; i < dataList.Count; i++)
		{
			list.Add(new List<int>
			{
				dataList[i],
				dataList[i],
				dataList[i]
			});
		}
		for (int j = 0; j < dataList.Count; j++)
		{
			for (int k = 0; k < dataList.Count; k++)
			{
				if (k != j)
				{
					list.Add(new List<int>
					{
						dataList[j],
						dataList[j],
						dataList[k]
					});
				}
			}
		}
		for (int l = 0; l < dataList.Count; l++)
		{
			for (int m = l + 1; m < dataList.Count; m++)
			{
				for (int n = m + 1; n < dataList.Count; n++)
				{
					list.Add(new List<int>
					{
						dataList[l],
						dataList[m],
						dataList[n]
					});
				}
			}
		}
		return list;
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00016A98 File Offset: 0x00014C98
	public GameObject GainMoneyEffect(Vector2 pos, int amount)
	{
		Singleton<GameManager>.Instance.GainMoney(amount);
		GameObject gameObject = Singleton<ObjectPool>.Instance.Spawn(this.GainMoneyPrefab).gameObject;
		gameObject.transform.position = pos + Vector2.up * 0.2f;
		gameObject.transform.localScale = Vector3.one;
		Singleton<Sound>.Instance.PlayEffect("Sound_GainCoin");
		GameRes.GainGoldBattleTurn += amount;
		return gameObject;
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00016B18 File Offset: 0x00014D18
	public void GainPerfectEffect(Vector2 pos, int amount)
	{
		Singleton<GameManager>.Instance.GainPerfectElement(amount);
		Singleton<ObjectPool>.Instance.Spawn(this.GainPerfectPrefab).gameObject.transform.position = pos + Vector2.up * 0.2f;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00016B6C File Offset: 0x00014D6C
	public void FrostTurretEffect(Vector2 pos, float distance, float frostTime)
	{
		ReusableObject reusableObject = Singleton<ObjectPool>.Instance.Spawn(this.FrostExplosion);
		reusableObject.transform.position = pos;
		reusableObject.transform.localScale = Vector3.one * distance;
		foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(pos, distance, LayerMask.GetMask(new string[]
		{
			StaticData.TurretMask
		})))
		{
			ConcreteContent component = collider2D.GetComponent<ConcreteContent>();
			FrostEffect effect = this.FrostEffect(collider2D.transform.position);
			component.Frost(frostTime, effect);
		}
		Singleton<Sound>.Instance.PlayEffect("Sound_EnemyExplosionFrost");
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00016C0F File Offset: 0x00014E0F
	public FrostEffect FrostEffect(Vector2 pos)
	{
		FrostEffect frostEffect = Singleton<ObjectPool>.Instance.Spawn(this.FrostEffectPrefab) as FrostEffect;
		frostEffect.transform.position = pos;
		Singleton<Sound>.Instance.PlayEffect("Sound_EnemyExplosionFrost");
		return frostEffect;
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00016C46 File Offset: 0x00014E46
	public static string FormElementName(ElementType element, int quality)
	{
		return "" + StaticData.ElementDIC[element].GetElementName + quality.ToString();
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00016C6E File Offset: 0x00014E6E
	public static string GetElementName(ElementType element)
	{
		return StaticData.ElementDIC[element].GetElementName;
	}

	// Token: 0x0400041D RID: 1053
	[Header("静态数据")]
	public static Vector2Int BoardOffset;

	// Token: 0x0400041E RID: 1054
	public static LayerMask PathLayer = 1088;

	// Token: 0x0400041F RID: 1055
	public static string TrapMask = "Trap";

	// Token: 0x04000420 RID: 1056
	public static string ConcreteTileMask = "ConcreteTile";

	// Token: 0x04000421 RID: 1057
	public static string GroundTileMask = "GroundTile";

	// Token: 0x04000422 RID: 1058
	public static string TempTileMask = "TempTile";

	// Token: 0x04000423 RID: 1059
	public static string TempTurretMask = "TempTurret";

	// Token: 0x04000424 RID: 1060
	public static string TurretMask = "Turret";

	// Token: 0x04000425 RID: 1061
	public static string TempGroundMask = "TempGround";

	// Token: 0x04000426 RID: 1062
	public static string Untagged = "Untagged";

	// Token: 0x04000427 RID: 1063
	public static string OnlyRefactorTag = "OnlyCompositeTurret";

	// Token: 0x04000428 RID: 1064
	public static string UndropablePoint = "UnDropablePoint";

	// Token: 0x04000429 RID: 1065
	public static LayerMask EnemyLayerMask = 2048;

	// Token: 0x0400042A RID: 1066
	public static LayerMask TurretLayerMask = 8192;

	// Token: 0x0400042B RID: 1067
	public static LayerMask GetGroundLayer = 4352;

	// Token: 0x0400042C RID: 1068
	public static LayerMask GetSelectLayer = 1344;

	// Token: 0x0400042D RID: 1069
	public static Vector2 LeftTipsPos;

	// Token: 0x0400042E RID: 1070
	public static Vector2 LeftMidTipsPos;

	// Token: 0x0400042F RID: 1071
	public static Vector2 RightTipsPos;

	// Token: 0x04000430 RID: 1072
	public static Vector2 RightMidTipsPos;

	// Token: 0x04000431 RID: 1073
	public static Vector2 MidTipsPos;

	// Token: 0x04000432 RID: 1074
	public static Vector2 MidUpPos;

	// Token: 0x04000433 RID: 1075
	public static bool LockKeyboard = false;

	// Token: 0x04000434 RID: 1076
	public static float[,] QualityChances = new float[,]
	{
		{
			1f,
			0f,
			0f,
			0f,
			0f
		},
		{
			0.6f,
			0.3f,
			0.1f,
			0f,
			0f
		},
		{
			0.4f,
			0.3f,
			0.25f,
			0.05f,
			0f
		},
		{
			0.3f,
			0.3f,
			0.25f,
			0.15f,
			0f
		},
		{
			0.2f,
			0.25f,
			0.25f,
			0.25f,
			0.05f
		},
		{
			0.15f,
			0.25f,
			0.25f,
			0.25f,
			0.1f
		}
	};

	// Token: 0x04000435 RID: 1077
	public static float[,] RareChances = new float[,]
	{
		{
			0.1f,
			0f,
			0f,
			0f,
			0f,
			0f
		},
		{
			0.1f,
			0.1f,
			0f,
			0f,
			0f,
			0f
		},
		{
			0.1f,
			0.1f,
			0.1f,
			0f,
			0f,
			0f
		},
		{
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0f,
			0f
		},
		{
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0f
		},
		{
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.1f
		}
	};

	// Token: 0x04000436 RID: 1078
	public static float[] ElementBenefit = new float[]
	{
		1f,
		2f,
		2.5f,
		3f
	};

	// Token: 0x04000437 RID: 1079
	[Header("工厂类")]
	[SerializeField]
	private TileFactory _tileFactory;

	// Token: 0x04000438 RID: 1080
	[SerializeField]
	private TileContentFactory _contentFactory;

	// Token: 0x04000439 RID: 1081
	[SerializeField]
	private TileShapeFactory _shapeFactory;

	// Token: 0x0400043A RID: 1082
	[SerializeField]
	private EnemyFactory _enemyFactory;

	// Token: 0x0400043B RID: 1083
	[SerializeField]
	private TurretStrategyFactory _strategyFactory;

	// Token: 0x0400043C RID: 1084
	[SerializeField]
	private NonEnemyFactory _nonEnemyFactory;

	// Token: 0x0400043D RID: 1085
	[Header("战斗基础数值")]
	public int SystemMaxLevel;

	// Token: 0x0400043E RID: 1086
	public int[] LevelUpMoney;

	// Token: 0x0400043F RID: 1087
	public int StartCoin;

	// Token: 0x04000440 RID: 1088
	public int BaseWaveIncome;

	// Token: 0x04000441 RID: 1089
	public int WaveMultiplyIncome;

	// Token: 0x04000442 RID: 1090
	public int ShopRefreshCost;

	// Token: 0x04000443 RID: 1091
	public int BaseShapeCost;

	// Token: 0x04000444 RID: 1092
	public int MultipleShapeCost;

	// Token: 0x04000445 RID: 1093
	public int BuyGroundCost;

	// Token: 0x04000446 RID: 1094
	public int BuyGroundCostMultyply;

	// Token: 0x04000447 RID: 1095
	public int SwitchTrapCost;

	// Token: 0x04000448 RID: 1096
	public int SwitchTurretCost;

	// Token: 0x04000449 RID: 1097
	public int SwitchTrapCostMultiply;

	// Token: 0x0400044A RID: 1098
	[Header("场景数值设置")]
	public static int GroundSize = 25;

	// Token: 0x0400044B RID: 1099
	public static float EnvrionmentBaseVolume = 0.5f;

	// Token: 0x0400044C RID: 1100
	public float TileSize;

	// Token: 0x0400044D RID: 1101
	public static int maxLevel = 5;

	// Token: 0x0400044E RID: 1102
	public static int elementN = 5;

	// Token: 0x0400044F RID: 1103
	public static int maxQuality = 5;

	// Token: 0x04000450 RID: 1104
	public static float DefaultCritDmg = 2f;

	// Token: 0x04000451 RID: 1105
	public static float DefaultSplashDmg = 0.4f;

	// Token: 0x04000452 RID: 1106
	public static int BonusTrapMaxCoinPerTurn = 100;

	// Token: 0x04000453 RID: 1107
	public static float EarlySkillPercent = 0.25f;

	// Token: 0x04000454 RID: 1108
	public static int MaxEnemyAmount = 250;

	// Token: 0x04000455 RID: 1109
	private static bool showDamage;

	// Token: 0x04000456 RID: 1110
	private static bool showIntensify;

	// Token: 0x04000457 RID: 1111
	[Header("元素加成")]
	public float GoldAttackIntensify = 0.3f;

	// Token: 0x04000458 RID: 1112
	public float WoodFirerateIntensify = 0.3f;

	// Token: 0x04000459 RID: 1113
	public float WaterSlowIntensify = 0.5f;

	// Token: 0x0400045A RID: 1114
	public float FireCritIntensify = 0.2f;

	// Token: 0x0400045B RID: 1115
	public float DustSplashIntensify = 0.25f;

	// Token: 0x0400045C RID: 1116
	public static Dictionary<ElementType, Element> ElementDIC;

	// Token: 0x0400045D RID: 1117
	[Header("Prefabs")]
	public RangeHolder RangeHolder;

	// Token: 0x0400045E RID: 1118
	public RangeIndicator RangeIndicatorPrefab;

	// Token: 0x0400045F RID: 1119
	public JumpDamage JumpDamagePrefab;

	// Token: 0x04000460 RID: 1120
	public Sprite UnrevealTrap;

	// Token: 0x04000461 RID: 1121
	public ParticalControl LandedEffect;

	// Token: 0x04000462 RID: 1122
	public ReusableObject GainMoneyPrefab;

	// Token: 0x04000463 RID: 1123
	public ReusableObject FrostExplosion;

	// Token: 0x04000464 RID: 1124
	public ReusableObject FrostEffectPrefab;

	// Token: 0x04000465 RID: 1125
	public ReusableObject GainPerfectPrefab;

	// Token: 0x04000466 RID: 1126
	public ReusableObject BlinkHolePrefab;

	// Token: 0x04000467 RID: 1127
	public Sprite[] ElementSprites;

	// Token: 0x04000468 RID: 1128
	public Color NormalBlue;

	// Token: 0x04000469 RID: 1129
	public Color HighlightBlue;

	// Token: 0x0400046A RID: 1130
	private static Collider2D[] allBuffer = new Collider2D[100];

	// Token: 0x0400046C RID: 1132
	[Header("CompositionAttributes")]
	public int[,] LevelUpCostPerRare = new int[,]
	{
		{
			75,
			150
		},
		{
			125,
			250
		},
		{
			200,
			400
		},
		{
			300,
			600
		},
		{
			400,
			800
		},
		{
			500,
			1000
		}
	};
}
