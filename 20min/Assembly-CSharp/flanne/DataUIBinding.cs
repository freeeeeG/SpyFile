using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000138 RID: 312
	public abstract class DataUIBinding<T> : MonoBehaviour where T : ScriptableObject
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x00022E3B File Offset: 0x0002103B
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x00022E43 File Offset: 0x00021043
		public T data
		{
			get
			{
				return this._data;
			}
			set
			{
				this._data = value;
				this.Refresh();
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00022E52 File Offset: 0x00021052
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06000846 RID: 2118
		public abstract void Refresh();

		// Token: 0x04000616 RID: 1558
		[SerializeField]
		private T _data;
	}
}
