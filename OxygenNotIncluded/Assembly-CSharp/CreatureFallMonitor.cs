using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class CreatureFallMonitor : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>
{
	// Token: 0x06001A91 RID: 6801 RVA: 0x0008DEA0 File Offset: 0x0008C0A0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.grounded.ToggleBehaviour(GameTags.Creatures.Falling, (CreatureFallMonitor.Instance smi) => smi.ShouldFall(), null);
	}

	// Token: 0x04000EC5 RID: 3781
	public static float FLOOR_DISTANCE = -0.065f;

	// Token: 0x04000EC6 RID: 3782
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State grounded;

	// Token: 0x04000EC7 RID: 3783
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State falling;

	// Token: 0x0200112F RID: 4399
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005B9A RID: 23450
		public bool canSwim;

		// Token: 0x04005B9B RID: 23451
		public bool checkHead = true;
	}

	// Token: 0x02001130 RID: 4400
	public new class Instance : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.GameInstance
	{
		// Token: 0x060078AF RID: 30895 RVA: 0x002D745C File Offset: 0x002D565C
		public Instance(IStateMachineTarget master, CreatureFallMonitor.Def def) : base(master, def)
		{
			this.navigator = master.GetComponent<Navigator>();
		}

		// Token: 0x060078B0 RID: 30896 RVA: 0x002D7480 File Offset: 0x002D5680
		public void SnapToGround()
		{
			Vector3 position = base.smi.transform.GetPosition();
			Vector3 position2 = Grid.CellToPosCBC(Grid.PosToCell(position), Grid.SceneLayer.Creatures);
			position2.x = position.x;
			base.smi.transform.SetPosition(position2);
			if (this.navigator.IsValidNavType(NavType.Floor))
			{
				this.navigator.SetCurrentNavType(NavType.Floor);
				return;
			}
			if (this.navigator.IsValidNavType(NavType.Hover))
			{
				this.navigator.SetCurrentNavType(NavType.Hover);
			}
		}

		// Token: 0x060078B1 RID: 30897 RVA: 0x002D7500 File Offset: 0x002D5700
		public bool ShouldFall()
		{
			if (base.gameObject.HasTag(GameTags.Stored))
			{
				return false;
			}
			Vector3 position = base.smi.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (Grid.IsValidCell(num) && Grid.Solid[num])
			{
				return false;
			}
			if (this.navigator.IsMoving())
			{
				return false;
			}
			if (this.CanSwimAtCurrentLocation())
			{
				return false;
			}
			if (this.navigator.CurrentNavType != NavType.Swim)
			{
				if (this.navigator.NavGrid.NavTable.IsValid(num, this.navigator.CurrentNavType))
				{
					return false;
				}
				if (this.navigator.CurrentNavType == NavType.Ceiling)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.LeftWall)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.RightWall)
				{
					return true;
				}
			}
			Vector3 vector = position;
			vector.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int num2 = Grid.PosToCell(vector);
			return !Grid.IsValidCell(num2) || !Grid.Solid[num2];
		}

		// Token: 0x060078B2 RID: 30898 RVA: 0x002D7608 File Offset: 0x002D5808
		public bool CanSwimAtCurrentLocation()
		{
			if (base.def.canSwim)
			{
				Vector3 position = base.transform.GetPosition();
				float num = 1f;
				if (!base.def.checkHead)
				{
					num = 0.5f;
				}
				position.y += base.transform.GetComponent<KBoxCollider2D>().size.y * num;
				if (Grid.IsSubstantialLiquid(Grid.PosToCell(position), 0.35f))
				{
					if (!GameComps.Gravities.Has(base.gameObject))
					{
						return true;
					}
					if (GameComps.Gravities.GetData(GameComps.Gravities.GetHandle(base.gameObject)).velocity.magnitude < 2f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04005B9C RID: 23452
		public string anim = "fall";

		// Token: 0x04005B9D RID: 23453
		private Navigator navigator;
	}
}
