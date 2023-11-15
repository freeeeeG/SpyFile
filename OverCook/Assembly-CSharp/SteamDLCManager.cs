using System;
using System.Collections.Generic;
using Steamworks;

// Token: 0x0200074F RID: 1871
public class SteamDLCManager : DLCManagerBase
{
	// Token: 0x060023DD RID: 9181 RVA: 0x000A3299 File Offset: 0x000A1699
	private void Awake()
	{
		this.m_appId = SteamUtils.GetAppID();
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x000A32A6 File Offset: 0x000A16A6
	protected override void Initialise()
	{
		base.Initialise();
		this.m_dlcInstalledCallback = Callback<DlcInstalled_t>.Create(new Callback<DlcInstalled_t>.DispatchDelegate(this.OnDlcInstalled));
		this.m_playerManager.EngagementChangeCallback += this.OnEngagementChanged;
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x000A32DC File Offset: 0x000A16DC
	protected override void Cleanup()
	{
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x000A32F8 File Offset: 0x000A16F8
	public override bool ShowDLCStorePage(DLCFrontendData data)
	{
		uint value = 0U;
		if (uint.TryParse(data.productId, out value))
		{
			SteamFriends.ActivateGameOverlayToStore((AppId_t)value, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			return true;
		}
		return false;
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000A3328 File Offset: 0x000A1728
	public override void RefreshDLC()
	{
		base.RefreshDLC();
		int dlccount = SteamApps.GetDLCCount();
		for (int i = 0; i < dlccount; i++)
		{
			AppId_t appId_t;
			bool flag;
			string name;
			if (SteamApps.BGetDLCDataByIndex(i, out appId_t, out flag, out name, 128) && SteamApps.BIsDlcInstalled(appId_t))
			{
				if (!this.m_allDlcIds.Contains(appId_t))
				{
					this.m_allDlcIds.Add(appId_t);
					base.AddDLCItem(new DLCManagerBase.DLCItem
					{
						name = name,
						productId = appId_t.ToString()
					});
				}
			}
		}
		if (DLCManagerBase.DLCUpdatedEvent != null)
		{
			DLCManagerBase.DLCUpdatedEvent();
		}
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x000A33DB File Offset: 0x000A17DB
	private void OnDlcInstalled(DlcInstalled_t param)
	{
		this.RefreshDLC();
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x000A33E3 File Offset: 0x000A17E3
	private void OnEngagementChanged(EngagementSlot slot, GamepadUser userBefore, GamepadUser userAfter)
	{
		if (slot == EngagementSlot.One && userBefore == null && userAfter != null)
		{
			this.RefreshDLC();
		}
	}

	// Token: 0x04001B77 RID: 7031
	private AppId_t m_appId;

	// Token: 0x04001B78 RID: 7032
	private HashSet<AppId_t> m_allDlcIds = new HashSet<AppId_t>();

	// Token: 0x04001B79 RID: 7033
	protected Callback<DlcInstalled_t> m_dlcInstalledCallback;
}
