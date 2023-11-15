using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Operations.Attack;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200106C RID: 4204
	public sealed class GoldmaneManAtArmsAI : AIController
	{
		// Token: 0x06005138 RID: 20792 RVA: 0x000F4511 File Offset: 0x000F2711
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x000F4539 File Offset: 0x000F2739
		protected override IEnumerator CProcess()
		{
			this._goldenWave.Initialize();
			this._goldenWaveSound.Initialize();
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this._idle.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x000F4548 File Offset: 0x000F2748
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!base.stuned && !(base.target == null) && this.character.movement.controller.isGrounded)
				{
					if (base.FindClosestPlayerBody(this.stopTrigger) != null)
					{
						if (this._tackle.CanUse())
						{
							yield return this._tackle.CRun(this);
						}
						yield return this.CAttack();
					}
					else
					{
						yield return this._chase.CRun(this);
						if (this._chase.result == Characters.AI.Behaviours.Behaviour.Result.Success)
						{
							yield return this.CAttack();
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x000F4557 File Offset: 0x000F2757
		private IEnumerator CAttack()
		{
			float num = this.character.transform.position.x - base.target.transform.position.x;
			this.character.lookingDirection = ((num > 0f) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			base.StartCoroutine(this._attack.CRun(this));
			if (this._attack.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				base.StartCoroutine(this.DoGoldenWave());
			}
			while (this._attack.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				yield return null;
			}
			if (MMMaths.Chance(this._idleChanceAfterAttack))
			{
				yield return this._idle.CRun(this);
			}
			yield break;
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x000F4566 File Offset: 0x000F2766
		private IEnumerator DoGoldenWave()
		{
			float elapsed = 0f;
			while (elapsed < 1f)
			{
				elapsed += this.character.chronometer.animation.deltaTime;
				yield return null;
			}
			if (base.stuned)
			{
				yield break;
			}
			Bounds platformBounds = this.character.movement.controller.collisionState.lastStandingCollider.bounds;
			float xPosition = this._goldenWaveStartPoint.position.x;
			float sizeX = this._goldenWaveArea.bounds.size.x;
			float extentsX = this._goldenWaveArea.bounds.extents.x;
			int sign = (this.character.lookingDirection == Character.LookingDirection.Right) ? 1 : -1;
			int count = this._goldenWaveCount;
			for (int j = 0; j < this._goldenWaveCount; j++)
			{
				float num = xPosition + (sizeX * (float)j + extentsX) * (float)sign + (float)j * this._goldenWaveDistance * (float)sign;
				if (num >= platformBounds.max.x || num <= platformBounds.min.x)
				{
					count = j + 1;
					break;
				}
			}
			int num2;
			for (int i = 0; i < count; i = num2 + 1)
			{
				this._goldenWaveArea.transform.position = new Vector3(xPosition + (sizeX * (float)i + extentsX) * (float)sign + (float)i * this._goldenWaveDistance * (float)sign, platformBounds.max.y);
				yield return null;
				this._goldenWaveEffect.Run(this.character);
				this._goldenWave.Run(this.character);
				this._goldenWaveSound.Run(this.character);
				this._cameraShake.Run(this.character);
				yield return this.character.chronometer.master.WaitForSeconds(this._goldenWaveTerm);
				if (base.stuned)
				{
					break;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x000F4575 File Offset: 0x000F2775
		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(this._goldenWaveArea.gameObject);
		}

		// Token: 0x04004144 RID: 16708
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004145 RID: 16709
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wander;

		// Token: 0x04004146 RID: 16710
		[SerializeField]
		[Chase.SubcomponentAttribute(true)]
		private Chase _chase;

		// Token: 0x04004147 RID: 16711
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private ActionAttack _attack;

		// Token: 0x04004148 RID: 16712
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _tackle;

		// Token: 0x04004149 RID: 16713
		[SerializeField]
		private SweepAttack2 _goldenWave;

		// Token: 0x0400414A RID: 16714
		[SerializeField]
		private PlaySound _goldenWaveSound;

		// Token: 0x0400414B RID: 16715
		[SerializeField]
		private SpawnEffect _goldenWaveEffect;

		// Token: 0x0400414C RID: 16716
		[SerializeField]
		[Subcomponent(typeof(CameraShake))]
		private CameraShake _cameraShake;

		// Token: 0x0400414D RID: 16717
		[SerializeField]
		[Range(0f, 1f)]
		private float _idleChanceAfterAttack;

		// Token: 0x0400414E RID: 16718
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x0400414F RID: 16719
		[Space]
		[SerializeField]
		[Header("GoldenWave")]
		private Transform _goldenWaveStartPoint;

		// Token: 0x04004150 RID: 16720
		[SerializeField]
		private Collider2D _goldenWaveArea;

		// Token: 0x04004151 RID: 16721
		[SerializeField]
		private float _goldenWaveTerm;

		// Token: 0x04004152 RID: 16722
		[SerializeField]
		private int _goldenWaveCount;

		// Token: 0x04004153 RID: 16723
		[SerializeField]
		private float _goldenWaveDistance;
	}
}
