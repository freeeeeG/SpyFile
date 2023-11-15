using System;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x0200012A RID: 298
	[CreateAssetMenu]
	public class TextLinkInfos : ScriptableObject
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001111B File Offset: 0x0000F31B
		public TextLinkInfos.TextLink[] texts
		{
			get
			{
				return this._texts;
			}
		}

		// Token: 0x04000464 RID: 1124
		[SerializeField]
		private TextLinkInfos.TextLink[] _texts;

		// Token: 0x0200012B RID: 299
		[Serializable]
		public class TextLink
		{
			// Token: 0x04000465 RID: 1125
			public TextLinkInfos.TextLink.Position position;

			// Token: 0x04000466 RID: 1126
			public string text;

			// Token: 0x0200012C RID: 300
			public enum Position
			{
				// Token: 0x04000468 RID: 1128
				Normal,
				// Token: 0x04000469 RID: 1129
				Below
			}
		}
	}
}
