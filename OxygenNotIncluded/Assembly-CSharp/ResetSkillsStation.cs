using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000681 RID: 1665
[AddComponentMenu("KMonoBehaviour/Workable/ResetSkillsStation")]
public class ResetSkillsStation : Workable
{
	// Token: 0x06002C75 RID: 11381 RVA: 0x000EC84B File Offset: 0x000EAA4B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x000EC85A File Offset: 0x000EAA5A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnAssign(this.assignable.assignee);
		this.assignable.OnAssign += this.OnAssign;
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x000EC88A File Offset: 0x000EAA8A
	private void OnAssign(IAssignableIdentity obj)
	{
		if (obj != null)
		{
			this.CreateChore();
			return;
		}
		if (this.chore != null)
		{
			this.chore.Cancel("Unassigned");
			this.chore = null;
		}
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x000EC8B8 File Offset: 0x000EAAB8
	private void CreateChore()
	{
		this.chore = new WorkChore<ResetSkillsStation>(Db.Get().ChoreTypes.UnlearnSkill, this, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x000EC8F1 File Offset: 0x000EAAF1
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<Operational>().SetActive(true, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, this);
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x000EC924 File Offset: 0x000EAB24
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.assignable.Unassign();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null)
		{
			component.ResetSkillLevels(true);
			component.SetHats(component.CurrentHat, null);
			component.ApplyTargetHat();
			this.notification = new Notification(MISC.NOTIFICATIONS.RESETSKILL.NAME, NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.RESETSKILL.TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
			worker.GetComponent<Notifier>().Add(this.notification, "");
		}
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x000EC9C5 File Offset: 0x000EABC5
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<Operational>().SetActive(false, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, this);
		this.chore = null;
	}

	// Token: 0x04001A31 RID: 6705
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04001A32 RID: 6706
	private Notification notification;

	// Token: 0x04001A33 RID: 6707
	private Chore chore;
}
