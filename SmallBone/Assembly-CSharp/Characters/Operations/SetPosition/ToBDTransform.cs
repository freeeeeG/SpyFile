using System;
using BehaviorDesigner.Runtime;
using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EDE RID: 3806
	public class ToBDTransform : Policy
	{
		// Token: 0x06004AAF RID: 19119 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x000DA628 File Offset: 0x000D8828
		public override Vector2 GetPosition()
		{
			Transform value = this._communicator.GetVariable<SharedTransform>(this._transformName).Value;
			if (value == null)
			{
				return base.transform.position;
			}
			if (!this._onPlatform)
			{
				return value.position;
			}
			return PlatformUtils.GetProjectionPointToPlatform(value.position, Vector2.down, ToBDTransform._belowCaster, this._groundMask, 100f);
		}

		// Token: 0x040039D0 RID: 14800
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x040039D1 RID: 14801
		[SerializeField]
		private string _transformName = "Destination";

		// Token: 0x040039D2 RID: 14802
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x040039D3 RID: 14803
		[SerializeField]
		private LayerMask _groundMask = Layers.groundMask;

		// Token: 0x040039D4 RID: 14804
		private static NonAllocCaster _belowCaster = new NonAllocCaster(1);
	}
}
