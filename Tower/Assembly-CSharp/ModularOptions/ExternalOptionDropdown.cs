using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x0200009D RID: 157
	[AddComponentMenu("Modular Options/External/Dropdown")]
	public class ExternalOptionDropdown : DropdownOption
	{
		// Token: 0x06000227 RID: 551 RVA: 0x00008F7B File Offset: 0x0000717B
		protected override void ApplySetting(int _value)
		{
			this.onValueChange.Invoke(_value);
		}

		// Token: 0x040001DD RID: 477
		public Dropdown.DropdownEvent onValueChange;
	}
}
