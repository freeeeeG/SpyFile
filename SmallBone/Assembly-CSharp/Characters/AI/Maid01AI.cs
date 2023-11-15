using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200107B RID: 4219
	public sealed class Maid01AI : AIController
	{
		// Token: 0x06005196 RID: 20886 RVA: 0x000F537A File Offset: 0x000F357A
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x000F53A2 File Offset: 0x000F35A2
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			base.StartCoroutine(this.CChangeStopTrigger());
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x000F53B1 File Offset: 0x000F35B1
		private IEnumerator CCombat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (base.FindClosestPlayerBody(this._dashAttackCollider) != null)
					{
						if (base.FindClosestPlayerBody(this._attackCollider) != null)
						{
							yield return this._attack.CRun(this);
						}
						else if (this._dashBuff.canUse)
						{
							yield return this.CDoDash();
							yield return this._dashAfterAttack.CRun(this);
						}
						else
						{
							yield return this._chase.CRun(this);
							if (this._chase.result == Characters.AI.Behaviours.Behaviour.Result.Success)
							{
								yield return this._attack.CRun(this);
							}
						}
					}
					else
					{
						yield return this._chase.CRun(this);
						if (this._chase.result == Characters.AI.Behaviours.Behaviour.Result.Success)
						{
							yield return this._attack.CRun(this);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x000F53C0 File Offset: 0x000F35C0
		private IEnumerator CDoDash()
		{
			base.destination = base.target.transform.position;
			this._dashBuff.TryStart();
			base.StartCoroutine(this._dash.CRun(this));
			yield return this.CStopDash();
			if (!this.character.hit.action.running)
			{
				this.character.CancelAction();
			}
			yield break;
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x000F53CF File Offset: 0x000F35CF
		private IEnumerator CStopDash()
		{
			while (!base.dead)
			{
				yield return null;
				if (this._dash.result != Characters.AI.Behaviours.Behaviour.Result.Doing)
				{
					break;
				}
				if (base.stuned)
				{
					this._dash.result = Characters.AI.Behaviours.Behaviour.Result.Done;
					break;
				}
			}
			yield break;
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x000F53DE File Offset: 0x000F35DE
		private IEnumerator CChangeStopTrigger()
		{
			while (!base.dead)
			{
				yield return null;
				if (this._dashBuff.canUse)
				{
					this.stopTrigger = this._dashAttackCollider;
				}
				else
				{
					this.stopTrigger = this._attackCollider;
				}
			}
			yield break;
		}

		// Token: 0x0400418E RID: 16782
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400418F RID: 16783
		[SerializeField]
		[Subcomponent(typeof(Chase))]
		private Chase _chase;

		// Token: 0x04004190 RID: 16784
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x04004191 RID: 16785
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _dashAfterAttack;

		// Token: 0x04004192 RID: 16786
		[SerializeField]
		private Collider2D _attackCollider;

		// Token: 0x04004193 RID: 16787
		[SerializeField]
		private Collider2D _dashAttackCollider;

		// Token: 0x04004194 RID: 16788
		[SerializeField]
		[Subcomponent(typeof(MoveToDestination))]
		private MoveToDestination _dash;

		// Token: 0x04004195 RID: 16789
		[SerializeField]
		private Characters.Actions.Action _dashBuff;
	}
}
