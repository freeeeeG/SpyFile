using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020009AE RID: 2478
[SerializationConfig(MemberSerialization.OptIn)]
public class PodLander : StateMachineComponent<PodLander.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060049CF RID: 18895 RVA: 0x0019FC6A File Offset: 0x0019DE6A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060049D0 RID: 18896 RVA: 0x0019FC80 File Offset: 0x0019DE80
	public void ReleaseAstronaut()
	{
		if (this.releasingAstronaut)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x060049D1 RID: 18897 RVA: 0x0019FCF9 File Offset: 0x0019DEF9
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x0400307A RID: 12410
	[Serialize]
	private int landOffLocation;

	// Token: 0x0400307B RID: 12411
	[Serialize]
	private float flightAnimOffset;

	// Token: 0x0400307C RID: 12412
	private float rocketSpeed;

	// Token: 0x0400307D RID: 12413
	public float exhaustEmitRate = 2f;

	// Token: 0x0400307E RID: 12414
	public float exhaustTemperature = 1000f;

	// Token: 0x0400307F RID: 12415
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x04003080 RID: 12416
	private GameObject soundSpeakerObject;

	// Token: 0x04003081 RID: 12417
	private bool releasingAstronaut;

	// Token: 0x02001832 RID: 6194
	public class StatesInstance : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.GameInstance
	{
		// Token: 0x0600910E RID: 37134 RVA: 0x0032835F File Offset: 0x0032655F
		public StatesInstance(PodLander master) : base(master)
		{
		}
	}

	// Token: 0x02001833 RID: 6195
	public class States : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander>
	{
		// Token: 0x0600910F RID: 37135 RVA: 0x00328368 File Offset: 0x00326568
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.landing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.landing.PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.flightAnimOffset = 50f;
			}).Update(delegate(PodLander.StatesInstance smi, float dt)
			{
				float num = 10f;
				smi.master.rocketSpeed = num - Mathf.Clamp(Mathf.Pow(smi.timeinstate / 3.5f, 4f), 0f, num - 2f);
				smi.master.flightAnimOffset -= dt * smi.master.rocketSpeed;
				KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
				component.Offset = Vector3.up * smi.master.flightAnimOffset;
				Vector3 positionIncludingOffset = component.PositionIncludingOffset;
				int num2 = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num2))
				{
					SimMessages.EmitMass(num2, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				if (component.Offset.y <= 0f)
				{
					smi.GoTo(this.crashed);
				}
			}, UpdateRate.SIM_33ms, false);
			this.crashed.PlayAnim("grounded").Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
				smi.master.rocketSpeed = 0f;
				smi.master.ReleaseAstronaut();
			});
		}

		// Token: 0x0400714F RID: 29007
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State landing;

		// Token: 0x04007150 RID: 29008
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State crashed;
	}
}
