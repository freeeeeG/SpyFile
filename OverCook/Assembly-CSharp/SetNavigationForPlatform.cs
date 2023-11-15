using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B0C RID: 2828
internal class SetNavigationForPlatform : MonoBehaviour
{
	// Token: 0x0600393B RID: 14651 RVA: 0x0010FBB2 File Offset: 0x0010DFB2
	private void Awake()
	{
		if (this.m_SelectableToFix != null && PlatformUtils.HasPlatformFlag(this.m_platformsActiveOn))
		{
			this.m_SelectableToFix.navigation = this.m_NavigationToSet;
		}
	}

	// Token: 0x04002E07 RID: 11783
	[SerializeField]
	private Selectable m_SelectableToFix;

	// Token: 0x04002E08 RID: 11784
	[SerializeField]
	private Navigation m_NavigationToSet;

	// Token: 0x04002E09 RID: 11785
	[Mask(typeof(PlatformUtils.Platforms))]
	public int m_platformsActiveOn;
}
