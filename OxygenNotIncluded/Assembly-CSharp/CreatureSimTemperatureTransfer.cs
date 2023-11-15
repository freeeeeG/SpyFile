using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;

// Token: 0x02000711 RID: 1809
public class CreatureSimTemperatureTransfer : SimTemperatureTransfer, ISim200ms
{
	// Token: 0x17000374 RID: 884
	// (get) Token: 0x060031BB RID: 12731 RVA: 0x0010848A File Offset: 0x0010668A
	// (set) Token: 0x060031BC RID: 12732 RVA: 0x00108492 File Offset: 0x00106692
	public float deltaEnergy
	{
		get
		{
			return this.deltaKJ;
		}
		protected set
		{
			this.deltaKJ = value;
		}
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x060031BD RID: 12733 RVA: 0x0010849B File Offset: 0x0010669B
	public float currentExchangeWattage
	{
		get
		{
			return this.deltaKJ * 5f * 1000f;
		}
	}

	// Token: 0x060031BE RID: 12734 RVA: 0x001084B0 File Offset: 0x001066B0
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		this.average_kilowatts_exchanged = new RunningWeightedAverage(-10f, 10f, 20, true);
		this.surfaceArea = 1f;
		this.thickness = 0.002f;
		this.groundTransferScale = 0f;
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Add(Db.Get().Attributes.ThermalConductivityBarrier);
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, this.thickness, DUPLICANTS.MODIFIERS.BASEDUPLICANT.NAME, false, false, true);
		attributeInstance.Add(modifier);
		this.averageTemperatureTransferPerSecond = new AttributeModifier("TemperatureDelta", 0f, DUPLICANTS.MODIFIERS.TEMPEXCHANGE.NAME, false, true, false);
		this.GetAttributes().Add(this.averageTemperatureTransferPerSecond);
		base.OnPrefabInit();
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x00108590 File Offset: 0x00106790
	public void Sim200ms(float dt)
	{
		this.average_kilowatts_exchanged.AddSample(this.currentExchangeWattage * 0.001f);
		this.averageTemperatureTransferPerSecond.SetValue(SimUtil.EnergyFlowToTemperatureDelta(this.average_kilowatts_exchanged.GetWeightedAverage, this.primaryElement.Element.specificHeatCapacity, this.primaryElement.Mass));
		float num = 0f;
		foreach (AttributeModifier attributeModifier in this.NonSimTemperatureModifiers)
		{
			num += attributeModifier.Value;
		}
		if (Sim.IsValidHandle(this.simHandle))
		{
			SimMessages.ModifyElementChunkEnergy(this.simHandle, num * dt * (this.primaryElement.Mass * 1000f) * this.primaryElement.Element.specificHeatCapacity * 0.001f);
		}
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x0010867C File Offset: 0x0010687C
	public void RefreshRegistration()
	{
		base.SimUnregister();
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get("ThermalConductivityBarrier");
		this.thickness = attributeInstance.GetTotalValue();
		this.simHandle = -1;
		base.SimRegister();
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x001086BE File Offset: 0x001068BE
	public static float PotentialEnergyFlowToCreature(int cell, PrimaryElement transfererPrimaryElement, SimTemperatureTransfer temperatureTransferer, float deltaTime = 1f)
	{
		return SimUtil.CalculateEnergyFlowCreatures(cell, transfererPrimaryElement.Temperature, transfererPrimaryElement.Element.specificHeatCapacity, transfererPrimaryElement.Element.thermalConductivity, temperatureTransferer.SurfaceArea, temperatureTransferer.Thickness);
	}

	// Token: 0x04001DCA RID: 7626
	public AttributeModifier averageTemperatureTransferPerSecond;

	// Token: 0x04001DCB RID: 7627
	private PrimaryElement primaryElement;

	// Token: 0x04001DCC RID: 7628
	public RunningWeightedAverage average_kilowatts_exchanged;

	// Token: 0x04001DCD RID: 7629
	public List<AttributeModifier> NonSimTemperatureModifiers = new List<AttributeModifier>();
}
