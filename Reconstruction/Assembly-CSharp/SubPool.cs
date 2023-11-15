using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class SubPool
{
	// Token: 0x1700031C RID: 796
	// (get) Token: 0x060007FB RID: 2043 RVA: 0x00014C0C File Offset: 0x00012E0C
	public string Name
	{
		get
		{
			return this.m_Name;
		}
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00014C14 File Offset: 0x00012E14
	public SubPool(string name, ReusableObject prefab)
	{
		this.m_prefab = prefab;
		this.m_Name = name;
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00014C38 File Offset: 0x00012E38
	public ReusableObject Spawn(Transform container)
	{
		ReusableObject reusableObject = null;
		foreach (ReusableObject reusableObject2 in this.m_objects)
		{
			if (!reusableObject2.gameObject.activeSelf)
			{
				reusableObject = reusableObject2;
				break;
			}
		}
		if (reusableObject == null)
		{
			reusableObject = Object.Instantiate<ReusableObject>(this.m_prefab);
			this.m_objects.Add(reusableObject);
			reusableObject.transform.SetParent(container.transform);
			reusableObject.ParentObj = container.transform;
		}
		reusableObject.gameObject.SetActive(true);
		reusableObject.OnSpawn();
		return reusableObject;
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00014CE8 File Offset: 0x00012EE8
	public void UnSpawn(ReusableObject go)
	{
		if (this.Contains(go))
		{
			go.OnUnSpawn();
			go.gameObject.SetActive(false);
		}
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00014D08 File Offset: 0x00012F08
	public void UnSpawnAll()
	{
		foreach (ReusableObject reusableObject in this.m_objects)
		{
			if (reusableObject.gameObject.activeSelf)
			{
				this.UnSpawn(reusableObject);
			}
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00014D68 File Offset: 0x00012F68
	public bool Contains(ReusableObject go)
	{
		return this.m_objects.Contains(go);
	}

	// Token: 0x040003CB RID: 971
	private ReusableObject m_prefab;

	// Token: 0x040003CC RID: 972
	private string m_Name;

	// Token: 0x040003CD RID: 973
	private List<ReusableObject> m_objects = new List<ReusableObject>();
}
