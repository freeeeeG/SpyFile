using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006F5 RID: 1781
public class DLCManagerBase : Manager
{
	// Token: 0x060021BA RID: 8634 RVA: 0x000A312B File Offset: 0x000A152B
	public virtual bool? HasDLC(int _pack)
	{
		throw new Exception("Deprecated code");
	}

	// Token: 0x060021BB RID: 8635 RVA: 0x000A3137 File Offset: 0x000A1537
	public GameSession GetDLCSessionPrefab(int _dlcPack)
	{
		throw new Exception("Deprecated code");
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x060021BC RID: 8636 RVA: 0x000A3143 File Offset: 0x000A1543
	public static int SupportedDLCLimit
	{
		get
		{
			return DLCManagerBase.c_dlcCount;
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x060021BD RID: 8637 RVA: 0x000A314A File Offset: 0x000A154A
	public List<DLCFrontendData> AllDlc
	{
		get
		{
			return this.m_allDlc;
		}
	}

	// Token: 0x060021BE RID: 8638 RVA: 0x000A3152 File Offset: 0x000A1552
	private void Awake()
	{
		this.Cleanup();
	}

	// Token: 0x060021BF RID: 8639 RVA: 0x000A315A File Offset: 0x000A155A
	private void Start()
	{
		this.Initialise();
		this.RefreshDLC();
	}

	// Token: 0x060021C0 RID: 8640 RVA: 0x000A3168 File Offset: 0x000A1568
	protected virtual void Initialise()
	{
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x000A3175 File Offset: 0x000A1575
	protected virtual void Cleanup()
	{
		this.m_DLCItems.Clear();
		DLCManagerBase.DLCUpdatedEvent = null;
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x000A3188 File Offset: 0x000A1588
	public virtual bool ShowDLCStorePage(DLCFrontendData data)
	{
		return false;
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x000A318B File Offset: 0x000A158B
	public virtual void RefreshDLC()
	{
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x000A3190 File Offset: 0x000A1590
	public bool IsDLCAvailable(DLCFrontendData data)
	{
		if (!data.IsAvailableOnThisPlatform())
		{
			return false;
		}
		if (data.m_IsFreeDLC)
		{
			return true;
		}
		DLCManagerBase.DLCItem dlcitem = this.FindDLCItem(data.productId);
		return dlcitem != null;
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x000A31CC File Offset: 0x000A15CC
	protected DLCManagerBase.DLCItem FindDLCItem(string productId)
	{
		for (int i = 0; i < this.m_DLCItems.Count; i++)
		{
			if (string.Equals(this.m_DLCItems[i].productId, productId, StringComparison.OrdinalIgnoreCase))
			{
				return this.m_DLCItems[i];
			}
		}
		return null;
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x000A3220 File Offset: 0x000A1620
	protected void AddDLCItem(DLCManagerBase.DLCItem item)
	{
		if (item == null)
		{
			throw new Exception("Invalid DLC item");
		}
		if (this.FindDLCItem(item.productId) == null)
		{
			this.m_DLCItems.Add(item);
		}
	}

	// Token: 0x040019F8 RID: 6648
	protected static readonly int c_dlcCount = 9;

	// Token: 0x040019F9 RID: 6649
	public const int c_MaxBitsPerDLCID = 4;

	// Token: 0x040019FA RID: 6650
	public const int c_NoDLCID = -1;

	// Token: 0x040019FB RID: 6651
	public static GenericVoid DLCUpdatedEvent;

	// Token: 0x040019FC RID: 6652
	protected PlayerManager m_playerManager;

	// Token: 0x040019FD RID: 6653
	[SerializeField]
	private List<DLCFrontendData> m_allDlc = new List<DLCFrontendData>();

	// Token: 0x040019FE RID: 6654
	protected List<DLCManagerBase.DLCItem> m_DLCItems = new List<DLCManagerBase.DLCItem>();

	// Token: 0x020006F6 RID: 1782
	public class DLCItem
	{
		// Token: 0x060021C9 RID: 8649 RVA: 0x000A3284 File Offset: 0x000A1684
		public void Save()
		{
		}

		// Token: 0x040019FF RID: 6655
		public string name;

		// Token: 0x04001A00 RID: 6656
		public string description;

		// Token: 0x04001A01 RID: 6657
		public string productId;

		// Token: 0x04001A02 RID: 6658
		public string path = string.Empty;

		// Token: 0x04001A03 RID: 6659
		public string bundlePath;

		// Token: 0x04001A04 RID: 6660
		public AssetBundle bundle;
	}
}
