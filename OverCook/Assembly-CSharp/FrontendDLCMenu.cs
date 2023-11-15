using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AB5 RID: 2741
public class FrontendDLCMenu : FrontendMenuBehaviour
{
	// Token: 0x0600367F RID: 13951 RVA: 0x000FF958 File Offset: 0x000FDD58
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.m_gamepadEngagementManager = GameUtils.RequireManager<GamepadEngagementManager>();
		this.m_dlcManager = GameUtils.RequireManager<DLCManager>();
		this.m_cardButtons = this.m_packSelectMenu.gameObject.RequestComponentsRecursive<DlcSelectButton>();
		if (this.m_packSelectMenu != null)
		{
			this.m_packSelectMenu.CarouselButtonClicked += this.OnPackSelected;
		}
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x000FF9C0 File Offset: 0x000FDDC0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		DLCManagerBase.DLCUpdatedEvent = (GenericVoid)Delegate.Remove(DLCManagerBase.DLCUpdatedEvent, new GenericVoid(this.OnDLCUpdated));
		if (this.m_packSelectMenu != null)
		{
			this.m_packSelectMenu.CarouselButtonClicked -= this.OnPackSelected;
		}
	}

	// Token: 0x06003681 RID: 13953 RVA: 0x000FFA1C File Offset: 0x000FDE1C
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		if (this.m_packSelectMenu == null || !this.m_packSelectMenu.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		for (int i = 0; i < this.m_cardButtons.Length; i++)
		{
			this.m_cardButtons[i].SetButtonCallbackHandler(this);
			this.m_cardButtons[i].SetFlipState(DlcSelectButton.FlipState.Front);
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = true;
		}
		if (this.m_gamepadEngagementManager != null)
		{
			this.m_engagementSuppressor = this.m_gamepadEngagementManager.Suppressor.AddSuppressor(this);
		}
		this.RefreshDLCButtons();
		DLCManagerBase.DLCUpdatedEvent = (GenericVoid)Delegate.Combine(DLCManagerBase.DLCUpdatedEvent, new GenericVoid(this.OnDLCUpdated));
		return true;
	}

	// Token: 0x06003682 RID: 13954 RVA: 0x000FFB04 File Offset: 0x000FDF04
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		if (this.m_packSelectMenu != null && !this.m_packSelectMenu.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		if (this.m_engagementSuppressor != null)
		{
			this.m_engagementSuppressor.Release();
			this.m_engagementSuppressor = null;
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = false;
		}
		DLCManagerBase.DLCUpdatedEvent = (GenericVoid)Delegate.Remove(DLCManagerBase.DLCUpdatedEvent, new GenericVoid(this.OnDLCUpdated));
		return true;
	}

	// Token: 0x06003683 RID: 13955 RVA: 0x000FFB9E File Offset: 0x000FDF9E
	public override void Close()
	{
		if (this.m_flippedCard != null)
		{
			this.SetFlippedCard(null, true);
		}
		else
		{
			base.Close();
		}
	}

	// Token: 0x06003684 RID: 13956 RVA: 0x000FFBC4 File Offset: 0x000FDFC4
	private void SetFlippedCard(DlcSelectButton card, bool autoSelect = true)
	{
		DlcSelectButton flippedCard = this.m_flippedCard;
		if (flippedCard != null)
		{
			flippedCard.SetFlipState(DlcSelectButton.FlipState.Front);
		}
		if (card != null)
		{
			this.m_flippedCard = card;
			this.m_flippedCard.SetFlipState(DlcSelectButton.FlipState.Back);
			if (autoSelect)
			{
				this.ReselectCard(this.m_flippedCard);
			}
		}
		else
		{
			this.m_flippedCard = null;
			if (autoSelect)
			{
				this.ReselectCard(flippedCard);
			}
		}
	}

	// Token: 0x06003685 RID: 13957 RVA: 0x000FFC38 File Offset: 0x000FE038
	protected override void Update()
	{
		base.Update();
		if (this.m_flippedCard != this.m_packSelectMenu.GetCurrentButton())
		{
			this.SetFlippedCard(null, false);
			if (this.m_flippedCard != null && this.m_flippedCard.m_flipState != DlcSelectButton.FlipState.Front)
			{
				this.SetFlippedCard((DlcSelectButton)this.m_packSelectMenu.GetCurrentButton(), true);
			}
		}
	}

	// Token: 0x06003686 RID: 13958 RVA: 0x000FFCA8 File Offset: 0x000FE0A8
	private void ReselectCard(DlcSelectButton card)
	{
		Selectable firstActiveButton = card.GetFirstActiveButton();
		if (firstActiveButton != null)
		{
			this.m_CachedEventSystem.SetSelectedGameObject(null);
			this.m_CachedEventSystem.SetSelectedGameObject(firstActiveButton.gameObject);
		}
		else if (this.m_BorderSelectables.selectOnUp != null && this.m_bSelectTopElementOnShow)
		{
			this.m_CachedEventSystem.SetSelectedGameObject(null);
			this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnUp.gameObject);
		}
	}

	// Token: 0x06003687 RID: 13959 RVA: 0x000FFD34 File Offset: 0x000FE134
	private void RefreshDLCButtons()
	{
		List<DLCFrontendData> list = new List<DLCFrontendData>(this.m_dlcManager.AllDlc);
		list.RemoveAll((DLCFrontendData x) => x == null || !x.IsAvailableOnThisPlatform() || !x.m_ShowOnDlcPage);
		int num = Mathf.CeilToInt(5f / (float)list.Count) * list.Count;
		int i;
		for (i = 0; i < num; i++)
		{
			this.m_cardButtons[i].Setup(list[i % list.Count]);
		}
		while (i < this.m_cardButtons.Length)
		{
			this.m_cardButtons[i].Setup(null);
			i++;
		}
	}

	// Token: 0x06003688 RID: 13960 RVA: 0x000FFDE2 File Offset: 0x000FE1E2
	private void OnDLCUpdated()
	{
		this.RefreshDLCButtons();
	}

	// Token: 0x06003689 RID: 13961 RVA: 0x000FFDEA File Offset: 0x000FE1EA
	private void OnPackSelected(CarouselButton button)
	{
		this.SetFlippedCard((DlcSelectButton)button, true);
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x000FFDF9 File Offset: 0x000FE1F9
	public void ShowStorePage(DLCFrontendData dlcData)
	{
		this.m_dlcManager.ShowDLCStorePage(dlcData);
	}

	// Token: 0x0600368B RID: 13963 RVA: 0x000FFE08 File Offset: 0x000FE208
	public void GotoChefSelectionScreen(DLCFrontendData data)
	{
		T17FrontendFlow.Instance.AutoOpenChefSelectionMenu(data);
		this.m_flippedCard = null;
		this.Close();
	}

	// Token: 0x0600368C RID: 13964 RVA: 0x000FFE24 File Offset: 0x000FE224
	public void OnContinueGameButtonPressed(DLCFrontendData dlcData)
	{
		int lastSaveSlot = GameUtils.GetMetaGameProgress().GetLastSaveSlot(dlcData.m_DLCID);
		SelectSaveDialog selectSaveDialog = T17FrontendFlow.Instance.gameObject.RequireComponentRecursive<SelectSaveDialog>();
		base.StartCoroutine(selectSaveDialog.LoadSlot(dlcData.m_DLCID, lastSaveSlot));
	}

	// Token: 0x0600368D RID: 13965 RVA: 0x000FFE68 File Offset: 0x000FE268
	public void OnNewGameButtonPressed(DLCFrontendData dlcData)
	{
		SelectSaveDialog selectSaveDialog = T17FrontendFlow.Instance.gameObject.RequireComponentRecursive<SelectSaveDialog>();
		selectSaveDialog.Mode = SaveDialogMode.NewGame;
		selectSaveDialog.DLC = dlcData.m_DLCID;
		selectSaveDialog.Show(this.m_CurrentGamepadUser, this, null, false);
	}

	// Token: 0x0600368E RID: 13966 RVA: 0x000FFEA8 File Offset: 0x000FE2A8
	public void OnLoadGameButtonPressed(DLCFrontendData dlcData)
	{
		SelectSaveDialog selectSaveDialog = T17FrontendFlow.Instance.gameObject.RequireComponentRecursive<SelectSaveDialog>();
		selectSaveDialog.Mode = SaveDialogMode.LoadGame;
		selectSaveDialog.DLC = dlcData.m_DLCID;
		selectSaveDialog.Show(this.m_CurrentGamepadUser, this, null, false);
	}

	// Token: 0x04002BCF RID: 11215
	private const int c_MinShownDLCButtons = 5;

	// Token: 0x04002BD0 RID: 11216
	[SerializeField]
	[AssignChildRecursive("CarouselRootMenu", Editorbility.Editable)]
	private CarouselRootMenu m_packSelectMenu;

	// Token: 0x04002BD1 RID: 11217
	private GamepadEngagementManager m_gamepadEngagementManager;

	// Token: 0x04002BD2 RID: 11218
	private Suppressor m_engagementSuppressor;

	// Token: 0x04002BD3 RID: 11219
	private DLCManager m_dlcManager;

	// Token: 0x04002BD4 RID: 11220
	private DlcSelectButton[] m_cardButtons = new DlcSelectButton[0];

	// Token: 0x04002BD5 RID: 11221
	private DlcSelectButton m_flippedCard;
}
