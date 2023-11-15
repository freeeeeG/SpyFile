using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001111 RID: 4369
	public sealed class ShieldKnightStatueAI : AIController
	{
		// Token: 0x060054FE RID: 21758 RVA: 0x000FE119 File Offset: 0x000FC319
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._attack
			};
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x000FCBBB File Offset: 0x000FADBB
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x000F4C35 File Offset: 0x000F2E35
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x000FE132 File Offset: 0x000FC332
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			while (!base.dead)
			{
				yield return this.Combat();
			}
			yield break;
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x000FE141 File Offset: 0x000FC341
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!base.stuned && !(base.target == null) && this.character.movement.controller.isGrounded)
				{
					if (base.FindClosestPlayerBody(this._guardTrigger) != null)
					{
						yield return this.Guard();
					}
					else if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						if (this._guard.running)
						{
							this.character.CancelAction();
						}
						yield return this._attack.CRun(this);
					}
					else if (this._guard.running)
					{
						this.character.CancelAction();
					}
				}
			}
			yield break;
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x000FE150 File Offset: 0x000FC350
		private IEnumerator Guard()
		{
			this._guard.TryStart();
			while (this._guard.running && base.FindClosestPlayerBody(this._guardTrigger) != null)
			{
				yield return null;
			}
			this._guardEnd.TryStart();
			while (this._guardEnd.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04004427 RID: 17447
		[Header("Behaviours")]
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private ActionAttack _attack;

		// Token: 0x04004428 RID: 17448
		[SerializeField]
		[Header("Guard")]
		private Characters.Actions.Action _guard;

		// Token: 0x04004429 RID: 17449
		[SerializeField]
		private Characters.Actions.Action _guardEnd;

		// Token: 0x0400442A RID: 17450
		[SerializeField]
		[Header("Range")]
		private Collider2D _guardTrigger;

		// Token: 0x0400442B RID: 17451
		[SerializeField]
		private Collider2D _attackTrigger;
	}
}
