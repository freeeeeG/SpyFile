using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000546 RID: 1350
public class RoomType : Resource
{
	// Token: 0x17000175 RID: 373
	// (get) Token: 0x060020C0 RID: 8384 RVA: 0x000AF6EC File Offset: 0x000AD8EC
	// (set) Token: 0x060020C1 RID: 8385 RVA: 0x000AF6F4 File Offset: 0x000AD8F4
	public string tooltip { get; private set; }

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x060020C2 RID: 8386 RVA: 0x000AF6FD File Offset: 0x000AD8FD
	// (set) Token: 0x060020C3 RID: 8387 RVA: 0x000AF705 File Offset: 0x000AD905
	public string description { get; set; }

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x060020C4 RID: 8388 RVA: 0x000AF70E File Offset: 0x000AD90E
	// (set) Token: 0x060020C5 RID: 8389 RVA: 0x000AF716 File Offset: 0x000AD916
	public string effect { get; private set; }

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x060020C6 RID: 8390 RVA: 0x000AF71F File Offset: 0x000AD91F
	// (set) Token: 0x060020C7 RID: 8391 RVA: 0x000AF727 File Offset: 0x000AD927
	public RoomConstraints.Constraint primary_constraint { get; private set; }

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x060020C8 RID: 8392 RVA: 0x000AF730 File Offset: 0x000AD930
	// (set) Token: 0x060020C9 RID: 8393 RVA: 0x000AF738 File Offset: 0x000AD938
	public RoomConstraints.Constraint[] additional_constraints { get; private set; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x060020CA RID: 8394 RVA: 0x000AF741 File Offset: 0x000AD941
	// (set) Token: 0x060020CB RID: 8395 RVA: 0x000AF749 File Offset: 0x000AD949
	public int priority { get; private set; }

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x060020CC RID: 8396 RVA: 0x000AF752 File Offset: 0x000AD952
	// (set) Token: 0x060020CD RID: 8397 RVA: 0x000AF75A File Offset: 0x000AD95A
	public bool single_assignee { get; private set; }

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x060020CE RID: 8398 RVA: 0x000AF763 File Offset: 0x000AD963
	// (set) Token: 0x060020CF RID: 8399 RVA: 0x000AF76B File Offset: 0x000AD96B
	public RoomDetails.Detail[] display_details { get; private set; }

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x060020D0 RID: 8400 RVA: 0x000AF774 File Offset: 0x000AD974
	// (set) Token: 0x060020D1 RID: 8401 RVA: 0x000AF77C File Offset: 0x000AD97C
	public bool priority_building_use { get; private set; }

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x060020D2 RID: 8402 RVA: 0x000AF785 File Offset: 0x000AD985
	// (set) Token: 0x060020D3 RID: 8403 RVA: 0x000AF78D File Offset: 0x000AD98D
	public RoomTypeCategory category { get; private set; }

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x060020D4 RID: 8404 RVA: 0x000AF796 File Offset: 0x000AD996
	// (set) Token: 0x060020D5 RID: 8405 RVA: 0x000AF79E File Offset: 0x000AD99E
	public RoomType[] upgrade_paths { get; private set; }

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x060020D6 RID: 8406 RVA: 0x000AF7A7 File Offset: 0x000AD9A7
	// (set) Token: 0x060020D7 RID: 8407 RVA: 0x000AF7AF File Offset: 0x000AD9AF
	public string[] effects { get; private set; }

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x060020D8 RID: 8408 RVA: 0x000AF7B8 File Offset: 0x000AD9B8
	// (set) Token: 0x060020D9 RID: 8409 RVA: 0x000AF7C0 File Offset: 0x000AD9C0
	public int sortKey { get; private set; }

	// Token: 0x060020DA RID: 8410 RVA: 0x000AF7CC File Offset: 0x000AD9CC
	public RoomType(string id, string name, string description, string tooltip, string effect, RoomTypeCategory category, RoomConstraints.Constraint primary_constraint, RoomConstraints.Constraint[] additional_constraints, RoomDetails.Detail[] display_details, int priority = 0, RoomType[] upgrade_paths = null, bool single_assignee = false, bool priority_building_use = false, string[] effects = null, int sortKey = 0) : base(id, name)
	{
		this.tooltip = tooltip;
		this.description = description;
		this.effect = effect;
		this.category = category;
		this.primary_constraint = primary_constraint;
		this.additional_constraints = additional_constraints;
		this.display_details = display_details;
		this.priority = priority;
		this.upgrade_paths = upgrade_paths;
		this.single_assignee = single_assignee;
		this.priority_building_use = priority_building_use;
		this.effects = effects;
		this.sortKey = sortKey;
		if (this.upgrade_paths != null)
		{
			RoomType[] upgrade_paths2 = this.upgrade_paths;
			for (int i = 0; i < upgrade_paths2.Length; i++)
			{
				Debug.Assert(upgrade_paths2[i] != null, name + " has a null upgrade path. Maybe it wasn't initialized yet.");
			}
		}
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x000AF87C File Offset: 0x000ADA7C
	public RoomType.RoomIdentificationResult isSatisfactory(Room candidate_room)
	{
		if (this.primary_constraint != null && !this.primary_constraint.isSatisfied(candidate_room))
		{
			return RoomType.RoomIdentificationResult.primary_unsatisfied;
		}
		if (this.additional_constraints != null)
		{
			RoomConstraints.Constraint[] additional_constraints = this.additional_constraints;
			for (int i = 0; i < additional_constraints.Length; i++)
			{
				if (!additional_constraints[i].isSatisfied(candidate_room))
				{
					return RoomType.RoomIdentificationResult.primary_satisfied;
				}
			}
		}
		return RoomType.RoomIdentificationResult.all_satisfied;
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x000AF8CC File Offset: 0x000ADACC
	public string GetCriteriaString()
	{
		string text = string.Concat(new string[]
		{
			"<b>",
			this.Name,
			"</b>\n",
			this.tooltip,
			"\n\n",
			ROOMS.CRITERIA.HEADER
		});
		if (this == Db.Get().RoomTypes.Neutral)
		{
			text = text + "\n    • " + ROOMS.CRITERIA.NEUTRAL_TYPE;
		}
		text += ((this.primary_constraint == null) ? "" : ("\n    • " + this.primary_constraint.name));
		if (this.additional_constraints != null)
		{
			foreach (RoomConstraints.Constraint constraint in this.additional_constraints)
			{
				text = text + "\n    • " + constraint.name;
			}
		}
		return text;
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000AF9A4 File Offset: 0x000ADBA4
	public string GetRoomEffectsString()
	{
		if (this.effects != null && this.effects.Length != 0)
		{
			string text = ROOMS.EFFECTS.HEADER;
			foreach (string id in this.effects)
			{
				Effect effect = Db.Get().effects.Get(id);
				text += Effect.CreateTooltip(effect, false, "\n    • ", false);
			}
			return text;
		}
		return null;
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000AFA10 File Offset: 0x000ADC10
	public void TriggerRoomEffects(KPrefabID triggerer, Effects target)
	{
		if (this.primary_constraint == null)
		{
			return;
		}
		if (triggerer == null)
		{
			return;
		}
		if (this.effects == null)
		{
			return;
		}
		if (this.primary_constraint.building_criteria(triggerer))
		{
			foreach (string effect_id in this.effects)
			{
				target.Add(effect_id, true);
			}
		}
	}

	// Token: 0x020011E9 RID: 4585
	public enum RoomIdentificationResult
	{
		// Token: 0x04005DF7 RID: 24055
		all_satisfied,
		// Token: 0x04005DF8 RID: 24056
		primary_satisfied,
		// Token: 0x04005DF9 RID: 24057
		primary_unsatisfied
	}
}
