using System;
using InControl;
using UnityEngine;

// Token: 0x02000AD0 RID: 2768
public class KeyboardRebindController : MonoBehaviour
{
	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x060037DD RID: 14301 RVA: 0x001077CB File Offset: 0x00105BCB
	public bool IsRebinding
	{
		get
		{
			return this.m_RebindDialog != null;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x060037DE RID: 14302 RVA: 0x001077D9 File Offset: 0x00105BD9
	public bool UnsavedChanges
	{
		get
		{
			return this.m_UnsavedChanges;
		}
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x001077E1 File Offset: 0x00105BE1
	public void SingleTimeInitialize()
	{
		this.m_RebindElementSets = base.GetComponentsInChildren<KeyboardRebindElementSet>(true);
		this.m_uiCancelButton = PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.UICancel);
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x001077FD File Offset: 0x00105BFD
	public void OnShow(BaseMenuBehaviour parentScreen)
	{
		this.m_ParentScreen = parentScreen;
		this.m_UnsavedChanges = false;
		this.RefreshBindingElements();
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x00107814 File Offset: 0x00105C14
	public void StartRebind(KeyboardRebindElement element)
	{
		if (this.ShowRebindDialog(element))
		{
			PCPadInputProvider.StartListeningForBinding(delegate(Key key)
			{
				this.HideRebindDialog();
				if (key != Key.Escape)
				{
					this.UpdateBinding(element, key);
				}
			});
		}
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x00107857 File Offset: 0x00105C57
	private void Update()
	{
		if (this.m_uiCancelButton != null && this.m_uiCancelButton.JustReleased())
		{
			this.m_uiCancelButton.ClaimPressEvent();
			this.m_uiCancelButton.ClaimReleaseEvent();
			this.HideRebindDialog();
		}
	}

	// Token: 0x060037E3 RID: 14307 RVA: 0x00107890 File Offset: 0x00105C90
	public void CancelRebind()
	{
		PCPadInputProvider.StopListeningForBinding();
		this.HideRebindDialog();
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x001078A0 File Offset: 0x00105CA0
	public void CancelAndCloseAllDialogs()
	{
		this.CancelRebind();
		if (this.m_UnsavedChanges)
		{
			this.DiscardChanges();
			this.m_UnsavedChanges = false;
		}
		if (null != this.m_UnsavedChangesDialog)
		{
			this.m_UnsavedChangesDialog.Hide();
			this.m_UnsavedChangesDialog = null;
		}
		if (null != this.m_InvalidDialog)
		{
			this.m_InvalidDialog.Hide();
			this.m_InvalidDialog = null;
		}
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x00107914 File Offset: 0x00105D14
	private void UpdateBinding(KeyboardRebindElement element, Key key)
	{
		this.m_UnsavedChanges = true;
		KeyboardRebindElementSet elementSet = element.ElementSet;
		for (int i = 0; i < elementSet.ElementCount; i++)
		{
			if (elementSet[i] == element)
			{
				elementSet[i].SetBinding(key);
			}
			else
			{
				elementSet[i].UnsetBinding(key);
			}
		}
		this.RefreshBindingElements();
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x00107980 File Offset: 0x00105D80
	public bool Validate()
	{
		if (this.m_RebindElementSets != null)
		{
			for (int i = 0; i < this.m_RebindElementSets.Length; i++)
			{
				for (int j = 0; j < this.m_RebindElementSets[i].ElementCount; j++)
				{
					if (!this.m_RebindElementSets[i][j].HasAnyBindings())
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x001079EC File Offset: 0x00105DEC
	private bool ShowRebindDialog(KeyboardRebindElement element)
	{
		if (this.m_RebindDialog != null)
		{
			return false;
		}
		this.m_ParentScreen.CachedEventSystem.SetSelectedGameObject(element.gameObject);
		this.m_RebindDialog = T17DialogBoxManager.GetDialog(false);
		if (this.m_RebindDialog != null)
		{
			string message = Localization.Get("Text.ControlsMenu.RemapBody", new LocToken[0]) + Localization.Get(element.ActionTag, new LocToken[0]) + Localization.Get("Text.ControlsMenu.Bracket", new LocToken[0]);
			this.m_RebindDialog.Initialize("Text.ControlsMenu.RemapTitle", message, null, null, null, T17DialogBox.Symbols.Unassigned, true, false, false);
			this.m_RebindDialog.Show();
		}
		return this.m_RebindDialog != null;
	}

	// Token: 0x060037E8 RID: 14312 RVA: 0x00107AA5 File Offset: 0x00105EA5
	private void HideRebindDialog()
	{
		if (this.m_RebindDialog != null)
		{
			this.m_RebindDialog.Hide();
			this.m_RebindDialog = null;
		}
	}

	// Token: 0x060037E9 RID: 14313 RVA: 0x00107ACC File Offset: 0x00105ECC
	public bool ShowUnsavedChangesDialog()
	{
		if (this.m_UnsavedChangesDialog != null)
		{
			return true;
		}
		this.m_UnsavedChangesDialog = T17DialogBoxManager.GetDialog(false);
		if (this.m_UnsavedChangesDialog != null)
		{
			this.m_UnsavedChangesDialog.Initialize("Text.Warning", "Text.Menu.UnsavedChanges.Body", "Text.Button.Discard", "Text.Button.Save", null, T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox unsavedChangesDialog = this.m_UnsavedChangesDialog;
			unsavedChangesDialog.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(unsavedChangesDialog.OnConfirm, new T17DialogBox.DialogEvent(delegate()
			{
				this.m_UnsavedChangesDialog = null;
				this.DiscardChanges();
				this.m_ParentScreen.Hide(true, false);
			}));
			T17DialogBox unsavedChangesDialog2 = this.m_UnsavedChangesDialog;
			unsavedChangesDialog2.OnDecline = (T17DialogBox.DialogEvent)Delegate.Combine(unsavedChangesDialog2.OnDecline, new T17DialogBox.DialogEvent(delegate()
			{
				this.m_UnsavedChangesDialog = null;
				if (this.Validate())
				{
					this.SaveChanges();
					this.m_ParentScreen.Hide(true, false);
				}
				else
				{
					this.ShowInvalidDialog();
				}
			}));
			this.m_UnsavedChangesDialog.Show();
		}
		return this.m_UnsavedChangesDialog != null;
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x00107B94 File Offset: 0x00105F94
	public bool ShowInvalidDialog()
	{
		if (this.m_InvalidDialog != null)
		{
			return true;
		}
		this.m_InvalidDialog = T17DialogBoxManager.GetDialog(false);
		if (this.m_InvalidDialog != null)
		{
			this.m_InvalidDialog.Initialize("Text.Warning", "Text.ControlsMenu.Warning", "Text.Button.Okay", null, null, T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox invalidDialog = this.m_InvalidDialog;
			invalidDialog.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(invalidDialog.OnConfirm, new T17DialogBox.DialogEvent(delegate()
			{
				this.m_InvalidDialog = null;
			}));
			this.m_InvalidDialog.Show();
		}
		return this.m_InvalidDialog != null;
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x00107C30 File Offset: 0x00106030
	public void OnDefaultsPressed()
	{
		ControlSchemeToggle componentInParent = base.GetComponentInParent<ControlSchemeToggle>();
		if (componentInParent != null)
		{
			if (componentInParent.IsCurrentSchemeSplit())
			{
				PCPadInputProvider.RestoreDefaultSplitBindings();
			}
			else
			{
				PCPadInputProvider.RestoreDefaultCombinedBindings();
			}
		}
		else
		{
			PCPadInputProvider.RestoreDefaultBindings();
		}
		this.RefreshBindingElements();
		this.m_UnsavedChanges = true;
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x00107C81 File Offset: 0x00106081
	public void OnApplyPressed()
	{
		if (!this.Validate())
		{
			this.ShowInvalidDialog();
			return;
		}
		this.SaveChanges();
		this.m_ParentScreen.Hide(true, false);
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x00107CAC File Offset: 0x001060AC
	private void RefreshBindingElements()
	{
		if (this.m_RebindElementSets != null)
		{
			for (int i = 0; i < this.m_RebindElementSets.Length; i++)
			{
				for (int j = 0; j < this.m_RebindElementSets[i].ElementCount; j++)
				{
					this.m_RebindElementSets[i][j].RefreshBindingText();
				}
			}
		}
	}

	// Token: 0x060037EE RID: 14318 RVA: 0x00107D10 File Offset: 0x00106110
	private void SaveChanges()
	{
		SaveManager saveManager = GameUtils.RequestManager<SaveManager>();
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		if (saveManager != null && metaGameProgress != null && metaGameProgress.SaveData != null)
		{
			PCPadInputProvider.SaveBindings(metaGameProgress.SaveData);
			saveManager.SaveMetaProgress(null);
		}
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x00107D60 File Offset: 0x00106160
	private void DiscardChanges()
	{
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		if (metaGameProgress != null && metaGameProgress.SaveData != null)
		{
			PCPadInputProvider.LoadBindings(metaGameProgress.SaveData);
		}
	}

	// Token: 0x060037F0 RID: 14320 RVA: 0x00107D95 File Offset: 0x00106195
	private void OnDestroy()
	{
		if (this.m_UnsavedChanges)
		{
			this.DiscardChanges();
		}
	}

	// Token: 0x04002CB5 RID: 11445
	private KeyboardRebindElementSet[] m_RebindElementSets;

	// Token: 0x04002CB6 RID: 11446
	private BaseMenuBehaviour m_ParentScreen;

	// Token: 0x04002CB7 RID: 11447
	private T17DialogBox m_RebindDialog;

	// Token: 0x04002CB8 RID: 11448
	private T17DialogBox m_UnsavedChangesDialog;

	// Token: 0x04002CB9 RID: 11449
	private T17DialogBox m_InvalidDialog;

	// Token: 0x04002CBA RID: 11450
	private bool m_UnsavedChanges;

	// Token: 0x04002CBB RID: 11451
	private ILogicalButton m_uiCancelButton;
}
