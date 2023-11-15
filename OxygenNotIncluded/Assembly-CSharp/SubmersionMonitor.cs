using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000739 RID: 1849
[AddComponentMenu("KMonoBehaviour/scripts/SubmersionMonitor")]
public class SubmersionMonitor : KMonoBehaviour, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x060032C6 RID: 12998 RVA: 0x0010DB6B File Offset: 0x0010BD6B
	public bool Dry
	{
		get
		{
			return this.dry;
		}
	}

	// Token: 0x060032C7 RID: 12999 RVA: 0x0010DB73 File Offset: 0x0010BD73
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnMove();
		this.CheckDry();
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMove), "SubmersionMonitor.OnSpawn");
	}

	// Token: 0x060032C8 RID: 13000 RVA: 0x0010DBAC File Offset: 0x0010BDAC
	private void OnMove()
	{
		this.position = Grid.PosToCell(base.gameObject);
		if (this.partitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.position);
		}
		else
		{
			Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
			Extents extents = new Extents(vector2I.x, vector2I.y, 1, 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDry();
	}

	// Token: 0x060032C9 RID: 13001 RVA: 0x0010DC4D File Offset: 0x0010BE4D
	private void OnDrawGizmosSelected()
	{
	}

	// Token: 0x060032CA RID: 13002 RVA: 0x0010DC4F File Offset: 0x0010BE4F
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMove));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060032CB RID: 13003 RVA: 0x0010DC83 File Offset: 0x0010BE83
	public void Configure(float _maxStamina, float _staminaRegenRate, float _cellLiquidThreshold = 0.95f)
	{
		this.cellLiquidThreshold = _cellLiquidThreshold;
	}

	// Token: 0x060032CC RID: 13004 RVA: 0x0010DC8C File Offset: 0x0010BE8C
	public void Sim1000ms(float dt)
	{
		this.CheckDry();
	}

	// Token: 0x060032CD RID: 13005 RVA: 0x0010DC94 File Offset: 0x0010BE94
	private void CheckDry()
	{
		if (!this.IsCellSafe())
		{
			if (!this.dry)
			{
				this.dry = true;
				base.Trigger(-2057657673, null);
				return;
			}
		}
		else if (this.dry)
		{
			this.dry = false;
			base.Trigger(1555379996, null);
		}
	}

	// Token: 0x060032CE RID: 13006 RVA: 0x0010DCE0 File Offset: 0x0010BEE0
	public bool IsCellSafe()
	{
		int cell = Grid.PosToCell(base.gameObject);
		return Grid.IsValidCell(cell) && Grid.IsSubstantialLiquid(cell, this.cellLiquidThreshold);
	}

	// Token: 0x060032CF RID: 13007 RVA: 0x0010DD14 File Offset: 0x0010BF14
	private void OnLiquidChanged(object data)
	{
		this.CheckDry();
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x0010DD1C File Offset: 0x0010BF1C
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.DryingOut
			};
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x060032D1 RID: 13009 RVA: 0x0010DD28 File Offset: 0x0010BF28
	public string WiltStateString
	{
		get
		{
			if (this.Dry)
			{
				return Db.Get().CreatureStatusItems.DryingOut.resolveStringCallback(CREATURES.STATUSITEMS.DRYINGOUT.NAME, this);
			}
			return "";
		}
	}

	// Token: 0x060032D2 RID: 13010 RVA: 0x0010DD5C File Offset: 0x0010BF5C
	public void SetIncapacitated(bool state)
	{
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x0010DD5E File Offset: 0x0010BF5E
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_SUBMERSION, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_SUBMERSION, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04001E7A RID: 7802
	private int position;

	// Token: 0x04001E7B RID: 7803
	private bool dry;

	// Token: 0x04001E7C RID: 7804
	protected float cellLiquidThreshold = 0.2f;

	// Token: 0x04001E7D RID: 7805
	private Extents extents;

	// Token: 0x04001E7E RID: 7806
	private HandleVector<int>.Handle partitionerEntry;
}
