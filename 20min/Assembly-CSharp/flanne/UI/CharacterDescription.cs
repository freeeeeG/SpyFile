using System;
using flanne.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000214 RID: 532
	public class CharacterDescription : MenuEntryDescription<CharacterMenu, CharacterData>
	{
		// Token: 0x06000BF6 RID: 3062 RVA: 0x0002C5EC File Offset: 0x0002A7EC
		public override void SetProperties(CharacterData data)
		{
			this.portrait.sprite = data.portrait;
			this.nameTMP.text = data.nameString;
			this.healthTMP.text = data.startHP.ToString();
			this.descriptionTMP.text = data.description;
		}

		// Token: 0x04000851 RID: 2129
		[SerializeField]
		private Image portrait;

		// Token: 0x04000852 RID: 2130
		[SerializeField]
		private TMP_Text nameTMP;

		// Token: 0x04000853 RID: 2131
		[SerializeField]
		private TMP_Text healthTMP;

		// Token: 0x04000854 RID: 2132
		[SerializeField]
		private TMP_Text descriptionTMP;
	}
}
