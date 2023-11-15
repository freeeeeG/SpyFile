using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000981 RID: 2433
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitDispenser")]
public class SolidConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x060047AF RID: 18351 RVA: 0x001946BD File Offset: 0x001928BD
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x060047B0 RID: 18352 RVA: 0x001946C5 File Offset: 0x001928C5
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x060047B1 RID: 18353 RVA: 0x001946C8 File Offset: 0x001928C8
	public SolidConduitFlow.ConduitContents ConduitContents
	{
		get
		{
			return this.GetConduitFlow().GetContents(this.utilityCell);
		}
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x060047B2 RID: 18354 RVA: 0x001946DB File Offset: 0x001928DB
	public bool IsDispensing
	{
		get
		{
			return this.dispensing;
		}
	}

	// Token: 0x060047B3 RID: 18355 RVA: 0x001946E3 File Offset: 0x001928E3
	public SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x001946F0 File Offset: 0x001928F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetOutputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x060047B5 RID: 18357 RVA: 0x0019476B File Offset: 0x0019296B
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060047B6 RID: 18358 RVA: 0x0019479A File Offset: 0x0019299A
	private void OnConduitConnectionChanged(object data)
	{
		this.dispensing = (this.dispensing && this.IsConnected);
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x060047B7 RID: 18359 RVA: 0x001947CC File Offset: 0x001929CC
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		this.operational.SetFlag(SolidConduitDispenser.outputConduitFlag, this.IsConnected);
		if (this.operational.IsOperational || this.alwaysDispense)
		{
			SolidConduitFlow conduitFlow = this.GetConduitFlow();
			if (conduitFlow.HasConduit(this.utilityCell) && conduitFlow.IsConduitEmpty(this.utilityCell))
			{
				Pickupable pickupable = this.FindSuitableItem();
				if (pickupable)
				{
					if (pickupable.PrimaryElement.Mass > 20f)
					{
						pickupable = pickupable.Take(20f);
					}
					conduitFlow.AddPickupable(this.utilityCell, pickupable);
					flag = true;
				}
			}
		}
		this.storage.storageNetworkID = this.GetConnectedNetworkID();
		this.dispensing = flag;
	}

	// Token: 0x060047B8 RID: 18360 RVA: 0x00194880 File Offset: 0x00192A80
	private bool isSolid(GameObject o)
	{
		PrimaryElement component = o.GetComponent<PrimaryElement>();
		return component == null || component.Element.IsLiquid || component.Element.IsGas;
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x001948B8 File Offset: 0x00192AB8
	private Pickupable FindSuitableItem()
	{
		List<GameObject> list = this.storage.items;
		if (this.solidOnly)
		{
			List<GameObject> list2 = new List<GameObject>(list);
			list2.RemoveAll(new Predicate<GameObject>(this.isSolid));
			list = list2;
		}
		if (list.Count < 1)
		{
			return null;
		}
		this.round_robin_index %= list.Count;
		GameObject gameObject = list[this.round_robin_index];
		this.round_robin_index++;
		if (!gameObject)
		{
			return null;
		}
		return gameObject.GetComponent<Pickupable>();
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x060047BA RID: 18362 RVA: 0x0019493C File Offset: 0x00192B3C
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x060047BB RID: 18363 RVA: 0x00194974 File Offset: 0x00192B74
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

	// Token: 0x060047BC RID: 18364 RVA: 0x001949C8 File Offset: 0x00192BC8
	private int GetOutputCell()
	{
		Building component = base.GetComponent<Building>();
		if (this.useSecondaryOutput)
		{
			foreach (ISecondaryOutput secondaryOutput in base.GetComponents<ISecondaryOutput>())
			{
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(component.NaturalBuildingCell(), CellOffset.none);
		}
		return component.GetUtilityOutputCell();
	}

	// Token: 0x04002F7E RID: 12158
	[SerializeField]
	public SimHashes[] elementFilter;

	// Token: 0x04002F7F RID: 12159
	[SerializeField]
	public bool invertElementFilter;

	// Token: 0x04002F80 RID: 12160
	[SerializeField]
	public bool alwaysDispense;

	// Token: 0x04002F81 RID: 12161
	[SerializeField]
	public bool useSecondaryOutput;

	// Token: 0x04002F82 RID: 12162
	[SerializeField]
	public bool solidOnly;

	// Token: 0x04002F83 RID: 12163
	private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

	// Token: 0x04002F84 RID: 12164
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002F85 RID: 12165
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04002F86 RID: 12166
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002F87 RID: 12167
	private int utilityCell = -1;

	// Token: 0x04002F88 RID: 12168
	private bool dispensing;

	// Token: 0x04002F89 RID: 12169
	private int round_robin_index;
}
