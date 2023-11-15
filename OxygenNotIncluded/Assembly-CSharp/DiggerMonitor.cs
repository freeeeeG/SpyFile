using System;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02000717 RID: 1815
public class DiggerMonitor : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>
{
	// Token: 0x060031E3 RID: 12771 RVA: 0x00108E30 File Offset: 0x00107030
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.EventTransition(GameHashes.BeginMeteorBombardment, (DiggerMonitor.Instance smi) => Game.Instance, this.dig, (DiggerMonitor.Instance smi) => smi.CanTunnel());
		this.dig.ToggleBehaviour(GameTags.Creatures.Tunnel, (DiggerMonitor.Instance smi) => true, null).GoTo(this.loop);
	}

	// Token: 0x04001DDA RID: 7642
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State loop;

	// Token: 0x04001DDB RID: 7643
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State dig;

	// Token: 0x0200147F RID: 5247
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x060084C7 RID: 33991 RVA: 0x003032B5 File Offset: 0x003014B5
		// (set) Token: 0x060084C8 RID: 33992 RVA: 0x003032BD File Offset: 0x003014BD
		public int depthToDig { get; set; }
	}

	// Token: 0x02001480 RID: 5248
	public new class Instance : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.GameInstance
	{
		// Token: 0x060084CA RID: 33994 RVA: 0x003032D0 File Offset: 0x003014D0
		public Instance(IStateMachineTarget master, DiggerMonitor.Def def) : base(master, def)
		{
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			this.OnDestinationReachedDelegate = new Action<object>(this.OnDestinationReached);
			master.Subscribe(387220196, this.OnDestinationReachedDelegate);
			master.Subscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x060084CB RID: 33995 RVA: 0x00303348 File Offset: 0x00301548
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			base.master.Unsubscribe(387220196, this.OnDestinationReachedDelegate);
			base.master.Unsubscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x060084CC RID: 33996 RVA: 0x003033AD File Offset: 0x003015AD
		private void OnDestinationReached(object data)
		{
			this.CheckInSolid();
		}

		// Token: 0x060084CD RID: 33997 RVA: 0x003033B8 File Offset: 0x003015B8
		private void CheckInSolid()
		{
			Navigator component = base.gameObject.GetComponent<Navigator>();
			if (component == null)
			{
				return;
			}
			int cell = Grid.PosToCell(base.gameObject);
			if (component.CurrentNavType != NavType.Solid && Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				return;
			}
			if (component.CurrentNavType == NavType.Solid && !Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Floor);
				base.gameObject.AddTag(GameTags.Creatures.Falling);
			}
		}

		// Token: 0x060084CE RID: 33998 RVA: 0x0030342B File Offset: 0x0030162B
		private void OnSolidChanged(int cell)
		{
			this.CheckInSolid();
		}

		// Token: 0x060084CF RID: 33999 RVA: 0x00303434 File Offset: 0x00301634
		public bool CanTunnel()
		{
			int num = Grid.PosToCell(this);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(num) == SubWorld.ZoneType.Space)
			{
				int num2 = num;
				while (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					num2 = Grid.CellAbove(num2);
				}
				if (!Grid.IsValidCell(num2))
				{
					return this.FoundValidDigCell();
				}
			}
			return false;
		}

		// Token: 0x060084D0 RID: 34000 RVA: 0x0030348C File Offset: 0x0030168C
		private bool FoundValidDigCell()
		{
			int num = base.smi.def.depthToDig;
			int num2 = Grid.PosToCell(base.smi.master.gameObject);
			this.lastDigCell = num2;
			int cell = Grid.CellBelow(num2);
			while (this.IsValidDigCell(cell, null) && num > 0)
			{
				cell = Grid.CellBelow(cell);
				num--;
			}
			if (num > 0)
			{
				cell = GameUtil.FloodFillFind<object>(new Func<int, object, bool>(this.IsValidDigCell), null, num2, base.smi.def.depthToDig, false, true);
			}
			this.lastDigCell = cell;
			return this.lastDigCell != -1;
		}

		// Token: 0x060084D1 RID: 34001 RVA: 0x00303528 File Offset: 0x00301728
		private bool IsValidDigCell(int cell, object arg = null)
		{
			if (Grid.IsValidCell(cell) && Grid.Solid[cell])
			{
				if (!Grid.HasDoor[cell] && !Grid.Foundation[cell])
				{
					ushort index = Grid.ElementIdx[cell];
					Element element = ElementLoader.elements[(int)index];
					return Grid.Element[cell].hardness < 150 && !element.HasTag(GameTags.RefinedMetal);
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					return Grid.Element[cell].hardness < 150 && !component.Element.HasTag(GameTags.RefinedMetal);
				}
			}
			return false;
		}

		// Token: 0x0400658B RID: 25995
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x0400658C RID: 25996
		private Action<object> OnDestinationReachedDelegate;
	}
}
