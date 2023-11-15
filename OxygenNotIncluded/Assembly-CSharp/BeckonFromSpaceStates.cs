using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
internal class BeckonFromSpaceStates : GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>
{
	// Token: 0x060002F4 RID: 756 RVA: 0x0001799C File Offset: 0x00015B9C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.beckoning;
		this.beckoning.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Beckoning, null).DefaultState(this.beckoning.pre);
		this.beckoning.pre.PlayAnim("beckoning_pre").OnAnimQueueComplete(this.beckoning.loop);
		this.beckoning.loop.PlayAnim("beckoning_loop").Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.MooEchoFX)).OnAnimQueueComplete(this.beckoning.pst);
		this.beckoning.pst.PlayAnim("beckoning_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.DoBeckon)).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.MooCheer)).BehaviourComplete(GameTags.Creatures.WantsToBeckon, false);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00017A9C File Offset: 0x00015C9C
	private static void MooEchoFX(BeckonFromSpaceStates.Instance smi)
	{
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("moo_call_fx_kanim", smi.master.transform.position, null, false, Grid.SceneLayer.Front, false);
		kbatchedAnimController.destroyOnAnimComplete = true;
		kbatchedAnimController.Play("moo_call", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00017AEC File Offset: 0x00015CEC
	private static void MooCheer(BeckonFromSpaceStates.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		ListPool<ScenePartitionerEntry, BeckonFromSpaceStates>.PooledList pooledList = ListPool<ScenePartitionerEntry, BeckonFromSpaceStates>.Allocate();
		Extents extents = new Extents((int)position.x, (int)position.y, 15);
		GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			KPrefabID kprefabID = (scenePartitionerEntry.obj as Pickupable).KPrefabID;
			if (!(kprefabID.gameObject == smi.gameObject) && kprefabID.HasTag("Moo"))
			{
				kprefabID.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(smi.def.choirAnims);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00017BC8 File Offset: 0x00015DC8
	private static void DoBeckon(BeckonFromSpaceStates.Instance smi)
	{
		Db.Get().Amounts.Beckoning.Lookup(smi.gameObject).value = 0f;
		WorldContainer myWorld = smi.GetMyWorld();
		Vector3 position = smi.transform.position;
		float num = (float)(myWorld.Height + myWorld.WorldOffset.y - 1);
		float layerZ = Grid.GetLayerZ(smi.def.sceneLayer);
		float num2 = (num - position.y) * Mathf.Tan(0.2617994f);
		float num3 = position.x + (float)UnityEngine.Random.Range(-5, 5);
		float num4 = num3 - num2;
		float num5 = num3 + num2;
		float num6 = position.x;
		bool customInitialFlip = false;
		if (num4 > (float)myWorld.WorldOffset.x && num4 < (float)(myWorld.WorldOffset.x + myWorld.Width))
		{
			num6 = num4;
			customInitialFlip = false;
		}
		else if (num4 > (float)myWorld.WorldOffset.x && num4 < (float)(myWorld.WorldOffset.x + myWorld.Width))
		{
			num6 = num5;
			customInitialFlip = true;
		}
		DebugUtil.DevAssert(myWorld.ContainsPoint(new Vector2(num6, num)), "Gassy Moo spawned outside world bounds", null);
		Vector3 position2 = new Vector3(num6, num, layerZ);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(smi.def.prefab), position2, Quaternion.identity, null, null, true, 0);
		GassyMooComet component = gameObject.GetComponent<GassyMooComet>();
		if (component != null)
		{
			component.spawnWithOffset = true;
			if (num6 != position.x)
			{
				component.SetCustomInitialFlip(customInitialFlip);
			}
		}
		gameObject.SetActive(true);
	}

	// Token: 0x04000209 RID: 521
	public BeckonFromSpaceStates.BeckoningState beckoning;

	// Token: 0x0400020A RID: 522
	public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State behaviourcomplete;

	// Token: 0x02000E72 RID: 3698
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005391 RID: 21393
		public string prefab;

		// Token: 0x04005392 RID: 21394
		public Grid.SceneLayer sceneLayer;

		// Token: 0x04005393 RID: 21395
		public HashedString[] choirAnims = new HashedString[]
		{
			"reply_loop"
		};
	}

	// Token: 0x02000E73 RID: 3699
	public new class Instance : GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.GameInstance
	{
		// Token: 0x06006F8C RID: 28556 RVA: 0x002B9787 File Offset: 0x002B7987
		public Instance(Chore<BeckonFromSpaceStates.Instance> chore, BeckonFromSpaceStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToBeckon);
		}
	}

	// Token: 0x02000E74 RID: 3700
	public class BeckoningState : GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State
	{
		// Token: 0x04005394 RID: 21396
		public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State pre;

		// Token: 0x04005395 RID: 21397
		public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State loop;

		// Token: 0x04005396 RID: 21398
		public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State pst;
	}
}
