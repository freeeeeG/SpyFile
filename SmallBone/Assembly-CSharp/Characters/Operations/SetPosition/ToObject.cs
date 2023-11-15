using System;
using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EED RID: 3821
	public class ToObject : Policy
	{
		// Token: 0x06004AE8 RID: 19176 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x000DBEAC File Offset: 0x000DA0AC
		public override Vector2 GetPosition()
		{
			if (!this._onPlatform)
			{
				return this._object.transform.position;
			}
			return PlatformUtils.GetProjectionPointToPlatform(this._object.transform.position, Vector2.down, ToObject._belowCaster, this._groundMask, 100f);
		}

		// Token: 0x04003A1A RID: 14874
		[SerializeField]
		private LayerMask _groundMask = Layers.groundMask;

		// Token: 0x04003A1B RID: 14875
		[SerializeField]
		private GameObject _object;

		// Token: 0x04003A1C RID: 14876
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x04003A1D RID: 14877
		private static NonAllocCaster _belowCaster = new NonAllocCaster(1);
	}
}
