using System;
using UnityEngine;

// Token: 0x02000140 RID: 320
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x1700031F RID: 799
	// (get) Token: 0x0600083F RID: 2111 RVA: 0x00015866 File Offset: 0x00013A66
	// (set) Token: 0x06000840 RID: 2112 RVA: 0x0001586D File Offset: 0x00013A6D
	public static T Instance
	{
		get
		{
			return Singleton<T>.m_instance;
		}
		set
		{
			Singleton<T>.m_instance = value;
		}
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00015878 File Offset: 0x00013A78
	protected virtual void Awake()
	{
		if (Singleton<T>.Instance != null)
		{
			Object.Destroy(base.gameObject);
			Debug.Log(base.name + "已经创建了相同singleton实例");
			this.alreadyExist = true;
			return;
		}
		Singleton<T>.m_instance = (this as T);
		this.alreadyExist = false;
	}

	// Token: 0x04000412 RID: 1042
	private static T m_instance;

	// Token: 0x04000413 RID: 1043
	protected bool alreadyExist;
}
