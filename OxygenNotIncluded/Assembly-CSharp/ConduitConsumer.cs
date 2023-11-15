using System;
using STRINGS;
using UnityEngine;

// Token: 0x020006F1 RID: 1777
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitConsumer")]
public class ConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x1700035D RID: 861
	// (get) Token: 0x060030AC RID: 12460 RVA: 0x00102336 File Offset: 0x00100536
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x060030AD RID: 12461 RVA: 0x0010233E File Offset: 0x0010053E
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x060030AE RID: 12462 RVA: 0x00102346 File Offset: 0x00100546
	public bool IsConnected
	{
		get
		{
			return Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16] != null && this.m_buildingComplete != null;
		}
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x060030AF RID: 12463 RVA: 0x00102380 File Offset: 0x00100580
	public bool CanConsume
	{
		get
		{
			bool result = false;
			if (this.IsConnected)
			{
				result = (this.GetConduitManager().GetContents(this.utilityCell).mass > 0f);
			}
			return result;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060030B0 RID: 12464 RVA: 0x001023BC File Offset: 0x001005BC
	public float stored_mass
	{
		get
		{
			if (this.storage == null)
			{
				return 0f;
			}
			if (!(this.capacityTag != GameTags.Any))
			{
				return this.storage.MassStored();
			}
			return this.storage.GetMassAvailable(this.capacityTag);
		}
	}

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060030B1 RID: 12465 RVA: 0x0010240C File Offset: 0x0010060C
	public float space_remaining_kg
	{
		get
		{
			float num = this.capacityKG - this.stored_mass;
			if (!(this.storage == null))
			{
				return Mathf.Min(this.storage.RemainingCapacity(), num);
			}
			return num;
		}
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x00102448 File Offset: 0x00100648
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x060030B3 RID: 12467 RVA: 0x00102451 File Offset: 0x00100651
	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x060030B4 RID: 12468 RVA: 0x00102459 File Offset: 0x00100659
	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x060030B5 RID: 12469 RVA: 0x00102479 File Offset: 0x00100679
	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x060030B6 RID: 12470 RVA: 0x001024A2 File Offset: 0x001006A2
	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x060030B7 RID: 12471 RVA: 0x001024AA File Offset: 0x001006AA
	// (set) Token: 0x060030B8 RID: 12472 RVA: 0x001024BF File Offset: 0x001006BF
	public bool IsSatisfied
	{
		get
		{
			return this.satisfied || !this.isConsuming;
		}
		set
		{
			this.satisfied = (value || this.forceAlwaysSatisfied);
		}
	}

	// Token: 0x060030B9 RID: 12473 RVA: 0x001024D4 File Offset: 0x001006D4
	private ConduitFlow GetConduitManager()
	{
		ConduitType conduitType = this.conduitType;
		if (conduitType == ConduitType.Gas)
		{
			return Game.Instance.gasConduitFlow;
		}
		if (conduitType != ConduitType.Liquid)
		{
			return null;
		}
		return Game.Instance.liquidConduitFlow;
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x060030BA RID: 12474 RVA: 0x0010250C File Offset: 0x0010070C
	public float MassAvailable
	{
		get
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			int inputCell = this.GetInputCell(conduitManager.conduitType);
			return conduitManager.GetContents(inputCell).mass;
		}
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x0010253C File Offset: 0x0010073C
	private int GetInputCell(ConduitType inputConduitType)
	{
		if (this.useSecondaryInput)
		{
			ISecondaryInput[] components = base.GetComponents<ISecondaryInput>();
			foreach (ISecondaryInput secondaryInput in components)
			{
				if (secondaryInput.HasSecondaryConduitType(inputConduitType))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(inputConduitType));
				}
			}
			global::Debug.LogWarning("No secondaryInput of type was found");
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(inputConduitType));
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x001025BC File Offset: 0x001007BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, true);
		}, null, null);
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetInputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x00102686 File Offset: 0x00100886
	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x001026B5 File Offset: 0x001008B5
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x060030BF RID: 12479 RVA: 0x001026CD File Offset: 0x001008CD
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x060030C0 RID: 12480 RVA: 0x001026D8 File Offset: 0x001008D8
	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming && this.isOn)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	// Token: 0x060030C1 RID: 12481 RVA: 0x00102704 File Offset: 0x00100904
	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		this.IsSatisfied = false;
		this.consumedLastTick = false;
		if (this.building.Def.CanMove)
		{
			this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
		}
		if (!this.IsConnected)
		{
			return;
		}
		ConduitFlow.ConduitContents contents = conduit_mgr.GetContents(this.utilityCell);
		if (contents.mass <= 0f)
		{
			return;
		}
		this.IsSatisfied = true;
		if (!this.alwaysConsume && !this.operational.MeetsRequirements(this.OperatingRequirement))
		{
			return;
		}
		float num = this.ConsumptionRate * dt;
		num = Mathf.Min(num, this.space_remaining_kg);
		Element element = ElementLoader.FindElementByHash(contents.element);
		if (contents.element != this.lastConsumedElement)
		{
			DiscoveredResources.Instance.Discover(element.tag, element.materialCategory);
		}
		float num2 = 0f;
		if (num > 0f)
		{
			ConduitFlow.ConduitContents conduitContents = conduit_mgr.RemoveElement(this.utilityCell, num);
			num2 = conduitContents.mass;
			this.lastConsumedElement = conduitContents.element;
		}
		bool flag = element.HasTag(this.capacityTag);
		if (num2 > 0f && this.capacityTag != GameTags.Any && !flag)
		{
			base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = 1,
				source = BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
			});
		}
		if (flag || this.wrongElementResult == ConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
		{
			if (num2 > 0f)
			{
				this.consumedLastTick = true;
				int disease_count = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				Element element2 = ElementLoader.FindElementByHash(contents.element);
				ConduitType conduitType = this.conduitType;
				if (conduitType != ConduitType.Gas)
				{
					if (conduitType == ConduitType.Liquid)
					{
						if (element2.IsLiquid)
						{
							this.storage.AddLiquid(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
							return;
						}
						global::Debug.LogWarning("Liquid conduit consumer consuming non liquid: " + element2.id.ToString());
						return;
					}
				}
				else
				{
					if (element2.IsGas)
					{
						this.storage.AddGasChunk(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
						return;
					}
					global::Debug.LogWarning("Gas conduit consumer consuming non gas: " + element2.id.ToString());
					return;
				}
			}
		}
		else if (num2 > 0f)
		{
			this.consumedLastTick = true;
			if (this.wrongElementResult == ConduitConsumer.WrongElementResult.Dump)
			{
				int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, true, -1);
			}
		}
	}

	// Token: 0x04001D3E RID: 7486
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04001D3F RID: 7487
	[SerializeField]
	public bool ignoreMinMassCheck;

	// Token: 0x04001D40 RID: 7488
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x04001D41 RID: 7489
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x04001D42 RID: 7490
	[SerializeField]
	public bool forceAlwaysSatisfied;

	// Token: 0x04001D43 RID: 7491
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x04001D44 RID: 7492
	[SerializeField]
	public bool keepZeroMassObject = true;

	// Token: 0x04001D45 RID: 7493
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x04001D46 RID: 7494
	[SerializeField]
	public bool isOn = true;

	// Token: 0x04001D47 RID: 7495
	[NonSerialized]
	public bool isConsuming = true;

	// Token: 0x04001D48 RID: 7496
	[NonSerialized]
	public bool consumedLastTick = true;

	// Token: 0x04001D49 RID: 7497
	[MyCmpReq]
	public Operational operational;

	// Token: 0x04001D4A RID: 7498
	[MyCmpReq]
	private Building building;

	// Token: 0x04001D4B RID: 7499
	public Operational.State OperatingRequirement;

	// Token: 0x04001D4C RID: 7500
	public ISecondaryInput targetSecondaryInput;

	// Token: 0x04001D4D RID: 7501
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04001D4E RID: 7502
	[MyCmpGet]
	private BuildingComplete m_buildingComplete;

	// Token: 0x04001D4F RID: 7503
	private int utilityCell = -1;

	// Token: 0x04001D50 RID: 7504
	public float consumptionRate = float.PositiveInfinity;

	// Token: 0x04001D51 RID: 7505
	public SimHashes lastConsumedElement = SimHashes.Vacuum;

	// Token: 0x04001D52 RID: 7506
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001D53 RID: 7507
	private bool satisfied;

	// Token: 0x04001D54 RID: 7508
	public ConduitConsumer.WrongElementResult wrongElementResult;

	// Token: 0x02001437 RID: 5175
	public enum WrongElementResult
	{
		// Token: 0x040064AE RID: 25774
		Destroy,
		// Token: 0x040064AF RID: 25775
		Dump,
		// Token: 0x040064B0 RID: 25776
		Store
	}
}
