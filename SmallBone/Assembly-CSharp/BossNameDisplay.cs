using System;
using GameResources;
using Scenes;
using UI.Boss;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class BossNameDisplay : MonoBehaviour
{
	// Token: 0x060002DB RID: 731 RVA: 0x0000B5CE File Offset: 0x000097CE
	public void ShowAppearanceText()
	{
		Scene<GameBase>.instance.uiManager.bossUI.appearnaceText.Appear(Localization.GetLocalizedString(this._nameKey), Localization.GetLocalizedString(this._subNameKey), 1.7f);
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0000B604 File Offset: 0x00009804
	public void HideAppearanceText()
	{
		Scene<GameBase>.instance.uiManager.bossUI.appearnaceText.Disappear(1.7f);
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0000B624 File Offset: 0x00009824
	public void ShowAndHideAppearanceText()
	{
		BossUIContainer bossUI = Scene<GameBase>.instance.uiManager.bossUI;
		bossUI.StartCoroutine(bossUI.appearnaceText.ShowAndHideText(Localization.GetLocalizedString(this._nameKey), Localization.GetLocalizedString(this._subNameKey)));
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0000B65C File Offset: 0x0000985C
	private void OnDestroy()
	{
		this.HideAppearanceText();
	}

	// Token: 0x04000265 RID: 613
	[SerializeField]
	private string _nameKey;

	// Token: 0x04000266 RID: 614
	[SerializeField]
	private string _subNameKey;

	// Token: 0x04000267 RID: 615
	[SerializeField]
	private string _chapterNameKey;
}
