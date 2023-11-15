using System;
using System.Collections.Generic;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000CA3 RID: 3235
[AddComponentMenu("KMonoBehaviour/scripts/CavityVisualizer")]
public class CavityVisualizer : KMonoBehaviour
{
	// Token: 0x060066FF RID: 26367 RVA: 0x00267130 File Offset: 0x00265330
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
		{
			foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
			{
				foreach (int item in hashSet)
				{
					this.cavityCells.Add(item);
				}
			}
		}
	}

	// Token: 0x06006700 RID: 26368 RVA: 0x00267208 File Offset: 0x00265408
	private void OnDrawGizmosSelected()
	{
		if (this.drawCavity)
		{
			Color[] array = new Color[]
			{
				Color.blue,
				Color.yellow
			};
			int num = 0;
			foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
			{
				Gizmos.color = array[num % array.Length];
				Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.125f);
				num++;
				foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
				{
					foreach (int cell in hashSet)
					{
						Gizmos.DrawCube(Grid.CellToPos(cell) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
					}
				}
			}
		}
		if (this.spawnCells != null && this.drawSpawnCells)
		{
			Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
			foreach (int cell2 in this.spawnCells)
			{
				Gizmos.DrawCube(Grid.CellToPos(cell2) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
			}
		}
	}

	// Token: 0x04004732 RID: 18226
	public List<int> cavityCells = new List<int>();

	// Token: 0x04004733 RID: 18227
	public List<int> spawnCells = new List<int>();

	// Token: 0x04004734 RID: 18228
	public bool drawCavity = true;

	// Token: 0x04004735 RID: 18229
	public bool drawSpawnCells = true;
}
