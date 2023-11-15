using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005DE RID: 1502
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class ConduitThresholdSensor : ConduitSensor
{
	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06002549 RID: 9545
	public abstract float CurrentValue { get; }

	// Token: 0x0600254A RID: 9546 RVA: 0x000CB563 File Offset: 0x000C9763
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ConduitThresholdSensor>(-905833192, ConduitThresholdSensor.OnCopySettingsDelegate);
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x000CB57C File Offset: 0x000C977C
	private void OnCopySettings(object data)
	{
		ConduitThresholdSensor component = ((GameObject)data).GetComponent<ConduitThresholdSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x0600254C RID: 9548 RVA: 0x000CB5B8 File Offset: 0x000C97B8
	protected override void ConduitUpdate(float dt)
	{
		if (this.GetContainedMass() <= 0f && !this.dirty)
		{
			return;
		}
		float currentValue = this.CurrentValue;
		this.dirty = false;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x000CB644 File Offset: 0x000C9844
	private float GetContainedMass()
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			return Conduit.GetFlowManager(this.conduitType).GetContents(cell).mass;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents.pickupableHandle);
		if (pickupable != null)
		{
			return pickupable.PrimaryElement.Mass;
		}
		return 0f;
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x0600254E RID: 9550 RVA: 0x000CB6B7 File Offset: 0x000C98B7
	// (set) Token: 0x0600254F RID: 9551 RVA: 0x000CB6BF File Offset: 0x000C98BF
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06002550 RID: 9552 RVA: 0x000CB6CF File Offset: 0x000C98CF
	// (set) Token: 0x06002551 RID: 9553 RVA: 0x000CB6D7 File Offset: 0x000C98D7
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x0400155F RID: 5471
	[SerializeField]
	[Serialize]
	protected float threshold;

	// Token: 0x04001560 RID: 5472
	[SerializeField]
	[Serialize]
	protected bool activateAboveThreshold = true;

	// Token: 0x04001561 RID: 5473
	[Serialize]
	private bool dirty = true;

	// Token: 0x04001562 RID: 5474
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001563 RID: 5475
	private static readonly EventSystem.IntraObjectHandler<ConduitThresholdSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ConduitThresholdSensor>(delegate(ConduitThresholdSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
