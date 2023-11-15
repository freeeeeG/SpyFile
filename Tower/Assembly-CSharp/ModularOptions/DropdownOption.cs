using System;
using TMPro;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x020000A5 RID: 165
	[RequireComponent(typeof(TMP_Dropdown))]
	public abstract class DropdownOption : OptionBase<int, IntDropdown>
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000919D File Offset: 0x0000739D
		// (set) Token: 0x06000241 RID: 577 RVA: 0x000091AA File Offset: 0x000073AA
		public override int Value
		{
			get
			{
				return this.dropdown.value;
			}
			set
			{
				if (this.dropdown.value == value)
				{
					this.OnValueChange(value);
					return;
				}
				this.dropdown.value = value;
				this.dropdown.RefreshShownValue();
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000091DC File Offset: 0x000073DC
		protected virtual void Awake()
		{
			this.dropdown = base.GetComponent<TMP_Dropdown>();
			this.dropdown.onValueChanged.AddListener(delegate(int _)
			{
				this.OnValueChange(_);
			});
			this.Value = OptionSaveSystem.LoadInt(this.optionName, this.defaultSetting.value);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000922D File Offset: 0x0000742D
		protected void OnValueChange(int _value)
		{
			OptionSaveSystem.SaveInt(this.optionName, _value);
			this.ApplySetting(_value);
			if (this.allowPresetCallback && this.preset != null)
			{
				this.preset.SetCustom();
			}
		}

		// Token: 0x040001EC RID: 492
		protected TMP_Dropdown dropdown;
	}
}
