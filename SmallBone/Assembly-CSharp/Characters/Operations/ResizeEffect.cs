using System;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DFB RID: 3579
	public class ResizeEffect : CharacterOperation
	{
		// Token: 0x0600479D RID: 18333 RVA: 0x000D0370 File Offset: 0x000CE570
		public override void Run(Character owner)
		{
			this._info.scaleX = new CustomFloat(this.GetScaleX());
			this._info.Spawn(this._end.position, owner, this._start.rotation.eulerAngles.z, 1f);
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x000D03C8 File Offset: 0x000CE5C8
		private float GetScaleX()
		{
			return Mathf.Abs((this._end.transform.position.x - this._start.transform.position.x) / this._originSize);
		}

		// Token: 0x040036A1 RID: 13985
		[SerializeField]
		private Transform _start;

		// Token: 0x040036A2 RID: 13986
		[SerializeField]
		private Transform _end;

		// Token: 0x040036A3 RID: 13987
		[SerializeField]
		private EffectInfo _info;

		// Token: 0x040036A4 RID: 13988
		[SerializeField]
		private float _originSize;
	}
}
