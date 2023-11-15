using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x020000AF RID: 175
	[AddComponentMenu("Modular Options/Preset")]
	[DefaultExecutionOrder(3)]
	public sealed class OptionPreset : DropdownOption
	{
		// Token: 0x06000263 RID: 611 RVA: 0x000094A8 File Offset: 0x000076A8
		private void Start()
		{
			for (int i = 0; i < this.sliderPresetData.Length; i++)
			{
				this.sliderPresetData[i].slider.preset = this;
			}
			for (int j = 0; j < this.dropdownPresetData.Length; j++)
			{
				this.dropdownPresetData[j].dropdown.preset = this;
			}
			for (int k = 0; k < this.togglePresetData.Length; k++)
			{
				this.togglePresetData[k].toggle.preset = this;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009527 File Offset: 0x00007727
		public void SetCustom()
		{
			this.Value = this.dropdown.options.Count - 1;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009544 File Offset: 0x00007744
		protected override void ApplySetting(int _value)
		{
			if (_value == this.dropdown.options.Count - 1)
			{
				return;
			}
			for (int i = 0; i < this.sliderPresetData.Length; i++)
			{
				this.sliderPresetData[i].slider.ApplyPreset(this.sliderPresetData[i].presetData[_value]);
			}
			for (int j = 0; j < this.dropdownPresetData.Length; j++)
			{
				this.dropdownPresetData[j].dropdown.ApplyPreset(this.dropdownPresetData[j].presetData[_value]);
			}
			for (int k = 0; k < this.togglePresetData.Length; k++)
			{
				this.togglePresetData[k].toggle.ApplyPreset(this.togglePresetData[k].presetData[_value]);
			}
		}

		// Token: 0x040001F4 RID: 500
		public SliderData[] sliderPresetData;

		// Token: 0x040001F5 RID: 501
		public DropdownData[] dropdownPresetData;

		// Token: 0x040001F6 RID: 502
		public ToggleData[] togglePresetData;
	}
}
