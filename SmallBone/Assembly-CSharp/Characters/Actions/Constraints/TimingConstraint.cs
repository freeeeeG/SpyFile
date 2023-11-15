using System;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000986 RID: 2438
	public class TimingConstraint : Constraint
	{
		// Token: 0x06003448 RID: 13384 RVA: 0x0009AC0C File Offset: 0x00098E0C
		public override bool Pass()
		{
			return this._action.owner.runningMotion == null || (this._timingCanCancel.x <= this._action.owner.runningMotion.normalizedTime && this._action.owner.runningMotion.normalizedTime <= this._timingCanCancel.y);
		}

		// Token: 0x04002A4E RID: 10830
		[SerializeField]
		[Information("현재 실행 중인 모션의 진행 정도가 범위 안에 있을 때만 캔슬 가능", InformationAttribute.InformationType.Info, false)]
		[Range(0f, 1f)]
		private Vector2 _timingCanCancel;
	}
}
