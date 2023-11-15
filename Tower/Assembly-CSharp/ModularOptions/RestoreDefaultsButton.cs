using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x02000081 RID: 129
	[AddComponentMenu("Modular Options/Button/Restore Defaults")]
	[RequireComponent(typeof(Button))]
	public class RestoreDefaultsButton : MonoBehaviour
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x00008468 File Offset: 0x00006668
		private void Awake()
		{
			for (int i = this.dropdowns.Count - 1; i >= 0; i--)
			{
				if (this.dropdowns[i] is OptionPreset)
				{
					this.presets.Add((OptionPreset)this.dropdowns[i]);
					this.dropdowns.RemoveAt(i);
				}
			}
			base.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.RestoreDefaults();
			});
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000084E4 File Offset: 0x000066E4
		public void RestoreDefaults()
		{
			for (int i = 0; i < this.sliders.Length; i++)
			{
				this.sliders[i].Value = this.sliders[i].defaultSetting.value;
			}
			for (int j = 0; j < this.dropdowns.Count; j++)
			{
				this.dropdowns[j].Value = this.dropdowns[j].defaultSetting.value;
			}
			for (int k = 0; k < this.toggles.Length; k++)
			{
				this.toggles[k].Value = this.toggles[k].defaultSetting.value;
			}
			for (int l = 0; l < this.presets.Count; l++)
			{
				this.presets[l].Value = this.presets[l].defaultSetting.value;
			}
		}

		// Token: 0x040001B5 RID: 437
		public SliderOption[] sliders;

		// Token: 0x040001B6 RID: 438
		public List<DropdownOption> dropdowns;

		// Token: 0x040001B7 RID: 439
		public ToggleOption[] toggles;

		// Token: 0x040001B8 RID: 440
		private List<OptionPreset> presets = new List<OptionPreset>();
	}
}
