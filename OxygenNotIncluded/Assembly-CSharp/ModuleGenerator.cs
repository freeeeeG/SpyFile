using System;

// Token: 0x020008A8 RID: 2216
public class ModuleGenerator : Generator
{
	// Token: 0x0600402B RID: 16427 RVA: 0x00167428 File Offset: 0x00165628
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	// Token: 0x0600402C RID: 16428 RVA: 0x00167444 File Offset: 0x00165644
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		this.clustercraft = craftInterface.GetComponent<Clustercraft>();
		Game.Instance.electricalConduitSystem.AddToVirtualNetworks(base.VirtualCircuitKey, this, true);
		base.OnSpawn();
	}

	// Token: 0x0600402D RID: 16429 RVA: 0x0016748D File Offset: 0x0016568D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.electricalConduitSystem.RemoveFromVirtualNetworks(base.VirtualCircuitKey, this, true);
	}

	// Token: 0x0600402E RID: 16430 RVA: 0x001674AC File Offset: 0x001656AC
	public override bool IsProducingPower()
	{
		return this.clustercraft.IsFlightInProgress();
	}

	// Token: 0x0600402F RID: 16431 RVA: 0x001674BC File Offset: 0x001656BC
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		if (this.IsProducingPower())
		{
			base.GenerateJoules(base.WattageRating * dt, false);
			if (this.poweringStatusItemHandle == Guid.Empty)
			{
				this.poweringStatusItemHandle = this.selectable.ReplaceStatusItem(this.notPoweringStatusItemHandle, Db.Get().BuildingStatusItems.ModuleGeneratorPowered, this);
				this.notPoweringStatusItemHandle = Guid.Empty;
				return;
			}
		}
		else if (this.notPoweringStatusItemHandle == Guid.Empty)
		{
			this.notPoweringStatusItemHandle = this.selectable.ReplaceStatusItem(this.poweringStatusItemHandle, Db.Get().BuildingStatusItems.ModuleGeneratorNotPowered, this);
			this.poweringStatusItemHandle = Guid.Empty;
		}
	}

	// Token: 0x040029E4 RID: 10724
	private Clustercraft clustercraft;

	// Token: 0x040029E5 RID: 10725
	private Guid poweringStatusItemHandle;

	// Token: 0x040029E6 RID: 10726
	private Guid notPoweringStatusItemHandle;
}
