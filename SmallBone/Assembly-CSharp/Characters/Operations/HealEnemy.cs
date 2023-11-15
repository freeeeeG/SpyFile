using System;
using System.Collections;
using System.Collections.Generic;
using FX;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DC3 RID: 3523
	public class HealEnemy : CharacterOperation
	{
		// Token: 0x060046CC RID: 18124 RVA: 0x000CD867 File Offset: 0x000CBA67
		static HealEnemy()
		{
			HealEnemy._enemyOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x000CD88E File Offset: 0x000CBA8E
		private void Start()
		{
			this._enemyWaveContainer = Map.Instance.waveContainer;
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x000CD8A0 File Offset: 0x000CBAA0
		public override void Run(Character owner)
		{
			Character character = this.SelectTarget(owner);
			this._effect.Spawn(character.transform.position, character, 0f, 1f).transform.SetParent(character.transform);
			this._background.Spawn(character.transform.position, character, 0f, 1f).transform.SetParent(character.transform);
			base.StartCoroutine(this.CRun(owner, character));
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x000CD926 File Offset: 0x000CBB26
		private IEnumerator CRun(Character owner, Character target)
		{
			int i = 0;
			while ((float)i < this._count)
			{
				target.health.Heal(this.GetAmount(target), true);
				yield return owner.chronometer.master.WaitForSeconds(this._delay);
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x000CD944 File Offset: 0x000CBB44
		private double GetAmount(Character target)
		{
			HealEnemy.HealType healType = this._healType;
			if (healType == HealEnemy.HealType.Percent)
			{
				return (double)this._amount.value * target.health.maximumHealth * 0.01;
			}
			if (healType != HealEnemy.HealType.Constnat)
			{
				return 0.0;
			}
			return (double)this._amount.value;
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x000CD99C File Offset: 0x000CBB9C
		private Character SelectTarget(Character owner)
		{
			List<Character> list = this.FindEnemiesInRange(this._range);
			if (list.Count <= 1)
			{
				return owner;
			}
			HealEnemy.TargetType targetType = this._targetType;
			if (targetType == HealEnemy.TargetType.LowestHealth)
			{
				Character character = owner;
				foreach (Character character2 in list)
				{
					if (character2.gameObject.activeSelf && !character2.health.dead && character2.health.percent < character.health.percent)
					{
						character = character2;
					}
				}
				return character;
			}
			if (targetType == HealEnemy.TargetType.Random)
			{
				return list.Random<Character>();
			}
			return null;
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x000CDA50 File Offset: 0x000CBC50
		private List<Character> FindEnemiesInRange(Collider2D collider)
		{
			collider.enabled = true;
			List<Character> components = HealEnemy._enemyOverlapper.OverlapCollider(collider).GetComponents<Character>(true);
			collider.enabled = false;
			return components;
		}

		// Token: 0x040035A6 RID: 13734
		[SerializeField]
		private Collider2D _range;

		// Token: 0x040035A7 RID: 13735
		[SerializeField]
		private HealEnemy.TargetType _targetType;

		// Token: 0x040035A8 RID: 13736
		[SerializeField]
		private HealEnemy.HealType _healType;

		// Token: 0x040035A9 RID: 13737
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x040035AA RID: 13738
		[SerializeField]
		private float _count;

		// Token: 0x040035AB RID: 13739
		[SerializeField]
		private float _delay;

		// Token: 0x040035AC RID: 13740
		[SerializeField]
		private EffectInfo _background;

		// Token: 0x040035AD RID: 13741
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x040035AE RID: 13742
		private EnemyWaveContainer _enemyWaveContainer;

		// Token: 0x040035AF RID: 13743
		private static readonly NonAllocOverlapper _enemyOverlapper = new NonAllocOverlapper(31);

		// Token: 0x02000DC4 RID: 3524
		private enum TargetType
		{
			// Token: 0x040035B1 RID: 13745
			LowestHealth,
			// Token: 0x040035B2 RID: 13746
			Random
		}

		// Token: 0x02000DC5 RID: 3525
		private enum HealType
		{
			// Token: 0x040035B4 RID: 13748
			Percent,
			// Token: 0x040035B5 RID: 13749
			Constnat
		}
	}
}
