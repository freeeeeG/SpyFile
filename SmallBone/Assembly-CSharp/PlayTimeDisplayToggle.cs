using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200006F RID: 111
public class PlayTimeDisplayToggle : MonoBehaviour
{
	// Token: 0x06000207 RID: 519 RVA: 0x00008ED8 File Offset: 0x000070D8
	private void Awake()
	{
		this._isHardmode = GameData.HardmodeProgress.hardmode;
		this._display.sprite = (this._isHardmode ? this._hardmodeTimerSprite : this._normalTimerSprite);
		this.Refresh();
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00008F0C File Offset: 0x0000710C
	private void Refresh()
	{
		if (!this._toggle)
		{
			return;
		}
		this._cachedShowTimer = GameData.Settings.showTimer;
		this._display.gameObject.SetActive(GameData.Settings.showTimer);
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00008F38 File Offset: 0x00007138
	private void Update()
	{
		if (!this._isHardmode && GameData.HardmodeProgress.hardmode)
		{
			this._isHardmode = true;
			this._display.sprite = this._hardmodeTimerSprite;
		}
		else if (this._isHardmode && !GameData.HardmodeProgress.hardmode)
		{
			this._isHardmode = false;
			this._display.sprite = this._normalTimerSprite;
		}
		if (this._cachedShowTimer == GameData.Settings.showTimer)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00008FA9 File Offset: 0x000071A9
	public void Toggle()
	{
		GameData.Settings.showTimer = !GameData.Settings.showTimer;
		this.Refresh();
	}

	// Token: 0x040001BD RID: 445
	[SerializeField]
	private Image _display;

	// Token: 0x040001BE RID: 446
	[SerializeField]
	private Sprite _normalTimerSprite;

	// Token: 0x040001BF RID: 447
	[SerializeField]
	private Sprite _hardmodeTimerSprite;

	// Token: 0x040001C0 RID: 448
	[SerializeField]
	private bool _toggle = true;

	// Token: 0x040001C1 RID: 449
	private bool _cachedShowTimer;

	// Token: 0x040001C2 RID: 450
	private bool _isHardmode;
}
