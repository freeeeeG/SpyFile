using System;
using Characters.Gear.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003CF RID: 975
	public class SkillDesc : MonoBehaviour
	{
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00035797 File Offset: 0x00033997
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x000357A0 File Offset: 0x000339A0
		public SkillInfo info
		{
			get
			{
				return this._info;
			}
			set
			{
				this._info = value;
				this._image.sprite = this._info.cachedIcon;
				this._image.preserveAspect = true;
				this._text.text = this._info.displayName;
			}
		}

		// Token: 0x04000F09 RID: 3849
		[SerializeField]
		private Image _image;

		// Token: 0x04000F0A RID: 3850
		[SerializeField]
		private Text _text;

		// Token: 0x04000F0B RID: 3851
		private SkillInfo _info;
	}
}
