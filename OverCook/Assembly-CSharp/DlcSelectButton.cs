using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AD4 RID: 2772
public class DlcSelectButton : CarouselButton
{
	// Token: 0x0600380F RID: 14351 RVA: 0x001081FD File Offset: 0x001065FD
	protected override void Initialise()
	{
		base.Initialise();
	}

	// Token: 0x06003810 RID: 14352 RVA: 0x00108208 File Offset: 0x00106608
	public void Setup(DLCFrontendData data)
	{
		this.m_flipAnimator = base.GetComponent<Animator>();
		this.m_dlcData = data;
		if (this.m_dlcData == null || !this.m_dlcData.IsAvailableOnThisPlatform())
		{
			base.gameObject.SetActive(false);
			return;
		}
		DLCManager dlcmanager = GameUtils.RequireManager<DLCManager>();
		this.m_isOwned = dlcmanager.IsDLCAvailable(this.m_dlcData);
		if (data.m_type == DLCType.Levels)
		{
			int lastSaveSlot = GameUtils.GetMetaGameProgress().GetLastSaveSlot(this.m_dlcData.m_DLCID);
			this.m_hasSaveData = (lastSaveSlot != -1);
		}
		else
		{
			this.m_hasSaveData = false;
		}
		base.Initialise();
		this.SetupUI();
		T17Button t17Button = base.Button as T17Button;
		T17Button t17Button2 = t17Button;
		t17Button2.OnButtonDeselect = (T17Button.T17ButtonDelegate)Delegate.Combine(t17Button2.OnButtonDeselect, new T17Button.T17ButtonDelegate(this.OnButtonDeselect));
	}

	// Token: 0x06003811 RID: 14353 RVA: 0x001082E2 File Offset: 0x001066E2
	public void SetButtonCallbackHandler(FrontendDLCMenu handler)
	{
		this.m_buttonHandler = handler;
	}

	// Token: 0x06003812 RID: 14354 RVA: 0x001082EC File Offset: 0x001066EC
	private void SetupUI()
	{
		if (this.m_imageFront == null || this.m_imageBack == null || this.m_titleTextFront == null || this.m_titleTextBack == null || this.m_descriptionText == null)
		{
			return;
		}
		this.m_imageFront.sprite = this.m_dlcData.m_PreviewImage;
		this.m_titleTextFront.SetLocalisedTextCatchAll(this.m_dlcData.m_NameLocalizationKey);
		this.m_titleTextBack.SetLocalisedTextCatchAll(this.m_dlcData.m_NameLocalizationKey);
		this.m_descriptionText.SetLocalisedTextCatchAll(this.m_dlcData.m_DescriptionLocalizationKey);
		if (this.m_topButton == null || this.m_middleButton == null || this.m_bottomButton == null)
		{
			return;
		}
		if (!this.m_isOwned)
		{
			this.SetupButton(this.m_topButton, null);
			this.SetupButton(this.m_middleButton, null);
			this.SetupButton(this.m_bottomButton, this.c_storeButtonText);
		}
		else
		{
			DLCType type = this.m_dlcData.m_type;
			if (type != DLCType.Levels)
			{
				if (type == DLCType.Avatars)
				{
					this.SetupButton(this.m_topButton, null);
					this.SetupButton(this.m_middleButton, null);
					this.SetupButton(this.m_bottomButton, this.c_chefsButtonText);
				}
			}
			else
			{
				if (this.m_hasSaveData)
				{
					this.SetupButton(this.m_topButton, this.c_continueButtonText);
				}
				else
				{
					this.SetupButton(this.m_topButton, null);
				}
				this.SetupButton(this.m_middleButton, this.c_newButtonText);
				this.SetupButton(this.m_bottomButton, this.c_loadButtonText);
			}
		}
		this.RefreshNewContentIndicator();
	}

	// Token: 0x06003813 RID: 14355 RVA: 0x001084C4 File Offset: 0x001068C4
	private void SetupButton(T17Button button, string label)
	{
		if (button != null)
		{
			if (string.IsNullOrEmpty(label))
			{
				button.gameObject.SetActive(false);
			}
			else
			{
				button.gameObject.SetActive(true);
				T17Text componentInChildren = button.gameObject.GetComponentInChildren<T17Text>();
				if (componentInChildren != null)
				{
					componentInChildren.SetLocalisedTextCatchAll(label);
				}
			}
		}
	}

	// Token: 0x06003814 RID: 14356 RVA: 0x00108524 File Offset: 0x00106924
	private void Update()
	{
		this.RefreshNewContentIndicator();
	}

