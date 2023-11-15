using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000099 RID: 153
	[AddComponentMenu("Modular Options/Display/V-Sync Toggle")]
	public sealed class VSyncToggle : ToggleOption
	{
		// Token: 0x0600021E RID: 542 RVA: 0x00008E8F File Offset: 0x0000708F
		protected override void ApplySetting(bool _value)
		{
			QualitySettings.vSyncCount = (_value ? 1 : 0);
		}
	}
}
