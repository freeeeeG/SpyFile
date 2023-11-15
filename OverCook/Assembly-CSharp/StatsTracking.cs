using System;
using UnityEngine;

// Token: 0x0200073F RID: 1855
[CreateAssetMenu(fileName = "StatsTracking", menuName = "Team17/Create StatsTracking")]
[Serializable]
public class StatsTracking : ScriptableObject
{
	// Token: 0x060023AC RID: 9132 RVA: 0x000AAD97 File Offset: 0x000A9197
	private StatsTracking()
	{
		StatsTracking.m_Instance = this;
	}

	// Token: 0x04001B31 RID: 6961
	private static StatsTracking m_Instance;

	// Token: 0x04001B32 RID: 6962
	public StatsTracking.Stat[] m_Stats;

	// Token: 0x04001B33 RID: 6963
	public StatsTracking.Trophy[] m_Tropies;

	// Token: 0x02000740 RID: 1856
	[Serializable]
	public enum STAT_TYPE
	{
		// Token: 0x04001B35 RID: 6965
		Counter,
		// Token: 0x04001B36 RID: 6966
		ID_Holder
	}

	// Token: 0x02000741 RID: 1857
	[Serializable]
	public class Stat
	{
		// Token: 0x04001B37 RID: 6967
		public STAT_IDS m_ID;

		// Token: 0x04001B38 RID: 6968
		public StatsTracking.STAT_TYPE m_StatType;

		// Token: 0x04001B39 RID: 6969
		public StatValidationList m_validationList;
	}

	// Token: 0x02000742 RID: 1858
	public enum StatCompare
	{
		// Token: 0x04001B3B RID: 6971
		EQUAL,
		// Token: 0x04001B3C RID: 6972
		GREATER_THAN_EQUAL,
		// Token: 0x04001B3D RID: 6973
		HAS_ID
	}

	// Token: 0x02000743 RID: 1859
	[Serializable]
	public class StatRule
	{
		// Token: 0x04001B3E RID: 6974
		public STAT_IDS m_StatID;

		// Token: 0x04001B3F RID: 6975
		public float m_RefValue;

		// Token: 0x04001B40 RID: 6976
		public StatsTracking.StatCompare m_Compare;
	}

	// Token: 0x02000744 RID: 1860
	[Serializable]
	public enum Combiner
	{
		// Token: 0x04001B42 RID: 6978
		NA,
		// Token: 0x04001B43 RID: 6979
		AND,
		// Token: 0x04001B44 RID: 6980
		OR
	}

	// Token: 0x02000745 RID: 1861
	[Serializable]
	public class Trophy
	{
		// Token: 0x04001B45 RID: 6981
		public string m_Name;

		// Token: 0x04001B46 RID: 6982
		public string m_APIName;

		// Token: 0x04001B47 RID: 6983
		public int m_TrophyID;

		// Token: 0x04001B48 RID: 6984
		public StatsTracking.StatRule[] m_Rules;

		// Token: 0x04001B49 RID: 6985
		public StatsTracking.Combiner m_CombineMode;
	}
}
