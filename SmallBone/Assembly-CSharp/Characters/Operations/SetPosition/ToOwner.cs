using System;
using UnityEngine;
using Utils;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EEF RID: 3823
	public class ToOwner : Policy
	{
		// Token: 0x06004AF1 RID: 19185 RVA: 0x000DC190 File Offset: 0x000DA390
		public override Vector2 GetPosition(Character owner)
		{
			return this._positionInfo.GetPosition(owner);
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x000DC19E File Offset: 0x000DA39E
		public override Vector2 GetPosition()
		{
			Debug.LogError("Invalid onwer");
			return base.transform.position;
		}

		// Token: 0x04003A23 RID: 14883
		[SerializeField]
		private PositionInfo _positionInfo = new PositionInfo(false, false, 0, PositionInfo.Pivot.Bottom);
	}
}
