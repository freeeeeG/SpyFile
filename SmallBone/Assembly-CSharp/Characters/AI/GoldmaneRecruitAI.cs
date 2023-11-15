using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001071 RID: 4209
	public sealed class GoldmaneRecruitAI : AIController
	{
		// Token: 0x06005157 RID: 20823 RVA: 0x000F4BD0 File Offset: 0x000F2DD0
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._wander,
				this._attack,
				this._chase,
				this._idle
			};
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x000F4C0D File Offset: 0x000F2E0D
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x000F4C35 File Offset: 0x000F2E35
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x000F4C3D File Offset: 0x000F2E3D
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			base.StartCoroutine(this.ChangeStopTrigger());
			while (!base.dead)
			{
				yield return this._wander.CRun(this);
				yield return this.Combat();
			}
			yield break;
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x000F4C4C File Offset: 0x000F2E4C
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!base.stuned && !(base.target == null) && this.character.movement.controller.isGrounded)
				{
					if (base.FindClosestPlayerBody(this._guardTrigger) != null)
					{
						if (this._guard.canUse)
						{
							yield return this.Guard();
						}
						else if (base.FindClosestPlayerBody(this._attackTrigger) != null)
						{
							yield return this._attack.CRun(this);
						}
						else
						{
							yield return this._chase.CRun(this);
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

		// Token: 0x0600515C RID: 20828 RVA: 0x000F4C5B File Offset: 0x000F2E5B
		private IEnumerator Guard()
		{
			this._guard.TryStart();
			while (this._guard.running)
			{
				yield return null;
			}
			yield return this._idle.CRun(this);
			yield break;
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x000F4C6A File Offset: 0x000F2E6A
		private IEnumerator ChangeStopTrigger()
		{
			while (!base.dead)
			{
				if (this._guard.canUse)
				{
					this.stopTrigger = this._guardTrigger;
				}
				else
				{
					this.stopTrigger = this._attackTrigger;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04004168 RID: 16744
		[Header("Behaviours")]
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004169 RID: 16745
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x0400416A RID: 16746
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private ActionAttack _attack;

		// Token: 0x0400416B RID: 16747
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x0400416C RID: 16748
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x0400416D RID: 16749
		[SerializeField]
		[Header("Guard")]
		private Characters.Actions.Action _guard;

		// Token: 0x0400416E RID: 16750
		[SerializeField]
		[Header("Range")]
		private Collider2D _attackTrigger;

		// Token: 0x0400416F RID: 16751
		[SerializeField]
		private Collider2D _guardTrigger;
	}
}
