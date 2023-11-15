using System;
using UnityEngine;

// Token: 0x0200049E RID: 1182
public class GasAndLiquidConsumerMonitor : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>
{
	// Token: 0x06001AA2 RID: 6818 RVA: 0x0008E4F0 File Offset: 0x0008C6F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).ScheduleGoTo((GasAndLiquidConsumerMonitor.Instance smi) => UnityEngine.Random.Range(smi.def.minCooldown, smi.def.maxCooldown), this.satisfied);
		this.satisfied.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.looking, false);
		this.looking.ToggleBehaviour((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.behaviourTag, (GasAndLiquidConsumerMonitor.Instance smi) => smi.targetCell != -1, delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.satisfied, true).Update("FindElement", delegate(GasAndLiquidConsumerMonitor.Instance smi, float dt)
		{
			smi.FindElement();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x04000ECD RID: 3789
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State cooldown;

	// Token: 0x04000ECE RID: 3790
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State satisfied;

	// Token: 0x04000ECF RID: 3791
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State looking;

	// Token: 0x0200113E RID: 4414
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005BB6 RID: 23478
		public Tag[] transitionTag = new Tag[]
		{
			GameTags.Creatures.Hungry
		};

		// Token: 0x04005BB7 RID: 23479
		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		// Token: 0x04005BB8 RID: 23480
		public float minCooldown = 5f;

		// Token: 0x04005BB9 RID: 23481
		public float maxCooldown = 5f;

		// Token: 0x04005BBA RID: 23482
		public Diet diet;

		// Token: 0x04005BBB RID: 23483
		public float consumptionRate = 0.5f;

		// Token: 0x04005BBC RID: 23484
		public Tag consumableElementTag = Tag.Invalid;
	}

	// Token: 0x0200113F RID: 4415
	public new class Instance : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.GameInstance
	{
		// Token: 0x060078CF RID: 30927 RVA: 0x002D7908 File Offset: 0x002D5B08
		public Instance(IStateMachineTarget master, GasAndLiquidConsumerMonitor.Def def) : base(master, def)
		{
			this.navigator = base.smi.GetComponent<Navigator>();
			DebugUtil.Assert(base.smi.def.diet != null || this.storage != null, "GasAndLiquidConsumerMonitor needs either a diet or a storage");
		}

		// Token: 0x060078D0 RID: 30928 RVA: 0x002D7960 File Offset: 0x002D5B60
		public void ClearTargetCell()
		{
			this.targetCell = -1;
			this.massUnavailableFrameCount = 0;
		}

		// Token: 0x060078D1 RID: 30929 RVA: 0x002D7970 File Offset: 0x002D5B70
		public void FindElement()
		{
			this.targetCell = -1;
			this.FindTargetCell();
		}

		// Token: 0x060078D2 RID: 30930 RVA: 0x002D797F File Offset: 0x002D5B7F
		public Element GetTargetElement()
		{
			return this.targetElement;
		}

		// Token: 0x060078D3 RID: 30931 RVA: 0x002D7988 File Offset: 0x002D5B88
		public bool IsConsumableCell(int cell, out Element element)
		{
			element = Grid.Element[cell];
			bool flag = true;
			bool flag2 = true;
			if (base.smi.def.consumableElementTag != Tag.Invalid)
			{
				flag = element.HasTag(base.smi.def.consumableElementTag);
			}
			if (base.smi.def.diet != null)
			{
				flag2 = false;
				Diet.Info[] infos = base.smi.def.diet.infos;
				for (int i = 0; i < infos.Length; i++)
				{
					if (infos[i].IsMatch(element.tag))
					{
						flag2 = true;
						break;
					}
				}
			}
			return flag && flag2;
		}

		// Token: 0x060078D4 RID: 30932 RVA: 0x002D7A28 File Offset: 0x002D5C28
		public void FindTargetCell()
		{
			GasAndLiquidConsumerMonitor.ConsumableCellQuery consumableCellQuery = new GasAndLiquidConsumerMonitor.ConsumableCellQuery(base.smi, 25);
			this.navigator.RunQuery(consumableCellQuery);
			if (consumableCellQuery.success)
			{
				this.targetCell = consumableCellQuery.GetResultCell();
				this.targetElement = consumableCellQuery.targetElement;
			}
		}

		// Token: 0x060078D5 RID: 30933 RVA: 0x002D7A70 File Offset: 0x002D5C70
		public void Consume(float dt)
		{
			int index = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(GasAndLiquidConsumerMonitor.Instance.OnMassConsumedCallback), this, "GasAndLiquidConsumerMonitor").index;
			SimMessages.ConsumeMass(Grid.PosToCell(this), this.targetElement.id, base.def.consumptionRate * dt, 3, index);
		}

		// Token: 0x060078D6 RID: 30934 RVA: 0x002D7ACC File Offset: 0x002D5CCC
		private static void OnMassConsumedCallback(Sim.MassConsumedCallback mcd, object data)
		{
			((GasAndLiquidConsumerMonitor.Instance)data).OnMassConsumed(mcd);
		}

		// Token: 0x060078D7 RID: 30935 RVA: 0x002D7ADC File Offset: 0x002D5CDC
		private void OnMassConsumed(Sim.MassConsumedCallback mcd)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (mcd.mass > 0f)
			{
				if (base.def.diet != null)
				{
					this.massUnavailableFrameCount = 0;
					Diet.Info dietInfo = base.def.diet.GetDietInfo(this.targetElement.tag);
					if (dietInfo == null)
					{
						return;
					}
					float calories = dietInfo.ConvertConsumptionMassToCalories(mcd.mass);
					CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = new CreatureCalorieMonitor.CaloriesConsumedEvent
					{
						tag = this.targetElement.tag,
						calories = calories
					};
					base.Trigger(-2038961714, caloriesConsumedEvent);
					return;
				}
				else if (this.storage != null)
				{
					this.storage.AddElement(this.targetElement.id, mcd.mass, mcd.temperature, mcd.diseaseIdx, mcd.diseaseCount, false, true);
					return;
				}
			}
			else
			{
				this.massUnavailableFrameCount++;
				if (this.massUnavailableFrameCount >= 2)
				{
					base.Trigger(801383139, null);
				}
			}
		}

		// Token: 0x04005BBD RID: 23485
		public int targetCell = -1;

		// Token: 0x04005BBE RID: 23486
		private Element targetElement;

		// Token: 0x04005BBF RID: 23487
		private Navigator navigator;

		// Token: 0x04005BC0 RID: 23488
		private int massUnavailableFrameCount;

		// Token: 0x04005BC1 RID: 23489
		[MyCmpGet]
		private Storage storage;
	}

	// Token: 0x02001140 RID: 4416
	public class ConsumableCellQuery : PathFinderQuery
	{
		// Token: 0x060078D8 RID: 30936 RVA: 0x002D7BDC File Offset: 0x002D5DDC
		public ConsumableCellQuery(GasAndLiquidConsumerMonitor.Instance smi, int maxIterations)
		{
			this.smi = smi;
			this.maxIterations = maxIterations;
		}

		// Token: 0x060078D9 RID: 30937 RVA: 0x002D7BF4 File Offset: 0x002D5DF4
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			int cell2 = Grid.CellAbove(cell);
			this.success = (this.smi.IsConsumableCell(cell, out this.targetElement) || (Grid.IsValidCell(cell2) && this.smi.IsConsumableCell(cell2, out this.targetElement)));
			if (!this.success)
			{
				int num = this.maxIterations - 1;
				this.maxIterations = num;
				return num <= 0;
			}
			return true;
		}

		// Token: 0x04005BC2 RID: 23490
		public bool success;

		// Token: 0x04005BC3 RID: 23491
		public Element targetElement;

		// Token: 0x04005BC4 RID: 23492
		private GasAndLiquidConsumerMonitor.Instance smi;

		// Token: 0x04005BC5 RID: 23493
		private int maxIterations;
	}
}
