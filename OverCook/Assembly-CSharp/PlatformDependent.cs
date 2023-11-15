using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class PlatformDependent : MonoBehaviour
{
	// Token: 0x060005C7 RID: 1479 RVA: 0x0002AD57 File Offset: 0x00029157
	private void Awake()
	{
		if (!PlatformUtils.HasPlatformFlag(this.m_platformsActiveOn))
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x040004C4 RID: 1220
	[Mask(typeof(PlatformUtils.Platforms))]
	public int m_platformsActiveOn;
}
