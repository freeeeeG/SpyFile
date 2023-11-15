using System;
using Characters;
using UnityEngine;

namespace FX.EffectProperties
{
	// Token: 0x02000289 RID: 649
	public class SimpleScale : EffectProperty
	{
		// Token: 0x06000CA9 RID: 3241 RVA: 0x0002957B File Offset: 0x0002777B
		public override void Apply(PoolObject spawned, Character owner, Target target)
		{
			spawned.transform.localScale = this._scale;
			spawned.transform.localEulerAngles = new Vector3(0f, 0f, this._angle);
		}

		// Token: 0x04000AE2 RID: 2786
		[SerializeField]
		private Vector3 _scale;

		// Token: 0x04000AE3 RID: 2787
		[SerializeField]
		private float _angle;
	}
}
