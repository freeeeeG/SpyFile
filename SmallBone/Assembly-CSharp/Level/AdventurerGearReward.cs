using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Gear.Items;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x02000473 RID: 1139
	public sealed class AdventurerGearReward : InteractiveObject
	{
		// Token: 0x060015BA RID: 5562 RVA: 0x000442D4 File Offset: 0x000424D4
		private void Start()
		{
			AdventurerGearReward.ExtraSeed extraSeed;
			if (!Map.Instance.TryGetComponent<AdventurerGearReward.ExtraSeed>(out extraSeed))
			{
				extraSeed = Map.Instance.gameObject.AddComponent<AdventurerGearReward.ExtraSeed>();
			}
			AdventurerGearReward.ExtraSeed extraSeed2 = extraSeed;
			extraSeed2.value += 1;
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + this._randomSeed + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex + (int)extraSeed.value));
			this._remainCount = this.count;
			base.StartCoroutine(this.CWaitForClear());
			if (!GameData.HardmodeProgress.hardmode)
			{
				IEnumerable<AdventurerGearReward.ItemWeight> source = from weight in this._gearWeights
				where !weight.onlyHardmode
				select weight;
				this._gearWeights = source.ToArray<AdventurerGearReward.ItemWeight>();
			}
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x000443BB File Offset: 0x000425BB
		private IEnumerator CWaitForClear()
		{
			while (Map.Instance.waveContainer.state != EnemyWaveContainer.State.Empty)
			{
				yield return null;
			}
			base.Activate();
			yield break;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x000443CC File Offset: 0x000425CC
		public override void InteractWith(Character character)
		{
			base.StartCoroutine(this.CShake());
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			float num = (float)UnityEngine.Random.Range(levelManager.currentChapter.currentStage.goldrewardAmount.x, levelManager.currentChapter.currentStage.goldrewardAmount.y) * levelManager.currentChapter.adventurerGoldRewardMultiplier;
			levelManager.DropGold((int)num, 5, base.transform.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			PurchasablePotion purchasablePotion = this.TakeOne(this._potionWeights);
			if (purchasablePotion != null)
			{
				PurchasablePotion purchasablePotion2 = UnityEngine.Object.Instantiate<PurchasablePotion>(purchasablePotion, base.transform.position + Vector3.up, Quaternion.identity);
				purchasablePotion2.name = purchasablePotion.name;
				purchasablePotion2.transform.parent = Map.Instance.transform;
				purchasablePotion2.Initialize();
				purchasablePotion2.dropMovement.Move(MMMaths.RandomBool() ? UnityEngine.Random.Range(0.5f, 2.5f) : UnityEngine.Random.Range(-2.5f, 0.5f), (float)UnityEngine.Random.Range(12, 20));
			}
			if (!this._dropDone)
			{
				Item item = this.TryToDropItem();
				if (item != null)
				{
					item.dropped.dropMovement.Move(MMMaths.RandomBool() ? UnityEngine.Random.Range(0.5f, 2.5f) : UnityEngine.Random.Range(-2.5f, 0.5f), (float)UnityEngine.Random.Range(12, 20));
					this._dropDone = true;
				}
			}
			this._remainCount--;
			if (this._remainCount <= 0)
			{
				base.Deactivate();
			}
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x00044580 File Offset: 0x00042780
		private Item TryToDropItem()
		{
			if (MMMaths.PercentChance(this._random, this._dropChance))
			{
				Item item = this.TakeOne(this._gearWeights);
				if (item != null && !Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item.HasGroup(item))
				{
					return Singleton<Service>.Instance.levelManager.DropItem(item, base.transform.position + Vector3.up);
				}
			}
			return null;
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x00044604 File Offset: 0x00042804
		public Item TakeOne(AdventurerGearReward.ItemWeight[] gearWeights)
		{
			int num = 0;
			foreach (AdventurerGearReward.ItemWeight itemWeight in gearWeights)
			{
				num += itemWeight.weight;
			}
			int num2 = this._random.Next(0, num) + 1;
			for (int j = 0; j < gearWeights.Length; j++)
			{
				num2 -= gearWeights[j].weight;
				if (num2 <= 0)
				{
					return gearWeights[j].gear;
				}
			}
			return null;
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x00044670 File Offset: 0x00042870
		public PurchasablePotion TakeOne(AdventurerGearReward.PotionWeight[] potionWeights)
		{
			int num = 0;
			foreach (AdventurerGearReward.PotionWeight potionWeight in potionWeights)
			{
				num += potionWeight.weight;
			}
			int num2 = this._random.Next(0, num) + 1;
			for (int j = 0; j < potionWeights.Length; j++)
			{
				num2 -= potionWeights[j].weight;
				if (num2 <= 0)
				{
					return potionWeights[j].potion;
				}
			}
			return null;
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x000446DB File Offset: 0x000428DB
		private IEnumerator CShake()
		{
			float elapsed = 0f;
			float intervalElapsed = 0f;
			Vector3 shakeVector = Vector3.zero;
			Vector3 originPosition = this._body.transform.localPosition;
			while (elapsed <= this._curve.duration)
			{
				float deltaTime = Chronometer.global.deltaTime;
				elapsed += deltaTime;
				intervalElapsed -= deltaTime;
				shakeVector -= 60f * deltaTime * shakeVector;
				if (intervalElapsed <= 0f)
				{
					intervalElapsed = this._interval;
					float num = 1f - this._curve.Evaluate(elapsed);
					shakeVector.x = UnityEngine.Random.Range(-this._power, this._power) * num;
					shakeVector.y = UnityEngine.Random.Range(-this._power, this._power) * num;
				}
				this._body.transform.localPosition = originPosition + shakeVector;
				yield return null;
			}
			this._body.transform.localPosition = originPosition;
			yield break;
		}

		// Token: 0x040012FE RID: 4862
		[SerializeField]
		[Range(0f, 100f)]
		private int _dropChance;

		// Token: 0x040012FF RID: 4863
		[SerializeField]
		private AdventurerGearReward.PotionWeight[] _potionWeights;

		// Token: 0x04001300 RID: 4864
		[SerializeField]
		private AdventurerGearReward.ItemWeight[] _gearWeights;

		// Token: 0x04001301 RID: 4865
		[SerializeField]
		private SpriteRenderer _body;

		// Token: 0x04001302 RID: 4866
		[SerializeField]
		private float _power = 0.2f;

		// Token: 0x04001303 RID: 4867
		[SerializeField]
		private Curve _curve;

		// Token: 0x04001304 RID: 4868
		[SerializeField]
		private float _interval = 0.1f;

		// Token: 0x04001305 RID: 4869
		private readonly int count = 3;

		// Token: 0x04001306 RID: 4870
		private readonly int _randomSeed = 1177618293;

		// Token: 0x04001307 RID: 4871
		private int _remainCount;

		// Token: 0x04001308 RID: 4872
		private bool _dropDone;

		// Token: 0x04001309 RID: 4873
		private System.Random _random;

		// Token: 0x02000474 RID: 1140
		private sealed class ExtraSeed : MonoBehaviour
		{
			// Token: 0x0400130A RID: 4874
			public short value;
		}

		// Token: 0x02000475 RID: 1141
		[Serializable]
		public class ItemWeight : IComparer<AdventurerGearReward.ItemWeight>
		{
			// Token: 0x060015C3 RID: 5571 RVA: 0x0004471A File Offset: 0x0004291A
			public int Compare(AdventurerGearReward.ItemWeight x, AdventurerGearReward.ItemWeight y)
			{
				if (x.weight > y.weight)
				{
					return 1;
				}
				if (x.weight < y.weight)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x0400130B RID: 4875
			public Item gear;

			// Token: 0x0400130C RID: 4876
			[Range(0f, 100f)]
			public int weight;

			// Token: 0x0400130D RID: 4877
			public bool onlyHardmode;
		}

		// Token: 0x02000476 RID: 1142
		[Serializable]
		public class PotionWeight : IComparer<AdventurerGearReward.PotionWeight>
		{
			// Token: 0x060015C5 RID: 5573 RVA: 0x0004473D File Offset: 0x0004293D
			public int Compare(AdventurerGearReward.PotionWeight x, AdventurerGearReward.PotionWeight y)
			{
				if (x.weight > y.weight)
				{
					return 1;
				}
				if (x.weight < y.weight)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x0400130E RID: 4878
			public PurchasablePotion potion;

			// Token: 0x0400130F RID: 4879
			[Range(0f, 100f)]
			public int weight;
		}
	}
}
