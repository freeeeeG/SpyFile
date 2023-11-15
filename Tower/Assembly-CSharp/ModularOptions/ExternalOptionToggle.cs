using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x0200009F RID: 159
	[AddComponentMenu("Modular Options/External/Toggle")]
	public class ExternalOptionToggle : ToggleOption
	{
		// Token: 0x0600022B RID: 555 RVA: 0x00008FA7 File Offset: 0x000071A7
		protected override void ApplySetting(bool _value)
		{
			this.onValueChange.Invoke(_value);
		}

		// Token: 0x040001DF RID: 479
		public Toggle.ToggleEvent onValueChange;
	}
}
