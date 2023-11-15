using System;
using System.Collections.Generic;

// Token: 0x02000CA6 RID: 3238
public static class WorldGenProgressStages
{
	// Token: 0x04004754 RID: 18260
	public static KeyValuePair<WorldGenProgressStages.Stages, float>[] StageWeights = new KeyValuePair<WorldGenProgressStages.Stages, float>[]
	{
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Failure, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SetupNoise, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateNoise, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateSolarSystem, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.WorldLayout, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.CompleteLayout, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NoiseMapBuilder, 9f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ClearingLevel, 0.5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Processing, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Borders, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ProcessRivers, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ConvertCellsToEdges, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DrawWorldBorder, 0.2f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlaceTemplates, 5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SettleSim, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DetectNaturalCavities, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlacingCreatures, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Complete, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NumberOfStages, 0f)
	};

	// Token: 0x02001BD0 RID: 7120
	public enum Stages
	{
		// Token: 0x04007E01 RID: 32257
		Failure,
		// Token: 0x04007E02 RID: 32258
		SetupNoise,
		// Token: 0x04007E03 RID: 32259
		GenerateNoise,
		// Token: 0x04007E04 RID: 32260
		GenerateSolarSystem,
		// Token: 0x04007E05 RID: 32261
		WorldLayout,
		// Token: 0x04007E06 RID: 32262
		CompleteLayout,
		// Token: 0x04007E07 RID: 32263
		NoiseMapBuilder,
		// Token: 0x04007E08 RID: 32264
		ClearingLevel,
		// Token: 0x04007E09 RID: 32265
		Processing,
		// Token: 0x04007E0A RID: 32266
		Borders,
		// Token: 0x04007E0B RID: 32267
		ProcessRivers,
		// Token: 0x04007E0C RID: 32268
		ConvertCellsToEdges,
		// Token: 0x04007E0D RID: 32269
		DrawWorldBorder,
		// Token: 0x04007E0E RID: 32270
		PlaceTemplates,
		// Token: 0x04007E0F RID: 32271
		SettleSim,
		// Token: 0x04007E10 RID: 32272
		DetectNaturalCavities,
		// Token: 0x04007E11 RID: 32273
		PlacingCreatures,
		// Token: 0x04007E12 RID: 32274
		Complete,
		// Token: 0x04007E13 RID: 32275
		NumberOfStages
	}
}
