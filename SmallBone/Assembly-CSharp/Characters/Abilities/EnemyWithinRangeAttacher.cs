using System;
using System.Collections;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009E9 RID: 2537
	public class EnemyWithinRangeAttacher : AbilityAttacher
	{
		// Token: 0x060035F9 RID: 13817 RVA: 0x000A023F File Offset: 0x0009E43F
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(128);
			this._overlapper.contactFilter.SetLayerMask(1024);
			if (this._optimizeRange)
			{
				this._range.enabled = false;
			}
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x000A027F File Offset: 0x0009E47F
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x000A028C File Offset: 0x0009E48C
		public override void StartAttach()
		{
			base.StartCoroutine("CCheck");
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x000A029A File Offset: 0x0009E49A
		public override void StopAttach()
		{
			base.StopCoroutine("CCheck");
			if (base.owner == null)
			{
				return;
			}
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x000A02D2 File Offset: 0x0009E4D2
		private IEnumerator CCheck()
		{
			for (;;)
			{
				using (new UsingCollider(this._range, this._optimizeRange))
				{
					this._overlapper.OverlapCollider(this._range);
				}
				if ((this._type == EnemyWithinRangeAttacher.Type.GreaterThanOrEqual && this._overlapper.results.Count >= this._numberOfEnemy) || (this._type == EnemyWithinRangeAttacher.Type.LessThan && this._overlapper.results.Count < this._numberOfEnemy) || (this._type == EnemyWithinRangeAttacher.Type.Equal && this._overlapper.results.Count == this._numberOfEnemy))
				{
					this.Attach();
				}
				else
				{
					this.Detach();
				}
				yield return Chronometer.global.WaitForSeconds(this._checkInterval);
			}
			yield break;
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x000A02E1 File Offset: 0x0009E4E1
		private void Attach()
		{
			if (this._attached)
			{
				return;
			}
			this._attached = true;
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x000A030F File Offset: 0x0009E50F
		private void Detach()
		{
			if (!this._attached)
			{
				return;
			}
			this._attached = false;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B58 RID: 11096
		[SerializeField]
		private EnemyWithinRangeAttacher.Type _type;

		// Token: 0x04002B59 RID: 11097
		[SerializeField]
		private int _numberOfEnemy;

		// Token: 0x04002B5A RID: 11098
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04002B5B RID: 11099
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		[SerializeField]
		private bool _optimizeRange = true;

		// Token: 0x04002B5C RID: 11100
		[SerializeField]
		private float _checkInterval = 0.25f;

		// Token: 0x04002B5D RID: 11101
		private NonAllocOverlapper _overlapper;

		// Token: 0x04002B5E RID: 11102
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B5F RID: 11103
		private bool _attached;

		// Token: 0x020009EA RID: 2538
		private enum Type
		{
			// Token: 0x04002B61 RID: 11105
			GreaterThanOrEqual,
			// Token: 0x04002B62 RID: 11106
			LessThan,
			// Token: 0x04002B63 RID: 11107
			Equal
		}
	}
}
