using System;
using GameResources;
using UnityEngine;

namespace CutScenes.Shots
{
	// Token: 0x020001C9 RID: 457
	public class TalkerInfo : MonoBehaviour
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0001B2D2 File Offset: 0x000194D2
		public Sprite portrait
		{
			get
			{
				return this._portrait;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x0001B2DA File Offset: 0x000194DA
		public new string name
		{
			get
			{
				return Localization.GetLocalizedString(this._nameKey);
			}
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0001B2E8 File Offset: 0x000194E8
		public virtual string[] GetNextText()
		{
			string format = "{0}/{1}";
			object textKey = this._textKey;
			int currentIndex = this._currentIndex;
			this._currentIndex = currentIndex + 1;
			return Localization.GetLocalizedStringArray(string.Format(format, textKey, currentIndex));
		}

		// Token: 0x040007DB RID: 2011
		[SerializeField]
		protected Sprite _portrait;

		// Token: 0x040007DC RID: 2012
		[SerializeField]
		protected string _nameKey;

		// Token: 0x040007DD RID: 2013
		[Tooltip("한 번의 컷씬에서 출력되는 대사들의 키 중 공통되는 부분까지")]
		[SerializeField]
		protected string _textKey;

		// Token: 0x040007DE RID: 2014
		protected int _currentIndex;
	}
}
