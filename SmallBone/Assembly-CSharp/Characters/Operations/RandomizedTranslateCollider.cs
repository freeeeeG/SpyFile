using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DFA RID: 3578
	public class RandomizedTranslateCollider : CharacterOperation
	{
		// Token: 0x0600479B RID: 18331 RVA: 0x000D02BC File Offset: 0x000CE4BC
		public override void Run(Character owner)
		{
			this._translate = this._center.position;
			float num = UnityEngine.Random.Range(-this._distribution, this._distribution);
			if (UnityEngine.Random.value > 0.5f)
			{
				this._translate.x = this._translate.x + num;
			}
			else
			{
				this._translate.x = this._translate.x - num;
			}
			if (UnityEngine.Random.value > 0.5f)
			{
				this._translate.y = this._translate.y + num;
			}
			else
			{
				this._translate.y = this._translate.y - num;
			}
			this._translate.z = 0f;
			this._targetCollider.transform.position = this._translate;
		}

		// Token: 0x0400369D RID: 13981
		[SerializeField]
		private Transform _center;

		// Token: 0x0400369E RID: 13982
		[SerializeField]
		private Collider2D _targetCollider;

		// Token: 0x0400369F RID: 13983
		[Range(0f, 10f)]
		[SerializeField]
		private float _distribution;

		// Token: 0x040036A0 RID: 13984
		private Vector3 _translate;
	}
}
