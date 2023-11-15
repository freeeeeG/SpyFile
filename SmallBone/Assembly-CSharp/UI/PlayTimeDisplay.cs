using System;
using System.Globalization;
using Data;
using Level;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x020003C5 RID: 965
	public class PlayTimeDisplay : MonoBehaviour
	{
		// Token: 0x060011EE RID: 4590 RVA: 0x00034E85 File Offset: 0x00033085
		private void Awake()
		{
			this._originalColor = this._text.color;
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00034E98 File Offset: 0x00033098
		private void Update()
		{
			string text = new TimeSpan(0, 0, GameData.Progress.playTime).ToString("hh\\ \\:\\ mm\\ \\:\\ ss", CultureInfo.InvariantCulture);
			this._text.text = text;
			if (Map.Instance == null || Map.Instance.pauseTimer)
			{
				this._text.color = Color.grey;
				return;
			}
			this._text.color = this._originalColor;
		}

		// Token: 0x04000ED6 RID: 3798
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04000ED7 RID: 3799
		private Color _originalColor;
	}
}
