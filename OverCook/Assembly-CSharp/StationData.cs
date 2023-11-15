using System;
using UnityEngine;

// Token: 0x0200056E RID: 1390
[Serializable]
public class StationData : ScriptableObject
{
	// Token: 0x040014C6 RID: 5318
	public GameObject m_prefab;

	// Token: 0x040014C7 RID: 5319
	public string m_name;

	// Token: 0x040014C8 RID: 5320
	public int m_cost = 25;
}
