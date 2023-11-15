using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Abilities;
using Characters.Movements;
using Characters.Operations;
using Data;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008B9 RID: 2233
	public sealed class Treasure : InscriptionInstance
	{
		// Token: 0x06002F92 RID: 12178 RVA: 0x0008EC5D File Offset: 0x0008CE5D
		protected override void Initialize()
		{
			this._hasChest.treasure = this;
			this._hasChest.Initialize();
			this._onSpawn.Initialize();
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x0008EC81 File Offset: 0x0008CE81
		public override void Attach()
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += this.OnMapChangedAndFadeIn;
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x0008ECA0 File Offset: 0x0008CEA0
		private void OnMapChangedAndFadeIn(Map old, Map @new)
		{
			if (this.keyword.step < 1)
			{
				return;
			}
			if (Map.Instance.type != Map.Type.Normal && Map.Instance.type != Map.Type.Special)
			{
				return;
			}
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			System.Random seed = new System.Random((int)(GameData.Save.instance.randomSeed + -612673708 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this.AttachReward(seed);
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x0008ED28 File Offset: 0x0008CF28
		private void AttachReward(System.Random seed)
		{
			List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
			allEnemies.PseudoShuffle(seed);
			for (int i = 0; i < allEnemies.Count; i++)
			{
				Character character = allEnemies[i];
				if (character.type == Character.Type.TrashMob && !(character.movement == null) && character.movement.baseConfig.type != Movement.Config.Type.AcceleratingFlying && character.movement.baseConfig.type != Movement.Config.Type.Flying)
				{
					this._reference.Stop();
					this._reference = this.StartCoroutineWithReference(this.CTryToAddAbility(character));
					return;
				}
			}
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x0008EDC0 File Offset: 0x0008CFC0
		private IEnumerator CTryToAddAbility(Character enemy)
		{
			while ((enemy != null && enemy.ability == null) || !enemy.gameObject.activeSelf)
			{
				yield return null;
			}
			if (enemy != null)
			{
				enemy.ability.Add(this._hasChest);
			}
			yield break;
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x0008EDD8 File Offset: 0x0008CFD8
		public void Spawn(Vector2 point)
		{
			TreasureChest treasureChest = null;
			if (this.keyword.step >= 1)
			{
				treasureChest = this._set2chest;
			}
			if (this.keyword.step >= 2)
			{
				treasureChest = this._set4chest;
			}
			if (treasureChest != null)
			{
				this._targetPoint.position = point;
				UnityEngine.Object.Instantiate<TreasureChest>(treasureChest, point, Quaternion.identity, Map.Instance.transform);
				base.StartCoroutine(this._onSpawn.CRun(base.character));
			}
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x0008EE5F File Offset: 0x0008D05F
		public override void Detach()
		{
			this._reference.Stop();
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= this.OnMapChangedAndFadeIn;
		}

		// Token: 0x04002745 RID: 10053
		[SerializeField]
		[Header("2세트 효과")]
		private TreasureChest _set2chest;

		// Token: 0x04002746 RID: 10054
		[SerializeField]
		[Header("4세트 효과")]
		private TreasureChest _set4chest;

		// Token: 0x04002747 RID: 10055
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002748 RID: 10056
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onSpawn;

		// Token: 0x04002749 RID: 10057
		[SerializeField]
		private Treasure.HasChest _hasChest;

		// Token: 0x0400274A RID: 10058
		private const int _randomSeed = -612673708;

		// Token: 0x0400274B RID: 10059
		private CoroutineReference _reference;

		// Token: 0x020008BA RID: 2234
		[Serializable]
		private sealed class HasChest : Ability
		{
			// Token: 0x06002F9C RID: 12188 RVA: 0x0008EE87 File Offset: 0x0008D087
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new Treasure.HasChest.Instance(owner, this);
			}

			// Token: 0x0400274C RID: 10060
			[NonSerialized]
			public Treasure treasure;

			// Token: 0x020008BB RID: 2235
			public class Instance : AbilityInstance<Treasure.HasChest>
			{
				// Token: 0x06002F9D RID: 12189 RVA: 0x0008EE90 File Offset: 0x0008D090
				public Instance(Character owner, Treasure.HasChest ability) : base(owner, ability)
				{
				}

				// Token: 0x06002F9E RID: 12190 RVA: 0x0008EE9A File Offset: 0x0008D09A
				protected override void OnAttach()
				{
					this.owner.health.onDied += this.OnDied;
				}

				// Token: 0x06002F9F RID: 12191 RVA: 0x00002191 File Offset: 0x00000391
				protected override void OnDetach()
				{
				}

				// Token: 0x06002FA0 RID: 12192 RVA: 0x0008EEB8 File Offset: 0x0008D0B8
				private void OnDied()
				{
					this.ability.treasure.Spawn(this.owner.transform.position);
					this.owner.health.onDied -= this.OnDied;
				}
			}
		}

		// Token: 0x020008BC RID: 2236
		[Serializable]
		public sealed class StageInfo
		{
			// Token: 0x0400274D RID: 10061
			[Header("2세트")]
			public CustomFloat goldAmount2Set;

			// Token: 0x0400274E RID: 10062
			[Header("4세트")]
			public CustomFloat goldAmount4Set;

			// Token: 0x0400274F RID: 10063
			[Range(0f, 100f)]
			[Header("상자 가중치")]
			public int goldChestWeight;

			// Token: 0x04002750 RID: 10064
			[Range(0f, 100f)]
			public int currencyBagChestWeight;

			// Token: 0x04002751 RID: 10065
			[Range(0f, 100f)]
			public int itemChechWeight;

			// Token: 0x04002752 RID: 10066
			[Header("자원 가중치")]
			public CurrencyPossibilities currencyPossibilities;

			// Token: 0x04002753 RID: 10067
			public RarityPossibilities currencyRarityPossibilities;

			// Token: 0x04002754 RID: 10068
			public CurrencyRangeByRarity goldRangeByRarity;

			// Token: 0x04002755 RID: 10069
			public CurrencyRangeByRarity darkQuartzRangeByRarity;

			// Token: 0x04002756 RID: 10070
			public CurrencyRangeByRarity boneRangeByRarity;

			// Token: 0x04002757 RID: 10071
			[Header("아이템 등급 가중치")]
			public RarityPossibilities itemRarityPossibilities;
		}
	}
}
