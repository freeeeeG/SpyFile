﻿using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class SweepStates : GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>
{
	// Token: 0x06000FEA RID: 4074 RVA: 0x00055D4C File Offset: 0x00053F4C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.beginPatrol;
		this.beginPatrol.Enter(delegate(SweepStates.Instance smi)
		{
			smi.sm.timeUntilBored.Set(30f, smi, false);
			smi.GoTo(this.moving);
			SweepStates.Instance smi2 = smi;
			smi2.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi2.OnStop, new Action<string, StateMachine.Status>(delegate(string data, StateMachine.Status status)
			{
				this.StopMoveSound(smi);
			}));
		});
		this.moving.Enter(delegate(SweepStates.Instance smi)
		{
		}).MoveTo((SweepStates.Instance smi) => this.GetNextCell(smi), this.pause, this.redirected, false).Update(delegate(SweepStates.Instance smi, float dt)
		{
			smi.sm.timeUntilBored.Set(smi.sm.timeUntilBored.Get(smi) - dt, smi, false);
			if (smi.sm.timeUntilBored.Get(smi) <= 0f)
			{
				smi.sm.bored.Set(true, smi, false);
				smi.sm.timeUntilBored.Set(30f, smi, false);
				smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("react_bored");
			}
			StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			Storage storage = smi2.sm.sweepLocker.Get(smi2);
			if (storage != null && smi.sm.headingRight.Get(smi) == smi.master.transform.position.x > storage.transform.position.x)
			{
				Navigator component = smi.master.gameObject.GetComponent<Navigator>();
				if (component.GetNavigationCost(Grid.PosToCell(storage)) >= component.maxProbingRadius - 1)
				{
					smi.GoTo(smi.sm.emoteRedirected);
				}
			}
		}, UpdateRate.SIM_1000ms, false);
		this.emoteRedirected.Enter(delegate(SweepStates.Instance smi)
		{
			this.StopMoveSound(smi);
			int cell = Grid.PosToCell(smi.master.gameObject);
			if (Grid.IsCellOffsetValid(cell, this.headingRight.Get(smi) ? 1 : -1, -1) && !Grid.Solid[Grid.OffsetCell(cell, this.headingRight.Get(smi) ? 1 : -1, -1)])
			{
				smi.Play("gap", KAnim.PlayMode.Once);
			}
			else
			{
				smi.Play("bump", KAnim.PlayMode.Once);
			}
			this.headingRight.Set(!this.headingRight.Get(smi), smi, false);
		}).OnAnimQueueComplete(this.pause);
		this.redirected.StopMoving().GoTo(this.emoteRedirected);
		this.sweep.PlayAnim("pickup").ToggleEffect("BotSweeping").Enter(delegate(SweepStates.Instance smi)
		{
			this.StopMoveSound(smi);
			smi.sm.bored.Set(false, smi, false);
			smi.sm.timeUntilBored.Set(30f, smi, false);
		}).OnAnimQueueComplete(this.moving);
		this.pause.Enter(delegate(SweepStates.Instance smi)
		{
			if (Grid.IsLiquid(Grid.PosToCell(smi)))
			{
				smi.GoTo(this.mopping);
				return;
			}
			if (this.TrySweep(smi))
			{
				smi.GoTo(this.sweep);
				return;
			}
			smi.GoTo(this.moving);
		});
		this.mopping.PlayAnim("mop_pre", KAnim.PlayMode.Once).QueueAnim("mop_loop", true, null).ToggleEffect("BotMopping").Enter(delegate(SweepStates.Instance smi)
		{
			smi.sm.timeUntilBored.Set(30f, smi, false);
			smi.sm.bored.Set(false, smi, false);
			this.StopMoveSound(smi);
		}).Update(delegate(SweepStates.Instance smi, float dt)
		{
			if (smi.timeinstate > 16f || !Grid.IsLiquid(Grid.PosToCell(smi)))
			{
				smi.GoTo(this.moving);
				return;
			}
			this.TryMop(smi, dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06000FEB RID: 4075 RVA: 0x00055EBD File Offset: 0x000540BD
	public void StopMoveSound(SweepStates.Instance smi)
	{
		LoopingSounds component = smi.gameObject.GetComponent<LoopingSounds>();
		component.StopSound(GlobalAssets.GetSound("SweepBot_mvmt_lp", false));
		component.StopAllSounds();
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x00055EE0 File Offset: 0x000540E0
	public void StartMoveSound(SweepStates.Instance smi)
	{
		LoopingSounds component = smi.gameObject.GetComponent<LoopingSounds>();
		if (!component.IsSoundPlaying(GlobalAssets.GetSound("SweepBot_mvmt_lp", false)))
		{
			component.StartSound(GlobalAssets.GetSound("SweepBot_mvmt_lp", false));
		}
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x00055F20 File Offset: 0x00054120
	public void TryMop(SweepStates.Instance smi, float dt)
	{
		int cell = Grid.PosToCell(smi);
		if (Grid.IsLiquid(cell))
		{
			Moppable.MopCell(cell, Mathf.Min(Grid.Mass[cell], 10f * dt), delegate(Sim.MassConsumedCallback mass_cb_info, object data)
			{
				if (this == null)
				{
					return;
				}
				if (mass_cb_info.mass > 0f)
				{
					SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(ElementLoader.elements[(int)mass_cb_info.elemIdx], mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
					substanceChunk.transform.SetPosition(substanceChunk.transform.GetPosition() + new Vector3((UnityEngine.Random.value - 0.5f) * 0.5f, 0f, 0f));
					this.TryStore(substanceChunk.gameObject, smi);
				}
			});
		}
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x00055F94 File Offset: 0x00054194
	public bool TrySweep(SweepStates.Instance smi)
	{
		int cell = Grid.PosToCell(smi);
		GameObject gameObject = Grid.Objects[cell, 3];
		if (gameObject != null)
		{
			ObjectLayerListItem nextItem = gameObject.GetComponent<Pickupable>().objectLayerListItem.nextItem;
			return nextItem != null && this.TryStore(nextItem.gameObject, smi);
		}
		return false;
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x00055FE4 File Offset: 0x000541E4
	public bool TryStore(GameObject go, SweepStates.Instance smi)
	{
		Pickupable pickupable = go.GetComponent<Pickupable>();
		if (pickupable == null)
		{
			return false;
		}
		Storage storage = smi.master.gameObject.GetComponents<Storage>()[1];
		if (storage.IsFull())
		{
			return false;
		}
		if (pickupable != null && pickupable.absorbable)
		{
			SingleEntityReceptacle component = smi.master.GetComponent<SingleEntityReceptacle>();
			if (pickupable.gameObject == component.Occupant)
			{
				return false;
			}
			bool flag;
			if (pickupable.TotalAmount > 10f)
			{
				pickupable.GetComponent<EntitySplitter>();
				pickupable = EntitySplitter.Split(pickupable, Mathf.Min(10f, storage.RemainingCapacity()), null);
				smi.gameObject.GetAmounts().GetValue(Db.Get().Amounts.InternalBattery.Id);
				storage.Store(pickupable.gameObject, false, false, true, false);
				flag = true;
			}
			else
			{
				smi.gameObject.GetAmounts().GetValue(Db.Get().Amounts.InternalBattery.Id);
				storage.Store(pickupable.gameObject, false, false, true, false);
				flag = true;
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x00056100 File Offset: 0x00054300
	public int GetNextCell(SweepStates.Instance smi)
	{
		int i = 0;
		int num = Grid.PosToCell(smi);
		int num2 = Grid.InvalidCell;
		if (!Grid.Solid[Grid.CellBelow(num)])
		{
			return Grid.InvalidCell;
		}
		if (Grid.Solid[num])
		{
			return Grid.InvalidCell;
		}
		while (i < 1)
		{
			num2 = (smi.sm.headingRight.Get(smi) ? Grid.CellRight(num) : Grid.CellLeft(num));
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || !Grid.IsValidCell(Grid.CellBelow(num2)) || !Grid.Solid[Grid.CellBelow(num2)])
			{
				break;
			}
			num = num2;
			i++;
		}
		if (num == Grid.PosToCell(smi))
		{
			return Grid.InvalidCell;
		}
		return num;
	}

	// Token: 0x040008BF RID: 2239
	public const float TIME_UNTIL_BORED = 30f;

	// Token: 0x040008C0 RID: 2240
	public const string MOVE_LOOP_SOUND = "SweepBot_mvmt_lp";

	// Token: 0x040008C1 RID: 2241
	public StateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.BoolParameter headingRight;

	// Token: 0x040008C2 RID: 2242
	private StateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.FloatParameter timeUntilBored;

	// Token: 0x040008C3 RID: 2243
	public StateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.BoolParameter bored;

	// Token: 0x040008C4 RID: 2244
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State beginPatrol;

	// Token: 0x040008C5 RID: 2245
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State moving;

	// Token: 0x040008C6 RID: 2246
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State pause;

	// Token: 0x040008C7 RID: 2247
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State mopping;

	// Token: 0x040008C8 RID: 2248
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State redirected;

	// Token: 0x040008C9 RID: 2249
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State emoteRedirected;

	// Token: 0x040008CA RID: 2250
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State sweep;

	// Token: 0x02000F90 RID: 3984
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F91 RID: 3985
	public new class Instance : GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.GameInstance
	{
		// Token: 0x0600725E RID: 29278 RVA: 0x002BF33F File Offset: 0x002BD53F
		public Instance(Chore<SweepStates.Instance> chore, SweepStates.Def def) : base(chore, def)
		{
		}

		// Token: 0x0600725F RID: 29279 RVA: 0x002BF349 File Offset: 0x002BD549
		public override void StartSM()
		{
			base.StartSM();
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().RobotStatusItems.Working, base.gameObject);
		}

		// Token: 0x06007260 RID: 29280 RVA: 0x002BF381 File Offset: 0x002BD581
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().RobotStatusItems.Working, false);
		}
	}
}
