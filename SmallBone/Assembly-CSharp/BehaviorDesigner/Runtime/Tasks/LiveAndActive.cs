using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014B9 RID: 5305
	public class LiveAndActive : Conditional
	{
		// Token: 0x06006738 RID: 26424 RVA: 0x0012AE59 File Offset: 0x00129059
		public override void OnStart()
		{
			this._ownerValue = this._owner.Value;
		}

		// Token: 0x06006739 RID: 26425 RVA: 0x0012AE6C File Offset: 0x0012906C
		public override TaskStatus OnUpdate()
		{
			if (this._ownerValue.liveAndActive)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		// Token: 0x0400534E RID: 21326
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x0400534F RID: 21327
		private Character _ownerValue;
	}
}
