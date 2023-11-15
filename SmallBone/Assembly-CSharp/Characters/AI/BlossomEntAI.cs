using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Level.Traps;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200103E RID: 4158
	public sealed class BlossomEntAI : AIController
	{
		// Token: 0x06005034 RID: 20532 RVA: 0x000F1CE8 File Offset: 0x000EFEE8
		private void Awake()
		{
			UnityEngine.Object.Instantiate<BlossomFog>(this._fogPrefab, this._fogPoint.position, Quaternion.identity, this._fogPoint);
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._wander,
				this._wanderAfterAttack,
				this._attack
			};
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x000F1D46 File Offset: 0x000EFF46
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x000F1D6E File Offset: 0x000EFF6E
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			base.StartCoroutine(this.StartAttackLoop());
			yield return this.Combat();
			yield break;
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x000F1D7D File Offset: 0x000EFF7D
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && !this._attacking)
				{
					yield return this._wanderAfterAttack.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x000F1D8C File Offset: 0x000EFF8C
		private IEnumerator StartAttackLoop()
		{
			while (!base.dead)
			{
				if (this._attack.CanUse())
				{
					this._attacking = true;
					base.StopAllBehaviour();
					yield return this._attack.CRun(this);
					this._attacking = false;
				}
				else
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x04004080 RID: 16512
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004081 RID: 16513
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wander;

		// Token: 0x04004082 RID: 16514
		[Subcomponent(typeof(WanderForDuration))]
		[SerializeField]
		private WanderForDuration _wanderAfterAttack;

		// Token: 0x04004083 RID: 16515
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x04004084 RID: 16516
		[SerializeField]
		private BlossomFog _fogPrefab;

		// Token: 0x04004085 RID: 16517
		[SerializeField]
		private Transform _fogPoint;

		// Token: 0x04004086 RID: 16518
		private bool _attacking;
	}
}
