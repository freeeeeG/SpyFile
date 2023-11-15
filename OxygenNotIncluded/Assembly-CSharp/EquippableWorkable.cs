using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007A3 RID: 1955
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/EquippableWorkable")]
public class EquippableWorkable : Workable, ISaveLoadable
{
	// Token: 0x06003654 RID: 13908 RVA: 0x0012586C File Offset: 0x00123A6C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Equipping;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_equip_clothing_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06003655 RID: 13909 RVA: 0x001258B9 File Offset: 0x00123AB9
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x06003656 RID: 13910 RVA: 0x001258C1 File Offset: 0x00123AC1
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x06003657 RID: 13911 RVA: 0x001258CA File Offset: 0x00123ACA
	protected override void OnSpawn()
	{
		base.SetWorkTime(1.5f);
		this.equippable.OnAssign += this.RefreshChore;
	}

	// Token: 0x06003658 RID: 13912 RVA: 0x001258EE File Offset: 0x00123AEE
	private void CreateChore()
	{
		global::Debug.Assert(this.chore == null, "chore should be null");
		this.chore = new EquipChore(this);
	}

	// Token: 0x06003659 RID: 13913 RVA: 0x0012590F File Offset: 0x00123B0F
	public void CancelChore(string reason = "")
	{
		if (this.chore != null)
		{
			this.chore.Cancel(reason);
			Prioritizable.RemoveRef(this.equippable.gameObject);
			this.chore = null;
		}
	}

	// Token: 0x0600365A RID: 13914 RVA: 0x0012593C File Offset: 0x00123B3C
	private void RefreshChore(IAssignableIdentity target)
	{
		if (this.chore != null)
		{
			this.CancelChore("Equipment Reassigned");
		}
		if (target != null && !target.GetSoleOwner().GetComponent<Equipment>().IsEquipped(this.equippable))
		{
			this.CreateChore();
		}
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x00125974 File Offset: 0x00123B74
	protected override void OnCompleteWork(Worker worker)
	{
		if (this.equippable.assignee != null)
		{
			Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
			if (soleOwner)
			{
				soleOwner.GetComponent<Equipment>().Equip(this.equippable);
				Prioritizable.RemoveRef(this.equippable.gameObject);
				this.chore = null;
			}
		}
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x001259CF File Offset: 0x00123BCF
	protected override void OnStopWork(Worker worker)
	{
		this.workTimeRemaining = this.GetWorkTime();
		base.OnStopWork(worker);
	}

	// Token: 0x0400212A RID: 8490
	[MyCmpReq]
	private Equippable equippable;

	// Token: 0x0400212B RID: 8491
	private Chore chore;

	// Token: 0x0400212C RID: 8492
	private global::QualityLevel quality;
}
