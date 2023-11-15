using System;
using System.Collections.Generic;
using Characters.Operations;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Abilities.Items
{
	// Token: 0x02000D04 RID: 3332
	[Serializable]
	public sealed class SymbolOfToughness : Ability
	{
		// Token: 0x0600433A RID: 17210 RVA: 0x000C3F98 File Offset: 0x000C2198
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new SymbolOfToughness.Instance(owner, this);
		}

		// Token: 0x04003365 RID: 13157
		[SerializeField]
		private string _counterAttackKey;

		// Token: 0x04003366 RID: 13158
		[Header("최대 데미지 증폭값 (percent) > 1")]
		[SerializeField]
		private float _damageMaxMultiplier;

		// Token: 0x04003367 RID: 13159
		[SerializeField]
		[Header("데미지 증폭값 (percent) = 1 + shield양 * damageConversionRatio")]
		private double _damageConversionRatio;

		// Token: 0x04003368 RID: 13160
		[SerializeField]
		private Transform _summonTransform;

		// Token: 0x04003369 RID: 13161
		[SerializeField]
		private Vector2 _offset = new Vector2(1f, 0.5f);

		// Token: 0x0400336A RID: 13162
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		[Header("Summon")]
		internal OperationRunner _operationRunner;

		// Token: 0x0400336B RID: 13163
		[SerializeField]
		private float _attackCooldownTime = 1f;

		// Token: 0x0400336C RID: 13164
		internal static List<SymbolOfToughness.Instance> _instances = new List<SymbolOfToughness.Instance>();

		// Token: 0x0400336D RID: 13165
		internal static int attackIndex = 0;

		// Token: 0x02000D05 RID: 3333
		public class Histroy
		{
			// Token: 0x0600433D RID: 17213 RVA: 0x000C3FDB File Offset: 0x000C21DB
			public Histroy(SymbolOfToughness.Instance instance)
			{
				this.instance = instance;
			}

			// Token: 0x0400336E RID: 13166
			public SymbolOfToughness.Instance instance;
		}

		// Token: 0x02000D06 RID: 3334
		public class Instance : AbilityInstance<SymbolOfToughness>
		{
			// Token: 0x0600433E RID: 17214 RVA: 0x000C3FEA File Offset: 0x000C21EA
			public Instance(Character owner, SymbolOfToughness ability) : base(owner, ability)
			{
			}

			// Token: 0x0600433F RID: 17215 RVA: 0x000C3FF4 File Offset: 0x000C21F4
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.HandleOnTakeDamage));
				this.owner.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
				this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.OnGiveDamage));
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HanldeOnGaveDamage));
				this._lastAttackTime = Time.time;
				SymbolOfToughness._instances.Add(this);
			}

			// Token: 0x06004340 RID: 17216 RVA: 0x000C40A4 File Offset: 0x000C22A4
			private bool HandleOnTakeDamage(ref Damage damage)
			{
				if (Time.time - this._lastAttackTime < this.ability._attackCooldownTime)
				{
					this._precondition = false;
					return false;
				}
				if (this.owner.health.shield.hasAny && this.owner.health.shield.amount > 0.0)
				{
					this._precondition = true;
					this._multiplier = (double)Mathf.Min(this.ability._damageMaxMultiplier, 1f + (float)(this.owner.health.shield.amount * this.ability._damageConversionRatio));
					return false;
				}
				this._precondition = false;
				return false;
			}

			// Token: 0x06004341 RID: 17217 RVA: 0x000C415C File Offset: 0x000C235C
			private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (!this._precondition)
				{
					return;
				}
				if (damageDealt < 1.0)
				{
					return;
				}
				Character character = originalDamage.attacker.character;
				if (character == null)
				{
					return;
				}
				this._lastAttackTime = Time.time;
				Vector3 one = Vector3.one;
				if (character.transform.position.x > this.owner.transform.position.x)
				{
					this.ability._summonTransform.position = new Vector2(this.owner.transform.position.x + this.ability._offset.x, this.owner.transform.position.y);
				}
				else
				{
					this.ability._summonTransform.position = new Vector2(this.owner.transform.position.x - this.ability._offset.x, this.owner.transform.position.y);
					one = new Vector3(-1f, 1f, 1f);
				}
				OperationRunner operationRunner = this.ability._operationRunner.Spawn();
				OperationInfos operationInfos = operationRunner.operationInfos;
				operationInfos.transform.SetPositionAndRotation(this.ability._summonTransform.position, Quaternion.identity);
				SortingGroup component = operationRunner.GetComponent<SortingGroup>();
				if (component != null)
				{
					SortingGroup sortingGroup = component;
					short num = SymbolOfToughness.Instance.spriteLayer;
					SymbolOfToughness.Instance.spriteLayer = num + 1;
					sortingGroup.sortingOrder = (int)num;
				}
				operationInfos.transform.localScale = one;
				operationInfos.Run(this.owner);
			}

			// Token: 0x06004342 RID: 17218 RVA: 0x000C4304 File Offset: 0x000C2504
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				if (!string.IsNullOrWhiteSpace(this.ability._counterAttackKey) && !damage.key.Equals(this.ability._counterAttackKey, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				if (!SymbolOfToughness._instances[SymbolOfToughness.attackIndex].Equals(this))
				{
					return false;
				}
				damage.percentMultiplier *= this._multiplier;
				return false;
			}

			// Token: 0x06004343 RID: 17219 RVA: 0x000C4368 File Offset: 0x000C2568
			private void HanldeOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (!string.IsNullOrWhiteSpace(this.ability._counterAttackKey) && !gaveDamage.key.Equals(this.ability._counterAttackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				if (!SymbolOfToughness._instances[SymbolOfToughness.attackIndex].Equals(this))
				{
					return;
				}
				SymbolOfToughness.attackIndex++;
				if (SymbolOfToughness.attackIndex >= SymbolOfToughness._instances.Count)
				{
					SymbolOfToughness.attackIndex = 0;
				}
			}

			// Token: 0x06004344 RID: 17220 RVA: 0x000C43DC File Offset: 0x000C25DC
			protected override void OnDetach()
			{
				SymbolOfToughness._instances.Remove(this);
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.HandleOnTakeDamage));
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HanldeOnGaveDamage));
			}

			// Token: 0x0400336F RID: 13167
			private double _multiplier;

			// Token: 0x04003370 RID: 13168
			private static short spriteLayer = short.MinValue;

			// Token: 0x04003371 RID: 13169
			private bool _precondition;

			// Token: 0x04003372 RID: 13170
			private float _lastAttackTime;
		}
	}
}
