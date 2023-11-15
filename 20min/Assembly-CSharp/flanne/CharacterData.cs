using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000062 RID: 98
	[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData", order = 1)]
	public class CharacterData : ScriptableObject
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00016D71 File Offset: 0x00014F71
		public string nameString
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.nameStringID.key);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00016D83 File Offset: 0x00014F83
		public string description
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.descriptionStringID.key);
			}
		}

		// Token: 0x04000260 RID: 608
		public LocalizedString nameStringID;

		// Token: 0x04000261 RID: 609
		public LocalizedString descriptionStringID;

		// Token: 0x04000262 RID: 610
		public RuntimeAnimatorController animController;

		// Token: 0x04000263 RID: 611
		public RuntimeAnimatorController uiAnimController;

		// Token: 0x04000264 RID: 612
		public Sprite portrait;

		// Token: 0x04000265 RID: 613
		public Sprite icon;

		// Token: 0x04000266 RID: 614
		public int startHP;

		// Token: 0x04000267 RID: 615
		public GameObject passivePrefab;

		// Token: 0x04000268 RID: 616
		public PowerupPoolProfile exclusivePowerups;
	}
}
