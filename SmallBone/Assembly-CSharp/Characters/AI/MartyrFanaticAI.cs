using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010D9 RID: 4313
	public class MartyrFanaticAI : AIController
	{
		// Token: 0x060053C6 RID: 21446 RVA: 0x000FB308 File Offset: 0x000F9508
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._suicide
			};
			this._speedBonus.Initialize();
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x000FB35B File Offset: 0x000F955B
		private void OnDestroy()
		{
			this._idleClipAfterWander = null;
			this._walkClipAfterWander = null;
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x000FB36B File Offset: 0x000F956B
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x000FB393 File Offset: 0x000F9593
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x000FB3A2 File Offset: 0x000F95A2
		private IEnumerator CCombat()
		{
			yield return this._wander.CRun(this);
			this._characterAnimation.SetIdle(this._idleClipAfterWander);
			this._characterAnimation.SetWalk(this._walkClipAfterWander);
			while (!base.dead)
			{
				if (base.FindClosestPlayerBody(this._attackTrigger) != null)
				{
					yield return this.CSuicide();
				}
				else
				{
					yield return this._chase.CRun(this);
					if (this._chase.result == Characters.AI.Behaviours.Behaviour.Result.Success)
					{
						yield return this.CSuicide();
					}
				}
			}
			yield break;
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x000FB3B1 File Offset: 0x000F95B1
		private IEnumerator CSuicide()
		{
			base.destination = base.target.transform.position;
			this._speedBonus.Run(this.character);
			yield return this._moveForSuicide.CRun(this);
			this._speedBonus.Stop();
			yield return this._suicide.CRun(this);
			yield break;
		}

		// Token: 0x0400434D RID: 17229
		[Subcomponent(typeof(CheckWithinSight))]
		[Header("Behaviours")]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400434E RID: 17230
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x0400434F RID: 17231
		[SerializeField]
		[Subcomponent(typeof(Chase))]
		private Chase _chase;

		// Token: 0x04004350 RID: 17232
		[SerializeField]
		[Subcomponent(typeof(MoveToDestination))]
		private MoveToDestination _moveForSuicide;

		// Token: 0x04004351 RID: 17233
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private Attack _suicide;

		// Token: 0x04004352 RID: 17234
		[Space]
		[SerializeField]
		[Header("Tools")]
		private Collider2D _attackTrigger;

		// Token: 0x04004353 RID: 17235
		[SerializeField]
		private AttachAbility _speedBonus;

		// Token: 0x04004354 RID: 17236
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x04004355 RID: 17237
		[SerializeField]
		private AnimationClip _idleClipAfterWander;

		// Token: 0x04004356 RID: 17238
		[SerializeField]
		private AnimationClip _walkClipAfterWander;
	}
}
