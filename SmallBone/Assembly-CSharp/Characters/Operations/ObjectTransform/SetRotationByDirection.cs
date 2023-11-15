using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FB4 RID: 4020
	public sealed class SetRotationByDirection : Operation
	{
		// Token: 0x06004DF7 RID: 19959 RVA: 0x000E934C File Offset: 0x000E754C
		public override void Run()
		{
			Vector2 vector = MMMaths.Vector3ToVector2(this._to.transform.position) - MMMaths.Vector3ToVector2(this._from.transform.position);
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this._target.transform.localRotation = Quaternion.Euler(0f, 0f, this._baseDegree.value + num);
		}

		// Token: 0x04003DED RID: 15853
		[SerializeField]
		private CustomFloat _baseDegree;

		// Token: 0x04003DEE RID: 15854
		[SerializeField]
		private Transform _from;

		// Token: 0x04003DEF RID: 15855
		[SerializeField]
		private Transform _to;

		// Token: 0x04003DF0 RID: 15856
		[SerializeField]
		private Transform _target;
	}
}
