using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class CropTendingStates : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>
{
	// Token: 0x06000340 RID: 832 RVA: 0x00019D88 File Offset: 0x00017F88
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findCrop;
		this.root.Exit(delegate(CropTendingStates.Instance smi)
		{
			this.UnreserveCrop(smi);
			if (!smi.tendedSucceeded)
			{
				this.RestoreSymbolsVisibility(smi);
			}
		});
		this.findCrop.Enter(delegate(CropTendingStates.Instance smi)
		{
			this.FindCrop(smi);
			if (smi.sm.targetCrop.Get(smi) == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			this.ReserverCrop(smi);
			smi.GoTo(this.moveToCrop);
		});
		this.moveToCrop.ToggleStatusItem(CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.NAME, CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).MoveTo((CropTendingStates.Instance smi) => smi.moveCell, this.tendCrop, this.behaviourcomplete, false).ParamTransition<GameObject>(this.targetCrop, this.behaviourcomplete, (CropTendingStates.Instance smi, GameObject p) => this.targetCrop.Get(smi) == null);
		this.tendCrop.DefaultState(this.tendCrop.pre).ToggleStatusItem(CREATURES.STATUSITEMS.DIVERGENT_TENDING.NAME, CREATURES.STATUSITEMS.DIVERGENT_TENDING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).ParamTransition<GameObject>(this.targetCrop, this.behaviourcomplete, (CropTendingStates.Instance smi, GameObject p) => this.targetCrop.Get(smi) == null).Enter(delegate(CropTendingStates.Instance smi)
		{
			smi.animSet = this.GetCropTendingAnimSet(smi);
			this.StoreSymbolsVisibility(smi);
		});
		this.tendCrop.pre.Face(this.targetCrop, 0f).PlayAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending_pre, KAnim.PlayMode.Once).OnAnimQueueComplete(this.tendCrop.tend);
		this.tendCrop.tend.Enter(delegate(CropTendingStates.Instance smi)
		{
			this.SetSymbolsVisibility(smi, false);
		}).QueueAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending, false, null).OnAnimQueueComplete(this.tendCrop.pst);
		this.tendCrop.pst.QueueAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending_pst, false, null).OnAnimQueueComplete(this.behaviourcomplete).Exit(delegate(CropTendingStates.Instance smi)
		{
			GameObject gameObject = smi.sm.targetCrop.Get(smi);
			if (gameObject != null)
			{
				if (smi.effect != null)
				{
					gameObject.GetComponent<Effects>().Add(smi.effect, true);
				}
				smi.tendedSucceeded = true;
				CropTendingStates.CropTendingEventData data = new CropTendingStates.CropTendingEventData
				{
					source = smi.gameObject,
					cropId = smi.sm.targetCrop.Get(smi).PrefabID()
				};
				smi.sm.targetCrop.Get(smi).Trigger(90606262, data);
				smi.Trigger(90606262, data);
			}
		});
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToTendCrops, false);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x0001A000 File Offset: 0x00018200
	private CropTendingStates.AnimSet GetCropTendingAnimSet(CropTendingStates.Instance smi)
	{
		CropTendingStates.AnimSet result;
		if (smi.def.animSetOverrides.TryGetValue(this.targetCrop.Get(smi).PrefabID(), out result))
		{
			return result;
		}
		return CropTendingStates.defaultAnimSet;
	}

	// Token: 0x06000342 RID: 834 RVA: 0x0001A03C File Offset: 0x0001823C
	private void FindCrop(CropTendingStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Crop crop = null;
		int moveCell = Grid.InvalidCell;
		int num = 100;
		int num2 = -1;
		foreach (Crop crop2 in Components.Crops.GetWorldItems(smi.gameObject.GetMyWorldId(), false))
		{
			if (smi.effect != null)
			{
				Effects component2 = crop2.GetComponent<Effects>();
				if (component2 != null)
				{
					bool flag = false;
					foreach (string effect_id in smi.def.ignoreEffectGroup)
					{
						if (component2.HasEffect(effect_id))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
				}
			}
			Growing component3 = crop2.GetComponent<Growing>();
			if ((!(component3 != null) || !component3.IsGrown()) && !crop2.HasTag(GameTags.Creatures.ReservedByCreature) && Vector2.SqrMagnitude(crop2.transform.position - smi.transform.position) <= 625f)
			{
				int num3;
				smi.def.interests.TryGetValue(crop2.PrefabID(), out num3);
				if (num3 >= num2)
				{
					bool flag2 = num3 > num2;
					int cell = Grid.PosToCell(crop2);
					int[] array = new int[]
					{
						Grid.CellLeft(cell),
						Grid.CellRight(cell)
					};
					int num4 = 100;
					int num5 = Grid.InvalidCell;
					for (int j = 0; j < array.Length; j++)
					{
						if (Grid.IsValidCell(array[j]))
						{
							int navigationCost = component.GetNavigationCost(array[j]);
							if (navigationCost != -1 && navigationCost < num4)
							{
								num4 = navigationCost;
								num5 = array[j];
							}
						}
					}
					if (num4 != -1 && num5 != Grid.InvalidCell && (flag2 || num4 < num))
					{
						moveCell = num5;
						num = num4;
						num2 = num3;
						crop = crop2;
					}
				}
			}
		}
		GameObject value = (crop != null) ? crop.gameObject : null;
		smi.sm.targetCrop.Set(value, smi, false);
		smi.moveCell = moveCell;
	}

	// Token: 0x06000343 RID: 835 RVA: 0x0001A270 File Offset: 0x00018470
	private void ReserverCrop(CropTendingStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCrop.Get(smi);
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x0001A2B8 File Offset: 0x000184B8
	private void UnreserveCrop(CropTendingStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCrop.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x0001A2EC File Offset: 0x000184EC
	private void SetSymbolsVisibility(CropTendingStates.Instance smi, bool isVisible)
	{
		if (this.targetCrop.Get(smi) != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					foreach (string str in hide_symbols_after_pre)
					{
						component.SetSymbolVisiblity(str, isVisible);
					}
				}
			}
		}
	}

	// Token: 0x06000346 RID: 838 RVA: 0x0001A35C File Offset: 0x0001855C
	private void StoreSymbolsVisibility(CropTendingStates.Instance smi)
	{
		if (this.targetCrop.Get(smi) != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					smi.symbolStates = new bool[hide_symbols_after_pre.Length];
					for (int i = 0; i < hide_symbols_after_pre.Length; i++)
					{
						smi.symbolStates[i] = component.GetSymbolVisiblity(hide_symbols_after_pre[i]);
					}
				}
			}
		}
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0001A3DC File Offset: 0x000185DC
	private void RestoreSymbolsVisibility(CropTendingStates.Instance smi)
	{
		if (this.targetCrop.Get(smi) != null && smi.symbolStates != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					for (int i = 0; i < hide_symbols_after_pre.Length; i++)
					{
						component.SetSymbolVisiblity(hide_symbols_after_pre[i], smi.symbolStates[i]);
					}
				}
			}
		}
	}

	// Token: 0x04000230 RID: 560
	private const int MAX_NAVIGATE_DISTANCE = 100;

	// Token: 0x04000231 RID: 561
	private const int MAX_SQR_EUCLIDEAN_DISTANCE = 625;

	// Token: 0x04000232 RID: 562
	private static CropTendingStates.AnimSet defaultAnimSet = new CropTendingStates.AnimSet
	{
		crop_tending_pre = "crop_tending_pre",
		crop_tending = "crop_tending_loop",
		crop_tending_pst = "crop_tending_pst"
	};

	// Token: 0x04000233 RID: 563
	public StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.TargetParameter targetCrop;

	// Token: 0x04000234 RID: 564
	private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State findCrop;

	// Token: 0x04000235 RID: 565
	private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State moveToCrop;

	// Token: 0x04000236 RID: 566
	private CropTendingStates.TendingStates tendCrop;

	// Token: 0x04000237 RID: 567
	private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State behaviourcomplete;

	// Token: 0x02000EA4 RID: 3748
	public class AnimSet
	{
		// Token: 0x04005405 RID: 21509
		public string crop_tending_pre;

		// Token: 0x04005406 RID: 21510
		public string crop_tending;

		// Token: 0x04005407 RID: 21511
		public string crop_tending_pst;

		// Token: 0x04005408 RID: 21512
		public string[] hide_symbols_after_pre;
	}

	// Token: 0x02000EA5 RID: 3749
	public class CropTendingEventData
	{
		// Token: 0x04005409 RID: 21513
		public GameObject source;

		// Token: 0x0400540A RID: 21514
		public Tag cropId;
	}

	// Token: 0x02000EA6 RID: 3750
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400540B RID: 21515
		public string effectId;

		// Token: 0x0400540C RID: 21516
		public string[] ignoreEffectGroup;

		// Token: 0x0400540D RID: 21517
		public Dictionary<Tag, int> interests = new Dictionary<Tag, int>();

		// Token: 0x0400540E RID: 21518
		public Dictionary<Tag, CropTendingStates.AnimSet> animSetOverrides = new Dictionary<Tag, CropTendingStates.AnimSet>();
	}

	// Token: 0x02000EA7 RID: 3751
	public new class Instance : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.GameInstance
	{
		// Token: 0x06006FFA RID: 28666 RVA: 0x002BA118 File Offset: 0x002B8318
		public Instance(Chore<CropTendingStates.Instance> chore, CropTendingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToTendCrops);
			this.effect = Db.Get().effects.TryGet(base.smi.def.effectId);
		}

		// Token: 0x0400540F RID: 21519
		public Effect effect;

		// Token: 0x04005410 RID: 21520
		public int moveCell;

		// Token: 0x04005411 RID: 21521
		public CropTendingStates.AnimSet animSet;

		// Token: 0x04005412 RID: 21522
		public bool tendedSucceeded;

		// Token: 0x04005413 RID: 21523
		public bool[] symbolStates;
	}

	// Token: 0x02000EA8 RID: 3752
	public class TendingStates : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State
	{
		// Token: 0x04005414 RID: 21524
		public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pre;

		// Token: 0x04005415 RID: 21525
		public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State tend;

		// Token: 0x04005416 RID: 21526
		public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pst;
	}
}
