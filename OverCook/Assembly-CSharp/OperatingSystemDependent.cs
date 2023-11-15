using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
internal class OperatingSystemDependent : MonoBehaviour
{
	// Token: 0x060005C5 RID: 1477 RVA: 0x0002AD31 File Offset: 0x00029131
	private void Awake()
	{
		if (!PlatformUtils.HasOperatingSystemFlag(this.m_operatingSystemActiveOn))
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x040004C3 RID: 1219
	[Mask(typeof(PlatformUtils.OperatingSystem))]
	public int m_operatingSystemActiveOn;
}
