using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AF3 RID: 2803
	[Serializable]
	public sealed class ObsessiveCompulsive : Ability
	{
		// Token: 0x06003940 RID: 14656 RVA: 0x000A8D55 File Offset: 0x000A6F55
		public override void Initialize()
		{
			base.Initialize();
			this._marking.Initialize();
			this._onLoseHealth.Initialize();
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x000A8D73 File Offset: 0x000A6F73
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ObsessiveCompulsive.Instance(owner, this);
		}

		// Token: 0x04002D70 RID: 11632
		[SerializeField]
		private SoundInfo _attachAudioClipInfo;

		// Token: 0x04002D71 RID: 11633
		[SerializeField]
		private SoundInfo _detachAudioClipInfo;

		// Token: 0x04002D72 RID: 11634
		[SerializeField]
		[Header("필터")]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x04002D73 RID: 11635
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04002D74 RID: 11636
		[SerializeField]
		private CharacterTypeBoolArray _targetTypeFilter;

		// Token: 0x04002D75 RID: 11637
		[Header("설정")]
		[SerializeField]
		private int _missionHitCount;

		// Token: 0x04002D76 RID: 11638
		[SerializeField]
		private float _markingDuration;

		// Token: 0x04002D77 RID: 11639
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002D78 RID: 11640
		[SerializeField]
		private int _loseHealthAmount;

		// Token: 0x04002D79 RID: 11641
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onLoseHealth;

		// Token: 0x04002D7A RID: 11642
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _marking;

		// Token: 0x02000AF4 RID: 2804
		public sealed class Instance : AbilityInstance<ObsessiveCompulsive>
		{
			// Token: 0x06003943 RID: 14659 RVA: 0x000A8D7C File Offset: 0x000A6F7C
			public Instance(Character owner, ObsessiveCompulsive ability) : base(owner, ability)
			{
			}

			// Token: 0x06003944 RID: 14660 RVA: 0x000A8D88 File Offset: 0x000A6F88
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				foreach (KeyValuePair<Character, ObsessiveCompulsive.Instance.TargetHistory> keyValuePair in this._previousHistory)
				{
					Character key = keyValuePair.Key;
					ObsessiveCompulsive.Instance.TargetHistory value = keyValuePair.Value;
					value.elapsed += deltaTime;
					if (value.hitCount >= this.ability._missionHitCount && !value.expired)
					{
						value.character.ability.Remove(this.ability._marking.ability);
						value.expired = true;
						value.elapsed = this.ability._markingDuration;
					}
					if (value.elapsed >= this.ability._markingDuration + this.ability._cooldownTime)
					{
						this._currentHistory.Remove(key);
						PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._detachAudioClipInfo, this.owner.transform.position);
					}
					else
					{
						if (value.elapsed >= this.ability._markingDuration && !value.expired)
						{
							if (key.health.dead)
							{
								this._currentHistory.Remove(key);
								continue;
							}
							value.expired = true;
							if (value.hitCount < this.ability._missionHitCount)
							{
								this.owner.health.TakeHealth((double)this.ability._loseHealthAmount);
								Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage((double)this.ability._loseHealthAmount, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds));
								this.owner.StartCoroutine(this.ability._onLoseHealth.CRun(this.owner));
							}
						}
						if (this._currentHistory.ContainsKey(key))
						{
							this._currentHistory[key] = value;
						}
						else
						{
							this._currentHistory.Add(key, value);
						}
					}
				}
				this._previousHistory.Clear();
				foreach (KeyValuePair<Character, ObsessiveCompulsive.Instance.TargetHistory> keyValuePair2 in this._currentHistory)
				{
					this._previousHistory.Add(keyValuePair2.Key, keyValuePair2.Value);
				}
			}

			// Token: 0x06003945 RID: 14661 RVA: 0x000A900C File Offset: 0x000A720C
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!this.ability._attackTypeFilter[gaveDamage.attackType] || !this.ability._motionTypeFilter[gaveDamage.motionType] || !this.ability._targetTypeFilter[character.type])
				{
					return;
				}
				if (this._previousHistory.ContainsKey(character))
				{
					ObsessiveCompulsive.Instance.TargetHistory value = this._previousHistory[character];
					value.hitCount++;
					this._previousHistory[character] = value;
					return;
				}
				character.ability.Add(this.ability._marking.ability);
				this._previousHistory.Add(character, new ObsessiveCompulsive.Instance.TargetHistory
				{
					character = character
				});
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attachAudioClipInfo, this.owner.transform.position);
			}

			// Token: 0x06003946 RID: 14662 RVA: 0x000A9107 File Offset: 0x000A7307
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				this._previousHistory = new Dictionary<Character, ObsessiveCompulsive.Instance.TargetHistory>();
				this._currentHistory = new Dictionary<Character, ObsessiveCompulsive.Instance.TargetHistory>();
			}

			// Token: 0x06003947 RID: 14663 RVA: 0x000A9146 File Offset: 0x000A7346
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x04002D7B RID: 11643
			[SerializeField]
			private IDictionary<Character, ObsessiveCompulsive.Instance.TargetHistory> _previousHistory;

			// Token: 0x04002D7C RID: 11644
			private IDictionary<Character, ObsessiveCompulsive.Instance.TargetHistory> _currentHistory;

			// Token: 0x02000AF5 RID: 2805
			private struct TargetHistory
			{
				// Token: 0x04002D7D RID: 11645
				public Character character;

				// Token: 0x04002D7E RID: 11646
				public int hitCount;

				// Token: 0x04002D7F RID: 11647
				public float elapsed;

				// Token: 0x04002D80 RID: 11648
				public bool expired;
			}
		}
	}
}
