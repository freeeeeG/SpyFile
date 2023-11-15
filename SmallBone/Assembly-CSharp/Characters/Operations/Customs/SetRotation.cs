using System;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FE9 RID: 4073
	public class SetRotation : CharacterOperation
	{
		// Token: 0x06004EB8 RID: 20152 RVA: 0x000EC30E File Offset: 0x000EA50E
		public override void Run(Character owner)
		{
			if (this._type == SetRotation.Type.Manually)
			{
				this.RotateEulerAngles();
				return;
			}
			if (this._type == SetRotation.Type.Random)
			{
				this.RotateRandomEulerAngles();
			}
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x000EC32E File Offset: 0x000EA52E
		private void RotateEulerAngles()
		{
			this._transform.rotation = Quaternion.Euler(0f, 0f, this._rotateValue);
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x000EC350 File Offset: 0x000EA550
		private void RotateRandomEulerAngles()
		{
			int num = UnityEngine.Random.Range(-180, 180);
			this._transform.rotation = Quaternion.Euler(0f, 0f, (float)num);
		}

		// Token: 0x04003ECF RID: 16079
		[SerializeField]
		private SetRotation.Type _type;

		// Token: 0x04003ED0 RID: 16080
		[SerializeField]
		private Transform _transform;

		// Token: 0x04003ED1 RID: 16081
		[Range(-180f, 180f)]
		[SerializeField]
		private float _rotateValue;

		// Token: 0x02000FEA RID: 4074
		private enum Type
		{
			// Token: 0x04003ED3 RID: 16083
			Manually,
			// Token: 0x04003ED4 RID: 16084
			Random
		}
	}
}
