using System;
using System.Collections;
using Characters.Gear.Items;
using Characters.Operations;
using FX;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CEC RID: 3308
	[Serializable]
	public sealed class OmenMisfortune : Ability
	{
		// Token: 0x060042E0 RID: 17120 RVA: 0x000C2DE3 File Offset: 0x000C0FE3
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OmenMisfortune.Instance(owner, this);
		}

		// Token: 0x0400332B RID: 13099
		[SerializeField]
		private Item _ownerItem;

		// Token: 0x0400332C RID: 13100
		[SerializeField]
		private float[] _angles = new float[]
		{
			90f,
			45f,
			135f,
			0f,
			180f
		};

		// Token: 0x0400332D RID: 13101
		[SerializeField]
		private float[] _lookAngles = new float[]
		{
			270f,
			245f,
			295f,
			220f,
			320f
		};

		// Token: 0x0400332E RID: 13102
		[SerializeField]
		private float _markDistanceFromTop = 1.5f;

		// Token: 0x0400332F RID: 13103
		[SerializeField]
		private float _attackInterval = 0.05f;

		// Token: 0x04003330 RID: 13104
		[SerializeField]
		private EffectInfo _mark;

		// Token: 0x04003331 RID: 13105
		[SerializeField]
		private SoundInfo _attachSound;

		// Token: 0x04003332 RID: 13106
		[SerializeField]
		private EffectInfo _attackEffect;

		// Token: 0x04003333 RID: 13107
		[SerializeField]
		private SoundInfo _attackSound;

		// Token: 0x04003334 RID: 13108
		[SerializeField]
		private EffectInfo _hitEffect;

		// Token: 0x04003335 RID: 13109
		[SerializeField]
		private SoundInfo _hitSound;

		// Token: 0x04003336 RID: 13110
		[SerializeField]
		private CustomFloat _attackDamage;

		// Token: 0x04003337 RID: 13111
		[SerializeField]
		private HitInfo _hitInfo;

		// Token: 0x04003338 RID: 13112
		[SerializeField]
		private int _maxStack;

		// Token: 0x02000CED RID: 3309
		public sealed class Instance : AbilityInstance<OmenMisfortune>
		{
			// Token: 0x060042E2 RID: 17122 RVA: 0x000C2E43 File Offset: 0x000C1043
			public Instance(Character owner, OmenMisfortune ability) : base(owner, ability)
			{
				this._effect = ability._mark;
				this._effectInstances = new EffectPoolInstance[ability._angles.Length];
			}

			// Token: 0x060042E3 RID: 17123 RVA: 0x000C2E7C File Offset: 0x000C107C
			protected override void OnAttach()
			{
				this._stack = 1;
				this.UpdateStack();
			}

			// Token: 0x060042E4 RID: 17124 RVA: 0x000C2E8B File Offset: 0x000C108B
			private IEnumerator CAttack()
			{
				Character attacker = this.ability._ownerItem.owner;
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._hitSound, this.owner.transform.position);
				if (this._effectInstances != null)
				{
					this.ability._hitInfo.ChangeAdaptiveDamageAttribute(attacker);
					if (this._effectInstances != null)
					{
						EffectPoolInstance[] array = this._effectInstances;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].animator.Play(this._attackKey);
							Damage damage = attacker.stat.GetDamage((double)this.ability._attackDamage.value, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), this.ability._hitInfo);
							attacker.Attack(this.owner, ref damage);
							this.ability._hitEffect.Spawn(MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), 0f, 1f);
							yield return Chronometer.global.WaitForSeconds(this.ability._attackInterval);
						}
						array = null;
					}
				}
				yield return Chronometer.global.WaitForSeconds(0.6f - this.ability._attackInterval);
				if (this._effectInstances != null)
				{
					for (int j = this._effectInstances.Length - 1; j >= 0; j--)
					{
						EffectPoolInstance effectPoolInstance = this._effectInstances[j];
						if (effectPoolInstance != null)
						{
							effectPoolInstance.Stop();
							this._effectInstances[j] = null;
						}
					}
				}
				base.remainTime = 0f;
				yield break;
			}

			// Token: 0x060042E5 RID: 17125 RVA: 0x000C2E9C File Offset: 0x000C109C
			public override void Refresh()
			{
				base.Refresh();
				if (this._attackRunning)
				{
					return;
				}
				this._stack++;
				this.UpdateStack();
				if (this._stack >= this.ability._maxStack)
				{
					this._attackRunning = true;
					Map.Instance.StartCoroutine(this.CAttack());
				}
			}

			// Token: 0x060042E6 RID: 17126 RVA: 0x000C2EF8 File Offset: 0x000C10F8
			private void UpdateStack()
			{
				Vector2 v = new Vector2(this.owner.collider.bounds.center.x, this.owner.collider.bounds.max.y) + Quaternion.Euler(0f, 0f, this.ability._angles[this._stack - 1]) * Vector3.right * this.ability._markDistanceFromTop;
				EffectPoolInstance effectPoolInstance = (this._effect == null) ? null : this._effect.Spawn(v, this.owner, 0f, 1f);
				effectPoolInstance.transform.parent = this.owner.attach.transform;
				effectPoolInstance.transform.rotation = Quaternion.AngleAxis(this.ability._lookAngles[this._stack - 1], Vector3.forward);
				this._effectInstances[this._stack - 1] = effectPoolInstance;
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attackSound, this.owner.transform.position);
			}

			// Token: 0x060042E7 RID: 17127 RVA: 0x000C3034 File Offset: 0x000C1234
			protected override void OnDetach()
			{
				for (int i = this._effectInstances.Length - 1; i >= 0; i--)
				{
					EffectPoolInstance effectPoolInstance = this._effectInstances[i];
					if (effectPoolInstance != null)
					{
						effectPoolInstance.Stop();
						this._effectInstances[i] = null;
					}
				}
			}

			// Token: 0x04003339 RID: 13113
			private readonly int _attackKey = Animator.StringToHash("SymbolOfDeath_Attack");

			// Token: 0x0400333A RID: 13114
			private readonly EffectInfo _effect;

			// Token: 0x0400333B RID: 13115
			private EffectPoolInstance[] _effectInstances;

			// Token: 0x0400333C RID: 13116
			private int _stack;

			// Token: 0x0400333D RID: 13117
			private bool _attackRunning;
		}
	}
}
