using System;
using System.Collections.Generic;
using Klei.AI;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x020003A0 RID: 928
public class MinionBrain : Brain
{
	// Token: 0x06001369 RID: 4969 RVA: 0x00065C6C File Offset: 0x00063E6C
	public bool IsCellClear(int cell)
	{
		if (Grid.Reserved[cell])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 0];
		return !(gameObject != null) || !(base.gameObject != gameObject) || gameObject.GetComponent<Navigator>().IsMoving();
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x00065CC2 File Offset: 0x00063EC2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Navigator.SetAbilities(new MinionPathFinderAbilities(this.Navigator));
		base.Subscribe<MinionBrain>(-1697596308, MinionBrain.AnimTrackStoredItemDelegate);
		base.Subscribe<MinionBrain>(-975551167, MinionBrain.OnUnstableGroundImpactDelegate);
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x00065D04 File Offset: 0x00063F04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (GameObject go in base.GetComponent<Storage>().items)
		{
			this.AddAnimTracker(go);
		}
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x00065D80 File Offset: 0x00063F80
	private void AnimTrackStoredItem(object data)
	{
		Storage component = base.GetComponent<Storage>();
		GameObject gameObject = (GameObject)data;
		this.RemoveTracker(gameObject);
		if (component.items.Contains(gameObject))
		{
			this.AddAnimTracker(gameObject);
		}
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x00065DB8 File Offset: 0x00063FB8
	private void AddAnimTracker(GameObject go)
	{
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component == null)
		{
			return;
		}
		if (component.AnimFiles != null && component.AnimFiles.Length != 0 && component.AnimFiles[0] != null && component.GetComponent<Pickupable>().trackOnPickup)
		{
			KBatchedAnimTracker kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.useTargetPoint = false;
			kbatchedAnimTracker.fadeOut = false;
			kbatchedAnimTracker.symbol = new HashedString("snapTo_chest");
			kbatchedAnimTracker.forceAlwaysVisible = true;
		}
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x00065E30 File Offset: 0x00064030
	private void RemoveTracker(GameObject go)
	{
		KBatchedAnimTracker component = go.GetComponent<KBatchedAnimTracker>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x00065E54 File Offset: 0x00064054
	public override void UpdateBrain()
	{
		base.UpdateBrain();
		if (Game.Instance == null)
		{
			return;
		}
		if (!Game.Instance.savedInfo.discoveredSurface)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space)
			{
				Game.Instance.savedInfo.discoveredSurface = true;
				DiscoveredSpaceMessage message = new DiscoveredSpaceMessage(base.gameObject.transform.GetPosition());
				Messenger.Instance.QueueMessage(message);
				Game.Instance.Trigger(-818188514, base.gameObject);
			}
		}
		if (!Game.Instance.savedInfo.discoveredOilField)
		{
			int cell2 = Grid.PosToCell(base.gameObject);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell2) == SubWorld.ZoneType.OilField)
			{
				Game.Instance.savedInfo.discoveredOilField = true;
			}
		}
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x00065F2C File Offset: 0x0006412C
	private void RegisterReactEmotePair(string reactable_id, Emote emote, float max_trigger_time)
	{
		if (base.gameObject == null)
		{
			return;
		}
		ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
		if (smi != null)
		{
			EmoteChore emoteChore = new EmoteChore(base.gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteIdle, emote, 1, null);
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.gameObject, reactable_id, Db.Get().ChoreTypes.Cough, max_trigger_time, 20f, float.PositiveInfinity, 0f);
			emoteChore.PairReactable(selfEmoteReactable);
			selfEmoteReactable.SetEmote(emote);
			selfEmoteReactable.PairEmote(emoteChore);
			smi.AddOneshotReactable(selfEmoteReactable);
		}
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x00065FC8 File Offset: 0x000641C8
	private void OnResearchComplete(object data)
	{
		if (Time.time - this.lastResearchCompleteEmoteTime > 1f)
		{
			this.RegisterReactEmotePair("ResearchComplete", Db.Get().Emotes.Minion.ResearchComplete, 3f);
			this.lastResearchCompleteEmoteTime = Time.time;
		}
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x00066018 File Offset: 0x00064218
	public Notification CreateCollapseNotification()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		return new Notification(MISC.NOTIFICATIONS.TILECOLLAPSE.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.TILECOLLAPSE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x00066078 File Offset: 0x00064278
	public void RemoveCollapseNotification(Notification notification)
	{
		Vector3 position = notification.clickFocus.GetPosition();
		position.z = -40f;
		WorldContainer myWorld = notification.clickFocus.gameObject.GetMyWorld();
		if (myWorld != null && myWorld.IsDiscovered)
		{
			CameraController.Instance.ActiveWorldStarWipe(myWorld.id, position, 10f, null);
		}
		base.gameObject.AddOrGet<Notifier>().Remove(notification);
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x000660E8 File Offset: 0x000642E8
	private void OnUnstableGroundImpact(object data)
	{
		GameObject telepad = GameUtil.GetTelepad(base.gameObject.GetMyWorld().id);
		Navigator component = base.GetComponent<Navigator>();
		Assignable assignable = base.GetComponent<MinionIdentity>().GetSoleOwner().GetAssignable(Db.Get().AssignableSlots.Bed);
		bool flag = assignable != null && component.CanReach(Grid.PosToCell(assignable.transform.GetPosition()));
		bool flag2 = telepad != null && component.CanReach(Grid.PosToCell(telepad.transform.GetPosition()));
		if (!flag && !flag2)
		{
			this.RegisterReactEmotePair("UnstableGroundShock", Db.Get().Emotes.Minion.Shock, 1f);
			Notification notification = this.CreateCollapseNotification();
			notification.customClickCallback = delegate(object o)
			{
				this.RemoveCollapseNotification(notification);
			};
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x000661ED File Offset: 0x000643ED
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x04000A67 RID: 2663
	[MyCmpReq]
	public Navigator Navigator;

	// Token: 0x04000A68 RID: 2664
	[MyCmpGet]
	public OxygenBreather OxygenBreather;

	// Token: 0x04000A69 RID: 2665
	private float lastResearchCompleteEmoteTime;

	// Token: 0x04000A6A RID: 2666
	private static readonly EventSystem.IntraObjectHandler<MinionBrain> AnimTrackStoredItemDelegate = new EventSystem.IntraObjectHandler<MinionBrain>(delegate(MinionBrain component, object data)
	{
		component.AnimTrackStoredItem(data);
	});

	// Token: 0x04000A6B RID: 2667
	private static readonly EventSystem.IntraObjectHandler<MinionBrain> OnUnstableGroundImpactDelegate = new EventSystem.IntraObjectHandler<MinionBrain>(delegate(MinionBrain component, object data)
	{
		component.OnUnstableGroundImpact(data);
	});
}
