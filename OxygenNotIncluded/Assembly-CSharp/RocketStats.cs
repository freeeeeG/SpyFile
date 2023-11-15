using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200099A RID: 2458
public class RocketStats
{
	// Token: 0x060048F4 RID: 18676 RVA: 0x0019ADB6 File Offset: 0x00198FB6
	public RocketStats(CommandModule commandModule)
	{
		this.commandModule = commandModule;
	}

	// Token: 0x060048F5 RID: 18677 RVA: 0x0019ADC8 File Offset: 0x00198FC8
	public float GetRocketMaxDistance()
	{
		float totalMass = this.GetTotalMass();
		float totalThrust = this.GetTotalThrust();
		float num = ROCKETRY.CalculateMassWithPenalty(totalMass);
		return Mathf.Max(0f, totalThrust - num);
	}

	// Token: 0x060048F6 RID: 18678 RVA: 0x0019ADF5 File Offset: 0x00198FF5
	public float GetTotalMass()
	{
		return this.GetDryMass() + this.GetWetMass();
	}

	// Token: 0x060048F7 RID: 18679 RVA: 0x0019AE04 File Offset: 0x00199004
	public float GetDryMass()
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				num += component.GetComponent<PrimaryElement>().Mass;
			}
		}
		return num;
	}

	// Token: 0x060048F8 RID: 18680 RVA: 0x0019AE80 File Offset: 0x00199080
	public float GetWetMass()
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				FuelTank component2 = component.GetComponent<FuelTank>();
				OxidizerTank component3 = component.GetComponent<OxidizerTank>();
				SolidBooster component4 = component.GetComponent<SolidBooster>();
				if (component2 != null)
				{
					num += component2.storage.MassStored();
				}
				if (component3 != null)
				{
					num += component3.storage.MassStored();
				}
				if (component4 != null)
				{
					num += component4.fuelStorage.MassStored();
				}
			}
		}
		return num;
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x0019AF4C File Offset: 0x0019914C
	public Tag GetEngineFuelTag()
	{
		RocketEngine mainEngine = this.GetMainEngine();
		if (mainEngine != null)
		{
			return mainEngine.fuelTag;
		}
		return null;
	}

	// Token: 0x060048FA RID: 18682 RVA: 0x0019AF78 File Offset: 0x00199178
	public float GetTotalFuel(bool includeBoosters = false)
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			FuelTank component = gameObject.GetComponent<FuelTank>();
			Tag engineFuelTag = this.GetEngineFuelTag();
			if (component != null)
			{
				num += component.storage.GetAmountAvailable(engineFuelTag);
			}
			if (includeBoosters)
			{
				SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
				if (component2 != null)
				{
					num += component2.fuelStorage.GetAmountAvailable(component2.fuelTag);
				}
			}
		}
		return num;
	}

	// Token: 0x060048FB RID: 18683 RVA: 0x0019B028 File Offset: 0x00199228
	public float GetTotalOxidizer(bool includeBoosters = false)
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			OxidizerTank component = gameObject.GetComponent<OxidizerTank>();
			if (component != null)
			{
				num += component.GetTotalOxidizerAvailable();
			}
			if (includeBoosters)
			{
				SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
				if (component2 != null)
				{
					num += component2.fuelStorage.GetAmountAvailable(GameTags.OxyRock);
				}
			}
		}
		return num;
	}

	// Token: 0x060048FC RID: 18684 RVA: 0x0019B0C8 File Offset: 0x001992C8
	public float GetAverageOxidizerEfficiency()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		dictionary[SimHashes.LiquidOxygen.CreateTag()] = 0f;
		dictionary[SimHashes.OxyRock.CreateTag()] = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			OxidizerTank component = gameObject.GetComponent<OxidizerTank>();
			if (component != null)
			{
				foreach (KeyValuePair<Tag, float> keyValuePair in component.GetOxidizersAvailable())
				{
					if (dictionary.ContainsKey(keyValuePair.Key))
					{
						Dictionary<Tag, float> dictionary2 = dictionary;
						Tag key = keyValuePair.Key;
						dictionary2[key] += keyValuePair.Value;
					}
				}
			}
		}
		float num = 0f;
		float num2 = 0f;
		foreach (KeyValuePair<Tag, float> keyValuePair2 in dictionary)
		{
			num += keyValuePair2.Value * RocketStats.oxidizerEfficiencies[keyValuePair2.Key];
			num2 += keyValuePair2.Value;
		}
		if (num2 == 0f)
		{
			return 0f;
		}
		return num / num2 * 100f;
	}

	// Token: 0x060048FD RID: 18685 RVA: 0x0019B258 File Offset: 0x00199458
	public float GetTotalThrust()
	{
		float totalFuel = this.GetTotalFuel(false);
		float totalOxidizer = this.GetTotalOxidizer(false);
		float averageOxidizerEfficiency = this.GetAverageOxidizerEfficiency();
		RocketEngine mainEngine = this.GetMainEngine();
		if (mainEngine == null)
		{
			return 0f;
		}
		return (mainEngine.requireOxidizer ? (Mathf.Min(totalFuel, totalOxidizer) * (mainEngine.efficiency * (averageOxidizerEfficiency / 100f))) : (totalFuel * mainEngine.efficiency)) + this.GetBoosterThrust();
	}

	// Token: 0x060048FE RID: 18686 RVA: 0x0019B2C4 File Offset: 0x001994C4
	public float GetBoosterThrust()
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			SolidBooster component = gameObject.GetComponent<SolidBooster>();
			if (component != null)
			{
				float amountAvailable = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag);
				float amountAvailable2 = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.Iron).tag);
				num += component.efficiency * Mathf.Min(amountAvailable, amountAvailable2);
			}
		}
		return num;
	}

	// Token: 0x060048FF RID: 18687 RVA: 0x0019B378 File Offset: 0x00199578
	public float GetEngineEfficiency()
	{
		RocketEngine mainEngine = this.GetMainEngine();
		if (mainEngine != null)
		{
			return mainEngine.efficiency;
		}
		return 0f;
	}

	// Token: 0x06004900 RID: 18688 RVA: 0x0019B3A4 File Offset: 0x001995A4
	public RocketEngine GetMainEngine()
	{
		RocketEngine rocketEngine = null;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			rocketEngine = gameObject.GetComponent<RocketEngine>();
			if (rocketEngine != null && rocketEngine.mainEngine)
			{
				break;
			}
		}
		return rocketEngine;
	}

	// Token: 0x06004901 RID: 18689 RVA: 0x0019B418 File Offset: 0x00199618
	public float GetTotalOxidizableFuel()
	{
		float totalFuel = this.GetTotalFuel(false);
		float totalOxidizer = this.GetTotalOxidizer(false);
		return Mathf.Min(totalFuel, totalOxidizer);
	}

	// Token: 0x04003021 RID: 12321
	private CommandModule commandModule;

	// Token: 0x04003022 RID: 12322
	public static Dictionary<Tag, float> oxidizerEfficiencies = new Dictionary<Tag, float>
	{
		{
			SimHashes.OxyRock.CreateTag(),
			ROCKETRY.OXIDIZER_EFFICIENCY.LOW
		},
		{
			SimHashes.LiquidOxygen.CreateTag(),
			ROCKETRY.OXIDIZER_EFFICIENCY.HIGH
		}
	};
}
