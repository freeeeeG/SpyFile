using System;
using Characters;
using Data;
using UnityEngine;

namespace Level
{
	// Token: 0x020004F0 RID: 1264
	[CreateAssetMenu]
	public sealed class HardmodeLevelInfo : ScriptableObject
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x0004DC91 File Offset: 0x0004BE91
		public static HardmodeLevelInfo instance
		{
			get
			{
				if (HardmodeLevelInfo._instance == null)
				{
					HardmodeLevelInfo._instance = Resources.Load<HardmodeLevelInfo>("HardmodeSetting/HardmodeLevelInfo");
				}
				return HardmodeLevelInfo._instance;
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0004DCB4 File Offset: 0x0004BEB4
		public int GetDarkEnemyTotalCount(System.Random random)
		{
			HardmodeLevelInfo.DarkEnemyInfoByLevel darkEnemyInfo = this._infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._darkEnemyInfo;
			return random.Next(darkEnemyInfo._totalRange.x, darkEnemyInfo._totalRange.y + 1);
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0004DCF4 File Offset: 0x0004BEF4
		public int GetDarkEnemyCountPerMap(System.Random random)
		{
			HardmodeLevelInfo.DarkEnemyInfoByLevel darkEnemyInfo = this._infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._darkEnemyInfo;
			return random.Next(darkEnemyInfo._perMapRange.x, darkEnemyInfo._perMapRange.y + 1);
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0004DD34 File Offset: 0x0004BF34
		public HardmodeLevelInfo.EnemyStatInfo GetEnemyStatInfoByType(Character.Type type)
		{
			switch (type)
			{
			case Character.Type.TrashMob:
			case Character.Type.Summoned:
				return this._infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._trashMobInfo;
			case Character.Type.Named:
				return this._infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._darkEnemyInfo._statInfo;
			case Character.Type.Adventurer:
				return this._infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._adventurerInfo;
			case Character.Type.Boss:
				return this._infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._bossInfo;
			default:
				return this._infoByLevel[GameData.HardmodeProgress.hardmodeLevel]._trashMobInfo;
			}
		}

		// Token: 0x040015A4 RID: 5540
		private static HardmodeLevelInfo _instance;

		// Token: 0x040015A5 RID: 5541
		[SerializeField]
		private HardmodeLevelInfo.InfoByLevel[] _infoByLevel;

		// Token: 0x020004F1 RID: 1265
		[Serializable]
		internal class DarkEnemyInfoByLevel
		{
			// Token: 0x040015A6 RID: 5542
			[SerializeField]
			internal HardmodeLevelInfo.EnemyStatInfo _statInfo;

			// Token: 0x040015A7 RID: 5543
			[MinMaxSlider(0f, 100f)]
			[SerializeField]
			internal Vector2Int _totalRange;

			// Token: 0x040015A8 RID: 5544
			[MinMaxSlider(0f, 10f)]
			[SerializeField]
			internal Vector2Int _perMapRange;
		}

		// Token: 0x020004F2 RID: 1266
		[Serializable]
		public struct EnemyStatInfo
		{
			// Token: 0x170004E7 RID: 1255
			// (get) Token: 0x060018D3 RID: 6355 RVA: 0x0004DDBB File Offset: 0x0004BFBB
			public float healthMultiplier
			{
				get
				{
					return this._healthMultiplier;
				}
			}

			// Token: 0x170004E8 RID: 1256
			// (get) Token: 0x060018D4 RID: 6356 RVA: 0x0004DDC3 File Offset: 0x0004BFC3
			public float attackMultiplier
			{
				get
				{
					return this._attackMultiplier;
				}
			}

			// Token: 0x170004E9 RID: 1257
			// (get) Token: 0x060018D5 RID: 6357 RVA: 0x0004DDCB File Offset: 0x0004BFCB
			public float castingBreakDamageMultiplier
			{
				get
				{
					return this._castingBreakDamageMultiplier;
				}
			}

			// Token: 0x040015A9 RID: 5545
			[SerializeField]
			[Range(0f, 10f)]
			private float _healthMultiplier;

			// Token: 0x040015AA RID: 5546
			[Range(0f, 10f)]
			[SerializeField]
			private float _attackMultiplier;

			// Token: 0x040015AB RID: 5547
			[Range(0f, 100f)]
			[SerializeField]
			private float _castingBreakDamageMultiplier;
		}

		// Token: 0x020004F3 RID: 1267
		[Serializable]
		internal class InfoByLevel
		{
			// Token: 0x040015AC RID: 5548
			[SerializeField]
			[Header("일반 적")]
			internal HardmodeLevelInfo.EnemyStatInfo _trashMobInfo;

			// Token: 0x040015AD RID: 5549
			[SerializeField]
			[Header("검은 적")]
			internal HardmodeLevelInfo.DarkEnemyInfoByLevel _darkEnemyInfo;

			// Token: 0x040015AE RID: 5550
			[Header("모험가")]
			[SerializeField]
			internal HardmodeLevelInfo.EnemyStatInfo _adventurerInfo;

			// Token: 0x040015AF RID: 5551
			[SerializeField]
			[Header("보스")]
			internal HardmodeLevelInfo.EnemyStatInfo _bossInfo;

			// Token: 0x040015B0 RID: 5552
			[SerializeField]
			[Header("금화 획득량")]
			internal Vector2Int _goldAmountMultiplier;
		}
	}
}
