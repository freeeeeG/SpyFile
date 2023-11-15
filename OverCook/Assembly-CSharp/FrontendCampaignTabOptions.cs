using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AAD RID: 2733
public class FrontendCampaignTabOptions : FrontendMenuBehaviour
{
	// Token: 0x0600362A RID: 13866 RVA: 0x000FDDA8 File Offset: 0x000FC1A8
	protected override void Awake()
	{
		base.Awake();
		if (T17FrontendFlow.Instance != null)
		{
			this.m_frontendRootMenu = T17FrontendFlow.Instance.m_Rootmenu;
			this.m_saveDialog = this.m_frontendRootMenu.SearchAllForMenuOfType<SelectSaveDialog>();
		}
		this.m_continueButton = this.m_continueButtonDLC;
		this.m_campaignOptions.SetActive(false);
		this.m_campaignOptionsDLC.SetActive(true);
		if (this.m_continueButton != null)
		{
			MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
			int num = -1;
			if (metaGameProgress != null)
			{
				num = metaGameProgress.GetLastSaveSlot(-1);
			}
			this.m_continueButton.gameObject.SetActive(num != -1);
		}
		Selectable[] componentsInChildren = base.transform.GetComponentsInChildren<Selectable>();
		if (componentsInChildren.Length > 0)
		{
			this.m_BorderSelectables.selectOnUp = componentsInChildren[0];
		}
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x000FDE78 File Offset: 0x000FC278
	public void OnContinueGameClicked()
	{
		int lastSaveSlot = GameUtils.GetMetaGameProgress().GetLastSaveSlot(-1);
		base.StartCoroutine(this.m_saveDialog.LoadSlot(-1, lastSaveSlot));
	}

	// Token: 0x0600362C RID: 13868 RVA: 0x000FDEA8 File Offset: 0x000FC2A8
	public void OnNewGameClicked(GameObject _invoker)
	{
		if (this.m_saveDialog != null && this.m_frontendRootMenu != null)
		{
			this.m_saveDialog.Mode = SaveDialogMode.NewGame;
			this.m_saveDialog.DLC = -1;
			this.m_frontendRootMenu.OpenFrontendMenu(this.m_saveDialog);
		}
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x000FDF00 File Offset: 0x000FC300
	public void OnLoadGameClicked(GameObject _invoker)
	{
		if (this.m_saveDialog != null && this.m_frontendRootMenu != null)
		{
			this.m_saveDialog.Mode = SaveDialogMode.LoadGame;
			this.m_saveDialog.DLC = -1;
			this.m_frontendRootMenu.OpenFrontendMenu(this.m_saveDialog);
		}
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x000FDF58 File Offset: 0x000FC358
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		this.RefreshNewContentIndicator();
		return true;
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x000FDF73 File Offset: 0x000FC373
	protected override void Update()
	{
		base.Update();
		this.RefreshNewContentIndicator();
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x000FDF84 File Offset: 0x000FC384
	private void RefreshNewContentIndicator()
	{
		if (this.m_newContentIndicator != null)
		{
			this.m_newContentIndicator.SetActive(false);
		}
		DLCManager dlcmanager = GameUtils.RequireManager<DLCManager>();
		SaveManager saveManager = GameUtils.RequireManager<SaveManager>();
		MetaGameProgress metaGameProgress = saveManager.GetMetaGameProgress();
		if (metaGameProgress != null)
		{
			List<DLCFrontendData> list = new List<DLCFrontendData>(dlcmanager.AllDlc);
			list.RemoveAll((DLCFrontendData x) => x == null || !x.IsAvailableOnThisPlatform() || !x.m_ShowOnDlcPage);
			for (int i = 0; i < list.Count; i++)
			{
				if (!metaGameProgress.GetDLCSeen(list[i]))
				{
					this.m_newContentIndicator.SetActive(true);
					break;
				}
			}
		}
	}

	// Token: 0x04002B91 RID: 11153
	[SerializeField]
	private Transform m_continueButton;

	// Token: 0x04002B92 RID: 11154
	private FrontendRootMenu m_frontendRootMenu;

	// Token: 0x04002B93 RID: 11155
	private SelectSaveDialog m_saveDialog;

	// Token: 0x04002B94 RID: 11156
	[SerializeField]
	private GameObject m_campaignOptions;

	// Token: 0x04002B95 RID: 11157
	[SerializeField]
	private GameObject m_campaignOptionsDLC;

	// Token: 0x04002B96 RID: 11158
	[SerializeField]
	private Transform m_continueButtonDLC;

	// Token: 0x04002B97 RID: 11159
	[SerializeField]
	private GameObject m_newContentIndicator;
}
