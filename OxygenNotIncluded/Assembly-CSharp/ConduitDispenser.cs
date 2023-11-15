using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006F4 RID: 1780
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitDispenser")]
public class ConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
	// Token: 0x1700036B RID: 875
	// (get) Token: 0x060030CC RID: 12492 RVA: 0x00102D27 File Offset: 0x00100F27
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x060030CD RID: 12493 RVA: 0x00102D2F File Offset: 0x00100F2F
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x060030CE RID: 12494 RVA: 0x00102D37 File Offset: 0x00100F37
	public ConduitFlow.ConduitContents ConduitContents
	{
		get
		{
			return this.GetConduitManager().GetContents(this.utilityCell);
		}
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x00102D4A File Offset: 0x00100F4A
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x00102D54 File Offset: 0x00100F54
	public ConduitFlow GetConduitManager()
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

	// Token: 0x060030D1 RID: 12497 RVA: 0x00102D89 File Offset: 0x00100F89
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x00102DA4 File Offset: 0x00100FA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, true);
		}, null, null);
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetOutputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x060030D3 RID: 12499 RVA: 0x00102E6F File Offset: 0x0010106F
	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060030D4 RID: 12500 RVA: 0x00102E9E File Offset: 0x0010109E
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x00102EA7 File Offset: 0x001010A7
	private void ConduitUpdate(float dt)
	{
		this.operational.SetFlag(ConduitDispenser.outputConduitFlag, this.IsConnected);
		this.blocked = false;
		if (this.isOn)
		{
			this.Dispense(dt);
		}
	}

	// Token: 0x060030D6 RID: 12502 RVA: 0x00102ED8 File Offset: 0x001010D8
	private void Dispense(float dt)
	{
		if (this.operational.IsOperational || this.alwaysDispense)
		{
			if (this.building.Def.CanMove)
			{
				this.utilityCell = this.GetOutputCell(this.GetConduitManager().conduitType);
			}
			PrimaryElement primaryElement = this.FindSuitableElement();
			if (primaryElement != null)
			{
				primaryElement.KeepZeroMassObject = true;
				this.empty = false;
				float num = this.GetConduitManager().AddElement(this.utilityCell, primaryElement.ElementID, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount);
				if (num > 0f)
				{
					int num2 = (int)(num / primaryElement.Mass * (float)primaryElement.DiseaseCount);
					primaryElement.ModifyDiseaseCount(-num2, "ConduitDispenser.ConduitUpdate");
					primaryElement.Mass -= num;
					this.storage.Trigger(-1697596308, primaryElement.gameObject);
					return;
				}
				this.blocked = true;
				return;
			}
			else
			{
				this.empty = true;
			}
		}
	}

	// Token: 0x060030D7 RID: 12503 RVA: 0x00102FD4 File Offset: 0x001011D4
	private PrimaryElement FindSuitableElement()
	{
		List<GameObject> items = this.storage.items;
		int count = items.Count;
		for (int i = 0; i < count; i++)
		{
			int index = (i + this.elementOutputOffset) % count;
			PrimaryElement component = items[index].GetComponent<PrimaryElement>();
			if (component != null && component.Mass > 0f && ((this.conduitType == ConduitType.Liquid) ? component.Element.IsLiquid : component.Element.IsGas) && (this.elementFilter == null || this.elementFilter.Length == 0 || (!this.invertElementFilter && this.IsFilteredElement(component.ElementID)) || (this.invertElementFilter && !this.IsFilteredElement(component.ElementID))))
			{
				this.elementOutputOffset = (this.elementOutputOffset + 1) % count;
				return component;
			}
		}
		return null;
	}

	// Token: 0x060030D8 RID: 12504 RVA: 0x001030B4 File Offset: 0x001012B4
	private bool IsFilteredElement(SimHashes element)
	{
		for (int num = 0; num != this.elementFilter.Length; num++)
		{
			if (this.elementFilter[num] == element)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x060030D9 RID: 12505 RVA: 0x001030E4 File Offset: 0x001012E4
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x00103128 File Offset: 0x00101328
	private int GetOutputCell(ConduitType outputConduitType)
	{
		Building component = base.GetComponent<Building>();
		if (this.useSecondaryOutput)
		{
			ISecondaryOutput[] components = base.GetComponents<ISecondaryOutput>();
			foreach (ISecondaryOutput secondaryOutput in components)
			{
				if (secondaryOutput.HasSecondaryConduitType(outputConduitType))
				{
					return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(outputConduitType));
				}
			}
			return Grid.OffsetCell(component.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(outputConduitType));
		}
		return component.GetUtilityOutputCell();
	}

	// Token: 0x04001D56 RID: 7510
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04001D57 RID: 7511
	[SerializeField]
	public SimHashes[] elementFilter;

	// Token: 0x04001D58 RID: 7512
	[SerializeField]
	public bool invertElementFilter;

	// Token: 0x04001D59 RID: 7513
	[SerializeField]
	public bool alwaysDispense;

	// Token: 0x04001D5A RID: 7514
	[SerializeField]
	public bool isOn = true;

	// Token: 0x04001D5B RID: 7515
	[SerializeField]
	public bool blocked;

	// Token: 0x04001D5C RID: 7516
	[SerializeField]
	public bool empty = true;

	// Token: 0x04001D5D RID: 7517
	[SerializeField]
	public bool useSecondaryOutput;

	// Token: 0x04001D5E RID: 7518
	private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

	// Token: 0x04001D5F RID: 7519
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001D60 RID: 7520
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04001D61 RID: 7521
	[MyCmpReq]
	private Building building;

	// Token: 0x04001D62 RID: 7522
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001D63 RID: 7523
	private int utilityCell = -1;

	// Token: 0x04001D64 RID: 7524
	private int elementOutputOffset;
}
