using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000ED8 RID: 3800
	[Serializable]
	public class TargetInfo
	{
		// Token: 0x06004A99 RID: 19097 RVA: 0x000D9C2C File Offset: 0x000D7E2C
		public Vector2 GetPosition()
		{
			Vector2 position = this._policy.GetPosition();
			return new Vector2(position.x + this._customOffsetX.value, position.y + this._customOffsetY.value);
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x000D9C70 File Offset: 0x000D7E70
		public Vector2 GetPosition(Character owner)
		{
			Vector2 position = this._policy.GetPosition(owner);
			return new Vector2(position.x + this._customOffsetX.value, position.y + this._customOffsetY.value);
		}

		// Token: 0x040039B4 RID: 14772
		[SerializeField]
		private CustomFloat _customOffsetX;

		// Token: 0x040039B5 RID: 14773
		[SerializeField]
		private CustomFloat _customOffsetY;

		// Token: 0x040039B6 RID: 14774
		[SerializeField]
		[Policy.SubcomponentAttribute(true)]
		private Policy _policy;
	}
}
