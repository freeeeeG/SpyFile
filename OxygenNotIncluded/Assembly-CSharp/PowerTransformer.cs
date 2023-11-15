using System;
using System.Diagnostics;

// Token: 0x02000673 RID: 1651
[DebuggerDisplay("{name}")]
public class PowerTransformer : Generator
{
	// Token: 0x06002BB8 RID: 11192 RVA: 0x000E88BA File Offset: 0x000E6ABA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.battery = base.GetComponent<Battery>();
		base.Subscribe<PowerTransformer>(-592767678, PowerTransformer.OnOperationalChangedDelegate);
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x06002BB9 RID: 11193 RVA: 0x000E88E5 File Offset: 0x000E6AE5
	public override void ApplyDeltaJoules(float joules_delta, bool can_over_power = false)
	{
		this.battery.ConsumeEnergy(-joules_delta);
		base.ApplyDeltaJoules(joules_delta, can_over_power);
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x000E88FC File Offset: 0x000E6AFC
	public override void ConsumeEnergy(float joules)
	{
		this.battery.ConsumeEnergy(joules);
		base.ConsumeEnergy(joules);
	}

	// Token: 0x06002BBB RID: 11195 RVA: 0x000E8911 File Offset: 0x000E6B11
	private void OnOperationalChanged(object data)
	{
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x000E8919 File Offset: 0x000E6B19
	private void UpdateJoulesLostPerSecond()
	{
		if (this.operational.IsOperational)
		{
			this.battery.joulesLostPerSecond = 0f;
			return;
		}
		this.battery.joulesLostPerSecond = 3.3333333f;
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x000E894C File Offset: 0x000E6B4C
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		float joulesAvailable = this.operational.IsOperational ? Math.Min(this.battery.JoulesAvailable, base.WattageRating * dt) : 0f;
		base.AssignJoulesAvailable(joulesAvailable);
		ushort circuitID = this.battery.CircuitID;
		ushort circuitID2 = base.CircuitID;
		bool flag = circuitID == circuitID2 && circuitID != ushort.MaxValue;
		if (this.mLoopDetected != flag)
		{
			this.mLoopDetected = flag;
			this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.PowerLoopDetected, this.mLoopDetected, this);
		}
	}

	// Token: 0x040019AE RID: 6574
	private Battery battery;

	// Token: 0x040019AF RID: 6575
	private bool mLoopDetected;

	// Token: 0x040019B0 RID: 6576
	private static readonly EventSystem.IntraObjectHandler<PowerTransformer> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<PowerTransformer>(delegate(PowerTransformer component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
