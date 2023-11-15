using System;
using UnityEngine;

namespace Level.Chapter4
{
	// Token: 0x0200063D RID: 1597
	[Serializable]
	public class Node : INode
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600200F RID: 8207 RVA: 0x0006103F File Offset: 0x0005F23F
		// (set) Token: 0x06002010 RID: 8208 RVA: 0x00061047 File Offset: 0x0005F247
		public string DisplayText
		{
			get
			{
				return this._displayText;
			}
			set
			{
				this._displayText = value;
			}
		}

		// Token: 0x04001B2D RID: 6957
		[SerializeField]
		private string _displayText = "display text";
	}
}
