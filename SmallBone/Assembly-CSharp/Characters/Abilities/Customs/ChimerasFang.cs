using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D38 RID: 3384
	[Serializable]
	public class ChimerasFang : Ability
	{
		// Token: 0x06004441 RID: 17473 RVA: 0x000C6548 File Offset: 0x000C4748
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x000C655B File Offset: 0x000C475B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ChimerasFang.Instance(owner, this);
		}

		// Token: 0x0400340C RID: 13324
		[SerializeField]
		private HitInfo _hitInfo;

		// Token: 0x0400340D RID: 13325
		[SerializeField]
		private CharacterTypeBoolArray _normalTypes;

		// Token: 0x0400340E RID: 13326
		[SerializeField]
		private int _healthPercent = 10;

		// Token: 0x0400340F RID: 13327
		[SerializeField]
		private CharacterTypeBoolArray _bossTypes;

		// Token: 0x04003410 RID: 13328
		[SerializeField]
		private int _healthPercentForBoss = 5;

		// Token: 0x04003411 RID: 13329
		[SerializeField]
		private float _timeScaleDuringKilling = 0.3f;

		// Token: 0x04003412 RID: 13330
		[SerializeField]
		private float _killingDelay = 1f;

		// Token: 0x04003413 RID: 13331
		[Space]
		[SerializeField]
		private Transform _operationPosition;

		// Token: 0x04003414 RID: 13332
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x02000D39 RID: 3385
		public class Instance : AbilityInstance<ChimerasFang>
		{
			// Token: 0x06004444 RID: 17476 RVA: 0x000C6591 File Offset: 0x000C4791
			public Instance(Character owner, ChimerasFang ability) : base(owner, ability)
			{
				this._targets = new HashSet<Character>();
			}

			// Token: 0x06004445 RID: 17477 RVA: 0x000C65A6 File Offset: 0x000C47A6
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			}

			// Token: 0x06004446 RID: 17478 RVA: 0x000C65CF File Offset: 0x000C47CF
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			}

			// Token: 0x06004447 RID: 17479 RVA: 0x000C65F8 File Offset: 0x000C47F8
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (target.character == null)
				{
					return;
				}
				if (target.character.health.dead)
				{
					return;
				}
				if (!target.transform.gameObject.activeSelf)
				{
					return;
				}
				if (target.character.status == null)
				{
					return;
				}
				if (!target.character.status.poisoned)
				{
					return;
				}
				if (this._targets.Contains(target.character))
				{
					return;
				}
				if (this.ability._normalTypes[target.character.type])
				{
					if (target.character.health.percent > (double)this.ability._healthPercent * 0.01)
					{
						return;
					}
				}
				else
				{
					if (!this.ability._bossTypes[target.character.type])
					{
						return;
					}
					if (target.character.health.percent > (double)this.ability._healthPercentForBoss * 0.01)
					{
						return;
					}
				}
				this.ability._operationPosition.position = target.transform.position;
				this.ability._operations.Run(target.character);
				target.character.chronometer.animation.AttachTimeScale(this, this.ability._timeScaleDuringKilling);
				target.character.StartCoroutine(this.CDelayedKill(target.character));
				this._targets.Add(target.character);
			}

			// Token: 0x06004448 RID: 17480 RVA: 0x000C677E File Offset: 0x000C497E
			private IEnumerator CDelayedKill(Character target)
			{
				yield return Chronometer.global.WaitForSeconds(this.ability._killingDelay);
				target.chronometer.animation.DetachTimeScale(this);
				if (target.health.dead)
				{
					this._targets.Remove(target);
					yield break;
				}
				Damage damage = this.owner.stat.GetDamage(target.health.currentHealth, MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._hitInfo);
				this.owner.Attack(target, ref damage);
				if (!target.health.dead && target.key != Key.Yggdrasil)
				{
					target.health.Kill();
				}
				this._targets.Remove(target);
				yield break;
			}

			// Token: 0x04003415 RID: 13333
			private HashSet<Character> _targets;
		}
	}
}
