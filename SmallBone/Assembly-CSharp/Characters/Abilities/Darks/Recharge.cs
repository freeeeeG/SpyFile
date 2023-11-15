using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Level;
using Level.Objects;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BC5 RID: 3013
	[Serializable]
	public sealed class Recharge : Ability
	{
		// Token: 0x06003E0A RID: 15882 RVA: 0x000B4541 File Offset: 0x000B2741
		public override void Initialize()
		{
			base.Initialize();
			this._onHealed.Initialize();
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x000B4554 File Offset: 0x000B2754
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Recharge.Instance(owner, this);
		}

		// Token: 0x04002FEF RID: 12271
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onHealed;

		// Token: 0x04002FF0 RID: 12272
		[SerializeField]
		private Character _toSummonPrefab;

		// Token: 0x04002FF1 RID: 12273
		[SerializeField]
		private float _summonCooldowntime;

		// Token: 0x04002FF2 RID: 12274
		[SerializeField]
		private float _summonLifeTime;

		// Token: 0x04002FF3 RID: 12275
		[SerializeField]
		private float _summonTargetMaxDistance;

		// Token: 0x04002FF4 RID: 12276
		[SerializeField]
		private SoundInfo _summonSound;

		// Token: 0x04002FF5 RID: 12277
		[SerializeField]
		[Range(0f, 1f)]
		private float _healPercent;

		// Token: 0x02000BC6 RID: 3014
		public sealed class Instance : AbilityInstance<Recharge>
		{
			// Token: 0x06003E0D RID: 15885 RVA: 0x000B455D File Offset: 0x000B275D
			public Instance(Character owner, Recharge ability) : base(owner, ability)
			{
				this._summons = new List<Recharge.Instance.Summons>();
			}

			// Token: 0x06003E0E RID: 15886 RVA: 0x000B4574 File Offset: 0x000B2774
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				for (int i = this._summons.Count - 1; i >= 0; i--)
				{
					Recharge.Instance.Summons summons = this._summons[i];
					summons.remainTime -= deltaTime;
					if (summons.remainTime <= 0f)
					{
						this.KillSummons(summons.character);
						this._summons.RemoveAt(i);
					}
				}
				this._remainCooldownTime -= deltaTime;
				if (this._remainCooldownTime <= 0f)
				{
					this.TryToSummon();
				}
			}

			// Token: 0x06003E0F RID: 15887 RVA: 0x000B4604 File Offset: 0x000B2804
			private void KillSummons(Character summon)
			{
				if (summon != null && !summon.health.dead && summon.gameObject.activeInHierarchy)
				{
					this.owner.health.PercentHeal(this.ability._healPercent);
					this.owner.StartCoroutine(this.ability._onHealed.CRun(this.owner));
					summon.health.Kill();
				}
			}

			// Token: 0x06003E10 RID: 15888 RVA: 0x000B4680 File Offset: 0x000B2880
			private void TryToSummon()
			{
				Character character = UnityEngine.Object.Instantiate<Character>(this.ability._toSummonPrefab, this.GetSummonPoint(), Quaternion.identity);
				DivineShieldEffect componentInChildren = character.GetComponentInChildren<DivineShieldEffect>();
				if (componentInChildren != null)
				{
					componentInChildren.Activate(this.owner);
				}
				this._summons.Add(new Recharge.Instance.Summons
				{
					character = character,
					remainTime = this.ability._summonLifeTime
				});
				Map.Instance.waveContainer.Attach(character);
				this._remainCooldownTime = this.ability._summonCooldowntime;
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._summonSound, this.owner.transform.position);
			}

			// Token: 0x06003E11 RID: 15889 RVA: 0x000B473C File Offset: 0x000B293C
			private Vector2 GetSummonPoint()
			{
				Collider2D collider2D;
				if (!this.owner.movement.TryGetClosestBelowCollider(out collider2D, Layers.footholdMask, 100f))
				{
					return this.owner.transform.position;
				}
				float minInclusive = Mathf.Max(collider2D.bounds.min.x, this.owner.transform.position.x - this.ability._summonTargetMaxDistance);
				float maxInclusive = Mathf.Min(collider2D.bounds.max.x, this.owner.transform.position.x + this.ability._summonTargetMaxDistance);
				return new Vector2(UnityEngine.Random.Range(minInclusive, maxInclusive), collider2D.bounds.max.y);
			}

			// Token: 0x06003E12 RID: 15890 RVA: 0x000B480E File Offset: 0x000B2A0E
			protected override void OnAttach()
			{
				this._summons.Clear();
			}

			// Token: 0x06003E13 RID: 15891 RVA: 0x000B481C File Offset: 0x000B2A1C
			protected override void OnDetach()
			{
				for (int i = this._summons.Count - 1; i >= 0; i--)
				{
					Recharge.Instance.Summons summons = this._summons[i];
					this.KillSummons(summons.character);
				}
				this._summons.Clear();
			}

			// Token: 0x04002FF6 RID: 12278
			private float _remainCooldownTime;

			// Token: 0x04002FF7 RID: 12279
			private List<Recharge.Instance.Summons> _summons;

			// Token: 0x02000BC7 RID: 3015
			private class Summons
			{
				// Token: 0x04002FF8 RID: 12280
				public Character character;

				// Token: 0x04002FF9 RID: 12281
				public float remainTime;
			}
		}
	}
}
