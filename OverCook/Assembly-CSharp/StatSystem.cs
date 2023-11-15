using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000746 RID: 1862
public class StatSystem : MonoBehaviour
{
	// Token: 0x060023B2 RID: 9138 RVA: 0x000AAE3C File Offset: 0x000A923C
	private void Awake()
	{
		this.m_saveManager = GameUtils.RequestManager<SaveManager>();
	}

	// Token: 0x060023B3 RID: 9139 RVA: 0x000AAE49 File Offset: 0x000A9249
	public void RegisterStatValueChanged(VoidGeneric<int, float, float> _callback)
	{
		this.m_statValueChangedCallback = (VoidGeneric<int, float, float>)Delegate.Combine(this.m_statValueChangedCallback, _callback);
	}

	// Token: 0x060023B4 RID: 9140 RVA: 0x000AAE62 File Offset: 0x000A9262
	public void UnregisterStatValueChanged(VoidGeneric<int, float, float> _callback)
	{
		this.m_statValueChangedCallback = (VoidGeneric<int, float, float>)Delegate.Remove(this.m_statValueChangedCallback, _callback);
	}

	// Token: 0x060023B5 RID: 9141 RVA: 0x000AAE7B File Offset: 0x000A927B
	public void RegisterTrophyUnlock(VoidGeneric<int> _callback)
	{
		this.m_trophyUnlockCallback = (VoidGeneric<int>)Delegate.Combine(this.m_trophyUnlockCallback, _callback);
	}

