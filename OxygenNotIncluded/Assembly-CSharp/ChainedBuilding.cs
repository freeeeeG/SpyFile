using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020006CA RID: 1738
public class ChainedBuilding : GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>
{
	// Token: 0x06002F48 RID: 12104 RVA: 0x000F97B8 File Offset: 0x000F79B8
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.unlinked;
		StatusItem statusItem = new StatusItem("NotLinkedToHeadStatusItem", BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.NAME, BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.TOOLTIP, "status_item_not_linked", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
		statusItem.resolveTooltipCallback = delegate(string tooltip, object obj)
		{
			ChainedBuilding.StatesInstance statesInstance = (ChainedBuilding.StatesInstance)obj;
			return tooltip.Replace("{headBuilding}", Strings.Get("STRINGS.BUILDINGS.PREFABS." + statesInstance.def.headBuildingTag.Name.ToUpper() + ".NAME")).Replace("{linkBuilding}", Strings.Get("STRINGS.BUILDINGS.PREFABS." + statesInstance.def.linkBuildingTag.Name.ToUpper() + ".NAME"));
		};
		this.root.OnSignal(this.doRelink, this.DEBUG_relink);
		this.unlinked.ParamTransition<bool>(this.isConnectedToHead, this.linked, GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.IsTrue).ToggleStatusItem(statusItem, (ChainedBuilding.StatesInstance smi) => smi);
		this.linked.ParamTransition<bool>(this.isConnectedToHead, this.unlinked, GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.IsFalse);
		this.DEBUG_relink.Enter(delegate(ChainedBuilding.StatesInstance smi)
		{
			smi.DEBUG_Relink();
		});
	}

	// Token: 0x04001C1A RID: 7194
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State unlinked;

	// Token: 0x04001C1B RID: 7195
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State linked;

	// Token: 0x04001C1C RID: 7196
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State DEBUG_relink;

	// Token: 0x04001C1D RID: 7197
	private StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.BoolParameter isConnectedToHead = new StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.BoolParameter();

	// Token: 0x04001C1E RID: 7198
	private StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.Signal doRelink;

	// Token: 0x02001408 RID: 5128
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400641A RID: 25626
		public Tag headBuildingTag;

		// Token: 0x0400641B RID: 25627
		public Tag linkBuildingTag;

		// Token: 0x0400641C RID: 25628
		public ObjectLayer objectLayer;
	}

	// Token: 0x02001409 RID: 5129
	public class StatesInstance : GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.GameInstance
	{
		// Token: 0x06008329 RID: 33577 RVA: 0x002FC9EC File Offset: 0x002FABEC
		public StatesInstance(IStateMachineTarget master, ChainedBuilding.Def def) : base(master, def)
		{
			BuildingDef def2 = master.GetComponent<Building>().Def;
			this.widthInCells = def2.WidthInCells;
			int cell = Grid.PosToCell(this);
			this.neighbourCheckCells = new List<int>
			{
				Grid.OffsetCell(cell, -(this.widthInCells - 1) / 2 - 1, 0),
				Grid.OffsetCell(cell, this.widthInCells / 2 + 1, 0)
			};
		}

		// Token: 0x0600832A RID: 33578 RVA: 0x002FCA5C File Offset: 0x002FAC5C
		public override void StartSM()
		{
			base.StartSM();
			bool foundHead = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			this.CollectToChain(ref pooledHashSet, ref foundHead, null);
			this.PropogateFoundHead(foundHead, pooledHashSet);
			this.PropagateChangedEvent(this, pooledHashSet);
			pooledHashSet.Recycle();
		}

		// Token: 0x0600832B RID: 33579 RVA: 0x002FCA98 File Offset: 0x002FAC98
		public void DEBUG_Relink()
		{
			bool foundHead = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			this.CollectToChain(ref pooledHashSet, ref foundHead, null);
			this.PropogateFoundHead(foundHead, pooledHashSet);
			pooledHashSet.Recycle();
		}

		// Token: 0x0600832C RID: 33580 RVA: 0x002FCAC8 File Offset: 0x002FACC8
		protected override void OnCleanUp()
		{
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			foreach (int cell in this.neighbourCheckCells)
			{
				bool foundHead = false;
				this.CollectNeighbourToChain(cell, ref pooledHashSet, ref foundHead, this);
				this.PropogateFoundHead(foundHead, pooledHashSet);
				this.PropagateChangedEvent(this, pooledHashSet);
				pooledHashSet.Clear();
			}
			pooledHashSet.Recycle();
			base.OnCleanUp();
		}

		// Token: 0x0600832D RID: 33581 RVA: 0x002FCB4C File Offset: 0x002FAD4C
		public HashSet<ChainedBuilding.StatesInstance> GetLinkedBuildings(ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain)
		{
			bool flag = false;
			this.CollectToChain(ref chain, ref flag, null);
			return chain;
		}

		// Token: 0x0600832E RID: 33582 RVA: 0x002FCB68 File Offset: 0x002FAD68
		private void PropogateFoundHead(bool foundHead, HashSet<ChainedBuilding.StatesInstance> chain)
		{
			foreach (ChainedBuilding.StatesInstance statesInstance in chain)
			{
				statesInstance.sm.isConnectedToHead.Set(foundHead, statesInstance, false);
			}
		}

		// Token: 0x0600832F RID: 33583 RVA: 0x002FCBC4 File Offset: 0x002FADC4
		private void PropagateChangedEvent(ChainedBuilding.StatesInstance changedLink, HashSet<ChainedBuilding.StatesInstance> chain)
		{
			foreach (ChainedBuilding.StatesInstance statesInstance in chain)
			{
				statesInstance.Trigger(-1009905786, changedLink);
			}
		}

		// Token: 0x06008330 RID: 33584 RVA: 0x002FCC18 File Offset: 0x002FAE18
		private void CollectToChain(ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain, ref bool foundHead, ChainedBuilding.StatesInstance ignoredLink = null)
		{
			if (ignoredLink != null && ignoredLink == this)
			{
				return;
			}
			if (chain.Contains(this))
			{
				return;
			}
			chain.Add(this);
			if (base.HasTag(base.def.headBuildingTag))
			{
				foundHead = true;
			}
			foreach (int cell in this.neighbourCheckCells)
			{
				this.CollectNeighbourToChain(cell, ref chain, ref foundHead, null);
			}
		}

		// Token: 0x06008331 RID: 33585 RVA: 0x002FCCA0 File Offset: 0x002FAEA0
		private void CollectNeighbourToChain(int cell, ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain, ref bool foundHead, ChainedBuilding.StatesInstance ignoredLink = null)
		{
			GameObject gameObject = Grid.Objects[cell, (int)base.def.objectLayer];
			if (gameObject == null)
			{
				return;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (!component.HasTag(base.def.linkBuildingTag) && !component.IsPrefabID(base.def.headBuildingTag))
			{
				return;
			}
			ChainedBuilding.StatesInstance smi = gameObject.GetSMI<ChainedBuilding.StatesInstance>();
			if (smi != null)
			{
				smi.CollectToChain(ref chain, ref foundHead, ignoredLink);
			}
		}

		// Token: 0x0400641D RID: 25629
		private int widthInCells;

		// Token: 0x0400641E RID: 25630
		private List<int> neighbourCheckCells;
	}
}
