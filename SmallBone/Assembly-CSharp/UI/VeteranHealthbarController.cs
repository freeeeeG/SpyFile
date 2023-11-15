using System;
using Characters;
using GameResources;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x02000383 RID: 899
	public class VeteranHealthbarController : MonoBehaviour
	{
		// Token: 0x0600106B RID: 4203 RVA: 0x00030973 File Offset: 0x0002EB73
		public void Appear(Character character, string nameKey, string titleKey)
		{
			this.LocalizeText(this._name, nameKey);
			this.LocalizeText(this._title, titleKey);
			this._healthbar.Initialize(character);
			this._animator.Appear();
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x000309A6 File Offset: 0x0002EBA6
		public void Disappear()
		{
			if (this._healthbar.gameObject.activeSelf)
			{
				this._animator.Disappear();
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x000309C5 File Offset: 0x0002EBC5
		private void LocalizeText(TMP_Text ui, string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return;
			}
			ui.text = Localization.GetLocalizedString(key);
		}

		// Token: 0x04000D78 RID: 3448
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x04000D79 RID: 3449
		[SerializeField]
		private TMP_Text _title;

		// Token: 0x04000D7A RID: 3450
		[SerializeField]
		private CharacterHealthBar _healthbar;

		// Token: 0x04000D7B RID: 3451
		[SerializeField]
		private HangingPanelAnimator _animator;
	}
}
