using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B98 RID: 2968
public class ModeSelectScreen : NewGameFlowScreen
{
	// Token: 0x06005C75 RID: 23669 RVA: 0x0021E1CD File Offset: 0x0021C3CD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.LoadWorldAndClusterData();
	}

	// Token: 0x06005C76 RID: 23670 RVA: 0x0021E1DC File Offset: 0x0021C3DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		HierarchyReferences component = this.survivalButton.GetComponent<HierarchyReferences>();
		this.survivalButtonHeader = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.survivalButtonSelectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle = this.survivalButton;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnterSurvival));
		MultiToggle multiToggle2 = this.survivalButton;
		multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExitSurvival));
		MultiToggle multiToggle3 = this.survivalButton;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(this.OnClickSurvival));
		HierarchyReferences component2 = this.nosweatButton.GetComponent<HierarchyReferences>();
		this.nosweatButtonHeader = component2.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.nosweatButtonSelectionFrame = component2.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle4 = this.nosweatButton;
		multiToggle4.onEnter = (System.Action)Delegate.Combine(multiToggle4.onEnter, new System.Action(this.OnHoverEnterNosweat));
		MultiToggle multiToggle5 = this.nosweatButton;
		multiToggle5.onExit = (System.Action)Delegate.Combine(multiToggle5.onExit, new System.Action(this.OnHoverExitNosweat));
		MultiToggle multiToggle6 = this.nosweatButton;
		multiToggle6.onClick = (System.Action)Delegate.Combine(multiToggle6.onClick, new System.Action(this.OnClickNosweat));
		this.closeButton.onClick += base.NavigateBackward;
	}

	// Token: 0x06005C77 RID: 23671 RVA: 0x0021E360 File Offset: 0x0021C560
	private void OnHoverEnterSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(1f);
		this.survivalButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.SURVIVAL_DESC;
	}

	// Token: 0x06005C78 RID: 23672 RVA: 0x0021E3C8 File Offset: 0x0021C5C8
	private void OnHoverExitSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(0f);
		this.survivalButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x06005C79 RID: 23673 RVA: 0x0021E42E File Offset: 0x0021C62E
	private void OnClickSurvival()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetSurvivalDefaults();
		base.NavigateForward();
	}

	// Token: 0x06005C7A RID: 23674 RVA: 0x0021E446 File Offset: 0x0021C646
	private void LoadWorldAndClusterData()
	{
		if (ModeSelectScreen.dataLoaded)
		{
			return;
		}
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		ModeSelectScreen.dataLoaded = true;
	}

	// Token: 0x06005C7B RID: 23675 RVA: 0x0021E478 File Offset: 0x0021C678
	private void OnHoverEnterNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(1f);
		this.nosweatButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.NOSWEAT_DESC;
	}

	// Token: 0x06005C7C RID: 23676 RVA: 0x0021E4E0 File Offset: 0x0021C6E0
	private void OnHoverExitNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(0f);
		this.nosweatButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x06005C7D RID: 23677 RVA: 0x0021E546 File Offset: 0x0021C746
	private void OnClickNosweat()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetNosweatDefaults();
		base.NavigateForward();
	}

	// Token: 0x04003E32 RID: 15922
	[SerializeField]
	private MultiToggle nosweatButton;

	// Token: 0x04003E33 RID: 15923
	private Image nosweatButtonHeader;

	// Token: 0x04003E34 RID: 15924
	private Image nosweatButtonSelectionFrame;

	// Token: 0x04003E35 RID: 15925
	[SerializeField]
	private MultiToggle survivalButton;

	// Token: 0x04003E36 RID: 15926
	private Image survivalButtonHeader;

	// Token: 0x04003E37 RID: 15927
	private Image survivalButtonSelectionFrame;

	// Token: 0x04003E38 RID: 15928
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x04003E39 RID: 15929
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003E3A RID: 15930
	[SerializeField]
	private KBatchedAnimController nosweatAnim;

	// Token: 0x04003E3B RID: 15931
	[SerializeField]
	private KBatchedAnimController survivalAnim;

	// Token: 0x04003E3C RID: 15932
	private static bool dataLoaded;
}
