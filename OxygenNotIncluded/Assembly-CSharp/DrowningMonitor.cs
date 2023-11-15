using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000719 RID: 1817
[AddComponentMenu("KMonoBehaviour/scripts/DrowningMonitor")]
public class DrowningMonitor : KMonoBehaviour, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x17000378 RID: 888
	// (get) Token: 0x060031E7 RID: 12775 RVA: 0x00108F83 File Offset: 0x00107183
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x060031E8 RID: 12776 RVA: 0x00108FA5 File Offset: 0x001071A5
	public bool Drowning
	{
		get
		{
			return this.drowning;
		}
	}

	// Token: 0x060031E9 RID: 12777 RVA: 0x00108FB0 File Offset: 0x001071B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.timeToDrown = 75f;
		if (DrowningMonitor.drowningEffect == null)
		{
			DrowningMonitor.drowningEffect = new Effect("Drowning", CREATURES.STATUSITEMS.DROWNING.NAME, CREATURES.STATUSITEMS.DROWNING.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.drowningEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.DROWNING.NAME, false, false, true));
		}
		if (DrowningMonitor.saturatedEffect == null)
		{
			DrowningMonitor.saturatedEffect = new Effect("Saturated", CREATURES.STATUSITEMS.SATURATED.NAME, CREATURES.STATUSITEMS.SATURATED.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.saturatedEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.SATURATED.NAME, false, false, true));
		}
	}

	// Token: 0x060031EA RID: 12778 RVA: 0x001090C0 File Offset: 0x001072C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.RegisterUpdate1000ms(this);
		this.OnMove();
		this.CheckDrowning(null);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMove), "DrowningMonitor.OnSpawn");
	}

	// Token: 0x060031EB RID: 12779 RVA: 0x00109110 File Offset: 0x00107310
	private void OnMove()
	{
		if (this.partitionerEntry.IsValid())
		{
			Extents ext = this.occupyArea.GetExtents();
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, ext);
		}
		else
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDrowning(null);
	}

	// Token: 0x060031EC RID: 12780 RVA: 0x0010918C File Offset: 0x0010738C
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMove));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.UnregisterUpdate1000ms(this);
		base.OnCleanUp();
	}

	// Token: 0x060031ED RID: 12781 RVA: 0x001091CC File Offset: 0x001073CC
	private void CheckDrowning(object data = null)
	{
		if (this.drowned)
		{
			return;
		}
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!this.IsCellSafe(cell))
		{
			if (!this.drowning)
			{
				this.drowning = true;
				base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Drowning, false);
				base.Trigger(1949704522, null);
			}
			if (this.timeToDrown <= 0f && this.canDrownToDeath)
			{
				DeathMonitor.Instance smi = this.GetSMI<DeathMonitor.Instance>();
				if (smi != null)
				{
					smi.Kill(Db.Get().Deaths.Drowned);
				}
				base.Trigger(-750750377, null);
				this.drowned = true;
			}
		}
		else if (this.drowning)
		{
			this.drowning = false;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Drowning);
			base.Trigger(99949694, null);
		}
		if (this.livesUnderWater)
		{
			this.saturatedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Saturated, this.saturatedStatusGuid, this.drowning, this);
		}
		else
		{
			this.drowningStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Drowning, this.drowningStatusGuid, this.drowning, this);
		}
		if (this.effects != null)
		{
			if (this.drowning)
			{
				if (this.livesUnderWater)
				{
					this.effects.Add(DrowningMonitor.saturatedEffect, false);
					return;
				}
				this.effects.Add(DrowningMonitor.drowningEffect, false);
				return;
			}
			else
			{
				if (this.livesUnderWater)
				{
					this.effects.Remove(DrowningMonitor.saturatedEffect);
					return;
				}
				this.effects.Remove(DrowningMonitor.drowningEffect);
			}
		}
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x00109374 File Offset: 0x00107574
	private static bool CellSafeTest(int testCell, object data)
	{
		int num = Grid.CellAbove(testCell);
		if (!Grid.IsValidCell(testCell) || !Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.IsSubstantialLiquid(testCell, 0.95f))
		{
			return false;
		}
		if (Grid.IsLiquid(testCell))
		{
			if (Grid.Element[num].IsLiquid)
			{
				return false;
			}
			if (Grid.Element[num].IsSolid)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060031EF RID: 12783 RVA: 0x001093D2 File Offset: 0x001075D2
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, this, DrowningMonitor.CellSafeTestDelegate);
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x060031F0 RID: 12784 RVA: 0x001093E6 File Offset: 0x001075E6
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Drowning
			};
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x060031F1 RID: 12785 RVA: 0x001093F2 File Offset: 0x001075F2
	public string WiltStateString
	{
		get
		{
			if (this.livesUnderWater)
			{
				return "    • " + CREATURES.STATUSITEMS.SATURATED.NAME;
			}
			return "    • " + CREATURES.STATUSITEMS.DROWNING.NAME;
		}
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x00109425 File Offset: 0x00107625
	private void OnLiquidChanged(object data)
	{
		this.CheckDrowning(null);
	}

	// Token: 0x060031F3 RID: 12787 RVA: 0x00109430 File Offset: 0x00107630
	public void SlicedSim1000ms(float dt)
	{
		this.CheckDrowning(null);
		if (this.drowning)
		{
			if (!this.drowned)
			{
				this.timeToDrown -= dt;
				if (this.timeToDrown <= 0f)
				{
					this.CheckDrowning(null);
					return;
				}
			}
		}
		else
		{
			this.timeToDrown += dt * 5f;
			this.timeToDrown = Mathf.Clamp(this.timeToDrown, 0f, 75f);
		}
	}

	// Token: 0x04001DDF RID: 7647
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001DE0 RID: 7648
	[MyCmpGet]
	private Effects effects;

	// Token: 0x04001DE1 RID: 7649
	private OccupyArea _occupyArea;

	// Token: 0x04001DE2 RID: 7650
	[Serialize]
	[SerializeField]
	private float timeToDrown;

	// Token: 0x04001DE3 RID: 7651
	[Serialize]
	private bool drowned;

	// Token: 0x04001DE4 RID: 7652
	private bool drowning;

	// Token: 0x04001DE5 RID: 7653
	protected const float MaxDrownTime = 75f;

	// Token: 0x04001DE6 RID: 7654
	protected const float RegenRate = 5f;

	// Token: 0x04001DE7 RID: 7655
	protected const float CellLiquidThreshold = 0.95f;

	// Token: 0x04001DE8 RID: 7656
	public bool canDrownToDeath = true;

	// Token: 0x04001DE9 RID: 7657
	public bool livesUnderWater;

	// Token: 0x04001DEA RID: 7658
	private Guid drowningStatusGuid;

	// Token: 0x04001DEB RID: 7659
	private Guid saturatedStatusGuid;

	// Token: 0x04001DEC RID: 7660
	private Extents extents;

	// Token: 0x04001DED RID: 7661
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001DEE RID: 7662
	public static Effect drowningEffect;

	// Token: 0x04001DEF RID: 7663
	public static Effect saturatedEffect;

	// Token: 0x04001DF0 RID: 7664
	private static readonly Func<int, object, bool> CellSafeTestDelegate = (int testCell, object data) => DrowningMonitor.CellSafeTest(testCell, data);
}
