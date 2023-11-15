using System;
using UnityEngine;

namespace UI.Adventurer
{
	// Token: 0x0200045A RID: 1114
	[CreateAssetMenu(menuName = "AdventurerUI")]
	public class AdventurerHealthBarInfo : ScriptableObject
	{
		// Token: 0x04001285 RID: 4741
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04001286 RID: 4742
		[SerializeField]
		private string _nameKey;

		// Token: 0x04001287 RID: 4743
		[SerializeField]
		private string _level;
	}
}
