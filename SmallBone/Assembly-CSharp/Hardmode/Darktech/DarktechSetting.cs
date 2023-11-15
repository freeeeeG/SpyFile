using System;
using System.Collections.Generic;
using Level;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x0200015F RID: 351
	[CreateAssetMenu]
	public sealed class DarktechSetting : ScriptableObject
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x000140BE File Offset: 0x000122BE
		public int 두개골제조기가격
		{
			get
			{
				return this._두개골제조기가격;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x000140C6 File Offset: 0x000122C6
		public int 보급품제조기가격
		{
			get
			{
				return this._보급품제조기가격;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x000140CE File Offset: 0x000122CE
		public IDictionary<ValueTuple<Chapter.Type, float>, CustomFloat> 흉조증폭기확률
		{
			get
			{
				return this._흉조증폭기확률Dict;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x000140D6 File Offset: 0x000122D6
		public DarktechSetting.ItemRotationEquipmentInfo[] 품목순화장치가중치
		{
			get
			{
				return this._품목순환장치아이템설정;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x000140DE File Offset: 0x000122DE
		public int 품목순환장치버프맵카운트
		{
			get
			{
				return this._품목순환장치버프맵카운트;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x000140E6 File Offset: 0x000122E6
		public IDictionary<ValueTuple<Chapter.Type, int>, float> 품목순환장치상품별가격Dict
		{
			get
			{
				return this._품목순환장치상품별가격Dict;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x000140EE File Offset: 0x000122EE
		public int[] 건강보조장치공격력버프가격
		{
			get
			{
				return this._건강보조장치공격력버프가격;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x000140F6 File Offset: 0x000122F6
		public float[] 건강보조장치공격력버프스텟
		{
			get
			{
				return this._건강보조장치공격력버프스텟;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x000140FE File Offset: 0x000122FE
		public int[] 건강보조장치체력버프가격
		{
			get
			{
				return this._건강보조장치체력버프가격;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00014106 File Offset: 0x00012306
		public float[] 건강보조장치체력버프스텟
		{
			get
			{
				return this._건강보조장치체력버프스텟;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0001410E File Offset: 0x0001230E
		public int[] 건강보조장치체력버프회복량
		{
			get
			{
				return this._건강보조장치체력버프회복량;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x00014116 File Offset: 0x00012316
		public int[] 건강보조장치속도버프가격
		{
			get
			{
				return this._건강보조장치속도버프가격;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0001411E File Offset: 0x0001231E
		public float[] 건강보조장치속도버프스텟
		{
			get
			{
				return this._건강보조장치속도버프스텟;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00014126 File Offset: 0x00012326
		public DarktechSetting.LuckyMeasuringInstrument 행운계측기설정
		{
			get
			{
				return this._행운계측기설정;
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00014130 File Offset: 0x00012330
		public int GetInscriptionBonusCostByStage(Chapter.Type chapterType, int stageIndex)
		{
			int result;
			if (this._각인합성장치사용가격Dict.TryGetValue(new ValueTuple<Chapter.Type, int>(chapterType, stageIndex), out result))
			{
				return result;
			}
			return 123;
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x00014157 File Offset: 0x00012357
		public float 뼈입자검출기변량
		{
			get
			{
				return this._뼈입자검출기변량;
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00014160 File Offset: 0x00012360
		public int GetGoldCalculatorCount(System.Random random, Chapter.Type chapterType, int stageIndex)
		{
			Vector2Int vector2Int;
			if (this._황금계산기골드바갯수Dict.TryGetValue(new ValueTuple<Chapter.Type, int>(chapterType, stageIndex), out vector2Int))
			{
				return random.Next(vector2Int.x, vector2Int.y + 1);
			}
			return 1;
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001419B File Offset: 0x0001239B
		public float 황금계산기변량
		{
			get
			{
				return this._황금계산기변량;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x000141A3 File Offset: 0x000123A3
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x000141AB File Offset: 0x000123AB
		public float 건강보조장치스탯증폭량 { get; set; } = 1f;

		// Token: 0x06000703 RID: 1795 RVA: 0x000141B4 File Offset: 0x000123B4
		public void Initialize()
		{
			this._흉조증폭기확률Dict = new Dictionary<ValueTuple<Chapter.Type, float>, CustomFloat>();
			foreach (DarktechSetting.ValueByStage<CustomFloat> valueByStage in this._흉조증폭기확률)
			{
				this._흉조증폭기확률Dict.Add(new ValueTuple<Chapter.Type, float>(valueByStage.type, (float)valueByStage.stage), valueByStage.value);
			}
			this._각인합성장치사용가격Dict = new Dictionary<ValueTuple<Chapter.Type, int>, int>();
			foreach (DarktechSetting.ValueByStage<int> valueByStage2 in this._각인합성장치사용가격)
			{
				this._각인합성장치사용가격Dict.Add(new ValueTuple<Chapter.Type, int>(valueByStage2.type, valueByStage2.stage), valueByStage2.value);
			}
			this._품목순환장치상품별가격Dict = new Dictionary<ValueTuple<Chapter.Type, int>, float>();
			foreach (DarktechSetting.ValueByStage<float> valueByStage3 in this._품목순환장치스테이지설정)
			{
				this._품목순환장치상품별가격Dict.Add(new ValueTuple<Chapter.Type, int>(valueByStage3.type, valueByStage3.stage), valueByStage3.value);
			}
			this._황금계산기골드바갯수Dict = new Dictionary<ValueTuple<Chapter.Type, int>, Vector2Int>();
			foreach (DarktechSetting.ValueByStage<Vector2Int> valueByStage4 in this._황금계산기골드바갯수)
			{
				this._황금계산기골드바갯수Dict.Add(new ValueTuple<Chapter.Type, int>(valueByStage4.type, valueByStage4.stage), valueByStage4.value);
			}
		}

		// Token: 0x0400051D RID: 1309
		[Header("두개골 제조기")]
		[SerializeField]
		private int _두개골제조기가격;

		// Token: 0x0400051E RID: 1310
		[Header("보급품 제조기")]
		[SerializeField]
		private int _보급품제조기가격;

		// Token: 0x0400051F RID: 1311
		[Header("흉조 증폭기")]
		[SerializeField]
		private DarktechSetting.ValueByStage<CustomFloat>[] _흉조증폭기확률;

		// Token: 0x04000520 RID: 1312
		private IDictionary<ValueTuple<Chapter.Type, float>, CustomFloat> _흉조증폭기확률Dict;

		// Token: 0x04000521 RID: 1313
		[Header("품목 순환 장치")]
		[SerializeField]
		private DarktechSetting.ItemRotationEquipmentInfo[] _품목순환장치아이템설정;

		// Token: 0x04000522 RID: 1314
		[SerializeField]
		private DarktechSetting.ValueByStage<float>[] _품목순환장치스테이지설정;

		// Token: 0x04000523 RID: 1315
		[SerializeField]
		private int _품목순환장치버프맵카운트;

		// Token: 0x04000524 RID: 1316
		private IDictionary<ValueTuple<Chapter.Type, int>, float> _품목순환장치상품별가격Dict;

		// Token: 0x04000525 RID: 1317
		[Header("건강 보조 장치")]
		[SerializeField]
		private int[] _건강보조장치공격력버프가격;

		// Token: 0x04000526 RID: 1318
		[SerializeField]
		private float[] _건강보조장치공격력버프스텟;

		// Token: 0x04000527 RID: 1319
		[SerializeField]
		private int[] _건강보조장치체력버프가격;

		// Token: 0x04000528 RID: 1320
		[SerializeField]
		private float[] _건강보조장치체력버프스텟;

		// Token: 0x04000529 RID: 1321
		[SerializeField]
		private int[] _건강보조장치체력버프회복량;

		// Token: 0x0400052A RID: 1322
		[SerializeField]
		private int[] _건강보조장치속도버프가격;

		// Token: 0x0400052B RID: 1323
		[SerializeField]
		private float[] _건강보조장치속도버프스텟;

		// Token: 0x0400052C RID: 1324
		[Header("행운 계측기")]
		[SerializeField]
		private DarktechSetting.LuckyMeasuringInstrument _행운계측기설정;

		// Token: 0x0400052D RID: 1325
		[SerializeField]
		[Header("각인 합성 장치")]
		private DarktechSetting.ValueByStage<int>[] _각인합성장치사용가격;

		// Token: 0x0400052E RID: 1326
		private IDictionary<ValueTuple<Chapter.Type, int>, int> _각인합성장치사용가격Dict;

		// Token: 0x0400052F RID: 1327
		[Header("뼈입자 검출기")]
		[SerializeField]
		private float _뼈입자검출기변량;

		// Token: 0x04000530 RID: 1328
		[SerializeField]
		[Header("황금 계산기")]
		private float _황금계산기변량;

		// Token: 0x04000531 RID: 1329
		[SerializeField]
		private DarktechSetting.ValueByStage<Vector2Int>[] _황금계산기골드바갯수;

		// Token: 0x04000532 RID: 1330
		private IDictionary<ValueTuple<Chapter.Type, int>, Vector2Int> _황금계산기골드바갯수Dict;

		// Token: 0x02000160 RID: 352
		[Serializable]
		private class ValueByStage<T>
		{
			// Token: 0x17000167 RID: 359
			// (get) Token: 0x06000705 RID: 1797 RVA: 0x000142FF File Offset: 0x000124FF
			public Chapter.Type type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x17000168 RID: 360
			// (get) Token: 0x06000706 RID: 1798 RVA: 0x00014307 File Offset: 0x00012507
			public int stage
			{
				get
				{
					return this._stage;
				}
			}

			// Token: 0x17000169 RID: 361
			// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001430F File Offset: 0x0001250F
			public T value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x04000534 RID: 1332
			[SerializeField]
			private Chapter.Type _type;

			// Token: 0x04000535 RID: 1333
			[SerializeField]
			private int _stage;

			// Token: 0x04000536 RID: 1334
			[SerializeField]
			private T _value;
		}

		// Token: 0x02000161 RID: 353
		[Serializable]
		public class ItemRotationEquipmentInfo
		{
			// Token: 0x04000537 RID: 1335
			public DroppedPurchasableReward item;

			// Token: 0x04000538 RID: 1336
			public int weight;

			// Token: 0x04000539 RID: 1337
			public int basePrice;
		}

		// Token: 0x02000162 RID: 354
		[Serializable]
		public class ItemRotationEquipmentPriceInfo
		{
			// Token: 0x0400053A RID: 1338
			public float multiplier;
		}

		// Token: 0x02000163 RID: 355
		[Serializable]
		public class LuckyMeasuringInstrument
		{
			// Token: 0x1700016A RID: 362
			// (get) Token: 0x0600070B RID: 1803 RVA: 0x00014317 File Offset: 0x00012517
			public RarityPossibilities weightByRarity
			{
				get
				{
					return this._weightByRarity;
				}
			}

			// Token: 0x1700016B RID: 363
			// (get) Token: 0x0600070C RID: 1804 RVA: 0x0001431F File Offset: 0x0001251F
			public int lootableCount
			{
				get
				{
					return this._lootableCount;
				}
			}

			// Token: 0x1700016C RID: 364
			// (get) Token: 0x0600070D RID: 1805 RVA: 0x00014327 File Offset: 0x00012527
			public int maxRefreshCount
			{
				get
				{
					return this._refreshCount;
				}
			}

			// Token: 0x1700016D RID: 365
			// (get) Token: 0x0600070E RID: 1806 RVA: 0x0001432F File Offset: 0x0001252F
			public int refreshPrice
			{
				get
				{
					return this._refreshPrice;
				}
			}

			// Token: 0x1700016E RID: 366
			// (get) Token: 0x0600070F RID: 1807 RVA: 0x00014337 File Offset: 0x00012537
			public int uniquePityCount
			{
				get
				{
					return this._uniquePityCount;
				}
			}

			// Token: 0x1700016F RID: 367
			// (get) Token: 0x06000710 RID: 1808 RVA: 0x0001433F File Offset: 0x0001253F
			public int legendaryPityCount
			{
				get
				{
					return this._legendaryPityCount;
				}
			}

			// Token: 0x0400053B RID: 1339
			[SerializeField]
			private RarityPossibilities _weightByRarity;

			// Token: 0x0400053C RID: 1340
			[SerializeField]
			private int _lootableCount;

			// Token: 0x0400053D RID: 1341
			[SerializeField]
			private int _refreshCount;

			// Token: 0x0400053E RID: 1342
			[SerializeField]
			private int _refreshPrice;

			// Token: 0x0400053F RID: 1343
			[SerializeField]
			private int _uniquePityCount;

			// Token: 0x04000540 RID: 1344
			[SerializeField]
			private int _legendaryPityCount;
		}
	}
}
