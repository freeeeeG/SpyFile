using System;
using GameModes;
using UnityEngine;

// Token: 0x0200076B RID: 1899
[Serializable]
public class SceneDirectoryData : ScriptableObject
{
	// Token: 0x04001BF2 RID: 7154
	public SceneDirectoryData.SceneDirectoryEntry[] Scenes = new SceneDirectoryData.SceneDirectoryEntry[0];

	// Token: 0x04001BF3 RID: 7155
	public static readonly int c_bitsPerTheme = GameUtils.GetRequiredBitCount(22);

	// Token: 0x0200076C RID: 1900
	public enum LevelTheme
	{
		// Token: 0x04001BF5 RID: 7157
		Null = -1,
		// Token: 0x04001BF6 RID: 7158
		Sushi,
		// Token: 0x04001BF7 RID: 7159
		Balloon,
		// Token: 0x04001BF8 RID: 7160
		Wizard,
		// Token: 0x04001BF9 RID: 7161
		Space,
		// Token: 0x04001BFA RID: 7162
		Rapids,
		// Token: 0x04001BFB RID: 7163
		Mine,
		// Token: 0x04001BFC RID: 7164
		Random,
		// Token: 0x04001BFD RID: 7165
		Beach,
		// Token: 0x04001BFE RID: 7166
		Resort,
		// Token: 0x04001BFF RID: 7167
		Wonderland,
		// Token: 0x04001C00 RID: 7168
		ChinaTown,
		// Token: 0x04001C01 RID: 7169
		Campsite,
		// Token: 0x04001C02 RID: 7170
		Treehouse,
		// Token: 0x04001C03 RID: 7171
		Keep,
		// Token: 0x04001C04 RID: 7172
		Courtyard,
		// Token: 0x04001C05 RID: 7173
		Battlements,
		// Token: 0x04001C06 RID: 7174
		Outside,
		// Token: 0x04001C07 RID: 7175
		Inside,
		// Token: 0x04001C08 RID: 7176
		Wonderland2,
		// Token: 0x04001C09 RID: 7177
		ChinaTown2,
		// Token: 0x04001C0A RID: 7178
		Summer,
		// Token: 0x04001C0B RID: 7179
		ChinaTown3,
		// Token: 0x04001C0C RID: 7180
		Count
	}

	// Token: 0x0200076D RID: 1901
	public enum World
	{
		// Token: 0x04001C0E RID: 7182
		Invalid = -1,
		// Token: 0x04001C0F RID: 7183
		Tutorial,
		// Token: 0x04001C10 RID: 7184
		One,
		// Token: 0x04001C11 RID: 7185
		Two,
		// Token: 0x04001C12 RID: 7186
		Three,
		// Token: 0x04001C13 RID: 7187
		Four,
		// Token: 0x04001C14 RID: 7188
		Five,
		// Token: 0x04001C15 RID: 7189
		Six,
		// Token: 0x04001C16 RID: 7190
		Seven,
		// Token: 0x04001C17 RID: 7191
		DLC2_One,
		// Token: 0x04001C18 RID: 7192
		DLC2_Two,
		// Token: 0x04001C19 RID: 7193
		DLC2_Three,
		// Token: 0x04001C1A RID: 7194
		DLC3_One,
		// Token: 0x04001C1B RID: 7195
		DLC4_One,
		// Token: 0x04001C1C RID: 7196
		DLC5_One,
		// Token: 0x04001C1D RID: 7197
		DLC5_Two,
		// Token: 0x04001C1E RID: 7198
		DLC5_Three,
		// Token: 0x04001C1F RID: 7199
		DLC7_One,
		// Token: 0x04001C20 RID: 7200
		DLC7_Two,
		// Token: 0x04001C21 RID: 7201
		DLC7_Three,
		// Token: 0x04001C22 RID: 7202
		DLC8_One,
		// Token: 0x04001C23 RID: 7203
		DLC8_Two,
		// Token: 0x04001C24 RID: 7204
		DLC8_Three,
		// Token: 0x04001C25 RID: 7205
		DLC9_One,
		// Token: 0x04001C26 RID: 7206
		DLC10_One,
		// Token: 0x04001C27 RID: 7207
		DLC11_One,
		// Token: 0x04001C28 RID: 7208
		DLC13_One,
		// Token: 0x04001C29 RID: 7209
		COUNT
	}

	// Token: 0x0200076E RID: 1902
	[Serializable]
	public class SceneDirectoryEntry
	{
		// Token: 0x0600248A RID: 9354 RVA: 0x000AD010 File Offset: 0x000AB410
		public SceneDirectoryData.PerPlayerCountDirectoryEntry GetSceneVarient(int _playerCount)
		{
			int num = this.SceneVarients.FindIndex_Predicate((SceneDirectoryData.PerPlayerCountDirectoryEntry x) => x.PlayerCount == _playerCount);
			if (num == -1)
			{
				num = this.SceneVarients.FindIndex_Predicate((SceneDirectoryData.PerPlayerCountDirectoryEntry x) => x.PlayerCount == -1);
			}
			return this.SceneVarients.TryAtIndex(num, null);
		}

		// Token: 0x04001C2A RID: 7210
		public string Label;

		// Token: 0x04001C2B RID: 7211
		public SceneDirectoryData.LevelTheme Theme = SceneDirectoryData.LevelTheme.Null;

