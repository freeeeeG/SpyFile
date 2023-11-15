using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000575 RID: 1397
public class DevTool_StoryTraits_Reveal : DevTool
{
	// Token: 0x060021FD RID: 8701 RVA: 0x000BA880 File Offset: 0x000B8A80
	protected override void RenderTo(DevPanel panel)
	{
		Option<int> cellIndexForUniqueBuilding = DevToolUtil.GetCellIndexForUniqueBuilding("Headquarters");
		bool flag = cellIndexForUniqueBuilding.IsSome();
		if (ImGuiEx.Button("Focus on headquaters", flag))
		{
			DevToolUtil.FocusCameraOnCell(cellIndexForUniqueBuilding.Unwrap());
		}
		if (!flag)
		{
			ImGuiEx.TooltipForPrevious("Couldn't find headquaters");
		}
		if (ImGui.CollapsingHeader("Search world for entity", ImGuiTreeNodeFlags.DefaultOpen))
		{
			Option<IReadOnlyList<WorldGenSpawner.Spawnable>> allSpawnables = this.GetAllSpawnables();
			if (!allSpawnables.HasValue)
			{
				ImGui.Text("Couldn't find a list of spawnables");
				return;
			}
			foreach (string text in this.GetPrefabIDsToSearchFor())
			{
				Option<int> cellIndexForSpawnable = this.GetCellIndexForSpawnable(text, allSpawnables.Value);
				string str = "\"" + text + "\"";
				bool hasValue = cellIndexForSpawnable.HasValue;
				if (ImGuiEx.Button("Reveal and focus on " + str, hasValue))
				{
					DevToolUtil.RevealAndFocusAt(cellIndexForSpawnable.Value);
				}
				if (!hasValue)
				{
					ImGuiEx.TooltipForPrevious("Couldn't find a cell that contained a spawnable with component " + str);
				}
			}
		}
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x000BA990 File Offset: 0x000B8B90
	public IEnumerable<string> GetPrefabIDsToSearchFor()
	{
		yield return "MegaBrainTank";
		yield return "GravitasCreatureManipulator";
		yield return "LonelyMinionHouse";
		yield return "FossilDig";
		yield break;
	}

	// Token: 0x060021FF RID: 8703 RVA: 0x000BA999 File Offset: 0x000B8B99
	private Option<ClusterManager> GetClusterManager()
	{
		if (ClusterManager.Instance == null)
		{
			return Option.None;
		}
		return ClusterManager.Instance;
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x000BA9C0 File Offset: 0x000B8BC0
	private Option<int> GetCellIndexForSpawnable(string prefabId, IReadOnlyList<WorldGenSpawner.Spawnable> spawnablesToSearch)
	{
		foreach (WorldGenSpawner.Spawnable spawnable in spawnablesToSearch)
		{
			if (prefabId == spawnable.spawnInfo.id)
			{
				return spawnable.cell;
			}
		}
		return Option.None;
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x000BAA30 File Offset: 0x000B8C30
	private Option<IReadOnlyList<WorldGenSpawner.Spawnable>> GetAllSpawnables()
	{
		WorldGenSpawner worldGenSpawner = UnityEngine.Object.FindObjectOfType<WorldGenSpawner>(true);
		if (worldGenSpawner == null)
		{
			return Option.None;
		}
		IReadOnlyList<WorldGenSpawner.Spawnable> spawnables = worldGenSpawner.GetSpawnables();
		if (spawnables == null)
		{
			return Option.None;
		}
		return Option.Some<IReadOnlyList<WorldGenSpawner.Spawnable>>(spawnables);
	}
}
