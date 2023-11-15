using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000485 RID: 1157
[AddComponentMenu("KMonoBehaviour/Workable/Breakable")]
public class Breakable : Workable
{
	// Token: 0x06001974 RID: 6516 RVA: 0x0008533D File Offset: 0x0008353D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_break_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x00085375 File Offset: 0x00083575
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Breakables.Add(this);
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x00085388 File Offset: 0x00083588
	public bool isBroken()
	{
		return this.hp == null || this.hp.HitPoints <= 0;
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x000853AC File Offset: 0x000835AC
	public Notification CreateDamageNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION, NotificationType.BadMinor, (List<Notification> notificationList, object data) => BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x00085404 File Offset: 0x00083604
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x0008546C File Offset: 0x0008366C
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.secondsPerTenPercentDamage = 2f;
		this.tenPercentDamage = Mathf.CeilToInt((float)this.hp.MaxHitPoints * 0.1f);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AngerDamage, this);
		this.notification = this.CreateDamageNotification();
		base.gameObject.AddOrGet<Notifier>().Add(this.notification, "");
		this.elapsedDamageTime = 0f;
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x000854F8 File Offset: 0x000836F8
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (this.elapsedDamageTime >= this.secondsPerTenPercentDamage)
		{
			this.elapsedDamageTime -= this.elapsedDamageTime;
			base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = this.tenPercentDamage,
				source = BUILDINGS.DAMAGESOURCES.MINION_DESTRUCTION,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.MINION_DESTRUCTION
			});
		}
		this.elapsedDamageTime += dt;
		return this.hp.HitPoints <= 0;
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x00085590 File Offset: 0x00083790
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AngerDamage, false);
		base.gameObject.AddOrGet<Notifier>().Remove(this.notification);
		if (worker != null)
		{
			worker.Trigger(-1734580852, null);
		}
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x000855EB File Offset: 0x000837EB
	public override bool InstantlyFinish(Worker worker)
	{
		return false;
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x000855EE File Offset: 0x000837EE
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Breakables.Remove(this);
	}

	// Token: 0x04000E13 RID: 3603
	private const float TIME_TO_BREAK_AT_FULL_HEALTH = 20f;

	// Token: 0x04000E14 RID: 3604
	private Notification notification;

	// Token: 0x04000E15 RID: 3605
	private float secondsPerTenPercentDamage = float.PositiveInfinity;

	// Token: 0x04000E16 RID: 3606
	private float elapsedDamageTime;

	// Token: 0x04000E17 RID: 3607
	private int tenPercentDamage = int.MaxValue;

	// Token: 0x04000E18 RID: 3608
	[MyCmpGet]
	private BuildingHP hp;
}
