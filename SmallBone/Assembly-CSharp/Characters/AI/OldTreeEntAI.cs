using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200105C RID: 4188
	public sealed class OldTreeEntAI : AIController
	{
		// Token: 0x060050D9 RID: 20697 RVA: 0x000F3575 File Offset: 0x000F1775
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x000F359D File Offset: 0x000F179D
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			if (!this._stopMove)
			{
				yield return this._wander.CRun(this);
			}
			yield return this.Combat();
			yield break;
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x000F35AC File Offset: 0x000F17AC
		private IEnumerator Combat()
		{
			this._effectBoundsX = this._effectCollider.bounds.size.x;
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						if (base.target.movement.isGrounded && base.target.movement.controller.collisionState.lastStandingCollider.bounds.size.x >= this._attackMinimumWidth)
						{
							this.SetEffectPosition();
							yield return this._attack.CRun(this);
						}
					}
					else if (!this._stopMove)
					{
						yield return this._chase.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x000F35BC File Offset: 0x000F17BC
		private void SetEffectPosition()
		{
			Bounds bounds = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
			if (bounds.max.x - 1f < base.target.transform.position.x + this._effectBoundsX / 2f)
			{
				this._effect.position = new Vector2(bounds.max.x - this._effectBoundsX / 2f, bounds.max.y);
				return;
			}
			if (bounds.min.x + 1f > base.target.transform.position.x - this._effectBoundsX / 2f)
			{
				this._effect.position = new Vector2(bounds.min.x + this._effectBoundsX / 2f, bounds.max.y);
				return;
			}
			this._effect.position = new Vector2(base.target.transform.position.x, bounds.max.y);
		}

		// Token: 0x040040FB RID: 16635
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040040FC RID: 16636
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wander;

		// Token: 0x040040FD RID: 16637
		[SerializeField]
		[Subcomponent(typeof(Chase))]
		private Chase _chase;

		// Token: 0x040040FE RID: 16638
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x040040FF RID: 16639
		[SerializeField]
		private Collider2D _attackTrigger;

		// Token: 0x04004100 RID: 16640
		[SerializeField]
		private float _attackMinimumWidth;

		// Token: 0x04004101 RID: 16641
		[SerializeField]
		private bool _stopMove;

		// Token: 0x04004102 RID: 16642
		[SerializeField]
		private Transform _effect;

		// Token: 0x04004103 RID: 16643
		[SerializeField]
		private Collider2D _effectCollider;

		// Token: 0x04004104 RID: 16644
		private float _effectBoundsX;
	}
}
