using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours.Attacks;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200129D RID: 4765
	public class ChaseAndAttack : Behaviour
	{
		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06005E75 RID: 24181 RVA: 0x0011590C File Offset: 0x00113B0C
		public Chase chase
		{
			get
			{
				return this._chase;
			}
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06005E76 RID: 24182 RVA: 0x00115914 File Offset: 0x00113B14
		public Attack attack
		{
			get
			{
				return this._attack;
			}
		}

		// Token: 0x06005E77 RID: 24183 RVA: 0x0011591C File Offset: 0x00113B1C
		private void Start()
		{
			this._childs = new List<Behaviour>
			{
				this._chase,
				this._attack
			};
		}

		// Token: 0x06005E78 RID: 24184 RVA: 0x00115941 File Offset: 0x00113B41
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				if (controller.target == null)
				{
					base.result = Behaviour.Result.Done;
					break;
				}
				if (controller.character.movement.controller.isGrounded)
				{
					if (controller.dead)
					{
						break;
					}
					if (controller.FindClosestPlayerBody(this._attackCollider) != null)
					{
						yield return this._attack.CRun(controller);
					}
					else
					{
						yield return this._chase.CRun(controller);
						if (this._chase.result == Behaviour.Result.Success)
						{
							yield return this._attack.CRun(controller);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x04004BEA RID: 19434
		[Chase.SubcomponentAttribute(true)]
		[SerializeField]
		private Chase _chase;

		// Token: 0x04004BEB RID: 19435
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private Attack _attack;

		// Token: 0x04004BEC RID: 19436
		[SerializeField]
		private Collider2D _attackCollider;
	}
}
