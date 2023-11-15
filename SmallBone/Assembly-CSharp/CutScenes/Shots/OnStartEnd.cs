using System;
using UnityEngine;

namespace CutScenes.Shots
{
	// Token: 0x020001C2 RID: 450
	public class OnStartEnd : MonoBehaviour
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0001B068 File Offset: 0x00019268
		public Shot onStart
		{
			get
			{
				return this._onStart;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0001B070 File Offset: 0x00019270
		public Shot onEnd
		{
			get
			{
				return this._onEnd;
			}
		}

		// Token: 0x040007CF RID: 1999
		[SerializeField]
		[Shot.SubcomponentAttribute]
		private Shot _onStart;

		// Token: 0x040007D0 RID: 2000
		[SerializeField]
		[Shot.SubcomponentAttribute]
		private Shot _onEnd;
	}
}