	// Token: 0x060023B6 RID: 9142 RVA: 0x000AAE94 File Offset: 0x000A9294
	public void UnregisterTrophyUnlock(VoidGeneric<int> _callback)
	{
		this.m_trophyUnlockCallback = (VoidGeneric<int>)Delegate.Remove(this.m_trophyUnlockCallback, _callback);
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x000AAEAD File Offset: 0x000A92AD
	public void RegisterTrophyProgress(VoidGeneric<int, float> _callback)
	{
		this.m_trophyProgressCallback = (VoidGeneric<int, float>)Delegate.Combine(this.m_trophyProgressCallback, _callback);
	}

	// Token: 0x060023B8 RID: 9144 RVA: 0x000AAEC6 File Offset: 0x000A92C6
	public void UnregisterTrophyProgress(VoidGeneric<int, float> _callback)
	{
		this.m_trophyProgressCallback = (VoidGeneric<int, float>)Delegate.Remove(this.m_trophyProgressCallback, _callback);
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000AAEDF File Offset: 0x000A92DF
	public void Clear()
	{
		this.m_Stats = null;
		this.m_Trophies = null;
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000AAEF0 File Offset: 0x000A92F0
	public void Setup(StatsTracking statsTracking)
	{
		if (this.m_Stats == null)
		{
			this.m_Stats = new FastList<StatSystem.Stat>(statsTracking.m_Stats.Length);
		}
		if (this.m_Trophies == null)
		{
			this.m_Trophies = new FastList<StatSystem.Trophy>(statsTracking.m_Tropies.Length);
		}
		for (int i = 0; i < statsTracking.m_Stats.Length; i++)
		{
			this.CreateStat(statsTracking.m_Stats[i].m_ID.ToString(), (int)statsTracking.m_Stats[i].m_ID, (int)statsTracking.m_Stats[i].m_StatType, statsTracking.m_Stats[i].m_validationList);
		}
		for (int j = 0; j < statsTracking.m_Tropies.Length; j++)
		{
			this.CreateTrophy(statsTracking.m_Tropies[j].m_Name, statsTracking.m_Tropies[j].m_APIName, statsTracking.m_Tropies[j].m_TrophyID, statsTracking.m_Tropies[j].m_Rules.Length, (int)statsTracking.m_Tropies[j].m_CombineMode);
			for (int k = 0; k < statsTracking.m_Tropies[j].m_Rules.Length; k++)
			{
				this.SetUpTrophy(statsTracking.m_Tropies[j].m_TrophyID, (int)statsTracking.m_Tropies[j].m_Rules[k].m_StatID, statsTracking.m_Tropies[j].m_Rules[k].m_RefValue, (int)statsTracking.m_Tropies[j].m_Rules[k].m_Compare);
			}
		}
	}

	// Token: 0x060023BB RID: 9147 RVA: 0x000AB068 File Offset: 0x000A9468
	public void CreateStat(string statName, int ID, int statType, StatValidationList validationList = null)
	{
		StatSystem.Stat stat = new StatSystem.Stat
		{
			m_Name = statName,
			m_ID = ID,
			m_Type = (StatSystem.STAT_TYPE)statType,
			m_validationList = validationList
		};
		if (stat.m_Type == StatSystem.STAT_TYPE.ST_ID_HOLDER)
		{
			stat.m_IDHolder = new Hashtable();
		}
		this.m_Stats.Add(stat);
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000AB0C0 File Offset: 0x000A94C0
	public void CreateTrophy(string trophyName, string apiName, int trophyID, int numberRules, int combiner)
	{
		StatSystem.Trophy item = new StatSystem.Trophy
		{
			m_Name = trophyName,
			m_APIName = apiName,
			m_TrophyID = trophyID,
			m_Rules = new StatSystem.StatRule[numberRules],
			m_RuleCount = 0,
			m_Combiner = (StatSystem.Combiner)combiner
		};
		this.m_Trophies.Add(item);
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x000AB114 File Offset: 0x000A9514
	public void SetUpTrophy(int trophyID, int statID, float refValue, int compareFunc)
	{
		StatSystem.Trophy trophy;
		bool flag = this.FindTrophy(trophyID, out trophy);
		if (flag)
		{
			StatSystem.Stat stat;
			bool flag2 = this.FindStat(statID, out stat);
			if (flag2)
			{
				trophy.m_Rules[trophy.m_RuleCount] = new StatSystem.StatRule();
				trophy.m_Rules[trophy.m_RuleCount].m_Stat = stat;
				trophy.m_Rules[trophy.m_RuleCount].m_RefValue = refValue;
				trophy.m_Rules[trophy.m_RuleCount].m_Compare = (StatSystem.StatCompare)compareFunc;
				trophy.m_RuleCount++;
			}
		}
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000AB1A8 File Offset: 0x000A95A8
	public void SetupDone()
	{
		int num = 0;
		for (int i = 0; i < this.m_Trophies.Count; i++)
		{
			num += this.m_Trophies._items[i].m_Rules.Length;
		}
		this.m_StatsRules = new StatSystem.StatRule[num];
		for (int j = 0; j < this.m_Stats.Count; j++)
		{
			int num2 = 0;
			for (int i = 0; i < this.m_Trophies.Count; i++)
			{
				for (int k = 0; k < this.m_Trophies._items[i].m_Rules.Length; k++)
				{
					if (this.m_Trophies._items[i].m_Rules[k].m_Stat == this.m_Stats._items[j])
					{
						num2++;
					}
				}
			}
			this.m_Stats._items[j].m_RefStats = new StatSystem.StatRule[num2];
		}
		num = 0;
		for (int i = 0; i < this.m_Trophies.Count; i++)
		{
			for (int k = 0; k < this.m_Trophies._items[i].m_Rules.Length; k++)
			{
				this.m_StatsRules[num] = this.m_Trophies._items[i].m_Rules[k];
				this.m_StatsRules[num].m_RefTrophy = this.m_Trophies._items[i];
				num++;
			}
		}
		for (int j = 0; j < this.m_Stats.Count; j++)
		{
			int num2 = 0;
			for (int i = 0; i < this.m_StatsRules.Length; i++)
			{
				if (this.m_StatsRules[i].m_Stat == this.m_Stats._items[j])
				{
					this.m_Stats._items[j].m_RefStats[num2] = this.m_StatsRules[i];
					num2++;
				}
			}
		}
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000AB398 File Offset: 0x000A9798
	public void LoadStats()
	{
		MetaGameProgress metaGameProgress = this.m_saveManager.GetMetaGameProgress();
		for (int i = 0; i < this.m_Stats.Count; i++)
		{
			StatSystem.Stat stat = this.m_Stats._items[i];
			int num;
			if (stat.m_Type == StatSystem.STAT_TYPE.ST_COUNTER)
			{
				float value = 0f;
				if (metaGameProgress.SaveData.Get(string.Concat(new object[]
				{
					"S_",
					stat.m_Name,
					"_V_",
					stat.m_ID
				}), out value, 0f))
				{
					stat.m_Value = value;
				}
			}
			else if (metaGameProgress.SaveData.Get(string.Concat(new object[]
			{
				"S_",
				stat.m_Name,
				"_HS_",
				stat.m_ID
			}), out num, 0))
			{
				stat.m_IDHolder = new Hashtable();
				for (int j = 0; j < num; j++)
				{
					int num2;
					int num3;
					if (metaGameProgress.SaveData.Get(string.Concat(new object[]
					{
						"S_",
						stat.m_Name,
						"_KEY_",
						stat.m_ID,
						"_",
						j.ToString()
					}), out num2, 0) && metaGameProgress.SaveData.Get(string.Concat(new object[]
					{
						"S_",
						stat.m_Name,
						"_V_",
						stat.m_ID,
						"_",
						j.ToString()
					}), out num3, 0))
					{
						stat.m_IDHolder[num2] = num3;
					}
				}
			}
		}
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x000AB57C File Offset: 0x000A997C
	public void ResetStat(int ID, ControlPadInput.PadNum pad)
	{
		if (pad != ControlPadInput.PadNum.One)
		{
			StatSystem.Stat stat = null;
			bool flag = this.FindStat(ID, out stat);
			if (flag)
			{
				this.m_statValueChangedCallback(ID, stat.m_Value, 0f);
				stat.m_Value = 0f;
				MetaGameProgress metaGameProgress = this.m_saveManager.GetMetaGameProgress();
				metaGameProgress.SaveData.Set(string.Concat(new object[]
				{
					"S_",
					stat.m_Name,
					"_V_",
					stat.m_ID
				}), stat.m_Value);
			}
		}
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x000AB614 File Offset: 0x000A9A14
	public void SetStat(int ID, float value, ControlPadInput.PadNum pad)
	{
		if (pad == ControlPadInput.PadNum.One)
		{
			StatSystem.Stat stat = null;
			bool flag = this.FindStat(ID, out stat);
			if (flag)
			{
				this.m_statValueChangedCallback(ID, stat.m_Value, value);
				stat.m_Value = value;
				this.CheckStatRulesLinkedToStat(stat, stat.m_Value);
				MetaGameProgress metaGameProgress = this.m_saveManager.GetMetaGameProgress();
				metaGameProgress.SaveData.Set(string.Concat(new object[]
				{
					"S_",
					stat.m_Name,
					"_V_",
					stat.m_ID
				}), stat.m_Value);
			}
		}
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000AB6B0 File Offset: 0x000A9AB0
	public void IncStat(int ID, float inc, ControlPadInput.PadNum pad)
	{
		if (pad == ControlPadInput.PadNum.One)
		{
			StatSystem.Stat stat = null;
			bool flag = this.FindStat(ID, out stat);
			if (flag)
			{
				this.m_statValueChangedCallback(ID, stat.m_Value, stat.m_Value + inc);
				stat.m_Value += inc;
				this.CheckStatRulesLinkedToStat(stat, stat.m_Value);
				MetaGameProgress metaGameProgress = this.m_saveManager.GetMetaGameProgress();
				metaGameProgress.SaveData.Set(string.Concat(new object[]
				{
					"S_",
					stat.m_Name,
					"_V_",
					stat.m_ID
				}), stat.m_Value);
			}
		}
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x000AB75C File Offset: 0x000A9B5C
	public int AddIDStat(int ID, int itemID, ControlPadInput.PadNum pad)
	{
		if (pad == ControlPadInput.PadNum.One)
		{
			StatSystem.Stat stat = null;
			bool flag = this.FindStat(ID, out stat);
			if (flag && stat.m_Type == StatSystem.STAT_TYPE.ST_ID_HOLDER)
			{
				if (stat.m_validationList != null && Array.IndexOf<int>(stat.m_validationList.m_ids, itemID) == -1)
				{
					return -1;
				}
				bool flag2 = !stat.m_IDHolder.ContainsKey(itemID);
				if (flag2)
				{
					stat.m_IDHolder[itemID] = 1;
				}
				else
				{
					int num = (int)stat.m_IDHolder[itemID];
					num++;
					stat.m_IDHolder[itemID] = num;
				}
				this.CheckStatRulesLinkedToStat(stat, (float)itemID);
				MetaGameProgress metaGameProgress = this.m_saveManager.GetMetaGameProgress();
				metaGameProgress.SaveData.Set(string.Concat(new object[]
				{
					"S_",
					stat.m_Name,
					"_HS_",
					stat.m_ID
				}), stat.m_IDHolder.Keys.Count);
				int num2 = 0;
				IDictionaryEnumerator enumerator = stat.m_IDHolder.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						metaGameProgress.SaveData.Set(string.Concat(new object[]
						{
							"S_",
							stat.m_Name,
							"_KEY_",
							stat.m_ID,
							"_",
							num2.ToString()
						}), (int)dictionaryEntry.Key);
						metaGameProgress.SaveData.Set(string.Concat(new object[]
						{
							"S_",
							stat.m_Name,
							"_V_",
							stat.m_ID,
							"_",
							num2.ToString()
						}), (int)dictionaryEntry.Value);
						num2++;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				return stat.m_IDHolder.Keys.Count;
			}
		}
		return -1;
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x000AB9BC File Offset: 0x000A9DBC
	private bool FindStat(int ID, out StatSystem.Stat stat)
	{
		for (int i = 0; i < this.m_Stats.Count; i++)
		{
			if (this.m_Stats._items[i].m_ID == ID)
			{
				stat = this.m_Stats._items[i];
				return true;
			}
		}
		stat = null;
		return false;
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x000ABA14 File Offset: 0x000A9E14
	private bool FindTrophy(int ID, out StatSystem.Trophy trophy)
	{
		for (int i = 0; i < this.m_Trophies.Count; i++)
		{
			if (this.m_Trophies._items[i].m_TrophyID == ID)
			{
				trophy = this.m_Trophies._items[i];
				return true;
			}
		}
		trophy = null;
		return false;
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x000ABA6C File Offset: 0x000A9E6C
	public string GetStatName(int ID)
	{
		StatSystem.Stat stat = null;
		if (this.FindStat(ID, out stat))
		{
			return stat.m_Name;
		}
		return null;
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000ABA94 File Offset: 0x000A9E94
	public string GetTrophyName(int ID)
	{
		StatSystem.Trophy trophy = null;
		if (this.FindTrophy(ID, out trophy))
		{
			return trophy.m_Name;
		}
		return null;
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x000ABABC File Offset: 0x000A9EBC
	public string GetTrophyApiName(int ID)
	{
		StatSystem.Trophy trophy = null;
		if (this.FindTrophy(ID, out trophy))
		{
			return trophy.m_APIName;
		}
		return null;
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x000ABAE4 File Offset: 0x000A9EE4
	public float GetTrophyProgress(int ID)
	{
		StatSystem.Trophy trophy = null;
		if (this.FindTrophy(ID, out trophy))
		{
			return trophy.GetProgress();
		}
		return 0f;
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x000ABB10 File Offset: 0x000A9F10
	private void CheckStatRulesLinkedToStat(StatSystem.Stat stat, float newValue)
	{
		bool flag = false;
		bool flag2 = false;
		uint num = 0U;
		while ((ulong)num < (ulong)((long)stat.m_RefStats.Length))
		{
			StatSystem.Trophy refTrophy = stat.m_RefStats[(int)((UIntPtr)num)].m_RefTrophy;
			if (refTrophy != null && !refTrophy.m_Unlocked)
			{
				this.m_trophyProgressCallback(refTrophy.m_TrophyID, refTrophy.GetProgress());
			}
			flag2 = true;
			flag |= stat.m_RefStats[(int)((UIntPtr)num)].Check(ref flag2, newValue);
			if (flag && flag2 && refTrophy != null && !refTrophy.m_Unlocked && stat.m_RefStats[(int)((UIntPtr)num)].m_RefTrophy.Check())
			{
				this.m_trophyUnlockCallback(refTrophy.m_TrophyID);
				refTrophy.m_Unlocked = true;
			}
			num += 1U;
		}
	}

	// Token: 0x04001B4A RID: 6986
	private SaveManager m_saveManager;

	// Token: 0x04001B4B RID: 6987
	private VoidGeneric<int, float, float> m_statValueChangedCallback = delegate(int statId, float oldValue, float newValue)
	{
	};

	// Token: 0x04001B4C RID: 6988
	private VoidGeneric<int> m_trophyUnlockCallback = delegate(int trophyId)
	{
	};

	// Token: 0x04001B4D RID: 6989
	private VoidGeneric<int, float> m_trophyProgressCallback = delegate(int trophyId, float progress)
	{
	};

	// Token: 0x04001B4E RID: 6990
	private FastList<StatSystem.Stat> m_Stats;

	// Token: 0x04001B4F RID: 6991
	private StatSystem.StatRule[] m_StatsRules;

	// Token: 0x04001B50 RID: 6992
	private FastList<StatSystem.Trophy> m_Trophies;

	// Token: 0x02000747 RID: 1863
	private enum STAT_TYPE
	{
		// Token: 0x04001B55 RID: 6997
		ST_COUNTER,
		// Token: 0x04001B56 RID: 6998
		ST_ID_HOLDER
	}

	// Token: 0x02000748 RID: 1864
	private class Stat
	{
		// Token: 0x060023CE RID: 9166 RVA: 0x000ABBDC File Offset: 0x000A9FDC
		public Stat()
		{
			this.m_Name = "STAT:NONAME";
			this.m_ID = 0;
			this.m_Value = 0f;
			this.m_RefStats = null;
			this.m_Type = StatSystem.STAT_TYPE.ST_COUNTER;
			this.m_IDHolder = null;
		}

		// Token: 0x04001B57 RID: 6999
		public string m_Name;

		// Token: 0x04001B58 RID: 7000
		public int m_ID;

		// Token: 0x04001B59 RID: 7001
		public float m_Value;

		// Token: 0x04001B5A RID: 7002
		public StatSystem.StatRule[] m_RefStats;

		// Token: 0x04001B5B RID: 7003
		public StatSystem.STAT_TYPE m_Type;

		// Token: 0x04001B5C RID: 7004
		public Hashtable m_IDHolder;

		// Token: 0x04001B5D RID: 7005
		public StatValidationList m_validationList;
	}

	// Token: 0x02000749 RID: 1865
	public enum StatCompare
	{
		// Token: 0x04001B5F RID: 7007
		EQUAL,
		// Token: 0x04001B60 RID: 7008
		GREATER_THAN_EQUAL,
		// Token: 0x04001B61 RID: 7009
		HAS_ID
	}

	// Token: 0x0200074A RID: 1866
	private class StatRule
	{
		// Token: 0x060023CF RID: 9167 RVA: 0x000ABC16 File Offset: 0x000AA016
		public StatRule()
		{
			this.m_Stat = null;
			this.m_RefValue = 0f;
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x000ABC30 File Offset: 0x000AA030
		public bool Check(ref bool bRuleJustMadeTrueByNewValue, float newValue)
		{
			bool result = false;
			if (this.m_Stat.m_Type == StatSystem.STAT_TYPE.ST_COUNTER)
			{
				StatSystem.StatCompare compare = this.m_Compare;
				if (compare != StatSystem.StatCompare.EQUAL)
				{
					if (compare == StatSystem.StatCompare.GREATER_THAN_EQUAL)
					{
						if (this.m_Stat.m_Value >= this.m_RefValue)
						{
							result = true;
						}
					}
				}
				else if (this.m_Stat.m_Value == this.m_RefValue)
				{
					result = true;
				}
			}
			else
			{
				StatSystem.StatCompare compare2 = this.m_Compare;
				if (compare2 != StatSystem.StatCompare.EQUAL)
				{
					if (compare2 != StatSystem.StatCompare.GREATER_THAN_EQUAL)
					{
						if (compare2 == StatSystem.StatCompare.HAS_ID)
						{
							if (this.m_Stat.m_IDHolder.ContainsKey((int)this.m_RefValue))
							{
								result = true;
								if ((int)this.m_RefValue == (int)newValue)
								{
									bRuleJustMadeTrueByNewValue = true;
								}
								else
								{
									bRuleJustMadeTrueByNewValue = false;
								}
							}
						}
					}
					else if ((float)this.m_Stat.m_IDHolder.Keys.Count >= this.m_RefValue)
					{
						result = true;
					}
				}
				else if ((float)this.m_Stat.m_IDHolder.Keys.Count == this.m_RefValue)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x000ABD5C File Offset: 0x000AA15C
		public float GetProgress()
		{
			if (this.m_Stat.m_Type == StatSystem.STAT_TYPE.ST_COUNTER)
			{
				StatSystem.StatCompare compare = this.m_Compare;
				if (compare == StatSystem.StatCompare.EQUAL || compare == StatSystem.StatCompare.GREATER_THAN_EQUAL)
				{
					return Mathf.Clamp01(this.m_Stat.m_Value / this.m_RefValue);
				}
			}
			else if (this.m_Stat.m_Type == StatSystem.STAT_TYPE.ST_ID_HOLDER)
			{
				StatSystem.StatCompare compare2 = this.m_Compare;
				if (compare2 == StatSystem.StatCompare.EQUAL || compare2 == StatSystem.StatCompare.GREATER_THAN_EQUAL)
				{
					return Mathf.Clamp01((float)this.m_Stat.m_IDHolder.Count / this.m_RefValue);
				}
				if (compare2 == StatSystem.StatCompare.HAS_ID)
				{
					return (!this.m_Stat.m_IDHolder.ContainsKey((int)this.m_RefValue)) ? 0f : 1f;
				}
			}
			return 0f;
		}

		// Token: 0x04001B62 RID: 7010
		public StatSystem.Stat m_Stat;

		// Token: 0x04001B63 RID: 7011
		public float m_RefValue;

		// Token: 0x04001B64 RID: 7012
		public StatSystem.StatCompare m_Compare;

		// Token: 0x04001B65 RID: 7013
		public StatSystem.Trophy m_RefTrophy;
	}

	// Token: 0x0200074B RID: 1867
	private enum Combiner
	{
		// Token: 0x04001B67 RID: 7015
		C_NA,
		// Token: 0x04001B68 RID: 7016
		C_AND,
		// Token: 0x04001B69 RID: 7017
		C_OR
	}

	// Token: 0x0200074C RID: 1868
	private class Trophy
	{
		// Token: 0x060023D2 RID: 9170 RVA: 0x000ABE34 File Offset: 0x000AA234
		public Trophy()
		{
			this.m_Name = "XX";
			this.m_APIName = "XX";
			this.m_TrophyID = -1;
			this.m_TrophyProgress = 0f;
			this.m_Unlocked = false;
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000ABE6C File Offset: 0x000AA26C
		public bool Check()
		{
			bool flag = false;
			StatSystem.Combiner combiner = this.m_Combiner;
			if (combiner != StatSystem.Combiner.C_NA)
			{
				if (combiner != StatSystem.Combiner.C_AND)
				{
					if (combiner == StatSystem.Combiner.C_OR)
					{
						flag = false;
					}
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				flag = false;
			}
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this.m_Rules.Length))
			{
				bool flag2 = false;
				bool flag3 = this.m_Rules[(int)((UIntPtr)num)].Check(ref flag2, 0f);
				StatSystem.Combiner combiner2 = this.m_Combiner;
				if (combiner2 != StatSystem.Combiner.C_NA)
				{
					if (combiner2 != StatSystem.Combiner.C_AND)
					{
						if (combiner2 == StatSystem.Combiner.C_OR)
						{
							flag = (flag || flag3);
						}
					}
					else
					{
						flag = (flag && flag3);
					}
				}
				else
				{
					flag = flag3;
				}
				num += 1U;
			}
			return flag;
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000ABF24 File Offset: 0x000AA324
		public float GetProgress()
		{
			int num = this.m_Rules.Length;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				num2 += this.m_Rules[i].GetProgress();
			}
			return Mathf.Clamp01(num2 / (float)num);
		}

		// Token: 0x04001B6A RID: 7018
		public string m_Name;

		// Token: 0x04001B6B RID: 7019
		public string m_APIName;

		// Token: 0x04001B6C RID: 7020
		public int m_TrophyID;

		// Token: 0x04001B6D RID: 7021
		public float m_TrophyProgress;

		// Token: 0x04001B6E RID: 7022
		public bool m_Unlocked;

		// Token: 0x04001B6F RID: 7023
		public StatSystem.StatRule[] m_Rules;

		// Token: 0x04001B70 RID: 7024
		public int m_RuleCount;

		// Token: 0x04001B71 RID: 7025
		public StatSystem.Combiner m_Combiner;
	}
}
