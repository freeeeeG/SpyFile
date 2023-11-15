using System;
using UnityEngine;

namespace FX.Connections
{
	// Token: 0x02000299 RID: 665
	public class SimpleConnector : MonoBehaviour
	{
		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002A233 File Offset: 0x00028433
		private void Awake()
		{
			this._connection.Connect(this._start, this._startOffset, this._end, this._endOffset);
		}

		// Token: 0x04000B1A RID: 2842
		[SerializeField]
		private Connection _connection;

		// Token: 0x04000B1B RID: 2843
		[SerializeField]
		private Transform _start;

		// Token: 0x04000B1C RID: 2844
		[SerializeField]
		private Vector2 _startOffset;

		// Token: 0x04000B1D RID: 2845
		[SerializeField]
		private Transform _end;

		// Token: 0x04000B1E RID: 2846
		[SerializeField]
		private Vector2 _endOffset;
	}
}
