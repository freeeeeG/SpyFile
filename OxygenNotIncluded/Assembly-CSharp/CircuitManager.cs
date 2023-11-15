using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using STRINGS;
using UnityEngine;

// Token: 0x020006CF RID: 1743
public class CircuitManager
{
	// Token: 0x06002F5B RID: 12123 RVA: 0x000F9C86 File Offset: 0x000F7E86
	public void Connect(Generator generator)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.generators.Add(generator);
		this.dirty = true;
	}

	// Token: 0x06002F5C RID: 12124 RVA: 0x000F9CA4 File Offset: 0x000F7EA4
	public void Disconnect(Generator generator)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.generators.Remove(generator);
		this.dirty = true;
	}

	// Token: 0x06002F5D RID: 12125 RVA: 0x000F9CC2 File Offset: 0x000F7EC2
	public void Connect(IEnergyConsumer consumer)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.consumers.Add(consumer);
		this.dirty = true;
	}

	// Token: 0x06002F5E RID: 12126 RVA: 0x000F9CE0 File Offset: 0x000F7EE0
	public void Disconnect(IEnergyConsumer consumer, bool isDestroy)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.consumers.Remove(consumer);
		if (!isDestroy)
		{
			consumer.SetConnectionStatus(CircuitManager.ConnectionStatus.NotConnected);
		}
		this.dirty = true;
	}

	// Token: 0x06002F5F RID: 12127 RVA: 0x000F9D08 File Offset: 0x000F7F08
	public void Connect(WireUtilityNetworkLink bridge)
	{
		this.bridges.Add(bridge);
		this.dirty = true;
	}

	// Token: 0x06002F60 RID: 12128 RVA: 0x000F9D1E File Offset: 0x000F7F1E
	public void Disconnect(WireUtilityNetworkLink bridge)
	{
		this.bridges.Remove(bridge);
		this.dirty = true;
	}

	// Token: 0x06002F61 RID: 12129 RVA: 0x000F9D34 File Offset: 0x000F7F34
	public float GetPowerDraw(ushort circuitID, Generator generator)
	{
		float result = 0f;
		if ((int)circuitID < this.circuitInfo.Count)
		{
			CircuitManager.CircuitInfo value = this.circuitInfo[(int)circuitID];
			this.circuitInfo[(int)circuitID] = value;
			this.circuitInfo[(int)circuitID] = value;
		}
		return result;
	}

	// Token: 0x06002F62 RID: 12130 RVA: 0x000F9D7C File Offset: 0x000F7F7C
	public ushort GetCircuitID(int cell)
	{
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		return (ushort)((networkForCell == null) ? 65535 : networkForCell.id);
	}

	// Token: 0x06002F63 RID: 12131 RVA: 0x000F9DAC File Offset: 0x000F7FAC
	public ushort GetVirtualCircuitID(object virtualKey)
	{
		UtilityNetwork networkForVirtualKey = Game.Instance.electricalConduitSystem.GetNetworkForVirtualKey(virtualKey);
		return (ushort)((networkForVirtualKey == null) ? 65535 : networkForVirtualKey.id);
	}

	// Token: 0x06002F64 RID: 12132 RVA: 0x000F9DDB File Offset: 0x000F7FDB
	public ushort GetCircuitID(ICircuitConnected ent)
	{
		if (!ent.IsVirtual)
		{
			return this.GetCircuitID(ent.PowerCell);
		}
		return this.GetVirtualCircuitID(ent.VirtualCircuitKey);
	}

	// Token: 0x06002F65 RID: 12133 RVA: 0x000F9DFE File Offset: 0x000F7FFE
	public void Sim200msFirst(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x06002F66 RID: 12134 RVA: 0x000F9E07 File Offset: 0x000F8007
	public void RenderEveryTick(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x06002F67 RID: 12135 RVA: 0x000F9E10 File Offset: 0x000F8010
	private void Refresh(float dt)
	{
		UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem = Game.Instance.electricalConduitSystem;
		if (electricalConduitSystem.IsDirty || this.dirty)
		{
			electricalConduitSystem.Update();
			IList<UtilityNetwork> networks = electricalConduitSystem.GetNetworks();
			while (this.circuitInfo.Count < networks.Count)
			{
				CircuitManager.CircuitInfo circuitInfo = new CircuitManager.CircuitInfo
				{
					generators = new List<Generator>(),
					consumers = new List<IEnergyConsumer>(),
					batteries = new List<Battery>(),
					inputTransformers = new List<Battery>(),
					outputTransformers = new List<Generator>()
				};
				circuitInfo.bridgeGroups = new List<WireUtilityNetworkLink>[5];
				for (int i = 0; i < circuitInfo.bridgeGroups.Length; i++)
				{
					circuitInfo.bridgeGroups[i] = new List<WireUtilityNetworkLink>();
				}
				this.circuitInfo.Add(circuitInfo);
			}
			this.Rebuild();
		}
	}

	// Token: 0x06002F68 RID: 12136 RVA: 0x000F9EF0 File Offset: 0x000F80F0
	public void Rebuild()
	{
		for (int i = 0; i < this.circuitInfo.Count; i++)
		{
			CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[i];
			circuitInfo.generators.Clear();
			circuitInfo.consumers.Clear();
			circuitInfo.batteries.Clear();
			circuitInfo.inputTransformers.Clear();
			circuitInfo.outputTransformers.Clear();
			circuitInfo.minBatteryPercentFull = 1f;
			for (int j = 0; j < circuitInfo.bridgeGroups.Length; j++)
			{
				circuitInfo.bridgeGroups[j].Clear();
			}
			this.circuitInfo[i] = circuitInfo;
		}
		this.consumersShadow.AddRange(this.consumers);
		foreach (IEnergyConsumer energyConsumer in this.consumersShadow)
		{
			ushort circuitID = this.GetCircuitID(energyConsumer);
			if (circuitID != 65535)
			{
				Battery battery = energyConsumer as Battery;
				if (battery != null)
				{
					CircuitManager.CircuitInfo circuitInfo2 = this.circuitInfo[(int)circuitID];
					if (battery.powerTransformer != null)
					{
						circuitInfo2.inputTransformers.Add(battery);
					}
					else
					{
						circuitInfo2.batteries.Add(battery);
						circuitInfo2.minBatteryPercentFull = Mathf.Min(this.circuitInfo[(int)circuitID].minBatteryPercentFull, battery.PercentFull);
					}
					this.circuitInfo[(int)circuitID] = circuitInfo2;
				}
				else
				{
					this.circuitInfo[(int)circuitID].consumers.Add(energyConsumer);
				}
			}
		}
		this.consumersShadow.Clear();
		for (int k = 0; k < this.circuitInfo.Count; k++)
		{
			this.circuitInfo[k].consumers.Sort((IEnergyConsumer a, IEnergyConsumer b) => a.WattsNeededWhenActive.CompareTo(b.WattsNeededWhenActive));
		}
		foreach (Generator generator in this.generators)
		{
			ushort circuitID2 = this.GetCircuitID(generator);
			if (circuitID2 != 65535)
			{
				if (generator.GetType() == typeof(PowerTransformer))
				{
					this.circuitInfo[(int)circuitID2].outputTransformers.Add(generator);
				}
				else
				{
					this.circuitInfo[(int)circuitID2].generators.Add(generator);
				}
			}
		}
		foreach (WireUtilityNetworkLink wireUtilityNetworkLink in this.bridges)
		{
			ushort circuitID3 = this.GetCircuitID(wireUtilityNetworkLink);
			if (circuitID3 != 65535)
			{
				Wire.WattageRating maxWattageRating = wireUtilityNetworkLink.GetMaxWattageRating();
				this.circuitInfo[(int)circuitID3].bridgeGroups[(int)maxWattageRating].Add(wireUtilityNetworkLink);
			}
		}
		this.dirty = false;
	}

	// Token: 0x06002F69 RID: 12137 RVA: 0x000FA1C8 File Offset: 0x000F83C8
	private float GetBatteryJoulesAvailable(List<Battery> batteries, out int num_powered)
	{
		float result = 0f;
		num_powered = 0;
		for (int i = 0; i < batteries.Count; i++)
		{
			if (batteries[i].JoulesAvailable > 0f)
			{
				result = batteries[i].JoulesAvailable;
				num_powered = batteries.Count - i;
				break;
			}
		}
		return result;
	}

	// Token: 0x06002F6A RID: 12138 RVA: 0x000FA21C File Offset: 0x000F841C
	public void Sim200msLast(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime < 0.2f)
		{
			return;
		}
		this.elapsedTime -= 0.2f;
		for (int i = 0; i < this.circuitInfo.Count; i++)
		{
			CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[i];
			circuitInfo.wattsUsed = 0f;
			this.activeGenerators.Clear();
			List<Generator> list = circuitInfo.generators;
			List<IEnergyConsumer> list2 = circuitInfo.consumers;
			List<Battery> batteries = circuitInfo.batteries;
			List<Generator> outputTransformers = circuitInfo.outputTransformers;
			batteries.Sort((Battery a, Battery b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			bool flag = false;
			bool flag2 = list.Count > 0;
			for (int j = 0; j < list.Count; j++)
			{
				Generator generator = list[j];
				if (generator.JoulesAvailable > 0f)
				{
					flag = true;
					this.activeGenerators.Add(generator);
				}
			}
			this.activeGenerators.Sort((Generator a, Generator b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			if (!flag)
			{
				for (int k = 0; k < outputTransformers.Count; k++)
				{
					if (outputTransformers[k].JoulesAvailable > 0f)
					{
						flag = true;
					}
				}
			}
			float num = 1f;
			for (int l = 0; l < batteries.Count; l++)
			{
				Battery battery = batteries[l];
				if (battery.JoulesAvailable > 0f)
				{
					flag = true;
				}
				num = Mathf.Min(num, battery.PercentFull);
			}
			for (int m = 0; m < circuitInfo.inputTransformers.Count; m++)
			{
				Battery battery2 = circuitInfo.inputTransformers[m];
				num = Mathf.Min(num, battery2.PercentFull);
			}
			circuitInfo.minBatteryPercentFull = num;
			if (flag)
			{
				for (int n = 0; n < list2.Count; n++)
				{
					IEnergyConsumer energyConsumer = list2[n];
					float num2 = energyConsumer.WattsUsed * 0.2f;
					if (num2 > 0f)
					{
						bool flag3 = false;
						for (int num3 = 0; num3 < this.activeGenerators.Count; num3++)
						{
							Generator g = this.activeGenerators[num3];
							num2 = this.PowerFromGenerator(num2, g, energyConsumer);
							if (num2 <= 0f)
							{
								flag3 = true;
								break;
							}
						}
						if (!flag3)
						{
							for (int num4 = 0; num4 < outputTransformers.Count; num4++)
							{
								Generator g2 = outputTransformers[num4];
								num2 = this.PowerFromGenerator(num2, g2, energyConsumer);
								if (num2 <= 0f)
								{
									flag3 = true;
									break;
								}
							}
						}
						if (!flag3)
						{
							num2 = this.PowerFromBatteries(num2, batteries, energyConsumer);
							flag3 = (num2 <= 0.01f);
						}
						if (flag3)
						{
							circuitInfo.wattsUsed += energyConsumer.WattsUsed;
						}
						else
						{
							circuitInfo.wattsUsed += energyConsumer.WattsUsed - num2 / 0.2f;
						}
						energyConsumer.SetConnectionStatus(flag3 ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
					}
					else
					{
						energyConsumer.SetConnectionStatus(flag ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
					}
				}
			}
			else if (flag2)
			{
				for (int num5 = 0; num5 < list2.Count; num5++)
				{
					list2[num5].SetConnectionStatus(CircuitManager.ConnectionStatus.Unpowered);
				}
			}
			else
			{
				for (int num6 = 0; num6 < list2.Count; num6++)
				{
					list2[num6].SetConnectionStatus(CircuitManager.ConnectionStatus.NotConnected);
				}
			}
			this.circuitInfo[i] = circuitInfo;
		}
		for (int num7 = 0; num7 < this.circuitInfo.Count; num7++)
		{
			CircuitManager.CircuitInfo circuitInfo2 = this.circuitInfo[num7];
			circuitInfo2.batteries.Sort((Battery a, Battery b) => (a.Capacity - a.JoulesAvailable).CompareTo(b.Capacity - b.JoulesAvailable));
			circuitInfo2.inputTransformers.Sort((Battery a, Battery b) => (a.Capacity - a.JoulesAvailable).CompareTo(b.Capacity - b.JoulesAvailable));
			circuitInfo2.generators.Sort((Generator a, Generator b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			float num8 = 0f;
			this.ChargeTransformers<Generator>(circuitInfo2.inputTransformers, circuitInfo2.generators, ref num8);
			this.ChargeTransformers<Generator>(circuitInfo2.inputTransformers, circuitInfo2.outputTransformers, ref num8);
			float num9 = 0f;
			this.ChargeBatteries(circuitInfo2.batteries, circuitInfo2.generators, ref num9);
			this.ChargeBatteries(circuitInfo2.batteries, circuitInfo2.outputTransformers, ref num9);
			circuitInfo2.minBatteryPercentFull = 1f;
			for (int num10 = 0; num10 < circuitInfo2.batteries.Count; num10++)
			{
				float percentFull = circuitInfo2.batteries[num10].PercentFull;
				if (percentFull < circuitInfo2.minBatteryPercentFull)
				{
					circuitInfo2.minBatteryPercentFull = percentFull;
				}
			}
			for (int num11 = 0; num11 < circuitInfo2.inputTransformers.Count; num11++)
			{
				float percentFull2 = circuitInfo2.inputTransformers[num11].PercentFull;
				if (percentFull2 < circuitInfo2.minBatteryPercentFull)
				{
					circuitInfo2.minBatteryPercentFull = percentFull2;
				}
			}
			circuitInfo2.wattsUsed += num8 / 0.2f;
			this.circuitInfo[num7] = circuitInfo2;
		}
		for (int num12 = 0; num12 < this.circuitInfo.Count; num12++)
		{
			CircuitManager.CircuitInfo circuitInfo3 = this.circuitInfo[num12];
			circuitInfo3.batteries.Sort((Battery a, Battery b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			float num13 = 0f;
			this.ChargeTransformers<Battery>(circuitInfo3.inputTransformers, circuitInfo3.batteries, ref num13);
			circuitInfo3.wattsUsed += num13 / 0.2f;
			this.circuitInfo[num12] = circuitInfo3;
		}
		for (int num14 = 0; num14 < this.circuitInfo.Count; num14++)
		{
			CircuitManager.CircuitInfo circuitInfo4 = this.circuitInfo[num14];
			bool is_connected_to_something_useful = circuitInfo4.generators.Count + circuitInfo4.consumers.Count + circuitInfo4.outputTransformers.Count > 0;
			this.UpdateBatteryConnectionStatus(circuitInfo4.batteries, is_connected_to_something_useful, num14);
			bool flag4 = circuitInfo4.generators.Count > 0 || circuitInfo4.outputTransformers.Count > 0;
			if (!flag4)
			{
				using (List<Battery>.Enumerator enumerator = circuitInfo4.batteries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.JoulesAvailable > 0f)
						{
							flag4 = true;
							break;
						}
					}
				}
			}
			this.UpdateBatteryConnectionStatus(circuitInfo4.inputTransformers, flag4, num14);
			this.circuitInfo[num14] = circuitInfo4;
			for (int num15 = 0; num15 < circuitInfo4.generators.Count; num15++)
			{
				Generator generator2 = circuitInfo4.generators[num15];
				ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -generator2.JoulesAvailable, StringFormatter.Replace(BUILDINGS.PREFABS.GENERATOR.OVERPRODUCTION, "{Generator}", generator2.gameObject.GetProperName()), null);
			}
		}
		for (int num16 = 0; num16 < this.circuitInfo.Count; num16++)
		{
			CircuitManager.CircuitInfo circuitInfo5 = this.circuitInfo[num16];
			this.CheckCircuitOverloaded(0.2f, num16, circuitInfo5.wattsUsed);
		}
	}

	// Token: 0x06002F6B RID: 12139 RVA: 0x000FA9B0 File Offset: 0x000F8BB0
	private float PowerFromBatteries(float joules_needed, List<Battery> batteries, IEnergyConsumer c)
	{
		int num2;
		do
		{
			float num = this.GetBatteryJoulesAvailable(batteries, out num2) * (float)num2;
			float num3 = (num < joules_needed) ? num : joules_needed;
			joules_needed -= num3;
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, -num3, c.Name, null);
			float joules = num3 / (float)num2;
			for (int i = batteries.Count - num2; i < batteries.Count; i++)
			{
				batteries[i].ConsumeEnergy(joules);
			}
		}
		while (joules_needed >= 0.01f && num2 > 0);
		return joules_needed;
	}

	// Token: 0x06002F6C RID: 12140 RVA: 0x000FAA2C File Offset: 0x000F8C2C
	private float PowerFromGenerator(float joules_needed, Generator g, IEnergyConsumer c)
	{
		float num = Mathf.Min(g.JoulesAvailable, joules_needed);
		joules_needed -= num;
		g.ApplyDeltaJoules(-num, false);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, -num, c.Name, null);
		return joules_needed;
	}

	// Token: 0x06002F6D RID: 12141 RVA: 0x000FAA6C File Offset: 0x000F8C6C
	private void ChargeBatteries(List<Battery> sink_batteries, List<Generator> source_generators, ref float joules_used)
	{
		if (sink_batteries.Count == 0)
		{
			return;
		}
		foreach (Generator generator in source_generators)
		{
			for (bool flag = true; flag && generator.JoulesAvailable >= 1f; flag = this.ChargeBatteriesFromGenerator(sink_batteries, generator, ref joules_used))
			{
			}
		}
	}

	// Token: 0x06002F6E RID: 12142 RVA: 0x000FAADC File Offset: 0x000F8CDC
	private bool ChargeBatteriesFromGenerator(List<Battery> sink_batteries, Generator source_generator, ref float joules_used)
	{
		float num = source_generator.JoulesAvailable;
		float num2 = 0f;
		for (int i = 0; i < sink_batteries.Count; i++)
		{
			Battery battery = sink_batteries[i];
			if (battery != null && source_generator != null && battery.gameObject != source_generator.gameObject)
			{
				float num3 = battery.Capacity - battery.JoulesAvailable;
				if (num3 > 0f)
				{
					float num4 = Mathf.Min(num3, num / (float)(sink_batteries.Count - i));
					battery.AddEnergy(num4);
					num -= num4;
					num2 += num4;
				}
			}
		}
		if (num2 > 0f)
		{
			source_generator.ApplyDeltaJoules(-num2, false);
			joules_used += num2;
			return true;
		}
		return false;
	}

	// Token: 0x06002F6F RID: 12143 RVA: 0x000FAB8C File Offset: 0x000F8D8C
	private void UpdateBatteryConnectionStatus(List<Battery> batteries, bool is_connected_to_something_useful, int circuit_id)
	{
		foreach (Battery battery in batteries)
		{
			if (!(battery == null))
			{
				if (battery.powerTransformer == null)
				{
					battery.SetConnectionStatus(is_connected_to_something_useful ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.NotConnected);
				}
				else if ((int)this.GetCircuitID(battery) == circuit_id)
				{
					battery.SetConnectionStatus(is_connected_to_something_useful ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
				}
			}
		}
	}

	// Token: 0x06002F70 RID: 12144 RVA: 0x000FAC10 File Offset: 0x000F8E10
	private void ChargeTransformer<T>(Battery sink_transformer, List<T> source_energy_producers, ref float joules_used) where T : IEnergyProducer
	{
		if (source_energy_producers.Count <= 0)
		{
			return;
		}
		float num = Mathf.Min(sink_transformer.Capacity - sink_transformer.JoulesAvailable, sink_transformer.ChargeCapacity);
		if (num <= 0f)
		{
			return;
		}
		float num2 = num;
		float num3 = 0f;
		for (int i = 0; i < source_energy_producers.Count; i++)
		{
			T t = source_energy_producers[i];
			if (t.JoulesAvailable > 0f)
			{
				float num4 = Mathf.Min(t.JoulesAvailable, num2 / (float)(source_energy_producers.Count - i));
				t.ConsumeEnergy(num4);
				num2 -= num4;
				num3 += num4;
			}
		}
		sink_transformer.AddEnergy(num3);
		joules_used += num3;
	}

	// Token: 0x06002F71 RID: 12145 RVA: 0x000FACC4 File Offset: 0x000F8EC4
	private void ChargeTransformers<T>(List<Battery> sink_transformers, List<T> source_energy_producers, ref float joules_used) where T : IEnergyProducer
	{
		foreach (Battery sink_transformer in sink_transformers)
		{
			this.ChargeTransformer<T>(sink_transformer, source_energy_producers, ref joules_used);
		}
	}

	// Token: 0x06002F72 RID: 12146 RVA: 0x000FAD14 File Offset: 0x000F8F14
	private void CheckCircuitOverloaded(float dt, int id, float watts_used)
	{
		UtilityNetwork networkByID = Game.Instance.electricalConduitSystem.GetNetworkByID(id);
		if (networkByID != null)
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)networkByID;
			if (electricalUtilityNetwork != null)
			{
				electricalUtilityNetwork.UpdateOverloadTime(dt, watts_used, this.circuitInfo[id].bridgeGroups);
			}
		}
	}

	// Token: 0x06002F73 RID: 12147 RVA: 0x000FAD58 File Offset: 0x000F8F58
	public float GetWattsUsedByCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		return this.circuitInfo[(int)circuitID].wattsUsed;
	}

	// Token: 0x06002F74 RID: 12148 RVA: 0x000FAD7C File Offset: 0x000F8F7C
	public float GetWattsNeededWhenActive(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		float num = 0f;
		foreach (IEnergyConsumer energyConsumer in this.circuitInfo[(int)circuitID].consumers)
		{
			num += energyConsumer.WattsNeededWhenActive;
		}
		foreach (Battery battery in this.circuitInfo[(int)circuitID].inputTransformers)
		{
			num += battery.WattsNeededWhenActive;
		}
		return num;
	}

	// Token: 0x06002F75 RID: 12149 RVA: 0x000FAE44 File Offset: 0x000F9044
	public float GetWattsGeneratedByCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		float num = 0f;
		foreach (Generator generator in this.circuitInfo[(int)circuitID].generators)
		{
			if (!(generator == null) && generator.IsProducingPower())
			{
				num += generator.WattageRating;
			}
		}
		return num;
	}

	// Token: 0x06002F76 RID: 12150 RVA: 0x000FAECC File Offset: 0x000F90CC
	public float GetPotentialWattsGeneratedByCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		float num = 0f;
		foreach (Generator generator in this.circuitInfo[(int)circuitID].generators)
		{
			num += generator.WattageRating;
		}
		return num;
	}

	// Token: 0x06002F77 RID: 12151 RVA: 0x000FAF44 File Offset: 0x000F9144
	public float GetJoulesAvailableOnCircuit(ushort circuitID)
	{
		int num;
		return this.GetBatteryJoulesAvailable(this.GetBatteriesOnCircuit(circuitID), out num) * (float)num;
	}

	// Token: 0x06002F78 RID: 12152 RVA: 0x000FAF63 File Offset: 0x000F9163
	public ReadOnlyCollection<Generator> GetGeneratorsOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].generators.AsReadOnly();
	}

	// Token: 0x06002F79 RID: 12153 RVA: 0x000FAF85 File Offset: 0x000F9185
	public ReadOnlyCollection<IEnergyConsumer> GetConsumersOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].consumers.AsReadOnly();
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x000FAFA7 File Offset: 0x000F91A7
	public ReadOnlyCollection<Battery> GetTransformersOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].inputTransformers.AsReadOnly();
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x000FAFC9 File Offset: 0x000F91C9
	public List<Battery> GetBatteriesOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].batteries;
	}

	// Token: 0x06002F7C RID: 12156 RVA: 0x000FAFE6 File Offset: 0x000F91E6
	public float GetMinBatteryPercentFullOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return 0f;
		}
		return this.circuitInfo[(int)circuitID].minBatteryPercentFull;
	}

	// Token: 0x06002F7D RID: 12157 RVA: 0x000FB007 File Offset: 0x000F9207
	public bool HasBatteries(ushort circuitID)
	{
		return circuitID != ushort.MaxValue && this.circuitInfo[(int)circuitID].batteries.Count + this.circuitInfo[(int)circuitID].inputTransformers.Count > 0;
	}

	// Token: 0x06002F7E RID: 12158 RVA: 0x000FB043 File Offset: 0x000F9243
	public bool HasGenerators(ushort circuitID)
	{
		return circuitID != ushort.MaxValue && this.circuitInfo[(int)circuitID].generators.Count + this.circuitInfo[(int)circuitID].outputTransformers.Count > 0;
	}

	// Token: 0x06002F7F RID: 12159 RVA: 0x000FB07F File Offset: 0x000F927F
	public bool HasGenerators()
	{
		return this.generators.Count > 0;
	}

	// Token: 0x06002F80 RID: 12160 RVA: 0x000FB08F File Offset: 0x000F928F
	public bool HasConsumers(ushort circuitID)
	{
		return circuitID != ushort.MaxValue && this.circuitInfo[(int)circuitID].consumers.Count > 0;
	}

	// Token: 0x06002F81 RID: 12161 RVA: 0x000FB0B4 File Offset: 0x000F92B4
	public float GetMaxSafeWattageForCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return 0f;
		}
		ElectricalUtilityNetwork electricalUtilityNetwork = Game.Instance.electricalConduitSystem.GetNetworkByID((int)circuitID) as ElectricalUtilityNetwork;
		if (electricalUtilityNetwork == null)
		{
			return 0f;
		}
		return electricalUtilityNetwork.GetMaxSafeWattage();
	}

	// Token: 0x04001C27 RID: 7207
	public const ushort INVALID_ID = 65535;

	// Token: 0x04001C28 RID: 7208
	private const int SimUpdateSortKey = 1000;

	// Token: 0x04001C29 RID: 7209
	private const float MIN_POWERED_THRESHOLD = 0.01f;

	// Token: 0x04001C2A RID: 7210
	private bool dirty = true;

	// Token: 0x04001C2B RID: 7211
	private HashSet<Generator> generators = new HashSet<Generator>();

	// Token: 0x04001C2C RID: 7212
	private HashSet<IEnergyConsumer> consumers = new HashSet<IEnergyConsumer>();

	// Token: 0x04001C2D RID: 7213
	private HashSet<WireUtilityNetworkLink> bridges = new HashSet<WireUtilityNetworkLink>();

	// Token: 0x04001C2E RID: 7214
	private float elapsedTime;

	// Token: 0x04001C2F RID: 7215
	private List<CircuitManager.CircuitInfo> circuitInfo = new List<CircuitManager.CircuitInfo>();

	// Token: 0x04001C30 RID: 7216
	private List<IEnergyConsumer> consumersShadow = new List<IEnergyConsumer>();

	// Token: 0x04001C31 RID: 7217
	private List<Generator> activeGenerators = new List<Generator>();

	// Token: 0x0200140B RID: 5131
	private struct CircuitInfo
	{
		// Token: 0x04006423 RID: 25635
		public List<Generator> generators;

		// Token: 0x04006424 RID: 25636
		public List<IEnergyConsumer> consumers;

		// Token: 0x04006425 RID: 25637
		public List<Battery> batteries;

		// Token: 0x04006426 RID: 25638
		public List<Battery> inputTransformers;

		// Token: 0x04006427 RID: 25639
		public List<Generator> outputTransformers;

		// Token: 0x04006428 RID: 25640
		public List<WireUtilityNetworkLink>[] bridgeGroups;

		// Token: 0x04006429 RID: 25641
		public float minBatteryPercentFull;

		// Token: 0x0400642A RID: 25642
		public float wattsUsed;
	}

	// Token: 0x0200140C RID: 5132
	public enum ConnectionStatus
	{
		// Token: 0x0400642C RID: 25644
		NotConnected,
		// Token: 0x0400642D RID: 25645
		Unpowered,
		// Token: 0x0400642E RID: 25646
		Powered
	}
}
