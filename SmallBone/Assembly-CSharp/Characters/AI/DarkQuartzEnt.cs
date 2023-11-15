using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200108E RID: 4238
	public sealed class DarkQuartzEnt : AIController
	{
		// Token: 0x06005208 RID: 21000 RVA: 0x000F633F File Offset: 0x000F453F
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005209 RID: 21001 RVA: 0x000F6367 File Offset: 0x000F4567
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x000F6376 File Offset: 0x000F4576
		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(this._effect.gameObject);
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x000F6388 File Offset: 0x000F4588
		private IEnumerator Combat()
		{
			this._effectBoundsX = this._effectCollider.bounds.size.x;
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && this.character.movement.controller.isGrounded)
				{
					if (base.FindClosestPlayerBody(this._meleeAttackCollider) != null)
					{
						if (this._areaAttack.CanUse())
						{
							this.SetPosition();
							yield return this._areaAttack.CRun(this);
						}
						else
						{
							yield return this._meleeAttack.CRun(this);
						}
					}
					else if (base.FindClosestPlayerBody(this._areaAttackCollider) != null)
					{
						if (base.target.movement.isGrounded)
						{
							if (this._areaAttack.CanUse())
							{
								this.SetPosition();
								yield return this._areaAttack.CRun(this);
							}
							else
							{
								yield return this._chase.CRun(this);
							}
						}
					}
					else
					{
						yield return this._chase.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x000F6398 File Offset: 0x000F4598
		private void SetPosition()
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

		// Token: 0x040041DB RID: 16859
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040041DC RID: 16860
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wander;

		// Token: 0x040041DD RID: 16861
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x040041DE RID: 16862
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _meleeAttack;

		// Token: 0x040041DF RID: 16863
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _areaAttack;

		// Token: 0x040041E0 RID: 16864
		[SerializeField]
		private Collider2D _meleeAttackCollider;

		// Token: 0x040041E1 RID: 16865
		[SerializeField]
		private Collider2D _areaAttackCollider;

		// Token: 0x040041E2 RID: 16866
		[SerializeField]
		private Transform _effect;

		// Token: 0x040041E3 RID: 16867
		[SerializeField]
		private Collider2D _effectCollider;

		// Token: 0x040041E4 RID: 16868
		private float _effectBoundsX;
	}
}
