using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x020000A7 RID: 167
	[Serializable]
	public class UIDataType<T> where T : struct
	{
		// Token: 0x040001F1 RID: 497
		[Tooltip("Setting used if no saved setting exists. Can also be used externally to restore defaults.")]
		[SerializeField]
		public T value;
	}
}
