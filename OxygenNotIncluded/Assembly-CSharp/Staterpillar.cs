using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000705 RID: 1797
public class Staterpillar : KMonoBehaviour
{
	// Token: 0x0600316B RID: 12651 RVA: 0x00106F64 File Offset: 0x00105164
	protected override void OnPrefabInit()
	{
		this.dummyElement = new List<Tag>
		{
			SimHashes.Unobtanium.CreateTag()
		};
		this.connectorDef = Assets.GetBuildingDef(this.connectorDefId);
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x00106F92 File Offset: 0x00105192
	public void SpawnConnectorBuilding(int targetCell)
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			this.SpawnGenerator(targetCell);
			return;
		}
		this.SpawnConduitConnector(targetCell);
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x00106FB0 File Offset: 0x001051B0
	public void DestroyOrphanedConnectorBuilding()
	{
		KPrefabID building = this.GetConnectorBuilding();
		if (building != null)
		{
			this.connectorRef.Set(null);
			this.cachedGenerator = null;
			this.cachedConduitDispenser = null;
			GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Connector building", delegate(object o)
			{
				if (building != null)
				{
					Util.KDestroyGameObject(building.gameObject);
				}
			}, null, null);
		}
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x00107015 File Offset: 0x00105215
	public void EnableConnector()
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			this.EnableGenerator();
			return;
		}
		this.EnableConduitConnector();
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x0010702E File Offset: 0x0010522E
	public bool IsConnectorBuildingSpawned()
	{
		return this.GetConnectorBuilding() != null;
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x0010703C File Offset: 0x0010523C
	public bool IsConnected()
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			return this.GetGenerator().CircuitID != ushort.MaxValue;
		}
		return this.GetConduitDispenser().IsConnected;
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x00107069 File Offset: 0x00105269
	public KPrefabID GetConnectorBuilding()
	{
		return this.connectorRef.Get();
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x00107078 File Offset: 0x00105278
	private void SpawnConduitConnector(int targetCell)
	{
		if (this.GetConduitDispenser() == null)
		{
			GameObject gameObject = this.connectorDef.Build(targetCell, Orientation.R180, null, this.dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature, true, -1f);
			this.connectorRef = new Ref<KPrefabID>(gameObject.GetComponent<KPrefabID>());
			gameObject.SetActive(true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
		}
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x001070E2 File Offset: 0x001052E2
	private void EnableConduitConnector()
	{
		ConduitDispenser conduitDispenser = this.GetConduitDispenser();
		conduitDispenser.GetComponent<BuildingCellVisualizer>().enabled = true;
		conduitDispenser.storage = base.GetComponent<Storage>();
		conduitDispenser.SetOnState(true);
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x00107108 File Offset: 0x00105308
	public ConduitDispenser GetConduitDispenser()
	{
		if (this.cachedConduitDispenser == null)
		{
			KPrefabID kprefabID = this.connectorRef.Get();
			if (kprefabID != null)
			{
				this.cachedConduitDispenser = kprefabID.GetComponent<ConduitDispenser>();
			}
		}
		return this.cachedConduitDispenser;
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x0010714C File Offset: 0x0010534C
	private void DestroyOrphanedConduitDispenserBuilding()
	{
		ConduitDispenser dispenser = this.GetConduitDispenser();
		if (dispenser != null)
		{
			this.connectorRef.Set(null);
			GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Dispenser", delegate(object o)
			{
				if (dispenser != null)
				{
					Util.KDestroyGameObject(dispenser.gameObject);
				}
			}, null, null);
		}
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x001071A4 File Offset: 0x001053A4
	private void SpawnGenerator(int targetCell)
	{
		StaterpillarGenerator generator = this.GetGenerator();
		GameObject gameObject = null;
		if (generator != null)
		{
			gameObject = generator.gameObject;
		}
		if (!gameObject)
		{
			gameObject = this.connectorDef.Build(targetCell, Orientation.R180, null, this.dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature, true, -1f);
			StaterpillarGenerator component = gameObject.GetComponent<StaterpillarGenerator>();
			component.parent = new Ref<Staterpillar>(this);
			this.connectorRef = new Ref<KPrefabID>(component.GetComponent<KPrefabID>());
			gameObject.SetActive(true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
			component.enabled = false;
		}
		Attributes attributes = gameObject.gameObject.GetAttributes();
		bool flag = base.gameObject.GetSMI<WildnessMonitor.Instance>().wildness.value > 0f;
		if (flag)
		{
			attributes.Add(this.wildMod);
		}
		bool flag2 = base.gameObject.GetComponent<Effects>().HasEffect("Unhappy");
		CreatureCalorieMonitor.Instance smi = base.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
		if (smi.IsHungry() || flag2)
		{
			float calories0to = smi.GetCalories0to1();
			float num = 1f;
			if (calories0to <= 0f)
			{
				num = (flag ? 0.1f : 0.025f);
			}
			else if (calories0to <= 0.3f)
			{
				num = 0.5f;
			}
			else if (calories0to <= 0.5f)
			{
				num = 0.75f;
			}
			if (num < 1f)
			{
				float num2;
				if (flag)
				{
					num2 = Mathf.Lerp(0f, 25f, 1f - num);
				}
				else
				{
					num2 = (1f - num) * 100f;
				}
				AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -num2, BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.HUNGRY, false, false, true);
				attributes.Add(modifier);
			}
		}
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x00107362 File Offset: 0x00105562
	private void EnableGenerator()
	{
		StaterpillarGenerator generator = this.GetGenerator();
		generator.enabled = true;
		generator.GetComponent<BuildingCellVisualizer>().enabled = true;
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x0010737C File Offset: 0x0010557C
	public StaterpillarGenerator GetGenerator()
	{
		if (this.cachedGenerator == null)
		{
			KPrefabID kprefabID = this.connectorRef.Get();
			if (kprefabID != null)
			{
				this.cachedGenerator = kprefabID.GetComponent<StaterpillarGenerator>();
			}
		}
		return this.cachedGenerator;
	}

	// Token: 0x04001DA1 RID: 7585
	public ObjectLayer conduitLayer;

	// Token: 0x04001DA2 RID: 7586
	public string connectorDefId;

	// Token: 0x04001DA3 RID: 7587
	private IList<Tag> dummyElement;

	// Token: 0x04001DA4 RID: 7588
	private BuildingDef connectorDef;

	// Token: 0x04001DA5 RID: 7589
	[Serialize]
	private Ref<KPrefabID> connectorRef = new Ref<KPrefabID>();

	// Token: 0x04001DA6 RID: 7590
	private AttributeModifier wildMod = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -75f, BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.WILD, false, false, true);

	// Token: 0x04001DA7 RID: 7591
	private ConduitDispenser cachedConduitDispenser;

	// Token: 0x04001DA8 RID: 7592
	private StaterpillarGenerator cachedGenerator;
}
