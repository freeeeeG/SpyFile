using System;
using KSerialization;

// Token: 0x020008BE RID: 2238
public class NuclearWaste : GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>
{
	// Token: 0x060040D0 RID: 16592 RVA: 0x0016A120 File Offset: 0x00168320
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.PlayAnim((NuclearWaste.Instance smi) => smi.GetAnimToPlay(), KAnim.PlayMode.Once).Update(delegate(NuclearWaste.Instance smi, float dt)
		{
			smi.timeAlive += dt;
			string animToPlay = smi.GetAnimToPlay();
			if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
			{
				smi.Play(animToPlay, KAnim.PlayMode.Once);
			}
			if (smi.timeAlive >= 600f)
			{
				smi.GoTo(this.decayed);
			}
		}, UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.Absorb, delegate(NuclearWaste.Instance smi, object otherObject)
		{
			Pickupable pickupable = (Pickupable)otherObject;
			float timeAlive = pickupable.GetSMI<NuclearWaste.Instance>().timeAlive;
			float mass = pickupable.GetComponent<PrimaryElement>().Mass;
			float mass2 = smi.master.GetComponent<PrimaryElement>().Mass;
			float timeAlive2 = ((mass2 - mass) * smi.timeAlive + mass * timeAlive) / mass2;
			smi.timeAlive = timeAlive2;
			string animToPlay = smi.GetAnimToPlay();
			if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
			{
				smi.Play(animToPlay, KAnim.PlayMode.Once);
			}
			if (smi.timeAlive >= 600f)
			{
				smi.GoTo(this.decayed);
			}
		});
		this.decayed.Enter(delegate(NuclearWaste.Instance smi)
		{
			smi.GetComponent<Dumpable>().Dump();
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x04002A35 RID: 10805
	private const float lifetime = 600f;

	// Token: 0x04002A36 RID: 10806
	public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State idle;

	// Token: 0x04002A37 RID: 10807
	public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State decayed;

	// Token: 0x02001706 RID: 5894
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001707 RID: 5895
	public new class Instance : GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.GameInstance
	{
		// Token: 0x06008D45 RID: 36165 RVA: 0x0031B42D File Offset: 0x0031962D
		public Instance(IStateMachineTarget master, NuclearWaste.Def def) : base(master, def)
		{
		}

		// Token: 0x06008D46 RID: 36166 RVA: 0x0031B438 File Offset: 0x00319638
		public string GetAnimToPlay()
		{
			this.percentageRemaining = 1f - base.smi.timeAlive / 600f;
			if (this.percentageRemaining <= 0.33f)
			{
				return "idle1";
			}
			if (this.percentageRemaining <= 0.66f)
			{
				return "idle2";
			}
			return "idle3";
		}

		// Token: 0x04006D8F RID: 28047
		[Serialize]
		public float timeAlive;

		// Token: 0x04006D90 RID: 28048
		private float percentageRemaining;
	}
}
