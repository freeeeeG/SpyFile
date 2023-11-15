using System;
using System.Collections;
using GameResources;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000124 RID: 292
	public sealed class ShowTexts : Sequence
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00010FA1 File Offset: 0x0000F1A1
		public ShowTexts.Type type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00010FA9 File Offset: 0x0000F1A9
		public override IEnumerator CRun()
		{
			foreach (string key in this._texts)
			{
				yield return this._narration.CShowText(this, Localization.GetLocalizedString(key));
			}
			string[] array = null;
			yield break;
		}

		// Token: 0x04000453 RID: 1107
		[SerializeField]
		private ShowTexts.Type _type;

		// Token: 0x04000454 RID: 1108
		[SerializeField]
		private string[] _texts;

		// Token: 0x02000125 RID: 293
		public enum Type
		{
			// Token: 0x04000456 RID: 1110
			SplitText,
			// Token: 0x04000457 RID: 1111
			IntactText
		}
	}
}
