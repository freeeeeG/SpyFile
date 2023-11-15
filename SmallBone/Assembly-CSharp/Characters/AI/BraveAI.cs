using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.Movements;
using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010B7 RID: 4279
	public sealed class BraveAI : AIController
	{
		// Token: 0x060052F5 RID: 21237 RVA: 0x000F8B40 File Offset: 0x000F6D40
		private void Awake()
		{
			this.character.status.unstoppable.Attach(this);
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._moveToTargetHead,
				this._moveToTargetGround
			};
			this._attackOperations.Initialize();
			this._landingOperations.Initialize();
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x000F8BA8 File Offset: 0x000F6DA8
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x000F8BD0 File Offset: 0x000F6DD0
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x000F8BDF File Offset: 0x000F6DDF
		private IEnumerator CCombat()
		{
			this.character.movement.onGrounded += delegate()
			{
				this.character.movement.config.type = Movement.Config.Type.Walking;
				this.character.movement.controller.oneWayPlatformMask = 131072;
				base.StartCoroutine(this._landingOperations.CRun(this.character));
			};
			while (!base.dead)
			{
				this.character.movement.config.type = Movement.Config.Type.Walking;
				this.character.movement.config.gravity = -300f;
				this.character.movement.controller.terrainMask = Layers.terrainMask;
				yield return null;
				if (!(base.target == null))
				{
					if (base.stuned)
					{
						Debug.Log(base.name + " is stuned");
					}
					else if (this.CheckAttackable())
					{
						yield return this.CAttack();
						yield return this._idle.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x000F8BF0 File Offset: 0x000F6DF0
		private bool CheckAttackable()
		{
			if (base.FindClosestPlayerBody(this._attackTrigger) == null)
			{
				return false;
			}
			if (base.target.movement.controller.collisionState.lastStandingCollider == null)
			{
				return false;
			}
			if (!base.target.movement.controller.isGrounded)
			{
				return false;
			}
			Bounds bounds = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
			Bounds bounds2 = this.character.movement.controller.collisionState.lastStandingCollider.bounds;
			if (bounds.max.y != bounds2.max.y)
			{
				return false;
			}
			Bounds bounds3 = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
			float x = base.target.transform.position.x;
			float y = bounds3.max.y + this._attackHeight;
			Vector2 vector = new Vector2(x, y);
			Bounds bounds4 = this.character.collider.bounds;
			bounds4.size = new Vector2(0.2f, bounds4.size.y);
			bounds4.center = new Vector2(vector.x, vector.y + (bounds4.center.y - bounds4.min.y));
			return !this.TerrainColliding(bounds4);
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x000F8D7C File Offset: 0x000F6F7C
		private bool TerrainColliding(Bounds range)
		{
			NonAllocOverlapper.shared.contactFilter.SetLayerMask(Layers.terrainMask);
			return NonAllocOverlapper.shared.OverlapBox(range.center, range.size, 0f).results.Count != 0;
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x000F8DD1 File Offset: 0x000F6FD1
		private IEnumerator CAttack()
		{
			Bounds platform = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
			float x = base.target.transform.position.x;
			float y = platform.max.y + this._attackHeight;
			base.destination = new Vector2(x, y);
			this.character.movement.config.type = Movement.Config.Type.Flying;
			this.character.movement.controller.terrainMask = 0;
			this.character.movement.controller.oneWayPlatformMask = 0;
			yield return this._moveToTargetHead.CRun(this);
			yield return this._attackReady.CRun(this);
			y = platform.max.y;
			base.destination = new Vector2(x, y);
			base.StartCoroutine(this._attackOperations.CRun(this.character));
			this.character.movement.config.type = Movement.Config.Type.Walking;
			this.character.movement.controller.terrainMask = Layers.terrainMask;
			this.character.movement.controller.oneWayPlatformMask = 131072;
			yield break;
		}

		// Token: 0x0400429A RID: 17050
		[Header("Behaviours")]
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400429B RID: 17051
		[Subcomponent(typeof(MoveToDestinationWithFly))]
		[SerializeField]
		private MoveToDestinationWithFly _moveToTargetHead;

		// Token: 0x0400429C RID: 17052
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _attackReady;

		// Token: 0x0400429D RID: 17053
		[SerializeField]
		[Subcomponent(typeof(MoveToDestinationWithFly))]
		private MoveToDestinationWithFly _moveToTargetGround;

		// Token: 0x0400429E RID: 17054
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x0400429F RID: 17055
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _attackOperations;

		// Token: 0x040042A0 RID: 17056
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _landingOperations;

		// Token: 0x040042A1 RID: 17057
		[SerializeField]
		[Header("Tools")]
		[Space]
		private Collider2D _attackTrigger;

		// Token: 0x040042A2 RID: 17058
		[SerializeField]
		private float _attackHeight;

		// Token: 0x040042A3 RID: 17059
		private const float _widthCheckRange = 0.2f;
	}
}
