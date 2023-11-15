using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class Util_SetDontDestroyOnLoad : MonoBehaviour
{
	// Token: 0x06000B7D RID: 2941 RVA: 0x0002D0AD File Offset: 0x0002B2AD
	protected void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
