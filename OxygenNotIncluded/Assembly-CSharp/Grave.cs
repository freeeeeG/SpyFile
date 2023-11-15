using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000611 RID: 1553
public class Grave : StateMachineComponent<Grave.StatesInstance>
{
	// Token: 0x06002714 RID: 10004 RVA: 0x000D42DE File Offset: 0x000D24DE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Grave>(-1697596308, Grave.OnStorageChangedDelegate);
		this.epitaphIdx = UnityEngine.Random.Range(0, int.MaxValue);
	}

	// Token: 0x06002715 RID: 10005 RVA: 0x000D4308 File Offset: 0x000D2508
	protected override void OnSpawn()
	{
		base.GetComponent<Storage>().SetOffsets(Grave.DELIVERY_OFFSETS);
		Storage component = base.GetComponent<Storage>();
		Storage storage = component;
		storage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(storage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		KAnimFile anim = Assets.GetAnim("anim_bury_dupe_kanim");
		int num = 0;
		KAnim.Anim anim2;
		for (;;)
		{
			anim2 = anim.GetData().GetAnim(num);
			if (anim2 == null)
			{
				goto IL_8F;
			}
			if (anim2.name == "working_pre")
			{
				break;
			}
			num++;
		}
		float workTime = (float)(anim2.numFrames - 3) / anim2.frameRate;
		component.SetWorkTime(workTime);
		IL_8F:
		base.OnSpawn();
		base.smi.StartSM();
		Components.Graves.Add(this);
	}

	// Token: 0x06002716 RID: 10006 RVA: 0x000D43C0 File Offset: 0x000D25C0
	protected override void OnCleanUp()
	{
		Components.Graves.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002717 RID: 10007 RVA: 0x000D43D4 File Offset: 0x000D25D4
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.graveName = gameObject.name;
			Util.KDestroyGameObject(gameObject);
		}
	}

	// Token: 0x06002718 RID: 10008 RVA: 0x000D4403 File Offset: 0x000D2603
	private void OnWorkEvent(Workable workable, Workable.WorkableEvent evt)
	{
	}

	// Token: 0x0400166A RID: 5738
	[Serialize]
	public string graveName;

	// Token: 0x0400166B RID: 5739
	[Serialize]
	public int epitaphIdx;

	// Token: 0x0400166C RID: 5740
	[Serialize]
	public float burialTime = -1f;

	// Token: 0x0400166D RID: 5741
	private static readonly CellOffset[] DELIVERY_OFFSETS = new CellOffset[1];

	// Token: 0x0400166E RID: 5742
	private static readonly EventSystem.IntraObjectHandler<Grave> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Grave>(delegate(Grave component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x020012C5 RID: 4805
	public class StatesInstance : GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.GameInstance
	{
		// Token: 0x06007E76 RID: 32374 RVA: 0x002E874D File Offset: 0x002E694D
		public StatesInstance(Grave master) : base(master)
		{
		}

		// Token: 0x06007E77 RID: 32375 RVA: 0x002E8758 File Offset: 0x002E6958
		public void CreateFetchTask()
		{
			this.chore = new FetchChore(Db.Get().ChoreTypes.FetchCritical, base.GetComponent<Storage>(), 1f, new HashSet<Tag>
			{
				GameTags.Minion
			}, FetchChore.MatchCriteria.MatchID, GameTags.Corpse, null, null, true, null, null, null, Operational.State.Operational, 0);
			this.chore.allowMultifetch = false;
		}

		// Token: 0x06007E78 RID: 32376 RVA: 0x002E87B5 File Offset: 0x002E69B5
		public void CancelFetchTask()
		{
			this.chore.Cancel("Exit State");
			this.chore = null;
		}

		// Token: 0x040060AE RID: 24750
		private FetchChore chore;
	}

	// Token: 0x020012C6 RID: 4806
	public class States : GameStateMachine<Grave.States, Grave.StatesInstance, Grave>
	{
		// Token: 0x06007E79 RID: 32377 RVA: 0x002E87D0 File Offset: 0x002E69D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.PlayAnim("open").Enter("CreateFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CreateFetchTask();
			}).Exit("CancelFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CancelFetchTask();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GraveEmpty, null).EventTransition(GameHashes.OnStorageChange, this.full, null);
			this.full.PlayAnim("closed").ToggleMainStatusItem(Db.Get().BuildingStatusItems.Grave, null).Enter(delegate(Grave.StatesInstance smi)
			{
				if (smi.master.burialTime < 0f)
				{
					smi.master.burialTime = GameClock.Instance.GetTime();
				}
			});
		}

		// Token: 0x040060AF RID: 24751
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State empty;

		// Token: 0x040060B0 RID: 24752
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State full;
	}
}
