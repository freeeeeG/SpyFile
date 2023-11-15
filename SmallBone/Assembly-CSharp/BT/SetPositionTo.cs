using System;
using Characters.Operations.SetPosition;
using UnityEngine;

namespace BT
{
	// Token: 0x0200141E RID: 5150
	public class SetPositionTo : Node
	{
		// Token: 0x0600653B RID: 25915 RVA: 0x00125320 File Offset: 0x00123520
		protected override NodeState UpdateDeltatime(Context context)
		{
			this._object.position = this._targetInfo.GetPosition();
			return NodeState.Success;
		}

		// Token: 0x0400518B RID: 20875
		[SerializeField]
		private Transform _object;

		// Token: 0x0400518C RID: 20876
		[SerializeField]
		private TargetInfo _targetInfo;
	}
}
