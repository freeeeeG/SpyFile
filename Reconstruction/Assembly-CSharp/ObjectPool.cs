using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012F RID: 303
public class ObjectPool : Singleton<ObjectPool>
{
	// Token: 0x060007ED RID: 2029 RVA: 0x0001495A File Offset: 0x00012B5A
	public void ClearPools()
	{
		this.m_pools.Clear();
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00014968 File Offset: 0x00012B68
	public ReusableObject Spawn(string name)
	{
		if (!this.m_pools.ContainsKey(name))
		{
			this.RegisterNew(name);
		}
		SubPool subPool = this.m_pools[name];
		return subPool.Spawn(this.m_pools_parent[subPool]);
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x000149AC File Offset: 0x00012BAC
	public ReusableObject Spawn(ReusableObject gameObj)
	{
		if (!this.m_pools.ContainsKey(gameObj.name))
		{
			this.RegisterNew(gameObj);
		}
		SubPool subPool = this.m_pools[gameObj.name];
		return subPool.Spawn(this.m_pools_parent[subPool]);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x000149F8 File Offset: 0x00012BF8
	public void UnSpawn(ReusableObject go)
	{
		SubPool subPool = null;
		foreach (SubPool subPool2 in this.m_pools.Values)
		{
			if (subPool2.Contains(go))
			{
				subPool = subPool2;
				break;
			}
		}
		subPool.UnSpawn(go);
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00014A60 File Offset: 0x00012C60
	public void UnSpawnAll()
	{
		foreach (SubPool subPool in this.m_pools.Values)
		{
			subPool.UnSpawnAll();
		}
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x00014AB8 File Offset: 0x00012CB8
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
		SubPool subPool = new SubPool(name, prefab);
		this.m_pools.Add(subPool.Name, subPool);
		GameObject gameObject = new GameObject("Pool-" + name);
		this.m_pools_parent.Add(subPool, gameObject.transform);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x00014B34 File Offset: 0x00012D34
	private void RegisterNew(ReusableObject obj)
	{
		SubPool subPool = new SubPool(obj.name, obj);
		this.m_pools.Add(subPool.Name, subPool);
		GameObject gameObject = new GameObject("Pool-" + obj.name);
		this.m_pools_parent.Add(subPool, gameObject.transform);
	}

	// Token: 0x040003C6 RID: 966
	public string ResourceDir = "";

	// Token: 0x040003C7 RID: 967
	private Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();

	// Token: 0x040003C8 RID: 968
	private Dictionary<SubPool, Transform> m_pools_parent = new Dictionary<SubPool, Transform>();
}
