using System;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200103B RID: 4155
	public class Boss : MonoBehaviour
	{
		// Token: 0x06005026 RID: 20518 RVA: 0x000F1B2A File Offset: 0x000EFD2A
		public void ShowAppearanceText()
		{
			this._bossNameDisplay.ShowAppearanceText();
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x000F1B37 File Offset: 0x000EFD37
		public void HideAppearanceText()
		{
			this._bossNameDisplay.HideAppearanceText();
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x000F1B44 File Offset: 0x000EFD44
		private void OnDestroy()
		{
			this.HideAppearanceText();
		}

		// Token: 0x04004075 RID: 16501
		[SerializeField]
		private BossNameDisplay _bossNameDisplay;

		// Token: 0x04004076 RID: 16502
		[SerializeField]
		private AIController _boss;
	}
}
