using System;
using Data;
using GameResources;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003B2 RID: 946
	public sealed class ModeResourceSetter : MonoBehaviour
	{
		// Token: 0x0600117B RID: 4475 RVA: 0x00033CFF File Offset: 0x00031EFF
		private void Awake()
		{
			this._isHardmode = GameData.HardmodeProgress.hardmode;
			if (this._isHardmode)
			{
				this.SetHard();
				return;
			}
			this.SetNormal();
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00033D21 File Offset: 0x00031F21
		private void Update()
		{
			if (!this._isHardmode && GameData.HardmodeProgress.hardmode)
			{
				this._isHardmode = true;
				this.SetHard();
				return;
			}
			if (this._isHardmode && !GameData.HardmodeProgress.hardmode)
			{
				this._isHardmode = false;
				this.SetNormal();
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00033D5C File Offset: 0x00031F5C
		private void SetNormal()
		{
			this._playerFrame.sprite = HUDResource.playerFrame.normal;
			this._playerDavyJones.sprite = HUDResource.playerDavyJonesFrame.normal;
			this._playerQuintessenceFrame.sprite = HUDResource.playerQuintessenceFrame.normal;
			this._playerSkill2Frame.sprite = HUDResource.playerSkill2Frame.normal;
			this._playerSubBarFrame.sprite = HUDResource.playerSubBarFrame.normal;
			this._playerSubSkill1Frame.sprite = HUDResource.playerSubSkill1Frame.normal;
			this._playerSubSkill2Frame.sprite = HUDResource.playerSubSkill2Frame.normal;
			this._playerSubSkullFrame.sprite = HUDResource.playerSubSkullFrame.normal;
			this._unlock.sprite = HUDResource.unlock.normal;
			this._minimap.sprite = HUDResource.minimap.normal;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00033E3C File Offset: 0x0003203C
		private void SetHard()
		{
			this._playerFrame.sprite = HUDResource.playerFrame.hard;
			this._playerDavyJones.sprite = HUDResource.playerDavyJonesFrame.hard;
			this._playerQuintessenceFrame.sprite = HUDResource.playerQuintessenceFrame.hard;
			this._playerSkill2Frame.sprite = HUDResource.playerSkill2Frame.hard;
			this._playerSubBarFrame.sprite = HUDResource.playerSubBarFrame.hard;
			this._playerSubSkill1Frame.sprite = HUDResource.playerSubSkill1Frame.hard;
			this._playerSubSkill2Frame.sprite = HUDResource.playerSubSkill2Frame.hard;
			this._playerSubSkullFrame.sprite = HUDResource.playerSubSkullFrame.hard;
			this._unlock.sprite = HUDResource.unlock.hard;
			this._minimap.sprite = HUDResource.minimap.hard;
		}

		// Token: 0x04000E7A RID: 3706
		[SerializeField]
		private Image _playerFrame;

		// Token: 0x04000E7B RID: 3707
		[SerializeField]
		private Image _playerDavyJones;

		// Token: 0x04000E7C RID: 3708
		[SerializeField]
		private Image _playerQuintessenceFrame;

		// Token: 0x04000E7D RID: 3709
		[SerializeField]
		private Image _playerSkill2Frame;

		// Token: 0x04000E7E RID: 3710
		[SerializeField]
		private Image _playerSubBarFrame;

		// Token: 0x04000E7F RID: 3711
		[SerializeField]
		private Image _playerSubSkill1Frame;

		// Token: 0x04000E80 RID: 3712
		[SerializeField]
		private Image _playerSubSkill2Frame;

		// Token: 0x04000E81 RID: 3713
		[SerializeField]
		private Image _playerSubSkullFrame;

		// Token: 0x04000E82 RID: 3714
		[SerializeField]
		private Image _unlock;

		// Token: 0x04000E83 RID: 3715
		[SerializeField]
		private Image _minimap;

		// Token: 0x04000E84 RID: 3716
		private bool _isHardmode;
	}
}
