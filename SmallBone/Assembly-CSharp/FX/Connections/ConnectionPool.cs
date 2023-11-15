using System;
using System.Collections.Generic;
using UnityEngine;

namespace FX.Connections
{
	// Token: 0x02000294 RID: 660
	public class ConnectionPool : MonoBehaviour
	{
		// Token: 0x06000CCD RID: 3277 RVA: 0x00029B52 File Offset: 0x00027D52
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00029B5A File Offset: 0x00027D5A
		private void OnDisable()
		{
			this.DisconnectAll();
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00029B64 File Offset: 0x00027D64
		private void Initialize()
		{
			Connection[] componentsInChildren = base.GetComponentsInChildren<Connection>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Connection connect = componentsInChildren[i];
				this._pool.Enqueue(connect);
				connect.OnConnect += delegate()
				{
					this._connectings.Add(connect);
				};
				connect.OnDisconnect += delegate()
				{
					this._connectings.Remove(connect);
					this._pool.Enqueue(connect);
				};
			}
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00029BDC File Offset: 0x00027DDC
		public Connection GetConnection()
		{
			if (this._pool.Count == 0)
			{
				this.DisconnectFirstConnecting();
			}
			return this._pool.Dequeue();
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00029BFC File Offset: 0x00027DFC
		private void DisconnectFirstConnecting()
		{
			this._connectings[0].Disconnect();
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00029C0F File Offset: 0x00027E0F
		private void DisconnectAll()
		{
			while (this._connectings.Count > 0)
			{
				this.DisconnectFirstConnecting();
			}
		}

		// Token: 0x04000AFF RID: 2815
		private Queue<Connection> _pool = new Queue<Connection>();

		// Token: 0x04000B00 RID: 2816
		private List<Connection> _connectings = new List<Connection>();
	}
}
