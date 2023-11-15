using System;
using System.Collections;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DEB RID: 3563
	public class Leap2 : CharacterOperation
	{
		// Token: 0x0600475E RID: 18270 RVA: 0x000CF67A File Offset: 0x000CD87A
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CMoveToTarget(owner));
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x00048973 File Offset: 0x00046B73
		public override void Stop()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x000CF68A File Offset: 0x000CD88A
		private IEnumerator CMoveToTarget(Character owner)
		{
			float destination = (this._target == null) ? this._aiController.target.transform.position.x : this._target.transform.position.x;
			float source = owner.transform.position.x;
			for (;;)
			{
				float num = destination - source;
				Vector2 vector = (num > 0f) ? Vector2.right : Vector2.left;
				owner.movement.MoveHorizontal(vector);
				if (num > 0f)
				{
					if (owner.transform.position.x + vector.x * this._checkDistance > destination)
					{
						break;
					}
				}
				else if (owner.transform.position.x + vector.x * this._checkDistance < destination)
				{
					break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400366A RID: 13930
		[SerializeField]
		private AIController _aiController;

		// Token: 0x0400366B RID: 13931
		[SerializeField]
		private Transform _target;

		// Token: 0x0400366C RID: 13932
		[SerializeField]
		private float _checkDistance = 0.5f;
	}
}
