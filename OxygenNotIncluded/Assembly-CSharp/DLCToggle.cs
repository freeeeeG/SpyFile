using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AE4 RID: 2788
public class DLCToggle : KMonoBehaviour
{
	// Token: 0x060055D1 RID: 21969 RVA: 0x001F3648 File Offset: 0x001F1848
	protected override void OnPrefabInit()
	{
		this.expansion1Active = DlcManager.IsExpansion1Active();
		this.button.onClick += this.ToggleExpansion1Cicked;
		this.label.text = (this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1 : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1);
		this.logo.sprite = (this.expansion1Active ? GlobalResources.Instance().baseGameLogoSmall : GlobalResources.Instance().expansion1LogoSmall);
		this.logo.gameObject.SetActive(!this.expansion1Active);
	}

	// Token: 0x060055D2 RID: 21970 RVA: 0x001F36E0 File Offset: 0x001F18E0
	private void ToggleExpansion1Cicked()
	{
		Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.GetComponentInParent<Canvas>().gameObject, true).AddDefaultCancel().SetHeader(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1 : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1).AddSprite(this.expansion1Active ? GlobalResources.Instance().baseGameLogoSmall : GlobalResources.Instance().expansion1LogoSmall).AddPlainText(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_DESC : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_DESC).AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen screen)
		{
			DlcManager.ToggleDLC("EXPANSION1_ID");
		}, true);
	}

	// Token: 0x04003994 RID: 14740
	[SerializeField]
	private KButton button;

	// Token: 0x04003995 RID: 14741
	[SerializeField]
	private LocText label;

	// Token: 0x04003996 RID: 14742
	[SerializeField]
	private Image logo;

	// Token: 0x04003997 RID: 14743
	private bool expansion1Active;
}
