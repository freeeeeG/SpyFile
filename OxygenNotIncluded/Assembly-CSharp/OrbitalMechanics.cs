﻿using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020008CB RID: 2251
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/OrbitalMechanics")]
public class OrbitalMechanics : KMonoBehaviour
{
	// Token: 0x06004129 RID: 16681 RVA: 0x0016CB9F File Offset: 0x0016AD9F
	protected override void OnPrefabInit()
	{
		base.Subscribe<OrbitalMechanics>(-1298331547, this.OnClusterLocationChangedDelegate);
	}

	// Token: 0x0600412A RID: 16682 RVA: 0x0016CBB4 File Offset: 0x0016ADB4
	private void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		this.UpdateLocation(clusterLocationChangedEvent.newLocation);
	}

	// Token: 0x0600412B RID: 16683 RVA: 0x0016CBD4 File Offset: 0x0016ADD4
	protected override void OnCleanUp()
	{
		if (this.orbitingObjects != null)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					Util.KDestroyGameObject(@ref.Get());
				}
			}
		}
	}

	// Token: 0x0600412C RID: 16684 RVA: 0x0016CC40 File Offset: 0x0016AE40
	[ContextMenu("Rebuild")]
	private void Rebuild()
	{
		List<string> list = new List<string>();
		if (this.orbitingObjects != null)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					list.Add(@ref.Get().orbitalDBId);
					Util.KDestroyGameObject(@ref.Get());
				}
			}
			this.orbitingObjects = new List<Ref<OrbitalObject>>();
		}
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.CreateOrbitalObject(list[i]);
			}
		}
	}

	// Token: 0x0600412D RID: 16685 RVA: 0x0016CCF8 File Offset: 0x0016AEF8
	private void UpdateLocation(AxialI location)
	{
		if (this.orbitingObjects.Count > 0)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					Util.KDestroyGameObject(@ref.Get());
				}
			}
			this.orbitingObjects = new List<Ref<OrbitalObject>>();
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.POI);
		if (visibleEntityOfLayerAtCell != null)
		{
			ArtifactPOIClusterGridEntity component = visibleEntityOfLayerAtCell.GetComponent<ArtifactPOIClusterGridEntity>();
			if (component != null)
			{
				ArtifactPOIStates.Instance smi = component.GetSMI<ArtifactPOIStates.Instance>();
				if (smi != null && smi.configuration.poiType.orbitalObject != null)
				{
					foreach (string orbit_db_name in smi.configuration.poiType.orbitalObject)
					{
						this.CreateOrbitalObject(orbit_db_name);
					}
				}
			}
			HarvestablePOIClusterGridEntity component2 = visibleEntityOfLayerAtCell.GetComponent<HarvestablePOIClusterGridEntity>();
			if (component2 != null)
			{
				HarvestablePOIStates.Instance smi2 = component2.GetSMI<HarvestablePOIStates.Instance>();
				if (smi2 != null && smi2.configuration.poiType.orbitalObject != null)
				{
					List<string> orbitalObject = smi2.configuration.poiType.orbitalObject;
					KRandom krandom = new KRandom();
					float num = smi2.poiCapacity / smi2.configuration.GetMaxCapacity() * (float)smi2.configuration.poiType.maxNumOrbitingObjects;
					int num2 = 0;
					while ((float)num2 < num)
					{
						int index = krandom.Next(orbitalObject.Count);
						this.CreateOrbitalObject(orbitalObject[index]);
						num2++;
					}
					return;
				}
			}
		}
		else
		{
			Clustercraft component3 = base.GetComponent<Clustercraft>();
			if (component3 != null)
			{
				if (component3.GetOrbitAsteroid() != null || component3.Status == Clustercraft.CraftStatus.Launching)
				{
					this.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.orbit.Id);
					return;
				}
				if (component3.Status == Clustercraft.CraftStatus.Landing)
				{
					this.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.landed.Id);
				}
			}
		}
	}

	// Token: 0x0600412E RID: 16686 RVA: 0x0016CF28 File Offset: 0x0016B128
	public void CreateOrbitalObject(string orbit_db_name)
	{
		WorldContainer component = base.GetComponent<WorldContainer>();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(OrbitalBGConfig.ID), base.gameObject, null);
		OrbitalObject component2 = gameObject.GetComponent<OrbitalObject>();
		component2.Init(orbit_db_name, component, this.orbitingObjects);
		gameObject.SetActive(true);
		this.orbitingObjects.Add(new Ref<OrbitalObject>(component2));
	}

	// Token: 0x04002A6B RID: 10859
	[Serialize]
	private List<Ref<OrbitalObject>> orbitingObjects = new List<Ref<OrbitalObject>>();

	// Token: 0x04002A6C RID: 10860
	private EventSystem.IntraObjectHandler<OrbitalMechanics> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<OrbitalMechanics>(delegate(OrbitalMechanics cmp, object data)
	{
		cmp.OnClusterLocationChanged(data);
	});
}
