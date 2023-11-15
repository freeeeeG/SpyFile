using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BCD RID: 3021
	public sealed class DarkAbilityContainer : MonoBehaviour
	{
		// Token: 0x06003E2E RID: 15918 RVA: 0x000B4BDC File Offset: 0x000B2DDC
		public void Initialize(Character owner)
		{
			DarkAbilityContainer.extraSeed++;
			this._owner = owner;
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 1177618293 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex + DarkAbilityContainer.extraSeed));
			this._countWeight = new int[]
			{
				this._singleAbility,
				this._dualAbility
			};
			this._candidates = new List<ValueTuple<DarkAbility, float>>();
			foreach (WeightedDarkAbility weightedDarkAbility in this._weightedDarkAbility.components)
			{
				if (weightedDarkAbility.Available(owner))
				{
					this._candidates.Add(new ValueTuple<DarkAbility, float>(weightedDarkAbility.key, (float)weightedDarkAbility.value));
				}
			}
			this._weightedRandomizer = new WeightedRandomizer<DarkAbility>(this._candidates);
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x000B4CD0 File Offset: 0x000B2ED0
		public ICollection<DarkAbility> GetDarkAbility()
		{
			this._electedAbilities = new List<DarkAbility>();
			int i = this.EvaluateCount(this._random);
			while (i > 0)
			{
				DarkAbility darkAbility = this._weightedRandomizer.TakeOne(this._random);
				DarkAbility darkAbility2 = UnityEngine.Object.Instantiate<DarkAbility>(darkAbility, this._owner.attach.transform);
				darkAbility2.name = darkAbility.name;
				darkAbility2.Initialize();
				this._electedAbilities.Add(darkAbility2);
				i--;
				if (i > 0)
				{
					for (int j = this._candidates.Count; j >= 0; j--)
					{
						if (this._candidates[j].Item1 == darkAbility)
						{
							this._candidates.RemoveAt(j);
							this._weightedRandomizer = new WeightedRandomizer<DarkAbility>(this._candidates);
							break;
						}
					}
				}
			}
			return this._electedAbilities;
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x000B4DA4 File Offset: 0x000B2FA4
		private void OnDestroy()
		{
			DarkAbilityContainer.extraSeed--;
			if (this._owner == null || this._owner.health.dead)
			{
				return;
			}
			if (this._electedAbilities == null)
			{
				return;
			}
			foreach (DarkAbility darkAbility in this._electedAbilities)
			{
				darkAbility.RemoveFrom(this._owner);
			}
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x000B4E30 File Offset: 0x000B3030
		public int EvaluateCount(System.Random random)
		{
			int maxValue = Mathf.Max(this._countWeight.Sum(), 100);
			int num = random.Next(0, maxValue) + 1;
			for (int i = 0; i < this._countWeight.Length; i++)
			{
				num -= this._countWeight[i];
				if (num <= 0)
				{
					return i + 1;
				}
			}
			return 1;
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x000B4E84 File Offset: 0x000B3084
		public void ResetAllWeightValue()
		{
			WeightedDarkAbility[] components = this._weightedDarkAbility.components;
			for (int i = 0; i < components.Length; i++)
			{
				components[i].ResetValue();
			}
		}

		// Token: 0x04003008 RID: 12296
		private static int extraSeed;

		// Token: 0x04003009 RID: 12297
		private const int _randomSeed = 1177618293;

		// Token: 0x0400300A RID: 12298
		[Header("Ability 개수 설정")]
		[Range(0f, 100f)]
		[SerializeField]
		private int _singleAbility = 100;

		// Token: 0x0400300B RID: 12299
		[SerializeField]
		[Range(0f, 100f)]
		private int _dualAbility;

		// Token: 0x0400300C RID: 12300
		[Header("Ability Pool")]
		[SerializeField]
		[Subcomponent(typeof(WeightedDarkAbility))]
		private WeightedDarkAbility.Subcomponents _weightedDarkAbility;

		// Token: 0x0400300D RID: 12301
		private WeightedRandomizer<DarkAbility> _weightedRandomizer;

		// Token: 0x0400300E RID: 12302
		private List<ValueTuple<DarkAbility, float>> _candidates;

		// Token: 0x0400300F RID: 12303
		private System.Random _random;

		// Token: 0x04003010 RID: 12304
		private int[] _countWeight;

		// Token: 0x04003011 RID: 12305
		private List<DarkAbility> _electedAbilities;

		// Token: 0x04003012 RID: 12306
		private Character _owner;
	}
}
