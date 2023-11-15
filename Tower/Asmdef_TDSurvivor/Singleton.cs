using System;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00016DB4 File Offset: 0x00014FB4
	public static T GetStatus
	{
		get
		{
			return Singleton<T>.instance;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00016DBC File Offset: 0x00014FBC
	public static T Instance
	{
		get
		{
			if (Singleton<T>.instance != null)
			{
				return Singleton<T>.instance;
			}
			Singleton<T>.instance = Object.FindObjectOfType<T>();
			if (Singleton<T>.instance != null)
			{
				return Singleton<T>.instance;
			}
			Singleton<T>.instance = new GameObject
			{
				name = typeof(T).Name
			}.AddComponent<T>();
			return Singleton<T>.instance;
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x00016E2C File Offset: 0x0001502C
	public static bool HasInstance()
	{
		return Singleton<T>.instance != null;
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00016E3E File Offset: 0x0001503E
	protected virtual void Awake()
	{
		if (Singleton<T>.instance == null)
		{
			Singleton<T>.instance = Object.FindObjectOfType<T>();
			return;
		}
		if (Singleton<T>.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000541 RID: 1345
	private static T instance;
}
