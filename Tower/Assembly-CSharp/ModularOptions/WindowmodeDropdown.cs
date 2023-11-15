using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x0200009A RID: 154
	[AddComponentMenu("Modular Options/Display/Windowmode Dropdown")]
	public sealed class WindowmodeDropdown : DropdownOption
	{
		// Token: 0x06000220 RID: 544 RVA: 0x00008EA5 File Offset: 0x000070A5
		protected override void ApplySetting(int _value)
		{
			_value = Mathf.Min(_value, this.options.Length - 1);
			Screen.fullScreenMode = this.options[_value];
		}

		// Token: 0x040001D9 RID: 473
		[Tooltip("Setting for the corresponding dropdown index.")]
		public FullScreenMode[] options = new FullScreenMode[]
		{
			FullScreenMode.ExclusiveFullScreen,
			FullScreenMode.FullScreenWindow,
			FullScreenMode.Windowed
		};
	}
}
