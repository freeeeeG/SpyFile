using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020005BE RID: 1470
[AddComponentMenu("KMonoBehaviour/Workable/Bed")]
public class Bed : Workable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06002437 RID: 9271 RVA: 0x000C5822 File Offset: 0x000C3A22
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x000C5834 File Offset: 0x000C3A34
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.BasicBuildings.Add(this);
		this.sleepable = base.GetComponent<Sleepable>();
		Sleepable sleepable = this.sleepable;
		sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06002439 RID: 9273 RVA: 0x000C5885 File Offset: 0x000C3A85
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			this.AddEffects();
			return;
		}
		if (workable_event == Workable.WorkableEvent.WorkStopped)
		{
			this.RemoveEffects();
		}
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x000C589C File Offset: 0x000C3A9C
	private void AddEffects()
	{
		this.targetWorker = this.sleepable.worker;
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Add(effect_id, false);
			}
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject == null)
		{
			return;
		}
		RoomType roomType = roomOfGameObject.roomType;
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			if (keyValuePair.Key == roomType.Id)
			{
				this.targetWorker.GetComponent<Effects>().Add(keyValuePair.Value, false);
			}
		}
		roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), this.targetWorker.GetComponent<Effects>());
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x000C5998 File Offset: 0x000C3B98
	private void RemoveEffects()
	{
		if (this.targetWorker == null)
		{
			return;
		}
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Remove(effect_id);
			}
		}
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			this.targetWorker.GetComponent<Effects>().Remove(keyValuePair.Value);
		}
		this.targetWorker = null;
	}

	// Token: 0x0600243C RID: 9276 RVA: 0x000C5A44 File Offset: 0x000C3C44
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.effects != null)
		{
			foreach (string text in this.effects)
			{
				if (text != null && text != "")
				{
					Effect.AddModifierDescriptions(base.gameObject, list, text, false);
				}
			}
		}
		return list;
	}

	// Token: 0x0600243D RID: 9277 RVA: 0x000C5A98 File Offset: 0x000C3C98
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		if (this.sleepable != null)
		{
			Sleepable sleepable = this.sleepable;
			sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
		}
	}

	// Token: 0x040014C6 RID: 5318
	[MyCmpReq]
	private Sleepable sleepable;

	// Token: 0x040014C7 RID: 5319
	private Worker targetWorker;

	// Token: 0x040014C8 RID: 5320
	public string[] effects;

	// Token: 0x040014C9 RID: 5321
	private static Dictionary<string, string> roomSleepingEffects = new Dictionary<string, string>
	{
		{
			"Barracks",
			"BarracksStamina"
		},
		{
			"Luxury Barracks",
			"BarracksStamina"
		},
		{
			"Private Bedroom",
			"BedroomStamina"
		}
	};
}