	// Token: 0x06003815 RID: 14357 RVA: 0x0010852C File Offset: 0x0010692C
	private void RefreshNewContentIndicator()
	{
		SaveManager saveManager = GameUtils.RequireManager<SaveManager>();
		MetaGameProgress metaGameProgress = saveManager.GetMetaGameProgress();
		if (metaGameProgress != null)
		{
			bool dlcseen = metaGameProgress.GetDLCSeen(this.m_dlcData);
			this.m_newContentIndicator.SetActive(!dlcseen);
		}
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x00108570 File Offset: 0x00106970
	public void SetFlipState(DlcSelectButton.FlipState flipState)
	{
		this.m_flipState = flipState;
		DlcSelectButton.FlipState flipState2 = this.m_flipState;
		if (flipState2 != DlcSelectButton.FlipState.Front)
		{
			if (flipState2 == DlcSelectButton.FlipState.Back)
			{
				if (this.m_flipAnimator != null)
				{
					this.m_flipAnimator.SetTrigger("PostcardFlip");
				}
				base.Button.interactable = false;
				this.m_back.SetActive(true);
				this.m_front.SetActive(false);
			}
		}
		else
		{
			if (this.m_flipAnimator != null)
			{
				this.m_flipAnimator.SetTrigger("PostcardFlip");
			}
			base.Button.interactable = true;
			this.m_back.SetActive(false);
			this.m_front.SetActive(true);
		}
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x00108634 File Offset: 0x00106A34
	public Selectable GetFirstActiveButton()
	{
		if (this.m_back != null && this.m_back.gameObject.activeSelf)
		{
			if (this.m_topButton != null && this.m_topButton.isActiveAndEnabled && this.m_topButton.interactable)
			{
				return this.m_topButton;
			}
			if (this.m_middleButton != null && this.m_middleButton.isActiveAndEnabled && this.m_middleButton.interactable)
			{
				return this.m_middleButton;
			}
			if (this.m_bottomButton != null && this.m_bottomButton.isActiveAndEnabled && this.m_bottomButton.interactable)
			{
				return this.m_bottomButton;
			}
		}
		return base.Button;
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x00108715 File Offset: 0x00106B15
	public void OnTopButtonClicked()
	{
		if (this.m_buttonHandler != null)
		{
			this.m_buttonHandler.OnContinueGameButtonPressed(this.m_dlcData);
		}
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x00108739 File Offset: 0x00106B39
	public void OnMiddleButtonClicked()
	{
		if (this.m_buttonHandler != null)
		{
			this.m_buttonHandler.OnNewGameButtonPressed(this.m_dlcData);
		}
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x00108760 File Offset: 0x00106B60
	public void OnBottomButtonClicked()
	{
		if (!this.m_isOwned)
		{
			if (this.m_buttonHandler != null)
			{
				this.m_buttonHandler.ShowStorePage(this.m_dlcData);
			}
		}
		else if (this.m_dlcData.m_type == DLCType.Avatars)
		{
			if (this.m_buttonHandler != null)
			{
				this.m_buttonHandler.GotoChefSelectionScreen(this.m_dlcData);
			}
		}
		else if (this.m_dlcData.m_type == DLCType.Levels && this.m_buttonHandler != null)
		{
			this.m_buttonHandler.OnLoadGameButtonPressed(this.m_dlcData);
		}
	}

	// Token: 0x0600381B RID: 14363 RVA: 0x0010880C File Offset: 0x00106C0C
	private void OnButtonDeselect(T17Button sender)
	{
		SaveManager saveManager = GameUtils.RequireManager<SaveManager>();
		MetaGameProgress metaGameProgress = saveManager.GetMetaGameProgress();
		if (metaGameProgress != null)
		{
			metaGameProgress.SetDLCSeen(this.m_dlcData);
		}
		this.RefreshNewContentIndicator();
	}

	// Token: 0x04002CC6 RID: 11462
	private readonly string c_storeButtonText = "Text.DLC.Store";

	// Token: 0x04002CC7 RID: 11463
	private readonly string c_newButtonText = "Text.DLC.NewGame";

	// Token: 0x04002CC8 RID: 11464
	private readonly string c_continueButtonText = "Text.DLC.ContinueGame";

	// Token: 0x04002CC9 RID: 11465
	private readonly string c_loadButtonText = "Text.DLC.LoadGame";

	// Token: 0x04002CCA RID: 11466
	private readonly string c_chefsButtonText = "Text.DLC.GotoChefs";

	// Token: 0x04002CCB RID: 11467
	[SerializeField]
	private GameObject m_front;

	// Token: 0x04002CCC RID: 11468
	[SerializeField]
	private GameObject m_back;

	// Token: 0x04002CCD RID: 11469
	[SerializeField]
	private Image m_imageFront;

	// Token: 0x04002CCE RID: 11470
	[SerializeField]
	private Image m_imageBack;

	// Token: 0x04002CCF RID: 11471
	[SerializeField]
	private T17Text m_titleTextFront;

	// Token: 0x04002CD0 RID: 11472
	[SerializeField]
	private T17Text m_titleTextBack;

	// Token: 0x04002CD1 RID: 11473
	[SerializeField]
	private T17Text m_descriptionText;

	// Token: 0x04002CD2 RID: 11474
	[SerializeField]
	private T17Button m_topButton;

	// Token: 0x04002CD3 RID: 11475
	[SerializeField]
	private T17Button m_middleButton;

	// Token: 0x04002CD4 RID: 11476
	[SerializeField]
	private T17Button m_bottomButton;

	// Token: 0x04002CD5 RID: 11477
	[SerializeField]
	private GameObject m_newContentIndicator;

	// Token: 0x04002CD6 RID: 11478
	private FrontendDLCMenu m_buttonHandler;

	// Token: 0x04002CD7 RID: 11479
	private DLCFrontendData m_dlcData;

	// Token: 0x04002CD8 RID: 11480
	private bool m_isOwned;

	// Token: 0x04002CD9 RID: 11481
	private bool m_hasSaveData;

	// Token: 0x04002CDA RID: 11482
	public DlcSelectButton.FlipState m_flipState;

	// Token: 0x04002CDB RID: 11483
	private Animator m_flipAnimator;

	// Token: 0x02000AD5 RID: 2773
	public enum FlipState
	{
		// Token: 0x04002CDD RID: 11485
		Front,
		// Token: 0x04002CDE RID: 11486
		Back
	}
}
