using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008DC RID: 2268
public class PlantElementEmitter : StateMachineComponent<PlantElementEmitter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600418E RID: 16782 RVA: 0x0016F0DF File Offset: 0x0016D2DF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600418F RID: 16783 RVA: 0x0016F0F2 File Offset: 0x0016D2F2
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x04002AAE RID: 10926
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x04002AAF RID: 10927
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002AB0 RID: 10928
	public SimHashes emittedElement;

	// Token: 0x04002AB1 RID: 10929
	public float emitRate;

	// Token: 0x02001717 RID: 5911
	public class StatesInstance : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.GameInstance
	{
		// Token: 0x06008D6A RID: 36202 RVA: 0x0031B913 File Offset: 0x00319B13
		public StatesInstance(PlantElementEmitter master) : base(master)
		{
		}

		// Token: 0x06008D6B RID: 36203 RVA: 0x0031B91C File Offset: 0x00319B1C
		public bool IsWilting()
		{
			return !(base.master.wiltCondition == null) && base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}
	}

	// Token: 0x02001718 RID: 5912
	public class States : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter>
	{
		// Token: 0x06008D6C RID: 36204 RVA: 0x0031B958 File Offset: 0x00319B58
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.healthy;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.healthy.EventTransition(GameHashes.Wilt, this.wilted, (PlantElementEmitter.StatesInstance smi) => smi.IsWilting()).Update("PlantEmit", delegate(PlantElementEmitter.StatesInstance smi, float dt)
			{
				SimMessages.EmitMass(Grid.PosToCell(smi.master.gameObject), ElementLoader.FindElementByHash(smi.master.emittedElement).idx, smi.master.emitRate * dt, ElementLoader.FindElementByHash(smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
			}, UpdateRate.SIM_4000ms, false);
			this.wilted.EventTransition(GameHashes.WiltRecover, this.healthy, null);
		}

		// Token: 0x04006DAD RID: 28077
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State wilted;

		// Token: 0x04006DAE RID: 28078
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State healthy;
	}
}
