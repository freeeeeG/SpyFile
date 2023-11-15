using System;
using Data;
using TMPro;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class HardmodeLevelHUD : MonoBehaviour
{
	// Token: 0x06000348 RID: 840 RVA: 0x0000C647 File Offset: 0x0000A847
	private void Awake()
	{
		this._isHardmode = GameData.HardmodeProgress.hardmode;
		this._display.gameObject.SetActive(this._isHardmode);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x0000C66C File Offset: 0x0000A86C
	private void Update()
	{
		if (GameData.HardmodeProgress.hardmode && this._cachedLevel != GameData.HardmodeProgress.hardmodeLevel)
		{
			this._cachedLevel = GameData.HardmodeProgress.hardmodeLevel;
			this._text.text = GameData.HardmodeProgress.hardmodeLevel.ToString();
		}
		if (!this._isHardmode && GameData.HardmodeProgress.hardmode)
		{
			this._isHardmode = true;
			this._display.gameObject.SetActive(true);
			return;
		}
		if (this._isHardmode && !GameData.HardmodeProgress.hardmode)
		{
			this._isHardmode = false;
			this._display.gameObject.SetActive(false);
		}
	}

	// Token: 0x040002AC RID: 684
	[SerializeField]
	private GameObject _display;

	// Token: 0x040002AD RID: 685
	[SerializeField]
	private TextMeshProUGUI _text;

	// Token: 0x040002AE RID: 686
	private bool _isHardmode;

	// Token: 0x040002AF RID: 687
	private int _cachedLevel = -1;
}
