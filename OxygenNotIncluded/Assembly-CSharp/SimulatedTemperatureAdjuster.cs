using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000973 RID: 2419
public class SimulatedTemperatureAdjuster
{
	// Token: 0x0600473F RID: 18239 RVA: 0x00192904 File Offset: 0x00190B04
	public SimulatedTemperatureAdjuster(float simulated_temperature, float heat_capacity, float thermal_conductivity, Storage storage)
	{
		this.temperature = simulated_temperature;
		this.heatCapacity = heat_capacity;
		this.thermalConductivity = thermal_conductivity;
		this.storage = storage;
		storage.gameObject.Subscribe(824508782, new Action<object>(this.OnActivechanged));
		storage.gameObject.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		Operational component = storage.gameObject.GetComponent<Operational>();
		this.OnActivechanged(component);
	}

	// Token: 0x06004740 RID: 18240 RVA: 0x00192984 File Offset: 0x00190B84
	public List<Descriptor> GetDescriptors()
	{
		return SimulatedTemperatureAdjuster.GetDescriptors(this.temperature);
	}

	// Token: 0x06004741 RID: 18241 RVA: 0x00192994 File Offset: 0x00190B94
	public static List<Descriptor> GetDescriptors(float temperature)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedTemperature = GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
		Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.ITEM_TEMPERATURE_ADJUST, formattedTemperature), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ITEM_TEMPERATURE_ADJUST, formattedTemperature), Descriptor.DescriptorType.Effect, false);
		list.Add(item);
		return list;
	}

	// Token: 0x06004742 RID: 18242 RVA: 0x001929E4 File Offset: 0x00190BE4
	private void Register(SimTemperatureTransfer stt)
	{
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Combine(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			this.OnItemSimRegistered(stt);
		}
	}

	// Token: 0x06004743 RID: 18243 RVA: 0x00192A4C File Offset: 0x00190C4C
	private void Unregister(SimTemperatureTransfer stt)
	{
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, 0f, 0f, 0f);
		}
	}

	// Token: 0x06004744 RID: 18244 RVA: 0x00192AA4 File Offset: 0x00190CA4
	private void OnItemSimRegistered(SimTemperatureTransfer stt)
	{
		if (stt == null)
		{
			return;
		}
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			float num = this.temperature;
			float heat_capacity = this.heatCapacity;
			float thermal_conductivity = this.thermalConductivity;
			if (!this.active)
			{
				num = 0f;
				heat_capacity = 0f;
				thermal_conductivity = 0f;
			}
			SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, num, heat_capacity, thermal_conductivity);
		}
	}

	// Token: 0x06004745 RID: 18245 RVA: 0x00192B08 File Offset: 0x00190D08
	private void OnActivechanged(object data)
	{
		Operational operational = (Operational)data;
		this.active = operational.IsActive;
		if (this.active)
		{
			using (List<GameObject>.Enumerator enumerator = this.storage.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					if (gameObject != null)
					{
						SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
						this.OnItemSimRegistered(component);
					}
				}
				return;
			}
		}
		foreach (GameObject gameObject2 in this.storage.items)
		{
			if (gameObject2 != null)
			{
				SimTemperatureTransfer component2 = gameObject2.GetComponent<SimTemperatureTransfer>();
				this.Unregister(component2);
			}
		}
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x00192BE8 File Offset: 0x00190DE8
	public void CleanUp()
	{
		this.storage.gameObject.Unsubscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
				this.Unregister(component);
			}
		}
	}

	// Token: 0x06004747 RID: 18247 RVA: 0x00192C74 File Offset: 0x00190E74
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
		if (component == null)
		{
			return;
		}
		Pickupable component2 = gameObject.GetComponent<Pickupable>();
		if (component2 == null)
		{
			return;
		}
		if (this.active && component2.storage == this.storage)
		{
			this.Register(component);
			return;
		}
		this.Unregister(component);
	}

	// Token: 0x04002F32 RID: 12082
	private float temperature;

	// Token: 0x04002F33 RID: 12083
	private float heatCapacity;

	// Token: 0x04002F34 RID: 12084
	private float thermalConductivity;

	// Token: 0x04002F35 RID: 12085
	private bool active;

	// Token: 0x04002F36 RID: 12086
	private Storage storage;
}
