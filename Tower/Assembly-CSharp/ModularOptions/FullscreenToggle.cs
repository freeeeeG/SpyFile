using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x02000088 RID: 136
	[AddComponentMenu("Modular Options/Display/Fullscreen Toggle")]
	public sealed class FullscreenToggle : MonoBehaviour
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x000087B2 File Offset: 0x000069B2
		private void Awake()
		{
			this.toggle = base.GetComponent<Toggle>();
			this.toggle.isOn = Screen.fullScreen;
			this.toggle.onValueChanged.AddListener(delegate(bool _)
			{
				this.OnValueChange(_);
			});
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000087EC File Offset: 0x000069EC
		private void OnValueChange(bool _value)
		{
			Screen.fullScreen = _value;
		}

		// Token: 0x040001C0 RID: 448
		private Toggle toggle;
	}
}
