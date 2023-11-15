using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class GlobalTransform : MonoBehaviour
{
	// Token: 0x0600012D RID: 301 RVA: 0x00006B91 File Offset: 0x00004D91
	private void Awake()
	{
		base.transform.SetParent(null);
	}
}
