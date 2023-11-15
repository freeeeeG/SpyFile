using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020005A7 RID: 1447
public class BuildingComplete : Building
{
	// Token: 0x06002359 RID: 9049 RVA: 0x000C1839 File Offset: 0x000BFA39
	private bool WasReplaced()
	{
		return this.replacingTileLayer != ObjectLayer.NumLayers;
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000C1848 File Offset: 0x000BFA48
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(this.Def.SceneLayer);
		base.transform.SetPosition(position);
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Rotatable component2 = base.GetComponent<Rotatable>();
		if (component != null && component2 == null)
		{
			component.Offset = this.Def.GetVisualizerOffset();
		}
		KBoxCollider2D component3 = base.GetComponent<KBoxCollider2D>();
		if (component3 != null)
		{
			Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
			component3.offset += new Vector2(visualizerOffset.x, visualizerOffset.y);
		}
		Attributes attributes = this.GetAttributes();
		foreach (Klei.AI.Attribute attribute in this.Def.attributes)
		{
			attributes.Add(attribute);
		}
		foreach (AttributeModifier attributeModifier in this.Def.attributeModifiers)
		{
			Klei.AI.Attribute attribute2 = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
			if (attributes.Get(attribute2) == null)
			{
				attributes.Add(attribute2);
			}
			attributes.Add(attributeModifier);
		}
		foreach (AttributeInstance attributeInstance in attributes)
		{
			AttributeModifier item = new AttributeModifier(attributeInstance.Id, attributeInstance.GetTotalValue(), null, false, false, true);
			this.regionModifiers.Add(item);
		}
		if (this.Def.UseStructureTemperature)
		{
			GameComps.StructureTemperatures.Add(base.gameObject);
		}
		base.Subscribe<BuildingComplete>(1606648047, BuildingComplete.OnObjectReplacedDelegate);
		if (this.Def.Entombable)
		{
			base.Subscribe<BuildingComplete>(-1089732772, BuildingComplete.OnEntombedChange);
		}
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000C1A8C File Offset: 0x000BFC8C
	private void OnEntombedChanged()
	{
		if (base.gameObject.HasTag(GameTags.Entombed))
		{
			Components.EntombedBuildings.Add(this);
			return;
		}
		Components.EntombedBuildings.Remove(this);
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000C1AB7 File Offset: 0x000BFCB7
	public override void UpdatePosition()
	{
		base.UpdatePosition();
		GameScenePartitioner.Instance.UpdatePosition(this.scenePartitionerEntry, base.GetExtents());
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000C1AD5 File Offset: 0x000BFCD5
	private void OnObjectReplaced(object data)
	{
		this.replacingTileLayer = (ObjectLayer)data;
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x000C1AE4 File Offset: 0x000BFCE4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.primaryElement = base.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		if (this.Def.IsFoundation)
		{
			foreach (int num in base.PlacementCells)
			{
				Grid.Foundation[num] = true;
				Game.Instance.roomProber.SolidChangedEvent(num, false);
			}
		}
		if (Grid.IsValidCell(cell))
		{
			Vector3 position = Grid.CellToPosCBC(cell, this.Def.SceneLayer);
			base.transform.SetPosition(position);
		}
		if (this.primaryElement != null)
		{
			if (this.primaryElement.Mass == 0f)
			{
				this.primaryElement.Mass = this.Def.Mass[0];
			}
			float temperature = this.primaryElement.Temperature;
			if (temperature > 0f && !float.IsNaN(temperature) && !float.IsInfinity(temperature))
			{
				BuildingComplete.MinKelvinSeen = Mathf.Min(BuildingComplete.MinKelvinSeen, temperature);
			}
			PrimaryElement primaryElement = this.primaryElement;
			primaryElement.setTemperatureCallback = (PrimaryElement.SetTemperatureCallback)Delegate.Combine(primaryElement.setTemperatureCallback, new PrimaryElement.SetTemperatureCallback(this.OnSetTemperature));
		}
		if (!base.gameObject.HasTag(GameTags.RocketInSpace))
		{
			this.Def.MarkArea(cell, base.Orientation, this.Def.ObjectLayer, base.gameObject);
			if (this.Def.IsTilePiece)
			{
				this.Def.MarkArea(cell, base.Orientation, this.Def.TileLayer, base.gameObject);
				this.Def.RunOnArea(cell, base.Orientation, delegate(int c)
				{
					TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
				});
			}
		}
		base.RegisterBlockTileRenderer();
		if (this.Def.PreventIdleTraversalPastBuilding)
		{
			for (int j = 0; j < base.PlacementCells.Length; j++)
			{
				Grid.PreventIdleTraversal[base.PlacementCells[j]] = true;
			}
		}
		Components.BuildingCompletes.Add(this);
		BuildingConfigManager.Instance.AddBuildingCompleteKComponents(base.gameObject, this.Def.Tag);
		this.hasSpawnedKComponents = true;
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, this, base.GetExtents(), GameScenePartitioner.Instance.completeBuildings, null);
		if (this.prefabid.HasTag(GameTags.TemplateBuilding))
		{
			Components.TemplateBuildings.Add(this);
		}
		Attributes attributes = this.GetAttributes();
		if (attributes != null)
		{
			Deconstructable component = base.GetComponent<Deconstructable>();
			if (component != null)
			{
				int k = 1;
				while (k < component.constructionElements.Length)
				{
					Tag tag = component.constructionElements[k];
					Element element = ElementLoader.GetElement(tag);
					if (element != null)
					{
						using (List<AttributeModifier>.Enumerator enumerator = element.attributeModifiers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								AttributeModifier modifier = enumerator.Current;
								attributes.Add(modifier);
							}
							goto IL_341;
						}
						goto IL_2E1;
					}
					goto IL_2E1;
					IL_341:
					k++;
					continue;
					IL_2E1:
					GameObject gameObject = Assets.TryGetPrefab(tag);
					if (!(gameObject != null))
					{
						goto IL_341;
					}
					PrefabAttributeModifiers component2 = gameObject.GetComponent<PrefabAttributeModifiers>();
					if (component2 != null)
					{
						foreach (AttributeModifier modifier2 in component2.descriptors)
						{
							attributes.Add(modifier2);
						}
						goto IL_341;
					}
					goto IL_341;
				}
			}
		}
		BuildingInventory.Instance.RegisterBuilding(this);
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x000C1E70 File Offset: 0x000C0070
	private void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		BuildingComplete.MinKelvinSeen = Mathf.Min(BuildingComplete.MinKelvinSeen, temperature);
	}

	// Token: 0x06002360 RID: 9056 RVA: 0x000C1E82 File Offset: 0x000C0082
	public void SetCreationTime(float time)
	{
		this.creationTime = time;
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x000C1E8B File Offset: 0x000C008B
	private string GetInspectSound()
	{
		return GlobalAssets.GetSound("AI_Inspect_" + base.GetComponent<KPrefabID>().PrefabTag.Name, false);
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x000C1EB0 File Offset: 0x000C00B0
	protected override void OnCleanUp()
	{
		if (Game.quitting)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		if (this.hasSpawnedKComponents)
		{
			BuildingConfigManager.Instance.DestroyBuildingCompleteKComponents(base.gameObject, this.Def.Tag);
		}
		if (this.Def.UseStructureTemperature)
		{
			GameComps.StructureTemperatures.Remove(base.gameObject);
		}
		base.OnCleanUp();
		if (!this.WasReplaced() && base.gameObject.GetMyWorldId() != 255)
		{
			int cell = Grid.PosToCell(this);
			this.Def.UnmarkArea(cell, base.Orientation, this.Def.ObjectLayer, base.gameObject);
			if (this.Def.IsTilePiece)
			{
				this.Def.UnmarkArea(cell, base.Orientation, this.Def.TileLayer, base.gameObject);
				this.Def.RunOnArea(cell, base.Orientation, delegate(int c)
				{
					TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
				});
			}
			if (this.Def.IsFoundation)
			{
				foreach (int num in base.PlacementCells)
				{
					Grid.Foundation[num] = false;
					Game.Instance.roomProber.SolidChangedEvent(num, false);
				}
			}
			if (this.Def.PreventIdleTraversalPastBuilding)
			{
				for (int j = 0; j < base.PlacementCells.Length; j++)
				{
					Grid.PreventIdleTraversal[base.PlacementCells[j]] = false;
				}
			}
		}
		if (this.WasReplaced() && this.Def.IsTilePiece && this.replacingTileLayer != this.Def.TileLayer)
		{
			int cell2 = Grid.PosToCell(this);
			this.Def.UnmarkArea(cell2, base.Orientation, this.Def.TileLayer, base.gameObject);
			this.Def.RunOnArea(cell2, base.Orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
			});
		}
		Components.BuildingCompletes.Remove(this);
		Components.EntombedBuildings.Remove(this);
		Components.TemplateBuildings.Remove(this);
		base.UnregisterBlockTileRenderer();
		BuildingInventory.Instance.UnregisterBuilding(this);
		base.Trigger(-21016276, this);
	}

	// Token: 0x04001427 RID: 5159
	[MyCmpReq]
	private Modifiers modifiers;

	// Token: 0x04001428 RID: 5160
	[MyCmpGet]
	public KPrefabID prefabid;

	// Token: 0x04001429 RID: 5161
	public bool isManuallyOperated;

	// Token: 0x0400142A RID: 5162
	public bool isArtable;

	// Token: 0x0400142B RID: 5163
	public PrimaryElement primaryElement;

	// Token: 0x0400142C RID: 5164
	[Serialize]
	public float creationTime = -1f;

	// Token: 0x0400142D RID: 5165
	private bool hasSpawnedKComponents;

	// Token: 0x0400142E RID: 5166
	private ObjectLayer replacingTileLayer = ObjectLayer.NumLayers;

	// Token: 0x0400142F RID: 5167
	public List<AttributeModifier> regionModifiers = new List<AttributeModifier>();

	// Token: 0x04001430 RID: 5168
	private static readonly EventSystem.IntraObjectHandler<BuildingComplete> OnEntombedChange = new EventSystem.IntraObjectHandler<BuildingComplete>(delegate(BuildingComplete component, object data)
	{
		component.OnEntombedChanged();
	});

	// Token: 0x04001431 RID: 5169
	private static readonly EventSystem.IntraObjectHandler<BuildingComplete> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<BuildingComplete>(delegate(BuildingComplete component, object data)
	{
		component.OnObjectReplaced(data);
	});

	// Token: 0x04001432 RID: 5170
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x04001433 RID: 5171
	public static float MinKelvinSeen = float.MaxValue;
}
