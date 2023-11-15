using System;
using System.Collections.Generic;

// Token: 0x02000790 RID: 1936
public class EnergySim
{
	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x060035E5 RID: 13797 RVA: 0x00123825 File Offset: 0x00121A25
	public HashSet<Generator> Generators
	{
		get
		{
			return this.generators;
		}
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x0012382D File Offset: 0x00121A2D
	public void AddGenerator(Generator generator)
	{
		this.generators.Add(generator);
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x0012383C File Offset: 0x00121A3C
	public void RemoveGenerator(Generator generator)
	{
		this.generators.Remove(generator);
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x0012384B File Offset: 0x00121A4B
	public void AddManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Add(manual_generator);
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x0012385A File Offset: 0x00121A5A
	public void RemoveManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Remove(manual_generator);
	}

	// Token: 0x060035EA RID: 13802 RVA: 0x00123869 File Offset: 0x00121A69
	public void AddBattery(Battery battery)
	{
		this.batteries.Add(battery);
	}

	// Token: 0x060035EB RID: 13803 RVA: 0x00123878 File Offset: 0x00121A78
	public void RemoveBattery(Battery battery)
	{
		this.batteries.Remove(battery);
	}

	// Token: 0x060035EC RID: 13804 RVA: 0x00123887 File Offset: 0x00121A87
	public void AddEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Add(energy_consumer);
	}

	// Token: 0x060035ED RID: 13805 RVA: 0x00123896 File Offset: 0x00121A96
	public void RemoveEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Remove(energy_consumer);
	}

	// Token: 0x060035EE RID: 13806 RVA: 0x001238A8 File Offset: 0x00121AA8
	public void EnergySim200ms(float dt)
	{
		foreach (Generator generator in this.generators)
		{
			generator.EnergySim200ms(dt);
		}
		foreach (ManualGenerator manualGenerator in this.manualGenerators)
		{
			manualGenerator.EnergySim200ms(dt);
		}
		foreach (Battery battery in this.batteries)
		{
			battery.EnergySim200ms(dt);
		}
		foreach (EnergyConsumer energyConsumer in this.energyConsumers)
		{
			energyConsumer.EnergySim200ms(dt);
		}
	}

	// Token: 0x040020EA RID: 8426
	private HashSet<Generator> generators = new HashSet<Generator>();

	// Token: 0x040020EB RID: 8427
	private HashSet<ManualGenerator> manualGenerators = new HashSet<ManualGenerator>();

	// Token: 0x040020EC RID: 8428
	private HashSet<Battery> batteries = new HashSet<Battery>();

	// Token: 0x040020ED RID: 8429
	private HashSet<EnergyConsumer> energyConsumers = new HashSet<EnergyConsumer>();
}
