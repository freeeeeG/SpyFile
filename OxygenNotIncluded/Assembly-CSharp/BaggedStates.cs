using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class BaggedStates : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>
{
	// Token: 0x060002EC RID: 748 RVA: 0x000177D8 File Offset: 0x000159D8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.bagged;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.BAGGED.NAME, CREATURES.STATUSITEMS.BAGGED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.bagged.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagStart)).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim(new Func<BaggedStates.Instance, string>(BaggedStates.GetBaggedAnimName), KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Bagged, null, true).Transition(this.escape, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.Transition.ConditionCallback(BaggedStates.ShouldEscape), UpdateRate.SIM_4000ms).EventHandler(GameHashes.OnStore, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.OnStore)).Exit(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagEnd));
		this.escape.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.Unbag)).PlayAnim("escape").OnAnimQueueComplete(null);
	}

	// Token: 0x060002ED RID: 749 RVA: 0x000178E2 File Offset: 0x00015AE2
	public static string GetBaggedAnimName(BaggedStates.Instance smi)
	{
		return Baggable.GetBaggedAnimName(smi.gameObject);
	}

	// Token: 0x060002EE RID: 750 RVA: 0x000178EF File Offset: 0x00015AEF
	private static void BagStart(BaggedStates.Instance smi)
	{
		if (smi.baggedTime == 0f)
		{
			smi.baggedTime = GameClock.Instance.GetTime();
		}
		smi.UpdateFaller(true);
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00017915 File Offset: 0x00015B15
	private static void BagEnd(BaggedStates.Instance smi)
	{
		smi.baggedTime = 0f;
		smi.UpdateFaller(false);
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0001792C File Offset: 0x00015B2C
	private static void Unbag(BaggedStates.Instance smi)
	{
		Baggable component = smi.gameObject.GetComponent<Baggable>();
		if (component)
		{
			component.Free();
		}
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00017953 File Offset: 0x00015B53
	private static void OnStore(BaggedStates.Instance smi)
	{
		smi.UpdateFaller(true);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0001795C File Offset: 0x00015B5C
	private static bool ShouldEscape(BaggedStates.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Stored) && GameClock.Instance.GetTime() - smi.baggedTime >= smi.def.escapeTime;
	}

	// Token: 0x04000207 RID: 519
	public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State bagged;

	// Token: 0x04000208 RID: 520
	public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State escape;

	// Token: 0x02000E70 RID: 3696
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400538E RID: 21390
		public float escapeTime = 300f;
	}

	// Token: 0x02000E71 RID: 3697
	public new class Instance : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.GameInstance
	{
		// Token: 0x06006F88 RID: 28552 RVA: 0x002B96A3 File Offset: 0x002B78A3
		public Instance(Chore<BaggedStates.Instance> chore, BaggedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(BaggedStates.Instance.IsBagged, null);
		}

		// Token: 0x06006F89 RID: 28553 RVA: 0x002B96BC File Offset: 0x002B78BC
		public void UpdateFaller(bool bagged)
		{
			bool flag = bagged && !base.gameObject.HasTag(GameTags.Stored);
			bool flag2 = GameComps.Fallers.Has(base.gameObject);
			if (flag != flag2)
			{
				if (flag)
				{
					GameComps.Fallers.Add(base.gameObject, Vector2.zero);
					return;
				}
				GameComps.Fallers.Remove(base.gameObject);
			}
		}

		// Token: 0x0400538F RID: 21391
		[Serialize]
		public float baggedTime;

		// Token: 0x04005390 RID: 21392
		public static readonly Chore.Precondition IsBagged = new Chore.Precondition
		{
			id = "IsBagged",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.Bagged);
			}
		};
	}
}
