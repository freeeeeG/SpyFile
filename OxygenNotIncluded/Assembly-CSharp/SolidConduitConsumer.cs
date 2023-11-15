using System;
using UnityEngine;

// Token: 0x02000980 RID: 2432
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitConsumer")]
public class SolidConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x060047A3 RID: 18339 RVA: 0x00194363 File Offset: 0x00192563
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x060047A4 RID: 18340 RVA: 0x0019436B File Offset: 0x0019256B
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x060047A5 RID: 18341 RVA: 0x0019436E File Offset: 0x0019256E
	public bool IsConsuming
	{
		get
		{
			return this.consuming;
		}
	}

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x060047A6 RID: 18342 RVA: 0x00194378 File Offset: 0x00192578
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x060047A7 RID: 18343 RVA: 0x001943AF File Offset: 0x001925AF
	private SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x060047A8 RID: 18344 RVA: 0x001943BC File Offset: 0x001925BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetInputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x00194436 File Offset: 0x00192636
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x00194465 File Offset: 0x00192665
	private void OnConduitConnectionChanged(object data)
	{
		this.consuming = (this.consuming && this.IsConnected);
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x060047AB RID: 18347 RVA: 0x00194494 File Offset: 0x00192694
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		SolidConduitFlow conduitFlow = this.GetConduitFlow();
		if (this.IsConnected)
		{
			SolidConduitFlow.ConduitContents contents = conduitFlow.GetContents(this.utilityCell);
			if (contents.pickupableHandle.IsValid() && (this.alwaysConsume || this.operational.IsOperational))
			{
				float num = (this.capacityTag != GameTags.Any) ? this.storage.GetMassAvailable(this.capacityTag) : this.storage.MassStored();
				float num2 = Mathf.Min(this.storage.capacityKg, this.capacityKG);
				float num3 = Mathf.Max(0f, num2 - num);
				if (num3 > 0f)
				{
					Pickupable pickupable = conduitFlow.GetPickupable(contents.pickupableHandle);
					if (pickupable.PrimaryElement.Mass <= num3 || pickupable.PrimaryElement.Mass > num2)
					{
						Pickupable pickupable2 = conduitFlow.RemovePickupable(this.utilityCell);
						if (pickupable2)
						{
							this.storage.Store(pickupable2.gameObject, true, false, true, false);
							flag = true;
						}
					}
				}
			}
		}
		if (this.storage != null)
		{
			this.storage.storageNetworkID = this.GetConnectedNetworkID();
		}
		this.consuming = flag;
	}

	// Token: 0x060047AC RID: 18348 RVA: 0x001945D4 File Offset: 0x001927D4
	private int GetConnectedNetworkID()
	{
		GameObject gameObject = Grid.Objects[this.utilityCell, 20];
		SolidConduit solidConduit = (gameObject != null) ? gameObject.GetComponent<SolidConduit>() : null;
		UtilityNetwork utilityNetwork = (solidConduit != null) ? solidConduit.GetNetwork() : null;
		if (utilityNetwork == null)
		{
			return -1;
		}
		return utilityNetwork.id;
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x00194628 File Offset: 0x00192828
	private int GetInputCell()
	{
		if (this.useSecondaryInput)
		{
			foreach (ISecondaryInput secondaryInput in base.GetComponents<ISecondaryInput>())
			{
				if (secondaryInput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), CellOffset.none);
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x04002F74 RID: 12148
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x04002F75 RID: 12149
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x04002F76 RID: 12150
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x04002F77 RID: 12151
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x04002F78 RID: 12152
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002F79 RID: 12153
	[MyCmpReq]
	private Building building;

	// Token: 0x04002F7A RID: 12154
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04002F7B RID: 12155
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002F7C RID: 12156
	private int utilityCell = -1;

	// Token: 0x04002F7D RID: 12157
	private bool consuming;
}
