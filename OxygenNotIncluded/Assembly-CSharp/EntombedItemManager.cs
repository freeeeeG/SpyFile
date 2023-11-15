using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000796 RID: 1942
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemManager")]
public class EntombedItemManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x06003604 RID: 13828 RVA: 0x00123CA3 File Offset: 0x00121EA3
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.SpawnUncoveredObjects();
		this.AddMassToWorldIfPossible();
		this.PopulateEntombedItemVisualizers();
	}

	// Token: 0x06003605 RID: 13829 RVA: 0x00123CB8 File Offset: 0x00121EB8
	public static bool CanEntomb(Pickupable pickupable)
	{
		if (pickupable == null)
		{
			return false;
		}
		if (pickupable.storage != null)
		{
			return false;
		}
		int num = Grid.PosToCell(pickupable);
		return Grid.IsValidCell(num) && Grid.Solid[num] && !(Grid.Objects[num, 9] != null) && (pickupable.GetComponent<PrimaryElement>().Element.IsSolid && pickupable.GetComponent<ElementChunk>() != null);
	}

	// Token: 0x06003606 RID: 13830 RVA: 0x00123D3A File Offset: 0x00121F3A
	public void Add(Pickupable pickupable)
	{
		this.pickupables.Add(pickupable);
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x00123D48 File Offset: 0x00121F48
	public void Sim33ms(float dt)
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		HashSetPool<Pickupable, EntombedItemManager>.PooledHashSet pooledHashSet = HashSetPool<Pickupable, EntombedItemManager>.Allocate();
		foreach (Pickupable pickupable in this.pickupables)
		{
			if (EntombedItemManager.CanEntomb(pickupable))
			{
				pooledHashSet.Add(pickupable);
			}
		}
		this.pickupables.Clear();
		foreach (Pickupable pickupable2 in pooledHashSet)
		{
			int num = Grid.PosToCell(pickupable2);
			PrimaryElement component2 = pickupable2.GetComponent<PrimaryElement>();
			SimHashes elementID = component2.ElementID;
			float mass = component2.Mass;
			float temperature = component2.Temperature;
			byte diseaseIdx = component2.DiseaseIdx;
			int diseaseCount = component2.DiseaseCount;
			Element element = Grid.Element[num];
			if (elementID == element.id && mass > 0.010000001f && Grid.Mass[num] + mass < element.maxMass)
			{
				SimMessages.AddRemoveSubstance(num, ElementLoader.FindElementByHash(elementID).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, diseaseIdx, diseaseCount, true, -1);
			}
			else
			{
				component.AddItem(num);
				this.cells.Add(num);
				this.elementIds.Add((int)elementID);
				this.masses.Add(mass);
				this.temperatures.Add(temperature);
				this.diseaseIndices.Add(diseaseIdx);
				this.diseaseCounts.Add(diseaseCount);
			}
			Util.KDestroyGameObject(pickupable2.gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x00123F10 File Offset: 0x00122110
	public void OnSolidChanged(List<int> solid_changed_cells)
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		foreach (int num in solid_changed_cells)
		{
			if (!Grid.Solid[num])
			{
				pooledList.Add(num);
			}
		}
		ListPool<int, EntombedItemManager>.PooledList pooledList2 = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num2 = this.cells[i];
			foreach (int num3 in pooledList)
			{
				if (num2 == num3)
				{
					pooledList2.Add(i);
					break;
				}
			}
		}
		pooledList.Recycle();
		this.SpawnObjects(pooledList2);
		pooledList2.Recycle();
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x00123FFC File Offset: 0x001221FC
	private void SpawnUncoveredObjects()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int i2 = this.cells[i];
			if (!Grid.Solid[i2])
			{
				pooledList.Add(i);
			}
		}
		this.SpawnObjects(pooledList);
		pooledList.Recycle();
	}

	// Token: 0x0600360A RID: 13834 RVA: 0x00124054 File Offset: 0x00122254
	private void AddMassToWorldIfPossible()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num = this.cells[i];
			if (Grid.Solid[num] && Grid.Element[num].id == (SimHashes)this.elementIds[i])
			{
				pooledList.Add(i);
			}
		}
		pooledList.Sort();
		pooledList.Reverse();
		foreach (int item_idx in pooledList)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			this.RemoveItem(item_idx);
			if (item.mass > 1E-45f)
			{
				SimMessages.AddRemoveSubstance(item.cell, ElementLoader.FindElementByHash((SimHashes)item.elementId).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, -1);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x0012416C File Offset: 0x0012236C
	private void RemoveItem(int item_idx)
	{
		this.cells.RemoveAt(item_idx);
		this.elementIds.RemoveAt(item_idx);
		this.masses.RemoveAt(item_idx);
		this.temperatures.RemoveAt(item_idx);
		this.diseaseIndices.RemoveAt(item_idx);
		this.diseaseCounts.RemoveAt(item_idx);
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x001241C4 File Offset: 0x001223C4
	private EntombedItemManager.Item GetItem(int item_idx)
	{
		return new EntombedItemManager.Item
		{
			cell = this.cells[item_idx],
			elementId = this.elementIds[item_idx],
			mass = this.masses[item_idx],
			temperature = this.temperatures[item_idx],
			diseaseIdx = this.diseaseIndices[item_idx],
			diseaseCount = this.diseaseCounts[item_idx]
		};
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x0012424C File Offset: 0x0012244C
	private void SpawnObjects(List<int> uncovered_item_indices)
	{
		uncovered_item_indices.Sort();
		uncovered_item_indices.Reverse();
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int item_idx in uncovered_item_indices)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			component.RemoveItem(item.cell);
			this.RemoveItem(item_idx);
			Element element = ElementLoader.FindElementByHash((SimHashes)item.elementId);
			if (element != null)
			{
				element.substance.SpawnResource(Grid.CellToPosCCC(item.cell, Grid.SceneLayer.Ore), item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, false, false);
			}
		}
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x0012430C File Offset: 0x0012250C
	private void PopulateEntombedItemVisualizers()
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int cell in this.cells)
		{
			component.AddItem(cell);
		}
	}

	// Token: 0x040020F3 RID: 8435
	[Serialize]
	private List<int> cells = new List<int>();

	// Token: 0x040020F4 RID: 8436
	[Serialize]
	private List<int> elementIds = new List<int>();

	// Token: 0x040020F5 RID: 8437
	[Serialize]
	private List<float> masses = new List<float>();

	// Token: 0x040020F6 RID: 8438
	[Serialize]
	private List<float> temperatures = new List<float>();

	// Token: 0x040020F7 RID: 8439
	[Serialize]
	private List<byte> diseaseIndices = new List<byte>();

	// Token: 0x040020F8 RID: 8440
	[Serialize]
	private List<int> diseaseCounts = new List<int>();

	// Token: 0x040020F9 RID: 8441
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x02001518 RID: 5400
	private struct Item
	{
		// Token: 0x0400674A RID: 26442
		public int cell;

		// Token: 0x0400674B RID: 26443
		public int elementId;

		// Token: 0x0400674C RID: 26444
		public float mass;

		// Token: 0x0400674D RID: 26445
		public float temperature;

		// Token: 0x0400674E RID: 26446
		public byte diseaseIdx;

		// Token: 0x0400674F RID: 26447
		public int diseaseCount;
	}
}
