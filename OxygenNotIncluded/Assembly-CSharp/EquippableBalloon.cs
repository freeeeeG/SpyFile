using System;
using Database;
using KSerialization;
using TUNING;

// Token: 0x020007A1 RID: 1953
public class EquippableBalloon : StateMachineComponent<EquippableBalloon.StatesInstance>
{
	// Token: 0x06003646 RID: 13894 RVA: 0x00125673 File Offset: 0x00123873
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x00125696 File Offset: 0x00123896
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x06003648 RID: 13896 RVA: 0x001256AF File Offset: 0x001238AF
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003649 RID: 13897 RVA: 0x001256B7 File Offset: 0x001238B7
	public void SetBalloonOverride(BalloonOverrideSymbol balloonOverride)
	{
		base.smi.facadeAnim = balloonOverride.animFileID;
		base.smi.symbolID = balloonOverride.animFileSymbolID;
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x0600364A RID: 13898 RVA: 0x001256E4 File Offset: 0x001238E4
	public void ApplyBalloonOverrideToBalloonFx()
	{
		Equippable component = base.GetComponent<Equippable>();
		if (!component.IsNullOrDestroyed() && !component.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = component.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			BalloonFX.Instance smi = ((KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target).GetSMI<BalloonFX.Instance>();
			if (!smi.IsNullOrDestroyed())
			{
				new BalloonOverrideSymbol(base.smi.facadeAnim, base.smi.symbolID).ApplyTo(smi);
			}
		}
	}

	// Token: 0x0200151E RID: 5406
	public class StatesInstance : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.GameInstance
	{
		// Token: 0x060086E6 RID: 34534 RVA: 0x0030988A File Offset: 0x00307A8A
		public StatesInstance(EquippableBalloon master) : base(master)
		{
		}

		// Token: 0x04006756 RID: 26454
		[Serialize]
		public float transitionTime;

		// Token: 0x04006757 RID: 26455
		[Serialize]
		public string facadeAnim;

		// Token: 0x04006758 RID: 26456
		[Serialize]
		public string symbolID;
	}

	// Token: 0x0200151F RID: 5407
	public class States : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon>
	{
		// Token: 0x060086E7 RID: 34535 RVA: 0x00309894 File Offset: 0x00307A94
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Transition(this.destroy, (EquippableBalloon.StatesInstance smi) => GameClock.Instance.GetTime() >= smi.transitionTime, UpdateRate.SIM_200ms);
			this.destroy.Enter(delegate(EquippableBalloon.StatesInstance smi)
			{
				smi.master.GetComponent<Equippable>().Unassign();
			});
		}

		// Token: 0x04006759 RID: 26457
		public GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.State destroy;
	}
}
