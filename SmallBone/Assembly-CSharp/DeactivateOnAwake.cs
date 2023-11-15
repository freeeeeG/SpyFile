using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class DeactivateOnAwake : MonoBehaviour
{
	// Token: 0x06000176 RID: 374 RVA: 0x000075E7 File Offset: 0x000057E7
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}
}
