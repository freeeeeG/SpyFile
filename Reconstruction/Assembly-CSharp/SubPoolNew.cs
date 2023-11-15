using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class SubPoolNew
{
	// Token: 0x1700031D RID: 797
	// (get) Token: 0x06000809 RID: 2057 RVA: 0x00014FCF File Offset: 0x000131CF
	public string Name
	{
		get
		{
			return this.m_Name;
		}
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00014FD7 File Offset: 0x000131D7
	public SubPoolNew(string name, ReusableObject prefab)
	{
		this.m_prefab = prefab;
		this.m_Name = name;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00015014 File Offset: 0x00013214
	public ReusableObject Spawn(Transform container, Vector2 pos)
	{
		ReusableObject reusableObject = null;
		foreach (ReusableObject reusableObject2 in this.m_objects)
		{
			if (!reusableObject2.isActive)
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
		reusableObject.isActive = true;
		reusableObject.transform.position = pos;
		reusableObject.OnSpawn();
		return reusableObject;
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x000150CC File Offset: 0x000132CC
	public void UnSpawn(ReusableObject go)
	{
		if (this.Contains(go))
		{
			go.isActive = false;
			go.transform.position = this.farPos;
			go.OnUnSpawn();
		}
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x000150F8 File Offset: 0x000132F8
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

	// Token: 0x0600080E RID: 2062 RVA: 0x00015158 File Offset: 0x00013358
	public bool Contains(ReusableObject go)
	{
		return this.m_objects.Contains(go);
	}

	// Token: 0x040003D1 RID: 977
	private Vector3 farPos = new Vector3(0f, 0f, 1000f);

	// Token: 0x040003D2 RID: 978
	private ReusableObject m_prefab;

	// Token: 0x040003D3 RID: 979
	private string m_Name;

	// Token: 0x040003D4 RID: 980
	private List<ReusableObject> m_objects = new List<ReusableObject>();
}
