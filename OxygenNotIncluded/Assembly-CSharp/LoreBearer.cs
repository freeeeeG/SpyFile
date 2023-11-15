using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000854 RID: 2132
[AddComponentMenu("KMonoBehaviour/scripts/LoreBearer")]
public class LoreBearer : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06003E49 RID: 15945 RVA: 0x0015A289 File Offset: 0x00158489
	public string content
	{
		get
		{
			return Strings.Get("STRINGS.LORE.BUILDINGS." + base.gameObject.name + ".ENTRY");
		}
	}

	// Token: 0x06003E4A RID: 15946 RVA: 0x0015A2AF File Offset: 0x001584AF
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003E4B RID: 15947 RVA: 0x0015A2B7 File Offset: 0x001584B7
	public LoreBearer Internal_SetContent(LoreBearerAction action)
	{
		this.displayContentAction = action;
		return this;
	}

	// Token: 0x06003E4C RID: 15948 RVA: 0x0015A2C1 File Offset: 0x001584C1
	public static InfoDialogScreen ShowPopupDialog()
	{
		return (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
	}

	// Token: 0x06003E4D RID: 15949 RVA: 0x0015A2F4 File Offset: 0x001584F4
	private void OnClickRead()
	{
		InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(base.gameObject.GetComponent<KSelectable>().GetProperName()).AddDefaultOK(true);
		if (this.BeenClicked)
		{
			infoDialogScreen.AddPlainText(this.BeenSearched);
			return;
		}
		this.BeenClicked = true;
		if (DlcManager.IsExpansion1Active())
		{
			Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 1, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.gameObject.transform, 1.5f, false);
		}
		if (this.displayContentAction != null)
		{
			this.displayContentAction(infoDialogScreen);
			return;
		}
		LoreBearerUtil.UnlockNextJournalEntry(infoDialogScreen);
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x06003E4E RID: 15950 RVA: 0x0015A3BA File Offset: 0x001585BA
	public string SidescreenButtonText
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.NAME;
		}
	}

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x06003E4F RID: 15951 RVA: 0x0015A3D5 File Offset: 0x001585D5
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.TOOLTIP_ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.TOOLTIP;
		}
	}

	// Token: 0x06003E50 RID: 15952 RVA: 0x0015A3F0 File Offset: 0x001585F0
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06003E51 RID: 15953 RVA: 0x0015A3F3 File Offset: 0x001585F3
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06003E52 RID: 15954 RVA: 0x0015A3F6 File Offset: 0x001585F6
	public void OnSidescreenButtonPressed()
	{
		this.OnClickRead();
	}

	// Token: 0x06003E53 RID: 15955 RVA: 0x0015A3FE File Offset: 0x001585FE
	public bool SidescreenButtonInteractable()
	{
		return !this.BeenClicked;
	}

	// Token: 0x06003E54 RID: 15956 RVA: 0x0015A409 File Offset: 0x00158609
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x06003E55 RID: 15957 RVA: 0x0015A40D File Offset: 0x0015860D
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0400286D RID: 10349
	[Serialize]
	private bool BeenClicked;

	// Token: 0x0400286E RID: 10350
	public string BeenSearched = UI.USERMENUACTIONS.READLORE.ALREADY_SEARCHED;

	// Token: 0x0400286F RID: 10351
	private LoreBearerAction displayContentAction;
}
