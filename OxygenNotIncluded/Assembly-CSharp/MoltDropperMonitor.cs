using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200072A RID: 1834
public class MoltDropperMonitor : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>
{
	// Token: 0x06003271 RID: 12913 RVA: 0x0010BDC0 File Offset: 0x00109FC0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, delegate(MoltDropperMonitor.Instance smi)
		{
			smi.spawnedThisCycle = false;
		});
		this.satisfied.OnSignal(this.cellChangedSignal, this.drop, (MoltDropperMonitor.Instance smi) => smi.ShouldDropElement());
		this.drop.Enter(delegate(MoltDropperMonitor.Instance smi)
		{
			smi.Drop();
		}).EventTransition(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, this.satisfied, null);
	}

	// Token: 0x04001E4A RID: 7754
	public StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter droppedThisCycle = new StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter(false);

	// Token: 0x04001E4B RID: 7755
	public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State satisfied;

	// Token: 0x04001E4C RID: 7756
	public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State drop;

	// Token: 0x04001E4D RID: 7757
	public StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.Signal cellChangedSignal;

	// Token: 0x020014B2 RID: 5298
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006629 RID: 26153
		public string onGrowDropID;

		// Token: 0x0400662A RID: 26154
		public float massToDrop;

		// Token: 0x0400662B RID: 26155
		public SimHashes blockedElement;
	}

	// Token: 0x020014B3 RID: 5299
	public new class Instance : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.GameInstance
	{
		// Token: 0x0600859F RID: 34207 RVA: 0x003068A8 File Offset: 0x00304AA8
		public Instance(IStateMachineTarget master, MoltDropperMonitor.Def def) : base(master, def)
		{
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "ElementDropperMonitor.Instance");
		}

		// Token: 0x060085A0 RID: 34208 RVA: 0x003068D4 File Offset: 0x00304AD4
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		}

		// Token: 0x060085A1 RID: 34209 RVA: 0x003068F9 File Offset: 0x00304AF9
		private void OnCellChange()
		{
			base.sm.cellChangedSignal.Trigger(this);
		}

		// Token: 0x060085A2 RID: 34210 RVA: 0x0030690C File Offset: 0x00304B0C
		public bool ShouldDropElement()
		{
			return this.IsValidTimeToDrop() && !base.smi.HasTag(GameTags.Creatures.Hungry) && !base.smi.HasTag(GameTags.Creatures.Unhappy) && this.IsValidDropCell();
		}

		// Token: 0x060085A3 RID: 34211 RVA: 0x00306944 File Offset: 0x00304B44
		public void Drop()
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, base.def.onGrowDropID, Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			gameObject.GetComponent<PrimaryElement>().Mass = base.def.massToDrop;
			this.spawnedThisCycle = true;
			this.timeOfLastDrop = GameClock.Instance.GetTime();
		}

		// Token: 0x060085A4 RID: 34212 RVA: 0x003069A0 File Offset: 0x00304BA0
		private int GetDropSpawnLocation()
		{
			int num = Grid.PosToCell(base.gameObject);
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			return num;
		}

		// Token: 0x060085A5 RID: 34213 RVA: 0x003069D8 File Offset: 0x00304BD8
		public bool IsValidTimeToDrop()
		{
			return !this.spawnedThisCycle && (this.timeOfLastDrop <= 0f || GameClock.Instance.GetTime() - this.timeOfLastDrop > 600f);
		}

		// Token: 0x060085A6 RID: 34214 RVA: 0x00306A0C File Offset: 0x00304C0C
		public bool IsValidDropCell()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			return Grid.IsValidCell(num) && Grid.Element[num].id != base.def.blockedElement;
		}

		// Token: 0x0400662C RID: 26156
		[Serialize]
		public bool spawnedThisCycle;

		// Token: 0x0400662D RID: 26157
		[Serialize]
		public float timeOfLastDrop;
	}
}
