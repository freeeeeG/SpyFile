using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class PajamaDispenser : Workable, IDispenser
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060008DE RID: 2270 RVA: 0x000347D8 File Offset: 0x000329D8
	// (remove) Token: 0x060008DF RID: 2271 RVA: 0x00034810 File Offset: 0x00032A10
	public event System.Action OnStopWorkEvent;

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00034845 File Offset: 0x00032A45
	// (set) Token: 0x060008E1 RID: 2273 RVA: 0x00034850 File Offset: 0x00032A50
	private WorkChore<PajamaDispenser> Chore
	{
		get
		{
			return this.chore;
		}
		set
		{
			this.chore = value;
			if (this.chore != null)
			{
				base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, null);
				return;
			}
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, true);
		}
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x000348AF File Offset: 0x00032AAF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (PajamaDispenser.pajamaPrefab != null)
		{
			return;
		}
		PajamaDispenser.pajamaPrefab = Assets.GetPrefab(new Tag("SleepClinicPajamas"));
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x000348DC File Offset: 0x00032ADC
	protected override void OnCompleteWork(Worker worker)
	{
		Vector3 targetPoint = this.GetTargetPoint();
		targetPoint.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
		Util.KInstantiate(PajamaDispenser.pajamaPrefab, targetPoint, Quaternion.identity, null, null, true, 0).SetActive(true);
		this.hasDispenseChore = false;
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00034920 File Offset: 0x00032B20
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		if (this.Chore != null && this.Chore.smi.IsRunning())
		{
			this.Chore.Cancel("work interrupted");
		}
		this.Chore = null;
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
		if (this.OnStopWorkEvent != null)
		{
			this.OnStopWorkEvent();
		}
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x00034988 File Offset: 0x00032B88
	[ContextMenu("fetch")]
	public void FetchPajamas()
	{
		if (this.Chore != null)
		{
			return;
		}
		this.hasDispenseChore = true;
		this.Chore = new WorkChore<PajamaDispenser>(Db.Get().ChoreTypes.EquipmentFetch, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, false);
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x000349D4 File Offset: 0x00032BD4
	public void CancelFetch()
	{
		if (this.Chore == null)
		{
			return;
		}
		this.Chore.Cancel("User Cancelled");
		this.Chore = null;
		this.hasDispenseChore = false;
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, false);
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00034A29 File Offset: 0x00032C29
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00034A3F File Offset: 0x00032C3F
	public List<Tag> DispensedItems()
	{
		return PajamaDispenser.PajamaList;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00034A46 File Offset: 0x00032C46
	public Tag SelectedItem()
	{
		return PajamaDispenser.PajamaList[0];
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00034A53 File Offset: 0x00032C53
	public void SelectItem(Tag tag)
	{
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00034A55 File Offset: 0x00032C55
	public void OnOrderDispense()
	{
		this.FetchPajamas();
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x00034A5D File Offset: 0x00032C5D
	public void OnCancelDispense()
	{
		this.CancelFetch();
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x00034A65 File Offset: 0x00032C65
	public bool HasOpenChore()
	{
		return this.Chore != null;
	}

	// Token: 0x04000597 RID: 1431
	[Serialize]
	private bool hasDispenseChore;

	// Token: 0x04000598 RID: 1432
	private static GameObject pajamaPrefab = null;

	// Token: 0x0400059A RID: 1434
	private WorkChore<PajamaDispenser> chore;

	// Token: 0x0400059B RID: 1435
	private static List<Tag> PajamaList = new List<Tag>
	{
		"SleepClinicPajamas"
	};
}
