using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E0F RID: 3599
	public class TranslateFromTarget : CharacterOperation
	{
		// Token: 0x060047DE RID: 18398 RVA: 0x000D1340 File Offset: 0x000CF540
		public override void Run(Character owner)
		{
			Vector3 position = this._aIController.target.transform.position;
			bool flag = owner.transform.position.x < position.x;
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, flag ? Vector2.left : Vector2.right, 7f, 256);
			Vector3 zero = Vector3.zero;
			if (!hit)
			{
				zero = new Vector3(flag ? (position.x - this._distributionX) : (position.x + this._distributionX), position.y - this._offsetY, 0f);
			}
			else
			{
				RaycastHit2D hit2 = Physics2D.Raycast(base.transform.position, (!flag) ? Vector2.left : Vector2.right, 7f, 256);
				if (!hit2)
				{
					zero = new Vector3((!flag) ? (position.x - this._distributionX) : (position.x + this._distributionX), position.y - this._offsetY, 0f);
				}
				else if (hit.distance > hit2.distance)
				{
					zero = new Vector3(flag ? (position.x - hit.distance) : (position.x + hit.distance), position.y - this._offsetY, 0f);
				}
				else
				{
					zero = new Vector3(flag ? (position.x - hit2.distance) : (position.x + hit2.distance), position.y - this._offsetY, 0f);
				}
			}
			this._transform.position = zero;
			if (position.x > this._transform.position.x)
			{
				this._transform.localScale = new Vector3(1f, 1f, 1f);
				return;
			}
			this._transform.localScale = new Vector3(-1f, 1f, 1f);
		}

		// Token: 0x04003703 RID: 14083
		[SerializeField]
		private Transform _transform;

		// Token: 0x04003704 RID: 14084
		[SerializeField]
		private AIController _aIController;

		// Token: 0x04003705 RID: 14085
		[SerializeField]
		[Range(0f, 10f)]
		private float _offsetY;

		// Token: 0x04003706 RID: 14086
		[Range(0f, 10f)]
		[SerializeField]
		private float _distributionX;
	}
}
