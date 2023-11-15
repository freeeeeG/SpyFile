using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E0D RID: 3597
	public class TransformLooksAtTheTarget : CharacterOperation
	{
		// Token: 0x060047DA RID: 18394 RVA: 0x000D1308 File Offset: 0x000CF508
		public override void Run(Character owner)
		{
			this._transform.LookAt(this._controller.target.transform);
		}

		// Token: 0x040036FF RID: 14079
		[SerializeField]
		private Transform _transform;

		// Token: 0x04003700 RID: 14080
		[SerializeField]
		private AIController _controller;
	}
}
