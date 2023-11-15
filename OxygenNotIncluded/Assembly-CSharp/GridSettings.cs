﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007EB RID: 2027
[AddComponentMenu("KMonoBehaviour/scripts/GridSettings")]
public class GridSettings : KMonoBehaviour
{
	// Token: 0x060039AC RID: 14764 RVA: 0x00141ACC File Offset: 0x0013FCCC
	public static void Reset(int width, int height)
	{
		Grid.WidthInCells = width;
		Grid.HeightInCells = height;
		Grid.CellCount = width * height;
		Grid.WidthInMeters = 1f * (float)width;
		Grid.HeightInMeters = 1f * (float)height;
		Grid.CellSizeInMeters = 1f;
		Grid.HalfCellSizeInMeters = 0.5f;
		Grid.Element = new Element[Grid.CellCount];
		Grid.VisMasks = new Grid.VisFlags[Grid.CellCount];
		Grid.Visible = new byte[Grid.CellCount];
		Grid.Spawnable = new byte[Grid.CellCount];
		Grid.BuildMasks = new Grid.BuildFlags[Grid.CellCount];
		Grid.LightCount = new int[Grid.CellCount];
		Grid.Damage = new float[Grid.CellCount];
		Grid.NavMasks = new Grid.NavFlags[Grid.CellCount];
		Grid.NavValidatorMasks = new Grid.NavValidatorFlags[Grid.CellCount];
		Grid.Decor = new float[Grid.CellCount];
		Grid.Loudness = new float[Grid.CellCount];
		Grid.GravitasFacility = new bool[Grid.CellCount];
		Grid.WorldIdx = new byte[Grid.CellCount];
		Grid.ObjectLayers = new Dictionary<int, GameObject>[45];
		for (int i = 0; i < Grid.ObjectLayers.Length; i++)
		{
			Grid.ObjectLayers[i] = new Dictionary<int, GameObject>();
		}
		for (int j = 0; j < Grid.CellCount; j++)
		{
			Grid.Loudness[j] = 0f;
		}
		if (Game.Instance != null)
		{
			Game.Instance.gasConduitSystem.Initialize(Grid.WidthInCells, Grid.HeightInCells);
			Game.Instance.liquidConduitSystem.Initialize(Grid.WidthInCells, Grid.HeightInCells);
			Game.Instance.electricalConduitSystem.Initialize(Grid.WidthInCells, Grid.HeightInCells);
			Game.Instance.travelTubeSystem.Initialize(Grid.WidthInCells, Grid.HeightInCells);
			Game.Instance.gasConduitFlow.Initialize(Grid.CellCount);
			Game.Instance.liquidConduitFlow.Initialize(Grid.CellCount);
		}
		for (int k = 0; k < Grid.CellCount; k++)
		{
			Grid.WorldIdx[k] = byte.MaxValue;
		}
		Grid.OnReveal = null;
	}

	// Token: 0x060039AD RID: 14765 RVA: 0x00141CE4 File Offset: 0x0013FEE4
	public static void ClearGrid()
	{
		Grid.WidthInCells = 0;
		Grid.HeightInCells = 0;
		Grid.CellCount = 0;
		Grid.WidthInMeters = 0f;
		Grid.HeightInMeters = 0f;
		Grid.CellSizeInMeters = 0f;
		Grid.HalfCellSizeInMeters = 0f;
		Grid.Element = null;
		Grid.VisMasks = null;
		Grid.Visible = null;
		Grid.Spawnable = null;
		Grid.BuildMasks = null;
		Grid.NavValidatorMasks = null;
		Grid.LightCount = null;
		Grid.Damage = null;
		Grid.Decor = null;
		Grid.Loudness = null;
		Grid.GravitasFacility = null;
		Grid.ObjectLayers = null;
		Grid.WorldIdx = null;
		Grid.OnReveal = null;
		Grid.ResetNavMasksAndDetails();
	}

	// Token: 0x04002674 RID: 9844
	public const float CellSizeInMeters = 1f;
}
