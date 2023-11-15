using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003A1 RID: 929
	public class GearImageContainer : MonoBehaviour
	{
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600111C RID: 4380 RVA: 0x00032C66 File Offset: 0x00030E66
		// (set) Token: 0x0600111D RID: 4381 RVA: 0x00032C6E File Offset: 0x00030E6E
		public Image image
		{
			get
			{
				return this._image;
			}
			set
			{
				this._image = value;
			}
		}

		// Token: 0x04000E18 RID: 3608
		[SerializeField]
		private Image _image;
	}
}
