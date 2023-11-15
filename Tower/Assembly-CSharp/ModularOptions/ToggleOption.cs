using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x020000AD RID: 173
	[RequireComponent(typeof(Toggle))]
	public abstract class ToggleOption : OptionBase<bool, BoolToggle>
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600025C RID: 604 RVA: 0x000093D4 File Offset: 0x000075D4
		// (set) Token: 0x0600025D RID: 605 RVA: 0x000093E1 File Offset: 0x000075E1
		public override bool Value
		{
			get
			{
				return this.toggle.isOn;
			}
			set
			{
				if (this.toggle.isOn == value)
				{
					this.OnValueChange(value);
					return;
				}
				this.toggle.isOn = value;
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009408 File Offset: 0x00007608
		protected virtual void Awake()
		{
			this.toggle = base.GetComponent<Toggle>();
			this.toggle.onValueChanged.AddListener(delegate(bool _)
			{
				this.OnValueChange(_);
			});
			this.Value = OptionSaveSystem.LoadBool(this.optionName, this.defaultSetting.value);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00009459 File Offset: 0x00007659
		protected void OnValueChange(bool _value)
		{
			OptionSaveSystem.SaveBool(this.optionName, _value);
			this.ApplySetting(_value);
			if (this.allowPresetCallback && this.preset != null)
			{
				this.preset.SetCustom();
			}
		}

		// Token: 0x040001F3 RID: 499
		protected Toggle toggle;
	}
}
