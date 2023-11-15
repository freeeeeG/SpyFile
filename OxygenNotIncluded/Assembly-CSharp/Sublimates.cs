using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009F6 RID: 2550
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Sublimates")]
public class Sublimates : KMonoBehaviour, ISim200ms
{
	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06004C3C RID: 19516 RVA: 0x001ABA60 File Offset: 0x001A9C60
	public float Temperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x06004C3D RID: 19517 RVA: 0x001ABA6D File Offset: 0x001A9C6D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Sublimates>(-2064133523, Sublimates.OnAbsorbDelegate);
		base.Subscribe<Sublimates>(1335436905, Sublimates.OnSplitFromChunkDelegate);
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06004C3E RID: 19518 RVA: 0x001ABA9E File Offset: 0x001A9C9E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.flowAccumulator = Game.Instance.accumulators.Add("EmittedMass", this);
		this.RefreshStatusItem(Sublimates.EmitState.Emitting);
	}

	// Token: 0x06004C3F RID: 19519 RVA: 0x001ABAC8 File Offset: 0x001A9CC8
	protected override void OnCleanUp()
	{
		this.flowAccumulator = Game.Instance.accumulators.Remove(this.flowAccumulator);
		base.OnCleanUp();
	}

	// Token: 0x06004C40 RID: 19520 RVA: 0x001ABAEC File Offset: 0x001A9CEC
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			Sublimates component = pickupable.GetComponent<Sublimates>();
			if (component != null)
			{
				this.sublimatedMass += component.sublimatedMass;
			}
		}
	}

	// Token: 0x06004C41 RID: 19521 RVA: 0x001ABB2C File Offset: 0x001A9D2C
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		PrimaryElement component = pickupable.GetComponent<PrimaryElement>();
		Sublimates component2 = pickupable.GetComponent<Sublimates>();
		if (component2 == null)
		{
			return;
		}
		float mass = this.primaryElement.Mass;
		float mass2 = component.Mass;
		float num = mass / (mass2 + mass);
		this.sublimatedMass = component2.sublimatedMass * num;
		float num2 = 1f - num;
		component2.sublimatedMass *= num2;
	}

	// Token: 0x06004C42 RID: 19522 RVA: 0x001ABB98 File Offset: 0x001A9D98
	public void Sim200ms(float dt)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		bool flag = this.HasTag(GameTags.Sealed);
		Pickupable component = base.GetComponent<Pickupable>();
		Storage storage = (component != null) ? component.storage : null;
		if (flag && !this.decayStorage)
		{
			return;
		}
		if (flag && storage != null && storage.HasTag(GameTags.CorrosionProof))
		{
			return;
		}
		Element element = ElementLoader.FindElementByHash(this.info.sublimatedElement);
		if (this.primaryElement.Temperature <= element.lowTemp)
		{
			this.RefreshStatusItem(Sublimates.EmitState.BlockedOnTemperature);
			return;
		}
		float num2 = Grid.Mass[num];
		if (num2 < this.info.maxDestinationMass)
		{
			float num3 = this.primaryElement.Mass;
			if (num3 > 0f)
			{
				float num4 = Mathf.Pow(num3, this.info.massPower);
				float num5 = Mathf.Max(this.info.sublimationRate, this.info.sublimationRate * num4);
				num5 *= dt;
				num5 = Mathf.Min(num5, num3);
				this.sublimatedMass += num5;
				num3 -= num5;
				if (this.sublimatedMass > this.info.minSublimationAmount)
				{
					float num6 = this.sublimatedMass / this.primaryElement.Mass;
					byte diseaseIdx;
					int num7;
					if (this.info.diseaseIdx == 255)
					{
						diseaseIdx = this.primaryElement.DiseaseIdx;
						num7 = (int)((float)this.primaryElement.DiseaseCount * num6);
						this.primaryElement.ModifyDiseaseCount(-num7, "Sublimates.SimUpdate");
					}
					else
					{
						float num8 = this.sublimatedMass / this.info.sublimationRate;
						diseaseIdx = this.info.diseaseIdx;
						num7 = (int)((float)this.info.diseaseCount * num8);
					}
					float num9 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num2);
					if (num9 <= 0f)
					{
						this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
						return;
					}
					this.Emit(num, num9, this.primaryElement.Temperature, diseaseIdx, num7);
					this.sublimatedMass = Mathf.Max(0f, this.sublimatedMass - num9);
					this.primaryElement.Mass = Mathf.Max(0f, this.primaryElement.Mass - num9);
					this.UpdateStorage();
					this.RefreshStatusItem(Sublimates.EmitState.Emitting);
					if (flag && this.decayStorage && storage != null)
					{
						storage.Trigger(-794517298, new BuildingHP.DamageSourceInfo
						{
							damage = 1,
							source = BUILDINGS.DAMAGESOURCES.CORROSIVE_ELEMENT,
							popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CORROSIVE_ELEMENT,
							fullDamageEffectName = "smoke_damage_kanim"
						});
						return;
					}
				}
			}
			else if (this.sublimatedMass > 0f)
			{
				float num10 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num2);
				if (num10 > 0f)
				{
					this.Emit(num, num10, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount);
					this.sublimatedMass = Mathf.Max(0f, this.sublimatedMass - num10);
					this.primaryElement.Mass = Mathf.Max(0f, this.primaryElement.Mass - num10);
					this.UpdateStorage();
					this.RefreshStatusItem(Sublimates.EmitState.Emitting);
					return;
				}
				this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
				return;
			}
			else if (!this.primaryElement.KeepZeroMassObject)
			{
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
		}
		else
		{
			this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
		}
	}

	// Token: 0x06004C43 RID: 19523 RVA: 0x001ABF48 File Offset: 0x001AA148
	private void UpdateStorage()
	{
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null && component.storage != null)
		{
			component.storage.Trigger(-1697596308, base.gameObject);
		}
	}

	// Token: 0x06004C44 RID: 19524 RVA: 0x001ABF8C File Offset: 0x001AA18C
	private void Emit(int cell, float mass, float temperature, byte disease_idx, int disease_count)
	{
		SimMessages.AddRemoveSubstance(cell, this.info.sublimatedElement, CellEventLogger.Instance.SublimatesEmit, mass, temperature, disease_idx, disease_count, true, -1);
		Game.Instance.accumulators.Accumulate(this.flowAccumulator, mass);
		if (this.spawnFXHash != SpawnFXHashes.None)
		{
			base.transform.GetPosition().z = Grid.GetLayerZ(Grid.SceneLayer.Front);
			Game.Instance.SpawnFX(this.spawnFXHash, base.transform.GetPosition(), 0f);
		}
	}

	// Token: 0x06004C45 RID: 19525 RVA: 0x001AC014 File Offset: 0x001AA214
	public float AvgFlowRate()
	{
		return Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
	}

	// Token: 0x06004C46 RID: 19526 RVA: 0x001AC02C File Offset: 0x001AA22C
	private void RefreshStatusItem(Sublimates.EmitState newEmitState)
	{
		if (newEmitState == this.lastEmitState)
		{
			return;
		}
		switch (newEmitState)
		{
		case Sublimates.EmitState.Emitting:
			if (this.info.sublimatedElement == SimHashes.Oxygen)
			{
				this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingOxygenAvg, this);
			}
			else
			{
				this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingGasAvg, this);
			}
			break;
		case Sublimates.EmitState.BlockedOnPressure:
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedHighPressure, this);
			break;
		case Sublimates.EmitState.BlockedOnTemperature:
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedLowTemperature, this);
			break;
		}
		this.lastEmitState = newEmitState;
	}

	// Token: 0x040031B8 RID: 12728
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040031B9 RID: 12729
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040031BA RID: 12730
	[SerializeField]
	public SpawnFXHashes spawnFXHash;

	// Token: 0x040031BB RID: 12731
	public bool decayStorage;

	// Token: 0x040031BC RID: 12732
	[SerializeField]
	public Sublimates.Info info;

	// Token: 0x040031BD RID: 12733
	[Serialize]
	private float sublimatedMass;

	// Token: 0x040031BE RID: 12734
	private HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x040031BF RID: 12735
	private Sublimates.EmitState lastEmitState = (Sublimates.EmitState)(-1);

	// Token: 0x040031C0 RID: 12736
	private static readonly EventSystem.IntraObjectHandler<Sublimates> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x040031C1 RID: 12737
	private static readonly EventSystem.IntraObjectHandler<Sublimates> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data)
	{
		component.OnSplitFromChunk(data);
	});

	// Token: 0x0200187F RID: 6271
	[Serializable]
	public struct Info
	{
		// Token: 0x060091DE RID: 37342 RVA: 0x0032A9A1 File Offset: 0x00328BA1
		public Info(float rate, float min_amount, float max_destination_mass, float mass_power, SimHashes element, byte disease_idx = 255, int disease_count = 0)
		{
			this.sublimationRate = rate;
			this.minSublimationAmount = min_amount;
			this.maxDestinationMass = max_destination_mass;
			this.massPower = mass_power;
			this.sublimatedElement = element;
			this.diseaseIdx = disease_idx;
			this.diseaseCount = disease_count;
		}

		// Token: 0x0400722A RID: 29226
		public float sublimationRate;

		// Token: 0x0400722B RID: 29227
		public float minSublimationAmount;

		// Token: 0x0400722C RID: 29228
		public float maxDestinationMass;

		// Token: 0x0400722D RID: 29229
		public float massPower;

		// Token: 0x0400722E RID: 29230
		public byte diseaseIdx;

		// Token: 0x0400722F RID: 29231
		public int diseaseCount;

		// Token: 0x04007230 RID: 29232
		[HashedEnum]
		public SimHashes sublimatedElement;
	}

	// Token: 0x02001880 RID: 6272
	private enum EmitState
	{
		// Token: 0x04007232 RID: 29234
		Emitting,
		// Token: 0x04007233 RID: 29235
		BlockedOnPressure,
		// Token: 0x04007234 RID: 29236
		BlockedOnTemperature
	}
}
