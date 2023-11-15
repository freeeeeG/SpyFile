using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x020000A6 RID: 166
	public abstract class OptionBase<T, U> : MonoBehaviour where T : struct where U : UIDataType<T>
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000246 RID: 582
		// (set) Token: 0x06000247 RID: 583
		public abstract T Value { get; set; }

		// Token: 0x06000248 RID: 584 RVA: 0x00009274 File Offset: 0x00007474
		public void ApplyPreset(T _value)
		{
			this.allowPresetCallback = false;
			this.Value = _value;
			this.allowPresetCallback = true;
		}

		// Token: 0x06000249 RID: 585
		protected abstract void ApplySetting(T _value);

		// Token: 0x040001ED RID: 493
		[Tooltip("Key for saving & loading, with other possible re-use.")]
		public string optionName;

		// Token: 0x040001EE RID: 494
		public U defaultSetting;

		// Token: 0x040001EF RID: 495
		[HideInInspector]
		public OptionPreset preset;

		// Token: 0x040001F0 RID: 496
		protected bool allowPresetCallback = true;
	}
}
