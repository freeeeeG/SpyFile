using System;
using System.Runtime.CompilerServices;
using Characters.Gear.Synergy.Inscriptions;
using GameResources;
using Level.BlackMarket;
using Level.Npc.FieldNpcs;
using UnityEngine;

namespace Level
{
	// Token: 0x020004F7 RID: 1271
	public abstract class IStageInfo : ScriptableObject
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x0004E384 File Offset: 0x0004C584
		public AudioClip music
		{
			get
			{
				return this._music;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060018E1 RID: 6369 RVA: 0x0004E38C File Offset: 0x0004C58C
		public Sprite loadingScreenBackground
		{
			get
			{
				return this._loadingScreenBackground;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060018E2 RID: 6370 RVA: 0x0004E394 File Offset: 0x0004C594
		public float healthMultiplier
		{
			get
			{
				return this._healthMultiplier;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x0004E39C File Offset: 0x0004C59C
		public float adventurerHealthMultiplier
		{
			get
			{
				return this._adventurerHealthMultiplier;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060018E4 RID: 6372 RVA: 0x0004E3A4 File Offset: 0x0004C5A4
		public float adventurerAttackDamageMultiplier
		{
			get
			{
				return this._adventurerAttackDamageMultiplier;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x0004E3AC File Offset: 0x0004C5AC
		public float adventurerCastingBreakDamageMultiplier
		{
			get
			{
				return this._adventurerCastingBreakDamageMultiplier;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x0004E3B4 File Offset: 0x0004C5B4
		public Vector2Int adventurerLevel
		{
			get
			{
				return this._adventurerLevel;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x0004E3BC File Offset: 0x0004C5BC
		public Vector2Int goldrewardAmount
		{
			get
			{
				return this._goldrewardAmount;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060018E8 RID: 6376 RVA: 0x0004E3C4 File Offset: 0x0004C5C4
		public Vector2Int bonerewardAmount
		{
			get
			{
				return this._bonerewardAmount;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060018E9 RID: 6377 RVA: 0x0004E3CC File Offset: 0x0004C5CC
		public RarityPossibilities gearPossibilities
		{
			get
			{
				return this._gearPossibilities;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060018EA RID: 6378 RVA: 0x0004E3D4 File Offset: 0x0004C5D4
		public Level.BlackMarket.SettingsByStage marketSettings
		{
			get
			{
				return this._marketSettings;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x0004E3DC File Offset: 0x0004C5DC
		public Level.Npc.FieldNpcs.SettingsByStage fieldNpcSettings
		{
			get
			{
				return this._fieldNpcSettings;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060018EC RID: 6380 RVA: 0x0004E3E4 File Offset: 0x0004C5E4
		public CurrencyRangeByRarity goldRangeByRarity
		{
			get
			{
				return this._goldRangeByRarity;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060018ED RID: 6381 RVA: 0x0004E3EC File Offset: 0x0004C5EC
		public CurrencyRangeByRarity darkQuartzRangeByRarity
		{
			get
			{
				return this._darkQuartzRangeByRarity;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060018EE RID: 6382 RVA: 0x0004E3F4 File Offset: 0x0004C5F4
		public CurrencyRangeByRarity boneRangeByRarity
		{
			get
			{
				return this._boneRangeByRarity;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x0004E3FC File Offset: 0x0004C5FC
		public Treasure.StageInfo treasureInfo
		{
			get
			{
				return this._treasureInfo;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060018F0 RID: 6384
		[TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})]
		public abstract ValueTuple<PathNode, PathNode> currentMapPath { [return: TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})] get; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060018F1 RID: 6385
		[TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})]
		public abstract ValueTuple<PathNode, PathNode> nextMapPath { [return: TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})] get; }

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060018F2 RID: 6386
		public abstract ParallaxBackground background { get; }

		// Token: 0x060018F3 RID: 6387
		public abstract void Initialize();

		// Token: 0x060018F4 RID: 6388
		public abstract void Reset();

		// Token: 0x060018F5 RID: 6389
		public abstract PathNode Next(NodeIndex nodeIndex);

		// Token: 0x060018F6 RID: 6390
		public abstract void UpdateReferences();

		// Token: 0x040015B4 RID: 5556
		public MapReference[] maps;

		// Token: 0x040015B5 RID: 5557
		[SerializeField]
		private AudioClip _music;

		// Token: 0x040015B6 RID: 5558
		[Space]
		[SerializeField]
		private Sprite _loadingScreenBackground;

		// Token: 0x040015B7 RID: 5559
		[Space]
		[SerializeField]
		private float _healthMultiplier = 1f;

		// Token: 0x040015B8 RID: 5560
		[SerializeField]
		private float _adventurerHealthMultiplier = 1f;

		// Token: 0x040015B9 RID: 5561
		[SerializeField]
		private float _adventurerAttackDamageMultiplier = 1f;

		// Token: 0x040015BA RID: 5562
		[SerializeField]
		private float _adventurerCastingBreakDamageMultiplier = 1f;

		// Token: 0x040015BB RID: 5563
		[MinMaxSlider(1f, 99f)]
		[SerializeField]
		private Vector2Int _adventurerLevel = new Vector2Int(1, 99);

		// Token: 0x040015BC RID: 5564
		[SerializeField]
		private Vector2Int _goldrewardAmount = new Vector2Int(90, 110);

		// Token: 0x040015BD RID: 5565
		[SerializeField]
		private Vector2Int _bonerewardAmount = new Vector2Int(90, 110);

		// Token: 0x040015BE RID: 5566
		[Space]
		[SerializeField]
		private RarityPossibilities _gearPossibilities;

		// Token: 0x040015BF RID: 5567
		[SerializeField]
		private Level.BlackMarket.SettingsByStage _marketSettings;

		// Token: 0x040015C0 RID: 5568
		[SerializeField]
		private Level.Npc.FieldNpcs.SettingsByStage _fieldNpcSettings;

		// Token: 0x040015C1 RID: 5569
		[SerializeField]
		private CurrencyRangeByRarity _goldRangeByRarity;

		// Token: 0x040015C2 RID: 5570
		[SerializeField]
		private CurrencyRangeByRarity _darkQuartzRangeByRarity;

		// Token: 0x040015C3 RID: 5571
		[SerializeField]
		private CurrencyRangeByRarity _boneRangeByRarity;

		// Token: 0x040015C4 RID: 5572
		[SerializeField]
		private Treasure.StageInfo _treasureInfo;

		// Token: 0x040015C5 RID: 5573
		[NonSerialized]
		public int pathIndex = -1;

		// Token: 0x040015C6 RID: 5574
		[NonSerialized]
		public NodeIndex nodeIndex;
	}
}
