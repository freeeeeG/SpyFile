using System;
using UnityEngine;

// Token: 0x0200071C RID: 1820
public class ElementDropperMonitor : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>
{
	// Token: 0x060031FB RID: 12795 RVA: 0x00109708 File Offset: 0x00107908
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.DeathAnimComplete, delegate(ElementDropperMonitor.Instance smi)
		{
			smi.DropDeathElement();
		});
		this.satisfied.OnSignal(this.cellChangedSignal, this.readytodrop, (ElementDropperMonitor.Instance smi) => smi.ShouldDropElement());
		this.readytodrop.ToggleBehaviour(GameTags.Creatures.WantsToDropElements, (ElementDropperMonitor.Instance smi) => true, delegate(ElementDropperMonitor.Instance smi)
		{
			smi.GoTo(this.satisfied);
		}).EventHandler(GameHashes.ObjectMovementStateChanged, delegate(ElementDropperMonitor.Instance smi, object d)
		{
			if ((GameHashes)d == GameHashes.ObjectMovementWakeUp)
			{
				smi.GoTo(this.satisfied);
			}
		});
	}

	// Token: 0x04001DF4 RID: 7668
	public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State satisfied;

	// Token: 0x04001DF5 RID: 7669
	public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State readytodrop;

	// Token: 0x04001DF6 RID: 7670
	public StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.Signal cellChangedSignal;

	// Token: 0x0200148A RID: 5258
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040065AE RID: 26030
		public SimHashes dirtyEmitElement;

		// Token: 0x040065AF RID: 26031
		public float dirtyProbabilityPercent;

		// Token: 0x040065B0 RID: 26032
		public float dirtyCellToTargetMass;

		// Token: 0x040065B1 RID: 26033
		public float dirtyMassPerDirty;

		// Token: 0x040065B2 RID: 26034
		public float dirtyMassReleaseOnDeath;

		// Token: 0x040065B3 RID: 26035
		public byte emitDiseaseIdx = byte.MaxValue;

		// Token: 0x040065B4 RID: 26036
		public float emitDiseasePerKg;
	}

	// Token: 0x0200148B RID: 5259
	public new class Instance : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.GameInstance
	{
		// Token: 0x060084FF RID: 34047 RVA: 0x0030403E File Offset: 0x0030223E
		public Instance(IStateMachineTarget master, ElementDropperMonitor.Def def) : base(master, def)
		{
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "ElementDropperMonitor.Instance");
		}

		// Token: 0x06008500 RID: 34048 RVA: 0x0030406A File Offset: 0x0030226A
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		}

		// Token: 0x06008501 RID: 34049 RVA: 0x0030408F File Offset: 0x0030228F
		private void OnCellChange()
		{
			base.sm.cellChangedSignal.Trigger(this);
		}

		// Token: 0x06008502 RID: 34050 RVA: 0x003040A2 File Offset: 0x003022A2
		public bool ShouldDropElement()
		{
			return this.IsValidDropCell() && UnityEngine.Random.Range(0f, 100f) < base.def.dirtyProbabilityPercent;
		}

		// Token: 0x06008503 RID: 34051 RVA: 0x003040CC File Offset: 0x003022CC
		public void DropDeathElement()
		{
			this.DropElement(base.def.dirtyMassReleaseOnDeath, base.def.dirtyEmitElement, base.def.emitDiseaseIdx, Mathf.RoundToInt(base.def.dirtyMassReleaseOnDeath * base.def.dirtyMassPerDirty));
		}

		// Token: 0x06008504 RID: 34052 RVA: 0x0030411C File Offset: 0x0030231C
		public void DropPeriodicElement()
		{
			this.DropElement(base.def.dirtyMassPerDirty, base.def.dirtyEmitElement, base.def.emitDiseaseIdx, Mathf.RoundToInt(base.def.emitDiseasePerKg * base.def.dirtyMassPerDirty));
		}

		// Token: 0x06008505 RID: 34053 RVA: 0x0030416C File Offset: 0x0030236C
		public void DropElement(float mass, SimHashes element_id, byte disease_idx, int disease_count)
		{
			if (mass <= 0f)
			{
				return;
			}
			Element element = ElementLoader.FindElementByHash(element_id);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			if (element.IsGas || element.IsLiquid)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), element_id, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, disease_idx, disease_count, true, -1);
			}
			else if (element.IsSolid)
			{
				element.substance.SpawnResource(base.transform.GetPosition() + new Vector3(0f, 0.5f, 0f), mass, temperature, disease_idx, disease_count, false, true, false);
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, base.gameObject.transform, 1.5f, false);
		}

		// Token: 0x06008506 RID: 34054 RVA: 0x0030423C File Offset: 0x0030243C
		public bool IsValidDropCell()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			return Grid.IsValidCell(num) && Grid.IsGas(num) && Grid.Mass[num] <= 1f;
		}
	}
}
