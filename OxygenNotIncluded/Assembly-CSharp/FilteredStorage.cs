using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020005FF RID: 1535
public class FilteredStorage
{
	// Token: 0x06002674 RID: 9844 RVA: 0x000D109F File Offset: 0x000CF29F
	public void SetHasMeter(bool has_meter)
	{
		this.hasMeter = has_meter;
	}

	// Token: 0x06002675 RID: 9845 RVA: 0x000D10A8 File Offset: 0x000CF2A8
	public FilteredStorage(KMonoBehaviour root, Tag[] forbidden_tags, IUserControlledCapacity capacity_control, bool use_logic_meter, ChoreType fetch_chore_type)
	{
		this.root = root;
		this.forbiddenTags = forbidden_tags;
		this.capacityControl = capacity_control;
		this.useLogicMeter = use_logic_meter;
		this.choreType = fetch_chore_type;
		root.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		root.Subscribe(-543130682, new Action<object>(this.OnUserSettingsChanged));
		this.filterable = root.FindOrAdd<TreeFilterable>();
		TreeFilterable treeFilterable = this.filterable;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		this.storage = root.GetComponent<Storage>();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(-1852328367, new Action<object>(this.OnFunctionalChanged));
	}

	// Token: 0x06002676 RID: 9846 RVA: 0x000D119B File Offset: 0x000CF39B
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06002677 RID: 9847 RVA: 0x000D11B0 File Offset: 0x000CF3B0
	private void CreateMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.meter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_frame",
			"meter_level"
		});
	}

	// Token: 0x06002678 RID: 9848 RVA: 0x000D11FF File Offset: 0x000CF3FF
	private void CreateLogicMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.logicMeter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06002679 RID: 9849 RVA: 0x000D1232 File Offset: 0x000CF432
	public void SetMeter(MeterController meter)
	{
		this.hasMeter = true;
		this.meter = meter;
		this.UpdateMeter();
	}

	// Token: 0x0600267A RID: 9850 RVA: 0x000D1248 File Offset: 0x000CF448
	public void CleanUp()
	{
		if (this.filterable != null)
		{
			TreeFilterable treeFilterable = this.filterable;
			treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		}
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("Parent destroyed");
		}
	}

	// Token: 0x0600267B RID: 9851 RVA: 0x000D12A4 File Offset: 0x000CF4A4
	public void FilterChanged()
	{
		if (this.hasMeter)
		{
			if (this.meter == null)
			{
				this.CreateMeter();
			}
			if (this.logicMeter == null && this.useLogicMeter)
			{
				this.CreateLogicMeter();
			}
		}
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x0600267C RID: 9852 RVA: 0x000D12F4 File Offset: 0x000CF4F4
	private void OnUserSettingsChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x0600267D RID: 9853 RVA: 0x000D130D File Offset: 0x000CF50D
	private void OnStorageChanged(object data)
	{
		if (this.fetchList == null)
		{
			this.OnFilterChanged(this.filterable.GetTags());
		}
		this.UpdateMeter();
	}

	// Token: 0x0600267E RID: 9854 RVA: 0x000D132E File Offset: 0x000CF52E
	private void OnFunctionalChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x0600267F RID: 9855 RVA: 0x000D1344 File Offset: 0x000CF544
	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x06002680 RID: 9856 RVA: 0x000D137C File Offset: 0x000CF57C
	public bool IsFull()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float num = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
		return num >= 1f;
	}

	// Token: 0x06002681 RID: 9857 RVA: 0x000D13BD File Offset: 0x000CF5BD
	private void OnFetchComplete()
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x06002682 RID: 9858 RVA: 0x000D13D0 File Offset: 0x000CF5D0
	private float GetMaxCapacity()
	{
		float num = this.storage.capacityKg;
		if (this.capacityControl != null)
		{
			num = Mathf.Min(num, this.capacityControl.UserMaxCapacity);
		}
		return num;
	}

	// Token: 0x06002683 RID: 9859 RVA: 0x000D1404 File Offset: 0x000CF604
	private float GetMaxCapacityMinusStorageMargin()
	{
		return this.GetMaxCapacity() - this.storage.storageFullMargin;
	}

	// Token: 0x06002684 RID: 9860 RVA: 0x000D1418 File Offset: 0x000CF618
	private float GetAmountStored()
	{
		float result = this.storage.MassStored();
		if (this.capacityControl != null)
		{
			result = this.capacityControl.AmountStored;
		}
		return result;
	}

	// Token: 0x06002685 RID: 9861 RVA: 0x000D1448 File Offset: 0x000CF648
	private bool IsFunctional()
	{
		Operational component = this.storage.GetComponent<Operational>();
		return component == null || component.IsFunctional;
	}

	// Token: 0x06002686 RID: 9862 RVA: 0x000D1474 File Offset: 0x000CF674
	private void OnFilterChanged(HashSet<Tag> tags)
	{
		bool flag = tags != null && tags.Count != 0;
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("");
			this.fetchList = null;
		}
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float amountStored = this.GetAmountStored();
		float num = Mathf.Max(0f, maxCapacityMinusStorageMargin - amountStored);
		if (num > 0f && flag && this.IsFunctional())
		{
			num = Mathf.Max(0f, this.GetMaxCapacity() - amountStored);
			this.fetchList = new FetchList2(this.storage, this.choreType);
			this.fetchList.ShowStatusItem = false;
			this.fetchList.Add(tags, this.requiredTag, this.forbiddenTags, num, Operational.State.Functional);
			this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
		}
	}

	// Token: 0x06002687 RID: 9863 RVA: 0x000D1548 File Offset: 0x000CF748
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x06002688 RID: 9864 RVA: 0x000D156C File Offset: 0x000CF76C
	public void SetRequiredTag(Tag tag)
	{
		if (this.requiredTag != tag)
		{
			this.requiredTag = tag;
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x06002689 RID: 9865 RVA: 0x000D1594 File Offset: 0x000CF794
	public void AddForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags == null)
		{
			this.forbiddenTags = new Tag[0];
		}
		if (!this.forbiddenTags.Contains(forbidden_tag))
		{
			this.forbiddenTags = this.forbiddenTags.Append(forbidden_tag);
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x0600268A RID: 9866 RVA: 0x000D15E8 File Offset: 0x000CF7E8
	public void RemoveForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags != null)
		{
			List<Tag> list = new List<Tag>(this.forbiddenTags);
			list.Remove(forbidden_tag);
			this.forbiddenTags = list.ToArray();
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x0400160A RID: 5642
	public static readonly HashedString FULL_PORT_ID = "FULL";

	// Token: 0x0400160B RID: 5643
	private KMonoBehaviour root;

	// Token: 0x0400160C RID: 5644
	private FetchList2 fetchList;

	// Token: 0x0400160D RID: 5645
	private IUserControlledCapacity capacityControl;

	// Token: 0x0400160E RID: 5646
	private TreeFilterable filterable;

	// Token: 0x0400160F RID: 5647
	private Storage storage;

	// Token: 0x04001610 RID: 5648
	private MeterController meter;

	// Token: 0x04001611 RID: 5649
	private MeterController logicMeter;

	// Token: 0x04001612 RID: 5650
	private Tag requiredTag = Tag.Invalid;

	// Token: 0x04001613 RID: 5651
	private Tag[] forbiddenTags;

	// Token: 0x04001614 RID: 5652
	private bool hasMeter = true;

	// Token: 0x04001615 RID: 5653
	private bool useLogicMeter;

	// Token: 0x04001616 RID: 5654
	private ChoreType choreType;
}
