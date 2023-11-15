using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class ObjectPoolNew : Singleton<ObjectPoolNew>
{
	// Token: 0x06000801 RID: 2049 RVA: 0x00014D76 File Offset: 0x00012F76
	public void ClearPools()
	{
		this.m_pools.Clear();
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00014D84 File Offset: 0x00012F84
	public ReusableObject Spawn(string name, Vector2 pos)
	{
		if (!this.m_pools.ContainsKey(name))
		{
			this.RegisterNew(name);
		}
		SubPoolNew subPoolNew = this.m_pools[name];
		return subPoolNew.Spawn(this.m_pools_parent[subPoolNew], pos);
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x00014DC8 File Offset: 0x00012FC8
	public ReusableObject Spawn(ReusableObject gameObj, Vector2 pos)
	{
		if (!this.m_pools.ContainsKey(gameObj.name))
		{
			this.RegisterNew(gameObj);
		}
		SubPoolNew subPoolNew = this.m_pools[gameObj.name];
		return subPoolNew.Spawn(this.m_pools_parent[subPoolNew], pos);
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00014E14 File Offset: 0x00013014
	public void UnSpawn(ReusableObject go)
	{
		SubPoolNew subPoolNew = null;
		foreach (SubPoolNew subPoolNew2 in this.m_pools.Values)
		{
			if (subPoolNew2.Contains(go))
			{
				subPoolNew = subPoolNew2;
				break;
			}
		}
		subPoolNew.UnSpawn(go);
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x00014E7C File Offset: 0x0001307C
	public void UnSpawnAll()
	{
		foreach (SubPoolNew subPoolNew in this.m_pools.Values)
		{
			subPoolNew.UnSpawnAll();
		}
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00014ED4 File Offset: 0x000130D4
	private void RegisterNew(string name)
	{
		string path;
		if (string.IsNullOrEmpty(this.ResourceDir))
		{
			path = name;
		}
		else
		{
			path = this.ResourceDir + "/" + name;
		}
		ReusableObject prefab = Resources.Load<ReusableObject>(path);
		SubPoolNew subPoolNew = new SubPoolNew(name, prefab);
		this.m_pools.Add(subPoolNew.Name, subPoolNew);
		GameObject gameObject = new GameObject("Pool-" + name);
		this.m_pools_parent.Add(subPoolNew, gameObject.transform);
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00014F50 File Offset: 0x00013150
	private void RegisterNew(ReusableObject obj)
	{
		SubPoolNew subPoolNew = new SubPoolNew(obj.name, obj);
		this.m_pools.Add(subPoolNew.Name, subPoolNew);
		GameObject gameObject = new GameObject("Pool-" + obj.name);
		this.m_pools_parent.Add(subPoolNew, gameObject.transform);
	}

	// Token: 0x040003CE RID: 974
	public string ResourceDir = "";

	// Token: 0x040003CF RID: 975
	private Dictionary<string, SubPoolNew> m_pools = new Dictionary<string, SubPoolNew>();

	// Token: 0x040003D0 RID: 976
	private Dictionary<SubPoolNew, Transform> m_pools_parent = new Dictionary<SubPoolNew, Transform>();
}
