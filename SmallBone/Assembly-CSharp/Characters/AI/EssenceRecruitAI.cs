using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Abilities;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001119 RID: 4377
	public sealed class EssenceRecruitAI : AIController
	{
		// Token: 0x06005526 RID: 21798 RVA: 0x000FE510 File Offset: 0x000FC710
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._moveToTarget,
				this._chaseTeleport,
				this._attack
			};
			this._abilityAttacher.Initialize(this.character);
			this._abilityAttacher.StartAttach();
		}

		// Token: 0x06005527 RID: 21799 RVA: 0x000FE580 File Offset: 0x000FC780
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x000FE5A8 File Offset: 0x000FC7A8
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this._found.CRun(this);
			this._abilityAttacher.StopAttach();
			while (!base.dead)
			{
				if (base.FindClosestPlayerBody(this._attckTrigger) == null)
				{
					yield return this._moveToTarget.CRun(this);
					if (this._moveToTarget.result == Characters.AI.Behaviours.Behaviour.Result.Fail)
					{
						yield return this._chaseTeleport.CRun(this);
					}
				}
				yield return this._attack.CRun(this);
			}
			yield break;
		}

		// Token: 0x0400443D RID: 17469
		[SerializeField]
		private Collider2D _attckTrigger;

		// Token: 0x0400443E RID: 17470
		[Subcomponent(typeof(CheckWithinSightContinuous))]
		[SerializeField]
		private CheckWithinSightContinuous _checkWithinSight;

		// Token: 0x0400443F RID: 17471
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x04004440 RID: 17472
		[Subcomponent(typeof(MoveToTarget))]
		[SerializeField]
		private MoveToTarget _moveToTarget;

		// Token: 0x04004441 RID: 17473
		[SerializeField]
		[Subcomponent(typeof(ChaseTeleport))]
		private ChaseTeleport _chaseTeleport;

		// Token: 0x04004442 RID: 17474
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private Attack _found;

		// Token: 0x04004443 RID: 17475
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private Attack _attack;

		// Token: 0x04004444 RID: 17476
		[Space]
		[AbilityAttacher.SubcomponentAttribute]
		[SerializeField]
		private AbilityAttacher _abilityAttacher;
	}
}