		// Token: 0x04001C2C RID: 7212
		public SceneDirectoryData.World World = SceneDirectoryData.World.Invalid;

		// Token: 0x04001C2D RID: 7213
		public int StarCost;

		// Token: 0x04001C2E RID: 7214
		public Sprite LoadScreenOverride;

		// Token: 0x04001C2F RID: 7215
		public bool UseKitchenLoadingScreen = true;

		// Token: 0x04001C30 RID: 7216
		public bool HasScoreBoundaries = true;

		// Token: 0x04001C31 RID: 7217
		public bool ActuallyAllowed = true;

		// Token: 0x04001C32 RID: 7218
		public bool IsHidden;

		// Token: 0x04001C33 RID: 7219
		public bool AvailableInLobby = true;

		// Token: 0x04001C34 RID: 7220
		public bool LevelChainEnd;

		// Token: 0x04001C35 RID: 7221
		[SerializeField]
		[Mask(typeof(Kind))]
		public int m_supportedGameModes = -1;

		// Token: 0x04001C36 RID: 7222
		[Space]
		public SceneDirectoryData.PerPlayerCountDirectoryEntry[] SceneVarients = new SceneDirectoryData.PerPlayerCountDirectoryEntry[0];

		// Token: 0x04001C37 RID: 7223
		[ArrayIndex("Scenes", "", SerializationUtils.RootType.Top)]
		public int[] PreviousEntriesToUnlock = new int[0];
	}

	// Token: 0x0200076F RID: 1903
	[Serializable]
	public class StarBoundaries
	{
		// Token: 0x04001C39 RID: 7225
		public int m_OneStarScore;

		// Token: 0x04001C3A RID: 7226
		public int m_TwoStarScore;

		// Token: 0x04001C3B RID: 7227
		public int m_ThreeStarScore;

		// Token: 0x04001C3C RID: 7228
		public int m_FourStarScore;
	}

	// Token: 0x02000770 RID: 1904
	[Serializable]
	public class PerPlayerCountDirectoryEntry
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x0600248E RID: 9358 RVA: 0x000AD0E5 File Offset: 0x000AB4E5
		public int OneStarScore
		{
			get
			{
				return this.GetStarBoundaries().m_OneStarScore;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x0600248F RID: 9359 RVA: 0x000AD0F2 File Offset: 0x000AB4F2
		public int TwoStarScore
		{
			get
			{
				return this.GetStarBoundaries().m_TwoStarScore;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x000AD0FF File Offset: 0x000AB4FF
		public int ThreeStarScore
		{
			get
			{
				return this.GetStarBoundaries().m_ThreeStarScore;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x000AD10C File Offset: 0x000AB50C
		public int FourStarScore
		{
			get
			{
				return this.GetStarBoundaries().m_FourStarScore;
			}
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000AD119 File Offset: 0x000AB519
		public int GetPointsForStar(int _numStars)
		{
			switch (_numStars)
			{
			case 0:
				return 0;
			case 1:
				return this.OneStarScore;
			case 2:
				return this.TwoStarScore;
			case 3:
				return this.ThreeStarScore;
			case 4:
				return this.FourStarScore;
			default:
				return -1;
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000AD15C File Offset: 0x000AB55C
		public int GetStarForPoints(int _points, bool _inNGPlus = false)
		{
			if (_points < this.OneStarScore)
			{
				return 0;
			}
			if (_points < this.TwoStarScore)
			{
				return 1;
			}
			if (_points < this.ThreeStarScore)
			{
				return 2;
			}
			if (_points < this.FourStarScore || !_inNGPlus)
			{
				return 3;
			}
			return 4;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000AD1A8 File Offset: 0x000AB5A8
		private SceneDirectoryData.StarBoundaries GetStarBoundaries()
		{
			return this.m_PCStarBoundaries;
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000AD1B0 File Offset: 0x000AB5B0
		public SceneDirectoryData.StarBoundaries[] GetStarBoundariesAllPlatforms()
		{
			return new SceneDirectoryData.StarBoundaries[]
			{
				this.m_PCStarBoundaries,
				this.m_PS4StarBoundaries,
				this.m_XboxStarBoundaries,
				this.m_NXStarBoundaries
			};
		}

		// Token: 0x04001C3D RID: 7229
		public int PlayerCount = 1;

		// Token: 0x04001C3E RID: 7230
		public LevelConfigBase LevelConfig;

		// Token: 0x04001C3F RID: 7231
		[SceneName]
		public string SceneName;

		// Token: 0x04001C40 RID: 7232
		public Sprite Screenshot;

		// Token: 0x04001C41 RID: 7233
		[SerializeField]
		private SceneDirectoryData.StarBoundaries m_PCStarBoundaries = new SceneDirectoryData.StarBoundaries();

		// Token: 0x04001C42 RID: 7234
		[SerializeField]
		private SceneDirectoryData.StarBoundaries m_PS4StarBoundaries = new SceneDirectoryData.StarBoundaries();

		// Token: 0x04001C43 RID: 7235
		[SerializeField]
		private SceneDirectoryData.StarBoundaries m_XboxStarBoundaries = new SceneDirectoryData.StarBoundaries();

		// Token: 0x04001C44 RID: 7236
		[SerializeField]
		private SceneDirectoryData.StarBoundaries m_NXStarBoundaries = new SceneDirectoryData.StarBoundaries();
	}
}
