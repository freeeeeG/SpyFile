using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200091B RID: 2331
public static class RadiationGridManager
{
	// Token: 0x06004391 RID: 17297 RVA: 0x0017A9D0 File Offset: 0x00178BD0
	public static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x06004392 RID: 17298 RVA: 0x0017A9ED File Offset: 0x00178BED
	public static void Initialise()
	{
		RadiationGridManager.emitters = new List<RadiationGridEmitter>();
	}

	// Token: 0x06004393 RID: 17299 RVA: 0x0017A9F9 File Offset: 0x00178BF9
	public static void Shutdown()
	{
		RadiationGridManager.emitters.Clear();
	}

	// Token: 0x06004394 RID: 17300 RVA: 0x0017AA08 File Offset: 0x00178C08
	public static void Refresh()
	{
		for (int i = 0; i < RadiationGridManager.emitters.Count; i++)
		{
			if (RadiationGridManager.emitters[i].enabled)
			{
				RadiationGridManager.emitters[i].Emit();
			}
		}
	}

	// Token: 0x04002CBD RID: 11453
	public const float STANDARD_MASS_FALLOFF = 1000000f;

	// Token: 0x04002CBE RID: 11454
	public const int RADIATION_LINGER_RATE = 4;

	// Token: 0x04002CBF RID: 11455
	public static List<RadiationGridEmitter> emitters = new List<RadiationGridEmitter>();

	// Token: 0x04002CC0 RID: 11456
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x04002CC1 RID: 11457
	public static int[] previewLux;
}
