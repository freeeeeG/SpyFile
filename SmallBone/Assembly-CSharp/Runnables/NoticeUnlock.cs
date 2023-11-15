using System;
using Scenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000338 RID: 824
	public sealed class NoticeUnlock : Runnable
	{
		// Token: 0x06000FAB RID: 4011 RVA: 0x0002F677 File Offset: 0x0002D877
		public override void Run()
		{
			Scene<GameBase>.instance.uiManager.unlockNotice.Show(this._icon, this._name);
		}

		// Token: 0x04000CE2 RID: 3298
		[SerializeField]
		private Sprite _icon;

		// Token: 0x04000CE3 RID: 3299
		[SerializeField]
		private string _name;
	}
}
