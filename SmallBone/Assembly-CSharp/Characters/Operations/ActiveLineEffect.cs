using System;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DBA RID: 3514
	public class ActiveLineEffect : CharacterOperation
	{
		// Token: 0x060046B2 RID: 18098 RVA: 0x000CD2AC File Offset: 0x000CB4AC
		public override void Run(Character owner)
		{
			this._lineEffect.startPoint = this._startPoint.position;
			this._lineEffect.endPoint = this._endPoint.position;
			this._lineEffect.Run();
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x000CD2FA File Offset: 0x000CB4FA
		public override void Stop()
		{
			this._lineEffect.Hide();
		}

		// Token: 0x04003585 RID: 13701
		[SerializeField]
		private LineEffect _lineEffect;

		// Token: 0x04003586 RID: 13702
		[SerializeField]
		private Transform _startPoint;

		// Token: 0x04003587 RID: 13703
		[SerializeField]
		private Transform _endPoint;
	}
}
