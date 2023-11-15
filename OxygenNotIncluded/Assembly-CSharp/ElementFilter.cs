using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005FA RID: 1530
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ElementFilter")]
public class ElementFilter : KMonoBehaviour, ISaveLoadable, ISecondaryOutput
{
	// Token: 0x06002650 RID: 9808 RVA: 0x000D01EF File Offset: 0x000CE3EF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.InitializeStatusItems();
	}

	// Token: 0x06002651 RID: 9809 RVA: 0x000D0200 File Offset: 0x000CE400
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.inputCell = this.building.GetUtilityInputCell();
		this.outputCell = this.building.GetUtilityOutputCell();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.portInfo.offset);
		this.filteredCell = Grid.OffsetCell(cell, rotatedOffset);
		IUtilityNetworkMgr utilityNetworkMgr = (this.portInfo.conduitType == ConduitType.Solid) ? SolidConduit.GetFlowManager().networkMgr : Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.itemFilter = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.filteredCell, base.gameObject);
		utilityNetworkMgr.AddToNetworks(this.filteredCell, this.itemFilter, true);
		if (this.portInfo.conduitType == ConduitType.Gas || this.portInfo.conduitType == ConduitType.Liquid)
		{
			base.GetComponent<ConduitConsumer>().isConsuming = false;
		}
		this.OnFilterChanged(this.filterable.SelectedTag);
		this.filterable.onFilterChanged += this.OnFilterChanged;
		if (this.portInfo.conduitType == ConduitType.Solid)
		{
			SolidConduit.GetFlowManager().AddConduitUpdater(new Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
		}
		else
		{
			Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, ElementFilter.filterStatusItem, this);
		this.UpdateConduitExistsStatus();
		this.UpdateConduitBlockedStatus();
		ScenePartitionerLayer scenePartitionerLayer = null;
		switch (this.portInfo.conduitType)
		{
		case ConduitType.Gas:
			scenePartitionerLayer = GameScenePartitioner.Instance.gasConduitsLayer;
			break;
		case ConduitType.Liquid:
			scenePartitionerLayer = GameScenePartitioner.Instance.liquidConduitsLayer;
			break;
		case ConduitType.Solid:
			scenePartitionerLayer = GameScenePartitioner.Instance.solidConduitsLayer;
			break;
		}
		if (scenePartitionerLayer != null)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("ElementFilterConduitExists", base.gameObject, this.filteredCell, scenePartitionerLayer, delegate(object data)
			{
				this.UpdateConduitExistsStatus();
			});
		}
	}

	// Token: 0x06002652 RID: 9810 RVA: 0x000D040C File Offset: 0x000CE60C
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.filteredCell, this.itemFilter, true);
		if (this.portInfo.conduitType == ConduitType.Solid)
		{
			SolidConduit.GetFlowManager().RemoveConduitUpdater(new Action<float>(this.OnConduitTick));
		}
		else
		{
			Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.OnConduitTick));
		}
		if (this.partitionerEntry.IsValid() && GameScenePartitioner.Instance != null)
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		}
		base.OnCleanUp();
	}

	// Token: 0x06002653 RID: 9811 RVA: 0x000D04B4 File Offset: 0x000CE6B4
	private void OnConduitTick(float dt)
	{
		bool value = false;
		this.UpdateConduitBlockedStatus();
		if (this.operational.IsOperational)
		{
			if (this.portInfo.conduitType == ConduitType.Gas || this.portInfo.conduitType == ConduitType.Liquid)
			{
				ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
				ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
				int num = (contents.element.CreateTag() == this.filterable.SelectedTag) ? this.filteredCell : this.outputCell;
				ConduitFlow.ConduitContents contents2 = flowManager.GetContents(num);
				if (contents.mass > 0f && contents2.mass <= 0f)
				{
					value = true;
					float num2 = flowManager.AddElement(num, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
					if (num2 > 0f)
					{
						flowManager.RemoveElement(this.inputCell, num2);
					}
				}
			}
			else
			{
				SolidConduitFlow flowManager2 = SolidConduit.GetFlowManager();
				SolidConduitFlow.ConduitContents contents3 = flowManager2.GetContents(this.inputCell);
				Pickupable pickupable = flowManager2.GetPickupable(contents3.pickupableHandle);
				if (pickupable != null)
				{
					int num3 = (pickupable.GetComponent<KPrefabID>().PrefabTag == this.filterable.SelectedTag) ? this.filteredCell : this.outputCell;
					SolidConduitFlow.ConduitContents contents4 = flowManager2.GetContents(num3);
					Pickupable pickupable2 = flowManager2.GetPickupable(contents4.pickupableHandle);
					PrimaryElement primaryElement = null;
					if (pickupable2 != null)
					{
						primaryElement = pickupable2.PrimaryElement;
					}
					if (pickupable.PrimaryElement.Mass > 0f && (pickupable2 == null || primaryElement.Mass <= 0f))
					{
						value = true;
						Pickupable pickupable3 = flowManager2.RemovePickupable(this.inputCell);
						if (pickupable3 != null)
						{
							flowManager2.AddPickupable(num3, pickupable3);
						}
					}
				}
				else
				{
					flowManager2.RemovePickupable(this.inputCell);
				}
			}
		}
		this.operational.SetActive(value, false);
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x000D06B8 File Offset: 0x000CE8B8
	private void UpdateConduitExistsStatus()
	{
		bool flag = RequireOutputs.IsConnected(this.filteredCell, this.portInfo.conduitType);
		StatusItem status_item;
		switch (this.portInfo.conduitType)
		{
		case ConduitType.Gas:
			status_item = Db.Get().BuildingStatusItems.NeedGasOut;
			break;
		case ConduitType.Liquid:
			status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
			break;
		case ConduitType.Solid:
			status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		bool flag2 = this.needsConduitStatusItemGuid != Guid.Empty;
		if (flag == flag2)
		{
			this.needsConduitStatusItemGuid = this.selectable.ToggleStatusItem(status_item, this.needsConduitStatusItemGuid, !flag, null);
		}
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x000D076C File Offset: 0x000CE96C
	private void UpdateConduitBlockedStatus()
	{
		bool flag = Conduit.GetFlowManager(this.portInfo.conduitType).IsConduitEmpty(this.filteredCell);
		StatusItem conduitBlockedMultiples = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
		bool flag2 = this.conduitBlockedStatusItemGuid != Guid.Empty;
		if (flag == flag2)
		{
			this.conduitBlockedStatusItemGuid = this.selectable.ToggleStatusItem(conduitBlockedMultiples, this.conduitBlockedStatusItemGuid, !flag, null);
		}
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x000D07D8 File Offset: 0x000CE9D8
	private void OnFilterChanged(Tag tag)
	{
		bool on = !tag.IsValid || tag == GameTags.Void;
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x000D081C File Offset: 0x000CEA1C
	private void InitializeStatusItems()
	{
		if (ElementFilter.filterStatusItem == null)
		{
			ElementFilter.filterStatusItem = new StatusItem("Filter", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022, null);
			ElementFilter.filterStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				ElementFilter elementFilter = (ElementFilter)data;
				if (!elementFilter.filterable.SelectedTag.IsValid || elementFilter.filterable.SelectedTag == GameTags.Void)
				{
					str = string.Format(BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, BUILDINGS.PREFABS.GASFILTER.ELEMENT_NOT_SPECIFIED);
				}
				else
				{
					str = string.Format(BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, elementFilter.filterable.SelectedTag.ProperName());
				}
				return str;
			};
			ElementFilter.filterStatusItem.conditionalOverlayCallback = new Func<HashedString, object, bool>(this.ShowInUtilityOverlay);
		}
	}

	// Token: 0x06002658 RID: 9816 RVA: 0x000D0898 File Offset: 0x000CEA98
	private bool ShowInUtilityOverlay(HashedString mode, object data)
	{
		bool result = false;
		switch (((ElementFilter)data).portInfo.conduitType)
		{
		case ConduitType.Gas:
			result = (mode == OverlayModes.GasConduits.ID);
			break;
		case ConduitType.Liquid:
			result = (mode == OverlayModes.LiquidConduits.ID);
			break;
		case ConduitType.Solid:
			result = (mode == OverlayModes.SolidConveyor.ID);
			break;
		}
		return result;
	}

	// Token: 0x06002659 RID: 9817 RVA: 0x000D08F7 File Offset: 0x000CEAF7
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x000D0907 File Offset: 0x000CEB07
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		return this.portInfo.offset;
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x000D0914 File Offset: 0x000CEB14
	public int GetFilteredCell()
	{
		return this.filteredCell;
	}

	// Token: 0x040015EF RID: 5615
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x040015F0 RID: 5616
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040015F1 RID: 5617
	[MyCmpReq]
	private Building building;

	// Token: 0x040015F2 RID: 5618
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040015F3 RID: 5619
	[MyCmpReq]
	private Filterable filterable;

	// Token: 0x040015F4 RID: 5620
	private Guid needsConduitStatusItemGuid;

	// Token: 0x040015F5 RID: 5621
	private Guid conduitBlockedStatusItemGuid;

	// Token: 0x040015F6 RID: 5622
	private int inputCell = -1;

	// Token: 0x040015F7 RID: 5623
	private int outputCell = -1;

	// Token: 0x040015F8 RID: 5624
	private int filteredCell = -1;

	// Token: 0x040015F9 RID: 5625
	private FlowUtilityNetwork.NetworkItem itemFilter;

	// Token: 0x040015FA RID: 5626
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040015FB RID: 5627
	private static StatusItem filterStatusItem;
}
