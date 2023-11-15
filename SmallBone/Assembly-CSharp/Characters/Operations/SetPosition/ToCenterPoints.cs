using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EE1 RID: 3809
	public class ToCenterPoints : Policy
	{
		// Token: 0x06004ABC RID: 19132 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004ABD RID: 19133 RVA: 0x000DA8B8 File Offset: 0x000D8AB8
		public override Vector2 GetPosition()
		{
			Vector2 vector = Vector2.zero;
			foreach (Transform transform in this._points)
			{
				vector += transform.position;
			}
			if (this._points.Length > 1)
			{
				vector /= (float)this._points.Length;
			}
			return vector;
		}

		// Token: 0x040039DC RID: 14812
		[SerializeField]
		private Transform[] _points;
	}
}
