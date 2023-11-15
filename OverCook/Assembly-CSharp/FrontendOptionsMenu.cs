using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ABA RID: 2746
public class FrontendOptionsMenu : FrontendMenuBehaviour
{
	// Token: 0x060036A3 RID: 13987 RVA: 0x00100068 File Offset: 0x000FE468
	protected override void Awake()
	{
		base.Awake();
		if (this.m_VersionString != null)
		{
			this.m_VersionString.SetNonLocalizedText("Build #" + BuildVersion.m_VersionString);
		}
		this.m_BorderSelectables.selectOnUp = this.m_PCTopSelectable;
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x001000B7 File Offset: 0x000FE4B7
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.m_SyncOptions = base.gameObject.RequestInterfacesRecursive<ISyncUIWithOption>();
		this.m_gamepadEngagementManager = GameUtils.RequireManager<GamepadEngagementManager>();
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x001000DC File Offset: 0x000FE4DC
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = true;
		}
		if (this.m_gamepadEngagementManager != null)
		{
			this.m_engagementSuppressor = this.m_gamepadEngagementManager.Suppressor.AddSuppressor(this);
		}
		this.m_discard = false;
		this.ResetOptions();
		this.SyncAllOptions();
		InviteMonitor.InviteAccepted = (GenericVoid)Delegate.Combine(InviteMonitor.InviteAccepted, new GenericVoid(this.OnInviteAccepted));
		return this.m_ScrollView.Show(currentGamer, parent, invoker, hideInvoker) && base.Show(currentGamer, parent, invoker, hideInvoker);
	}

	// Token: 0x060036A6 RID: 13990 RVA: 0x00100184 File Offset: 0x000FE584
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		if (base.gameObject.activeSelf && !this.m_discard && metaGameProgress.AccessOptionsData.AnyChangesToCommit())
		{
			this.m_dialogBox = T17DialogBoxManager.GetDialog(false);
			this.m_dialogBox.Initialize("Text.Warning", "Text.Menu.UnsavedChanges.Body", "Text.Button.Discard", "Text.Button.Save", null, T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox dialogBox = this.m_dialogBox;
			dialogBox.OnDecline = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox.OnDecline, new T17DialogBox.DialogEvent(this.SaveAndClose));
			T17DialogBox dialogBox2 = this.m_dialogBox;
			dialogBox2.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox2.OnConfirm, new T17DialogBox.DialogEvent(this.ResetAndClose));
			this.m_dialogBox.Show();
			return false;
		}
		this.ResetOptions();
		InviteMonitor.InviteAccepted = (GenericVoid)Delegate.Remove(InviteMonitor.InviteAccepted, new GenericVoid(this.OnInviteAccepted));
		bool flag = this.m_ScrollView.Hide(restoreInvokerState, isTabSwitch);
		if (this.m_SafeAreaAdjuster.enabled)
		{
			this.m_SafeAreaAdjuster.gameObject.SetActive(false);
			this.m_SafeAreaAdjuster.Hide();
			if (this.m_SafeAreaInputSupressor != null)
			{
				this.m_CachedEventSystem.ReleaseSuppressor(this.m_SafeAreaInputSupressor);
			}
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = false;
		}
		if (this.m_engagementSuppressor != null)
		{
			this.m_engagementSuppressor.Release();
			this.m_engagementSuppressor = null;
		}
		return flag && base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x00100314 File Offset: 0x000FE714
	public void SyncAllOptions()
	{
		if (this.m_SyncOptions != null)
		{
			for (int i = 0; i < this.m_SyncOptions.Length; i++)
			{
				this.m_SyncOptions[i].SyncUIWithOption();
			}
		}
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x00100352 File Offset: 0x000FE752
	public void OnSaveClicked()
	{
		this.m_discard = true;
		this.SaveOptions();
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x00100361 File Offset: 0x000FE761
	private void SaveAndClose()
	{
		this.m_dialogBox = null;
		this.m_discard = false;
		this.SaveOptions();
		this.Hide(true, false);
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x00100380 File Offset: 0x000FE780
	public void ResetAndClose()
	{
		if (this.m_dialogBox != null && this.m_dialogBox.IsActive)
		{
			this.m_dialogBox.Hide();
		}
		this.m_dialogBox = null;
		this.m_discard = true;
		this.ResetOptions();
		this.Hide(true, false);
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x001003D8 File Offset: 0x000FE7D8
	private void SaveOptions()
	{
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		GameUtils.RequireManager<SaveManager>().SaveMetaProgress(null);
	}

	// Token: 0x060036AC RID: 13996 RVA: 0x001003F8 File Offset: 0x000FE7F8
	private void ResetOptions()
	{
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		metaGameProgress.AccessOptionsData.LoadFromSave();
	}

	// Token: 0x060036AD RID: 13997 RVA: 0x00100416 File Offset: 0x000FE816
	public void OnCancelClicked()
	{
		this.Hide(true, false);
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x00100421 File Offset: 0x000FE821
	public void ShowSafeAreaAdjuster()
	{
		this.m_SafeAreaAdjuster.gameObject.SetActive(true);
		this.m_SafeAreaAdjuster.Show();
		this.m_SafeAreaInputSupressor = this.m_CachedEventSystem.Disable(this);
	}

	// Token: 0x060036AF RID: 13999 RVA: 0x00100451 File Offset: 0x000FE851
	private void OnInviteAccepted()
	{
		this.ResetAndClose();
	}

	// Token: 0x060036B0 RID: 14000 RVA: 0x0010045C File Offset: 0x000FE85C
	protected override void Update()
	{
		base.Update();
		if (this.m_SafeAreaAdjuster.gameObject.activeInHierarchy && this.m_SafeAreaAdjuster.Completed)
		{
			this.m_SafeAreaAdjuster.gameObject.SetActive(false);
			this.m_CachedEventSystem.ReleaseSuppressor(this.m_SafeAreaInputSupressor);
		}
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x001004B8 File Offset: 0x000FE8B8
	protected override void OnDestroy()
	{
		if (this.m_dialogBox != null)
		{
			this.m_dialogBox.Hide();
		}
		InviteMonitor.InviteAccepted = (GenericVoid)Delegate.Remove(InviteMonitor.InviteAccepted, new GenericVoid(this.OnInviteAccepted));
		base.OnDestroy();
	}

	// Token: 0x04002BE4 RID: 11236
	[SerializeField]
	private Selectable m_PCTopSelectable;

	// Token: 0x04002BE5 RID: 11237
	[SerializeField]
	private Selectable m_ConsoleTopSelectable;

	// Token: 0x04002BE6 RID: 11238
	[SerializeField]
	private SafeAreaAdjuster m_SafeAreaAdjuster;

	// Token: 0x04002BE7 RID: 11239
	[SerializeField]
	private T17Text m_VersionString;

	// Token: 0x04002BE8 RID: 11240
	public T17ScrollView m_ScrollView;

	// Token: 0x04002BE9 RID: 11241
	private ISyncUIWithOption[] m_SyncOptions;

	// Token: 0x04002BEA RID: 11242
	private Suppressor m_SafeAreaInputSupressor;

	// Token: 0x04002BEB RID: 11243
	private T17DialogBox m_dialogBox;

	// Token: 0x04002BEC RID: 11244
	private GamepadEngagementManager m_gamepadEngagementManager;

	// Token: 0x04002BED RID: 11245
	private Suppressor m_engagementSuppressor;

	// Token: 0x04002BEE RID: 11246
	private bool m_discard;
}
