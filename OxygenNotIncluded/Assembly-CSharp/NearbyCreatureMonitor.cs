using System;
using System.Collections.Generic;

// Token: 0x0200072B RID: 1835
public class NearbyCreatureMonitor : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x06003273 RID: 12915 RVA: 0x0010BECA File Offset: 0x0010A0CA
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateNearbyCreatures", delegate(NearbyCreatureMonitor.Instance smi, float dt)
		{
			smi.UpdateNearbyCreatures(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x020014B5 RID: 5301
	public new class Instance : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000032 RID: 50
		// (add) Token: 0x060085AE RID: 34222 RVA: 0x00306A8C File Offset: 0x00304C8C
		// (remove) Token: 0x060085AF RID: 34223 RVA: 0x00306AC4 File Offset: 0x00304CC4
		public event Action<float, List<KPrefabID>, List<KPrefabID>> OnUpdateNearbyCreatures;

		// Token: 0x060085B0 RID: 34224 RVA: 0x00306AF9 File Offset: 0x00304CF9
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060085B1 RID: 34225 RVA: 0x00306B04 File Offset: 0x00304D04
		public void UpdateNearbyCreatures(float dt)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
			if (cavityForCell != null)
			{
				this.OnUpdateNearbyCreatures(dt, cavityForCell.creatures, cavityForCell.eggs);
			}
		}
	}
}
