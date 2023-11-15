using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Abilities.Darks;
using Data;
using FX;
using Hardmode;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x02000470 RID: 1136
	[CreateAssetMenu]
	public sealed class DarkEnemySelector : ScriptableObject
	{
		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x0004408E File Offset: 0x0004228E
		public static DarkEnemySelector instance
		{
			get
			{
				if (DarkEnemySelector._instance == null)
				{
					DarkEnemySelector._instance = Resources.Load<DarkEnemySelector>("HardmodeSetting/DarkEnemySelector");
				}
				return DarkEnemySelector._instance;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x000440B1 File Offset: 0x000422B1
		public EffectInfo[] introEffects
		{
			get
			{
				return this._introEffects;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x000440B9 File Offset: 0x000422B9
		public EffectInfo[] dieEffects
		{
			get
			{
				return this._dieEffects;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060015AD RID: 5549 RVA: 0x000440C1 File Offset: 0x000422C1
		public EffectInfo dieEffect
		{
			get
			{
				return this._dieEffect;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060015AE RID: 5550 RVA: 0x000440C9 File Offset: 0x000422C9
		public CharacterHealthBar healthbar
		{
			get
			{
				return this._healthBar;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060015AF RID: 5551 RVA: 0x000440D1 File Offset: 0x000422D1
		public SoundInfo introSound
		{
			get
			{
				return this._introSound;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x000440D9 File Offset: 0x000422D9
		public SoundInfo dieSound
		{
			get
			{
				return this._dieSound;
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x000440E4 File Offset: 0x000422E4
		public int SetTargetCountInStage()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._stageRandom = new System.Random((int)(GameData.Save.instance.randomSeed + 1177618293 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16));
			int darkEnemyTotalCount = HardmodeLevelInfo.instance.GetDarkEnemyTotalCount(this._stageRandom);
			return this._remainsInStage = darkEnemyTotalCount;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00044150 File Offset: 0x00042350
		public void SetTargetCountInMap()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._mapRandom = new System.Random((int)(GameData.Save.instance.randomSeed + 1177618293 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this._remainInMap = HardmodeLevelInfo.instance.GetDarkEnemyCountPerMap(this._mapRandom);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x000441C4 File Offset: 0x000423C4
		public void ElectIn(ICollection<Character> candidates)
		{
			if (!Map.TestingTool.darkenemy)
			{
				return;
			}
			if (Singleton<Service>.Instance.levelManager.currentChapter.type != Chapter.Type.Test && (this._remainInMap <= 0 || this._remainsInStage <= 0))
			{
				return;
			}
			if (candidates.Count <= 0)
			{
				Debug.LogError(string.Format("검은적이 배치되어 있지 않습니다. 하드모드 : {0}, 레벨 {1}", GameData.HardmodeProgress.hardmode, GameData.HardmodeProgress.hardmodeLevel));
				return;
			}
			Character target = (from candidate in candidates
			where candidate.key != Key.Hound
			select candidate).Random(this._mapRandom);
			this._constructors[Singleton<HardmodeManager>.Instance.currentLevel].Provide(target);
			this._remainsInStage--;
			this._remainInMap--;
		}

		// Token: 0x040012EE RID: 4846
		private const int _randomSeed = 1177618293;

		// Token: 0x040012EF RID: 4847
		private static DarkEnemySelector _instance;

		// Token: 0x040012F0 RID: 4848
		[SerializeField]
		private EffectInfo _dieEffect;

		// Token: 0x040012F1 RID: 4849
		[SerializeField]
		private EffectInfo[] _introEffects;

		// Token: 0x040012F2 RID: 4850
		[SerializeField]
		private EffectInfo[] _dieEffects;

		// Token: 0x040012F3 RID: 4851
		[SerializeField]
		private SoundInfo _introSound;

		// Token: 0x040012F4 RID: 4852
		[SerializeField]
		private SoundInfo _dieSound;

		// Token: 0x040012F5 RID: 4853
		[SerializeField]
		private CharacterHealthBar _healthBar;

		// Token: 0x040012F6 RID: 4854
		[SerializeField]
		private DarkAbilityConstructor[] _constructors;

		// Token: 0x040012F7 RID: 4855
		[SerializeField]
		private DarkAbilityConstructor _constructorForTest;

		// Token: 0x040012F8 RID: 4856
		private int _remainsInStage;

		// Token: 0x040012F9 RID: 4857
		private int _remainInMap;

		// Token: 0x040012FA RID: 4858
		private System.Random _stageRandom;

		// Token: 0x040012FB RID: 4859
		private System.Random _mapRandom;
	}
}
