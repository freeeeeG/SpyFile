using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x020000AC RID: 172
	[RequireComponent(typeof(Slider))]
	public abstract class SliderOption : OptionBase<float, FloatSlider>
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00009309 File Offset: 0x00007509
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00009316 File Offset: 0x00007516
		public override float Value
		{
			get
			{
				return this.slider.value;
			}
			set
			{
				if (this.slider.value == value)
				{
					this.OnValueChange(value);
					return;
				}
				this.slider.value = value;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000933C File Offset: 0x0000753C
		protected virtual void Awake()
		{
			this.slider = base.GetComponent<Slider>();
			this.slider.onValueChanged.AddListener(delegate(float _)
			{
				this.OnValueChange(_);
			});
			this.Value = OptionSaveSystem.LoadFloat(this.optionName, this.defaultSetting.value);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000938D File Offset: 0x0000758D
		protected void OnValueChange(float _value)
		{
			OptionSaveSystem.SaveFloat(this.optionName, _value);
			this.ApplySetting(_value);
			if (this.allowPresetCallback && this.preset != null)
			{
				this.preset.SetCustom();
			}
		}

		// Token: 0x040001F2 RID: 498
		protected Slider slider;
	}
}
