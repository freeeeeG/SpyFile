using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FAA RID: 4010
	public class FlipObject : Operation
	{
		// Token: 0x06004DDA RID: 19930 RVA: 0x000E8E14 File Offset: 0x000E7014
		public override void Run()
		{
			float x = this._flipX ? (-this._object.localScale.x) : this._object.localScale.x;
			float y = this._flipY ? (-this._object.localScale.y) : this._object.localScale.y;
			this._object.localScale = new Vector2(x, y);
		}

		// Token: 0x04003DC5 RID: 15813
		[SerializeField]
		private Transform _object;

		// Token: 0x04003DC6 RID: 15814
		[SerializeField]
		private bool _flipX;

		// Token: 0x04003DC7 RID: 15815
		[SerializeField]
		private bool _flipY;
	}
}
