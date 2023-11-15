using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A21 RID: 2593
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Vent")]
public class Vent : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06004DAE RID: 19886 RVA: 0x001B39F3 File Offset: 0x001B1BF3
	// (set) Token: 0x06004DAF RID: 19887 RVA: 0x001B39FB File Offset: 0x001B1BFB
	public int SortKey
	{
		get
		{
			return this.sortKey;
		}
		set
		{
			this.sortKey = value;
		}
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x001B3A04 File Offset: 0x001B1C04
	public void UpdateVentedMass(SimHashes element, float mass)
	{
		if (!this.lifeTimeVentMass.ContainsKey(element))
		{
			this.lifeTimeVentMass.Add(element, mass);
			return;
		}
		Dictionary<SimHashes, float> dictionary = this.lifeTimeVentMass;
		dictionary[element] += mass;
	}

	// Token: 0x06004DB1 RID: 19889 RVA: 0x001B3A46 File Offset: 0x001B1C46
	public float GetVentedMass(SimHashes element)
	{
		if (this.lifeTimeVentMass.ContainsKey(element))
		{
			return this.lifeTimeVentMass[element];
		}
		return 0f;
	}

	// Token: 0x06004DB2 RID: 19890 RVA: 0x001B3A68 File Offset: 0x001B1C68
	public bool Closed()
	{
		bool flag = false;
		return (this.operational.Flags.TryGetValue(LogicOperationalController.LogicOperationalFlag, out flag) && !flag) || (this.operational.Flags.TryGetValue(BuildingEnabledButton.EnabledFlag, out flag) && !flag);
	}

	// Token: 0x06004DB3 RID: 19891 RVA: 0x001B3AB4 File Offset: 0x001B1CB4
	protected override void OnSpawn()
	{
		Building component = base.GetComponent<Building>();
		this.cell = component.GetUtilityOutputCell();
		this.smi = new Vent.StatesInstance(this);
		this.smi.StartSM();
	}

	// Token: 0x06004DB4 RID: 19892 RVA: 0x001B3AEC File Offset: 0x001B1CEC
	public Vent.State GetEndPointState()
	{
		Vent.State result = Vent.State.Invalid;
		Endpoint endpoint = this.endpointType;
		if (endpoint != Endpoint.Source)
		{
			if (endpoint == Endpoint.Sink)
			{
				result = Vent.State.Ready;
				int num = this.cell;
				if (!this.IsValidOutputCell(num))
				{
					result = (Grid.Solid[num] ? Vent.State.Blocked : Vent.State.OverPressure);
				}
			}
		}
		else
		{
			result = (this.IsConnected() ? Vent.State.Ready : Vent.State.Blocked);
		}
		return result;
	}

	// Token: 0x06004DB5 RID: 19893 RVA: 0x001B3B40 File Offset: 0x001B1D40
	public bool IsConnected()
	{
		UtilityNetwork networkForCell = Conduit.GetNetworkManager(this.conduitType).GetNetworkForCell(this.cell);
		return networkForCell != null && (networkForCell as FlowUtilityNetwork).HasSinks;
	}

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x001B3B74 File Offset: 0x001B1D74
	public bool IsBlocked
	{
		get
		{
			return this.GetEndPointState() != Vent.State.Ready;
		}
	}

	// Token: 0x06004DB7 RID: 19895 RVA: 0x001B3B84 File Offset: 0x001B1D84
	private bool IsValidOutputCell(int output_cell)
	{
		bool result = false;
		if ((this.structure == null || !this.structure.IsEntombed() || !this.Closed()) && !Grid.Solid[output_cell])
		{
			result = (Grid.Mass[output_cell] < this.overpressureMass);
		}
		return result;
	}

	// Token: 0x06004DB8 RID: 19896 RVA: 0x001B3BD8 File Offset: 0x001B1DD8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		string formattedMass = GameUtil.GetFormattedMass(this.overpressureMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVER_PRESSURE_MASS, formattedMass), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.OVER_PRESSURE_MASS, formattedMass), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04003293 RID: 12947
	private int cell = -1;

	// Token: 0x04003294 RID: 12948
	private int sortKey;

	// Token: 0x04003295 RID: 12949
	[Serialize]
	public Dictionary<SimHashes, float> lifeTimeVentMass = new Dictionary<SimHashes, float>();

	// Token: 0x04003296 RID: 12950
	private Vent.StatesInstance smi;

	// Token: 0x04003297 RID: 12951
	[SerializeField]
	public ConduitType conduitType = ConduitType.Gas;

	// Token: 0x04003298 RID: 12952
	[SerializeField]
	public Endpoint endpointType;

	// Token: 0x04003299 RID: 12953
	[SerializeField]
	public float overpressureMass = 1f;

	// Token: 0x0400329A RID: 12954
	[NonSerialized]
	public bool showConnectivityIcons = true;

	// Token: 0x0400329B RID: 12955
	[MyCmpGet]
	[NonSerialized]
	public Structure structure;

	// Token: 0x0400329C RID: 12956
	[MyCmpGet]
	[NonSerialized]
	public Operational operational;

	// Token: 0x020018A6 RID: 6310
	public enum State
	{
		// Token: 0x04007290 RID: 29328
		Invalid,
		// Token: 0x04007291 RID: 29329
		Ready,
		// Token: 0x04007292 RID: 29330
		Blocked,
		// Token: 0x04007293 RID: 29331
		OverPressure,
		// Token: 0x04007294 RID: 29332
		Closed
	}

	// Token: 0x020018A7 RID: 6311
	public class StatesInstance : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.GameInstance
	{
		// Token: 0x06009259 RID: 37465 RVA: 0x0032BA11 File Offset: 0x00329C11
		public StatesInstance(Vent master) : base(master)
		{
			this.exhaust = master.GetComponent<Exhaust>();
		}

		// Token: 0x0600925A RID: 37466 RVA: 0x0032BA26 File Offset: 0x00329C26
		public bool NeedsExhaust()
		{
			return this.exhaust != null && base.master.GetEndPointState() != Vent.State.Ready && base.master.endpointType == Endpoint.Source;
		}

		// Token: 0x0600925B RID: 37467 RVA: 0x0032BA54 File Offset: 0x00329C54
		public bool Blocked()
		{
			return base.master.GetEndPointState() == Vent.State.Blocked && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x0600925C RID: 37468 RVA: 0x0032BA74 File Offset: 0x00329C74
		public bool OverPressure()
		{
			return this.exhaust != null && base.master.GetEndPointState() == Vent.State.OverPressure && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x0600925D RID: 37469 RVA: 0x0032BAA4 File Offset: 0x00329CA4
		public void CheckTransitions()
		{
			if (this.NeedsExhaust())
			{
				base.smi.GoTo(base.sm.needExhaust);
				return;
			}
			if (base.master.Closed())
			{
				base.smi.GoTo(base.sm.closed);
				return;
			}
			if (this.Blocked())
			{
				base.smi.GoTo(base.sm.open.blocked);
				return;
			}
			if (this.OverPressure())
			{
				base.smi.GoTo(base.sm.open.overPressure);
				return;
			}
			base.smi.GoTo(base.sm.open.idle);
		}

		// Token: 0x0600925E RID: 37470 RVA: 0x0032BB57 File Offset: 0x00329D57
		public StatusItem SelectStatusItem(StatusItem gas_status_item, StatusItem liquid_status_item)
		{
			if (base.master.conduitType != ConduitType.Gas)
			{
				return liquid_status_item;
			}
			return gas_status_item;
		}

		// Token: 0x04007295 RID: 29333
		private Exhaust exhaust;
	}

	// Token: 0x020018A8 RID: 6312
	public class States : GameStateMachine<Vent.States, Vent.StatesInstance, Vent>
	{
		// Token: 0x0600925F RID: 37471 RVA: 0x0032BB6C File Offset: 0x00329D6C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.open.idle;
			this.root.Update("CheckTransitions", delegate(Vent.StatesInstance smi, float dt)
			{
				smi.CheckTransitions();
			}, UpdateRate.SIM_200ms, false);
			this.open.TriggerOnEnter(GameHashes.VentOpen, null);
			this.closed.TriggerOnEnter(GameHashes.VentClosed, null);
			this.open.blocked.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentObstructed, Db.Get().BuildingStatusItems.LiquidVentObstructed), null);
			this.open.overPressure.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, Db.Get().BuildingStatusItems.LiquidVentOverPressure), null);
		}

		// Token: 0x04007296 RID: 29334
		public Vent.States.OpenState open;

		// Token: 0x04007297 RID: 29335
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State closed;

		// Token: 0x04007298 RID: 29336
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State needExhaust;

		// Token: 0x02002203 RID: 8707
		public class OpenState : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State
		{
			// Token: 0x04009840 RID: 38976
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State idle;

			// Token: 0x04009841 RID: 38977
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State blocked;

			// Token: 0x04009842 RID: 38978
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State overPressure;
		}
	}
}
