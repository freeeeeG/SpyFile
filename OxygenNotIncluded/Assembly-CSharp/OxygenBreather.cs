using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020008D3 RID: 2259
[RequireComponent(typeof(Health))]
[AddComponentMenu("KMonoBehaviour/scripts/OxygenBreather")]
public class OxygenBreather : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x0600414D RID: 16717 RVA: 0x0016DB7C File Offset: 0x0016BD7C
	public float CO2EmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.co2Accumulator);
		}
	}

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x0600414E RID: 16718 RVA: 0x0016DB93 File Offset: 0x0016BD93
	public HandleVector<int>.Handle O2Accumulator
	{
		get
		{
			return this.o2Accumulator;
		}
	}

	// Token: 0x0600414F RID: 16719 RVA: 0x0016DB9B File Offset: 0x0016BD9B
	protected override void OnPrefabInit()
	{
		GameUtil.SubscribeToTags<OxygenBreather>(this, OxygenBreather.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x06004150 RID: 16720 RVA: 0x0016DBA9 File Offset: 0x0016BDA9
	public bool IsLowOxygenAtMouthCell()
	{
		return this.GetOxygenPressure(this.mouthCell) < this.lowOxygenThreshold;
	}

	// Token: 0x06004151 RID: 16721 RVA: 0x0016DBC0 File Offset: 0x0016BDC0
	protected override void OnSpawn()
	{
		this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(this);
		this.o2Accumulator = Game.Instance.accumulators.Add("O2", this);
		this.co2Accumulator = Game.Instance.accumulators.Add("CO2", this);
		KSelectable component = base.GetComponent<KSelectable>();
		component.AddStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, this);
		component.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, this);
		this.temperature = Db.Get().Amounts.Temperature.Lookup(this);
		NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
	}

	// Token: 0x06004152 RID: 16722 RVA: 0x0016DC7E File Offset: 0x0016BE7E
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.o2Accumulator);
		Game.Instance.accumulators.Remove(this.co2Accumulator);
		this.SetGasProvider(null);
		base.OnCleanUp();
	}

	// Token: 0x06004153 RID: 16723 RVA: 0x0016DCB9 File Offset: 0x0016BEB9
	public void Consume(Sim.MassConsumedCallback mass_consumed)
	{
		if (this.onSimConsume != null)
		{
			this.onSimConsume(mass_consumed);
		}
	}

	// Token: 0x06004154 RID: 16724 RVA: 0x0016DCD0 File Offset: 0x0016BED0
	public void Sim200ms(float dt)
	{
		if (!base.gameObject.HasTag(GameTags.Dead))
		{
			float num = this.airConsumptionRate.GetTotalValue() * dt;
			bool flag = this.gasProvider.ConsumeGas(this, num);
			if (flag)
			{
				if (this.gasProvider.ShouldEmitCO2())
				{
					float num2 = num * this.O2toCO2conversion;
					Game.Instance.accumulators.Accumulate(this.co2Accumulator, num2);
					this.accumulatedCO2 += num2;
					if (this.accumulatedCO2 >= this.minCO2ToEmit)
					{
						this.accumulatedCO2 -= this.minCO2ToEmit;
						Vector3 position = base.transform.GetPosition();
						Vector3 vector = position;
						vector.x += (this.facing.GetFacing() ? (-this.mouthOffset.x) : this.mouthOffset.x);
						vector.y += this.mouthOffset.y;
						vector.z -= 0.5f;
						if (Mathf.FloorToInt(vector.x) != Mathf.FloorToInt(position.x))
						{
							vector.x = Mathf.Floor(position.x) + (this.facing.GetFacing() ? 0.01f : 0.99f);
						}
						CO2Manager.instance.SpawnBreath(vector, this.minCO2ToEmit, this.temperature.value, this.facing.GetFacing());
					}
				}
				else if (this.gasProvider.ShouldStoreCO2())
				{
					Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						float num3 = num * this.O2toCO2conversion;
						Game.Instance.accumulators.Accumulate(this.co2Accumulator, num3);
						this.accumulatedCO2 += num3;
						if (this.accumulatedCO2 >= this.minCO2ToEmit)
						{
							this.accumulatedCO2 -= this.minCO2ToEmit;
							equippable.GetComponent<Storage>().AddGasChunk(SimHashes.CarbonDioxide, this.minCO2ToEmit, this.temperature.value, byte.MaxValue, 0, false, true);
						}
					}
				}
			}
			if (flag != this.hasAir)
			{
				this.hasAirTimer.Start();
				if (this.hasAirTimer.TryStop(2f))
				{
					this.hasAir = flag;
					return;
				}
			}
			else
			{
				this.hasAirTimer.Stop();
			}
		}
	}

	// Token: 0x06004155 RID: 16725 RVA: 0x0016DF29 File Offset: 0x0016C129
	private void OnDeath(object data)
	{
		base.enabled = false;
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, false);
		component.RemoveStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, false);
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x0016DF68 File Offset: 0x0016C168
	private int GetMouthCellAtCell(int cell, CellOffset[] offsets)
	{
		float num = 0f;
		int result = cell;
		foreach (CellOffset offset in offsets)
		{
			int num2 = Grid.OffsetCell(cell, offset);
			float oxygenPressure = this.GetOxygenPressure(num2);
			if (oxygenPressure > num && oxygenPressure > this.noOxygenThreshold)
			{
				num = oxygenPressure;
				result = num2;
			}
		}
		return result;
	}

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06004157 RID: 16727 RVA: 0x0016DFC0 File Offset: 0x0016C1C0
	public int mouthCell
	{
		get
		{
			int cell = Grid.PosToCell(this);
			return this.GetMouthCellAtCell(cell, this.breathableCells);
		}
	}

	// Token: 0x06004158 RID: 16728 RVA: 0x0016DFE1 File Offset: 0x0016C1E1
	public bool IsBreathableElementAtCell(int cell, CellOffset[] offsets = null)
	{
		return this.GetBreathableElementAtCell(cell, offsets) != SimHashes.Vacuum;
	}

	// Token: 0x06004159 RID: 16729 RVA: 0x0016DFF8 File Offset: 0x0016C1F8
	public SimHashes GetBreathableElementAtCell(int cell, CellOffset[] offsets = null)
	{
		if (offsets == null)
		{
			offsets = this.breathableCells;
		}
		int mouthCellAtCell = this.GetMouthCellAtCell(cell, offsets);
		if (!Grid.IsValidCell(mouthCellAtCell))
		{
			return SimHashes.Vacuum;
		}
		Element element = Grid.Element[mouthCellAtCell];
		if (!element.IsGas || !element.HasTag(GameTags.Breathable) || Grid.Mass[mouthCellAtCell] <= this.noOxygenThreshold)
		{
			return SimHashes.Vacuum;
		}
		return element.id;
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x0600415A RID: 16730 RVA: 0x0016E068 File Offset: 0x0016C268
	public bool IsUnderLiquid
	{
		get
		{
			return Grid.Element[this.mouthCell].IsLiquid;
		}
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x0600415B RID: 16731 RVA: 0x0016E07B File Offset: 0x0016C27B
	public bool IsSuffocating
	{
		get
		{
			return !this.hasAir;
		}
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x0600415C RID: 16732 RVA: 0x0016E086 File Offset: 0x0016C286
	public SimHashes GetBreathableElement
	{
		get
		{
			return this.GetBreathableElementAtCell(Grid.PosToCell(this), null);
		}
	}

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x0600415D RID: 16733 RVA: 0x0016E095 File Offset: 0x0016C295
	public bool IsBreathableElement
	{
		get
		{
			return this.IsBreathableElementAtCell(Grid.PosToCell(this), null);
		}
	}

	// Token: 0x0600415E RID: 16734 RVA: 0x0016E0A4 File Offset: 0x0016C2A4
	private float GetOxygenPressure(int cell)
	{
		if (Grid.IsValidCell(cell) && Grid.Element[cell].HasTag(GameTags.Breathable))
		{
			return Grid.Mass[cell];
		}
		return 0f;
	}

	// Token: 0x0600415F RID: 16735 RVA: 0x0016E0D2 File Offset: 0x0016C2D2
	public OxygenBreather.IGasProvider GetGasProvider()
	{
		return this.gasProvider;
	}

	// Token: 0x06004160 RID: 16736 RVA: 0x0016E0DA File Offset: 0x0016C2DA
	public void SetGasProvider(OxygenBreather.IGasProvider gas_provider)
	{
		if (this.gasProvider != null)
		{
			this.gasProvider.OnClearOxygenBreather(this);
		}
		this.gasProvider = gas_provider;
		if (this.gasProvider != null)
		{
			this.gasProvider.OnSetOxygenBreather(this);
		}
	}

	// Token: 0x04002A7D RID: 10877
	public static CellOffset[] DEFAULT_BREATHABLE_OFFSETS = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(-1, 1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x04002A7E RID: 10878
	public float O2toCO2conversion = 0.5f;

	// Token: 0x04002A7F RID: 10879
	public float lowOxygenThreshold;

	// Token: 0x04002A80 RID: 10880
	public float noOxygenThreshold;

	// Token: 0x04002A81 RID: 10881
	public Vector2 mouthOffset;

	// Token: 0x04002A82 RID: 10882
	[Serialize]
	public float accumulatedCO2;

	// Token: 0x04002A83 RID: 10883
	[SerializeField]
	public float minCO2ToEmit = 0.3f;

	// Token: 0x04002A84 RID: 10884
	private bool hasAir = true;

	// Token: 0x04002A85 RID: 10885
	private Timer hasAirTimer = new Timer();

	// Token: 0x04002A86 RID: 10886
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04002A87 RID: 10887
	[MyCmpGet]
	private Facing facing;

	// Token: 0x04002A88 RID: 10888
	private HandleVector<int>.Handle o2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04002A89 RID: 10889
	private HandleVector<int>.Handle co2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04002A8A RID: 10890
	private AmountInstance temperature;

	// Token: 0x04002A8B RID: 10891
	private AttributeInstance airConsumptionRate;

	// Token: 0x04002A8C RID: 10892
	public CellOffset[] breathableCells;

	// Token: 0x04002A8D RID: 10893
	public Action<Sim.MassConsumedCallback> onSimConsume;

	// Token: 0x04002A8E RID: 10894
	private OxygenBreather.IGasProvider gasProvider;

	// Token: 0x04002A8F RID: 10895
	private static readonly EventSystem.IntraObjectHandler<OxygenBreather> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<OxygenBreather>(GameTags.Dead, delegate(OxygenBreather component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x0200170E RID: 5902
	public interface IGasProvider
	{
		// Token: 0x06008D55 RID: 36181
		void OnSetOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x06008D56 RID: 36182
		void OnClearOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x06008D57 RID: 36183
		bool ConsumeGas(OxygenBreather oxygen_breather, float amount);

		// Token: 0x06008D58 RID: 36184
		bool ShouldEmitCO2();

		// Token: 0x06008D59 RID: 36185
		bool ShouldStoreCO2();

		// Token: 0x06008D5A RID: 36186
		bool IsLowOxygen();
	}
}
