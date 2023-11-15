using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x0200009B RID: 155
	[AddComponentMenu("Modular Options/Display Slider Value")]
	[RequireComponent(typeof(Slider))]
	public sealed class DisplaySliderValue : MonoBehaviour
	{
		// Token: 0x06000222 RID: 546 RVA: 0x00008EE4 File Offset: 0x000070E4
		private void Awake()
		{
			this.formattingOverride = base.GetComponent<ISliderDisplayFormatter>();
			this.slider = base.GetComponent<Slider>();
			this.SetDisplayText(this.slider.value);
			this.slider.onValueChanged.AddListener(delegate(float _)
			{
				this.SetDisplayText(_);
			});
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00008F36 File Offset: 0x00007136
		private void SetDisplayText(float _value)
		{
			if (this.formattingOverride != null)
			{
				this.displayText.text = this.formattingOverride.OverrideFormatting(_value);
				return;
			}
			this.displayText.text = _value.ToString();
		}

		// Token: 0x040001DA RID: 474
		[Tooltip("Text UI to use for displaying the slider value.")]
		public TextMeshProUGUI displayText;

		// Token: 0x040001DB RID: 475
		private Slider slider;

		// Token: 0x040001DC RID: 476
		private ISliderDisplayFormatter formattingOverride;
	}
}
