using System;
using GameResources;
using TMPro;
using UnityEngine;

namespace UI.Adventurer
{
	// Token: 0x0200045D RID: 1117
	public class AdventurerNamePool : MonoBehaviour
	{
		// Token: 0x0600153C RID: 5436 RVA: 0x00042D18 File Offset: 0x00040F18
		private void OnEnable()
		{
			if (string.IsNullOrWhiteSpace(this._poolKey))
			{
				return;
			}
			string[] localizedStringArray = Localization.GetLocalizedStringArray(this._poolKey);
			if (localizedStringArray.Length == 0)
			{
				return;
			}
			this._text.text = localizedStringArray.Random<string>();
		}

		// Token: 0x04001291 RID: 4753
		[SerializeField]
		private string _poolKey;

		// Token: 0x04001292 RID: 4754
		[SerializeField]
		[GetComponent]
		private TMP_Text _text;
	}
}
