﻿using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020000D8 RID: 216
internal class NestingPoopState : GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>
{
	// Token: 0x060003D9 RID: 985 RVA: 0x0001DA44 File Offset: 0x0001BC44
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtopoop;
		this.goingtopoop.MoveTo((NestingPoopState.Instance smi) => smi.GetPoopPosition(), this.pooping, this.failedtonest, false);
		this.failedtonest.Enter(delegate(NestingPoopState.Instance smi)
		{
			smi.SetLastPoopCell();
		}).GoTo(this.pooping);
		this.pooping.Enter(delegate(NestingPoopState.Instance smi)
		{
			smi.master.GetComponent<Facing>().SetFacing(Grid.PosToCell(smi.master.gameObject) > smi.targetPoopCell);
		}).ToggleStatusItem(CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).PlayAnim("poop").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.Enter(delegate(NestingPoopState.Instance smi)
		{
			smi.SetLastPoopCell();
		}).PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
	}

	// Token: 0x0400028B RID: 651
	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State goingtopoop;

	// Token: 0x0400028C RID: 652
	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State pooping;

	// Token: 0x0400028D RID: 653
	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State behaviourcomplete;

	// Token: 0x0400028E RID: 654
	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State failedtonest;

	// Token: 0x02000F0B RID: 3851
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060070D3 RID: 28883 RVA: 0x002BB751 File Offset: 0x002B9951
		public Def(Tag tag)
		{
			this.nestingPoopElement = tag;
		}

		// Token: 0x040054BF RID: 21695
		public Tag nestingPoopElement = Tag.Invalid;
	}

	// Token: 0x02000F0C RID: 3852
	public new class Instance : GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.GameInstance
	{
		// Token: 0x060070D4 RID: 28884 RVA: 0x002BB76B File Offset: 0x002B996B
		public Instance(Chore<NestingPoopState.Instance> chore, NestingPoopState.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Poop);
		}

		// Token: 0x060070D5 RID: 28885 RVA: 0x002BB7A8 File Offset: 0x002B99A8
		private static bool IsValidNestingCell(int cell, object arg)
		{
			return Grid.IsValidCell(cell) && !Grid.Solid[cell] && Grid.IsValidCell(Grid.CellBelow(cell)) && Grid.Solid[Grid.CellBelow(cell)] && (NestingPoopState.Instance.IsValidPoopFromCell(cell, true) || NestingPoopState.Instance.IsValidPoopFromCell(cell, false));
		}

		// Token: 0x060070D6 RID: 28886 RVA: 0x002BB800 File Offset: 0x002B9A00
		private static bool IsValidPoopFromCell(int cell, bool look_left)
		{
			if (look_left)
			{
				int num = Grid.CellDownLeft(cell);
				int num2 = Grid.CellLeft(cell);
				return Grid.IsValidCell(num) && Grid.Solid[num] && Grid.IsValidCell(num2) && !Grid.Solid[num2];
			}
			int num3 = Grid.CellDownRight(cell);
			int num4 = Grid.CellRight(cell);
			return Grid.IsValidCell(num3) && Grid.Solid[num3] && Grid.IsValidCell(num4) && !Grid.Solid[num4];
		}

		// Token: 0x060070D7 RID: 28887 RVA: 0x002BB888 File Offset: 0x002B9A88
		public int GetPoopPosition()
		{
			this.targetPoopCell = this.GetTargetPoopCell();
			List<Direction> list = new List<Direction>();
			if (NestingPoopState.Instance.IsValidPoopFromCell(this.targetPoopCell, true))
			{
				list.Add(Direction.Left);
			}
			if (NestingPoopState.Instance.IsValidPoopFromCell(this.targetPoopCell, false))
			{
				list.Add(Direction.Right);
			}
			if (list.Count > 0)
			{
				Direction d = list[UnityEngine.Random.Range(0, list.Count)];
				int cellInDirection = Grid.GetCellInDirection(this.targetPoopCell, d);
				if (Grid.IsValidCell(cellInDirection))
				{
					return cellInDirection;
				}
			}
			if (Grid.IsValidCell(this.targetPoopCell))
			{
				return this.targetPoopCell;
			}
			if (!Grid.IsValidCell(Grid.PosToCell(this)))
			{
				global::Debug.LogWarning("This is bad, how is Mole occupying an invalid cell?");
			}
			return Grid.PosToCell(this);
		}

		// Token: 0x060070D8 RID: 28888 RVA: 0x002BB938 File Offset: 0x002B9B38
		private int GetTargetPoopCell()
		{
			CreatureCalorieMonitor.Instance smi = base.smi.GetSMI<CreatureCalorieMonitor.Instance>();
			this.currentlyPoopingElement = smi.stomach.GetNextPoopEntry();
			int start_cell;
			if (this.currentlyPoopingElement == base.smi.def.nestingPoopElement && base.smi.def.nestingPoopElement != Tag.Invalid && this.lastPoopCell != -1)
			{
				start_cell = this.lastPoopCell;
			}
			else
			{
				start_cell = Grid.PosToCell(this);
			}
			int num = GameUtil.FloodFillFind<object>(new Func<int, object, bool>(NestingPoopState.Instance.IsValidNestingCell), null, start_cell, 8, false, true);
			if (num == -1)
			{
				CellOffset[] array = new CellOffset[]
				{
					new CellOffset(0, 0),
					new CellOffset(-1, 0),
					new CellOffset(1, 0),
					new CellOffset(-1, -1),
					new CellOffset(1, -1)
				};
				num = Grid.OffsetCell(this.lastPoopCell, array[UnityEngine.Random.Range(0, array.Length)]);
				int num2 = Grid.CellAbove(num);
				while (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					num = num2;
					num2 = Grid.CellAbove(num);
				}
			}
			return num;
		}

		// Token: 0x060070D9 RID: 28889 RVA: 0x002BBA67 File Offset: 0x002B9C67
		public void SetLastPoopCell()
		{
			if (this.currentlyPoopingElement == base.smi.def.nestingPoopElement)
			{
				this.lastPoopCell = Grid.PosToCell(this);
			}
		}

		// Token: 0x040054C0 RID: 21696
		[Serialize]
		private int lastPoopCell = -1;

		// Token: 0x040054C1 RID: 21697
		public int targetPoopCell = -1;

		// Token: 0x040054C2 RID: 21698
		private Tag currentlyPoopingElement = Tag.Invalid;
	}
}
