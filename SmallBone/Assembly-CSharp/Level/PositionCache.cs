using System;
using UnityEngine;

namespace Level
{
	// Token: 0x0200047F RID: 1151
	public class PositionCache : MonoBehaviour
	{
		// Token: 0x060015F0 RID: 5616 RVA: 0x00044CDE File Offset: 0x00042EDE
		private void Awake()
		{
			if (this._transform == null)
			{
				this._transform = base.transform;
			}
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x00044CFA File Offset: 0x00042EFA
		public Vector2 Load()
		{
			return this._position;
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00044D02 File Offset: 0x00042F02
		public void Save()
		{
			this._position = this._transform.position;
		}

		// Token: 0x04001330 RID: 4912
		[SerializeField]
		private Transform _transform;

		// Token: 0x04001331 RID: 4913
		private Vector2 _position;
	}
}
