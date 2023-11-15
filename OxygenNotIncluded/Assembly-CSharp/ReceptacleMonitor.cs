using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000732 RID: 1842
[SkipSaveFileSerialization]
public class ReceptacleMonitor : StateMachineComponent<ReceptacleMonitor.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
	// Token: 0x1700038D RID: 909
	// (get) Token: 0x0600329F RID: 12959 RVA: 0x0010CEF7 File Offset: 0x0010B0F7
	public bool Replanted
	{
		get
		{
			return this.replanted;
		}
	}

	// Token: 0x060032A0 RID: 12960 RVA: 0x0010CEFF File Offset: 0x0010B0FF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060032A1 RID: 12961 RVA: 0x0010CF12 File Offset: 0x0010B112
	public PlantablePlot GetReceptacle()
	{
		return (PlantablePlot)base.smi.sm.receptacle.Get(base.smi);
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x0010CF34 File Offset: 0x0010B134
	public void SetReceptacle(PlantablePlot plot = null)
	{
		if (plot == null)
		{
			base.smi.sm.receptacle.Set(null, base.smi, false);
			this.replanted = false;
			return;
		}
		base.smi.sm.receptacle.Set(plot, base.smi, false);
		this.replanted = true;
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x0010CF98 File Offset: 0x0010B198
	public void Sim1000ms(float dt)
	{
		if (base.smi.sm.receptacle.Get(base.smi) == null)
		{
			base.smi.GoTo(base.smi.sm.wild);
			return;
		}
		Operational component = base.smi.sm.receptacle.Get(base.smi).GetComponent<Operational>();
		if (component == null)
		{
			base.smi.GoTo(base.smi.sm.operational);
			return;
		}
		if (component.IsOperational)
		{
			base.smi.GoTo(base.smi.sm.operational);
			return;
		}
		base.smi.GoTo(base.smi.sm.inoperational);
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x060032A4 RID: 12964 RVA: 0x0010D069 File Offset: 0x0010B269
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Receptacle
			};
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x060032A5 RID: 12965 RVA: 0x0010D078 File Offset: 0x0010B278
	public string WiltStateString
	{
		get
		{
			string text = "";
			if (base.smi.IsInsideState(base.smi.sm.inoperational))
			{
				text += CREATURES.STATUSITEMS.RECEPTACLEINOPERATIONAL.NAME;
			}
			return text;
		}
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x0010D0BA File Offset: 0x0010B2BA
	public bool HasReceptacle()
	{
		return !base.smi.IsInsideState(base.smi.sm.wild);
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x0010D0DA File Offset: 0x0010B2DA
	public bool HasOperationalReceptacle()
	{
		return base.smi.IsInsideState(base.smi.sm.operational);
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x0010D0F7 File Offset: 0x0010B2F7
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_RECEPTACLE, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RECEPTACLE, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04001E6A RID: 7786
	private bool replanted;

	// Token: 0x020014C0 RID: 5312
	public class StatesInstance : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.GameInstance
	{
		// Token: 0x060085CB RID: 34251 RVA: 0x003074F6 File Offset: 0x003056F6
		public StatesInstance(ReceptacleMonitor master) : base(master)
		{
		}
	}

	// Token: 0x020014C1 RID: 5313
	public class States : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor>
	{
		// Token: 0x060085CC RID: 34252 RVA: 0x00307500 File Offset: 0x00305700
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.wild;
			base.serializable = StateMachine.SerializeType.Never;
			this.wild.TriggerOnEnter(GameHashes.ReceptacleOperational, null);
			this.inoperational.TriggerOnEnter(GameHashes.ReceptacleInoperational, null);
			this.operational.TriggerOnEnter(GameHashes.ReceptacleOperational, null);
		}

		// Token: 0x04006658 RID: 26200
		public StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.ObjectParameter<SingleEntityReceptacle> receptacle;

		// Token: 0x04006659 RID: 26201
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State wild;

		// Token: 0x0400665A RID: 26202
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State inoperational;

		// Token: 0x0400665B RID: 26203
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State operational;
	}
}
