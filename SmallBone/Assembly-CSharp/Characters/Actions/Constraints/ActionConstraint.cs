using System;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x0200096C RID: 2412
	public class ActionConstraint : Constraint
	{
		// Token: 0x06003407 RID: 13319 RVA: 0x0009A160 File Offset: 0x00098360
		public override bool Pass()
		{
			Motion runningMotion = this._action.owner.runningMotion;
			if (runningMotion == null)
			{
				return true;
			}
			for (int i = 0; i < this._exceptions.values.Length; i++)
			{
				ActionConstraint.Exception ex = this._exceptions.values[i];
				if (ex.motion == runningMotion)
				{
					return ex.range.x != ex.range.y && MMMaths.Range(runningMotion.normalizedTime, ex.range);
				}
			}
			return this._canCancel.GetOrDefault(runningMotion.action.type);
		}

		// Token: 0x04002A1A RID: 10778
		[SerializeField]
		private ActionTypeBoolArray _canCancel;

		// Token: 0x04002A1B RID: 10779
		[SerializeField]
		private ActionConstraint.Exception.Reorderable _exceptions;

		// Token: 0x0200096D RID: 2413
		[Serializable]
		public class Exception
		{
			// Token: 0x17000B45 RID: 2885
			// (get) Token: 0x06003409 RID: 13321 RVA: 0x0009A207 File Offset: 0x00098407
			public Motion motion
			{
				get
				{
					return this._motion;
				}
			}

			// Token: 0x17000B46 RID: 2886
			// (get) Token: 0x0600340A RID: 13322 RVA: 0x0009A20F File Offset: 0x0009840F
			public Vector2 range
			{
				get
				{
					return this._range;
				}
			}

			// Token: 0x04002A1C RID: 10780
			[SerializeField]
			private Motion _motion;

			// Token: 0x04002A1D RID: 10781
			[SerializeField]
			[MinMaxSlider(0f, 1f)]
			private Vector2 _range;

			// Token: 0x0200096E RID: 2414
			[Serializable]
			internal class Reorderable : ReorderableArray<ActionConstraint.Exception>
			{
			}
		}
	}
}
