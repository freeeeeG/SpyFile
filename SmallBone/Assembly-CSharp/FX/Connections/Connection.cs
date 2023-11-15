using System;
using UnityEngine;

namespace FX.Connections
{
	// Token: 0x02000296 RID: 662
	public abstract class Connection : MonoBehaviour
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x00029C8C File Offset: 0x00027E8C
		protected Vector3 startPosition
		{
			get
			{
				return this._start.position + this._startOffset;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00029CA4 File Offset: 0x00027EA4
		protected Vector3 endPosition
		{
			get
			{
				return this._end.position + this._endOffset;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00029CBC File Offset: 0x00027EBC
		// (set) Token: 0x06000CDA RID: 3290 RVA: 0x00029CC4 File Offset: 0x00027EC4
		public bool connecting { get; private set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00029CCD File Offset: 0x00027ECD
		public virtual bool lostConnection
		{
			get
			{
				return this._start == null || this._end == null;
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000CDC RID: 3292 RVA: 0x00029CEC File Offset: 0x00027EEC
		// (remove) Token: 0x06000CDD RID: 3293 RVA: 0x00029D24 File Offset: 0x00027F24
		public event Action OnConnect;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000CDE RID: 3294 RVA: 0x00029D5C File Offset: 0x00027F5C
		// (remove) Token: 0x06000CDF RID: 3295 RVA: 0x00029D94 File Offset: 0x00027F94
		public event Action OnDisconnect;

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00029DCC File Offset: 0x00027FCC
		public void Connect(Transform start, Vector2 startOffset, Transform end, Vector2 endOffset)
		{
			this.connecting = true;
			this._start = start;
			this._end = end;
			this._startOffset = startOffset;
			this._endOffset = endOffset;
			this.Show();
			Action onConnect = this.OnConnect;
			if (onConnect == null)
			{
				return;
			}
			onConnect();
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00029E1D File Offset: 0x0002801D
		public void Disconnect()
		{
			this.connecting = false;
			this.Hide();
			Action onDisconnect = this.OnDisconnect;
			if (onDisconnect == null)
			{
				return;
			}
			onDisconnect();
		}

		// Token: 0x06000CE2 RID: 3298
		protected abstract void Show();

		// Token: 0x06000CE3 RID: 3299
		protected abstract void Hide();

		// Token: 0x04000B03 RID: 2819
		private Transform _start;

		// Token: 0x04000B04 RID: 2820
		private Transform _end;

		// Token: 0x04000B05 RID: 2821
		private Vector3 _startOffset = new Vector3(0f, 0f);

		// Token: 0x04000B06 RID: 2822
		private Vector3 _endOffset = new Vector3(0f, 0f);
	}
}
