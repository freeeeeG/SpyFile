using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x020000A3 RID: 163
	[CreateAssetMenu(fileName = "UISoundData", menuName = "DataContainer/UI/SelectableSound")]
	public class SelectableUISoundData : ScriptableObject
	{
		// Token: 0x040001E7 RID: 487
		public AudioClip submitSound;

		// Token: 0x040001E8 RID: 488
		public AudioClip selectionSound;

		// Token: 0x040001E9 RID: 489
		public AudioClip deselectionSound;
	}
}
