using System;
using Scenes;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000123 RID: 291
	public class SetVisible : MonoBehaviour
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x00010F5A File Offset: 0x0000F15A
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x00010F62 File Offset: 0x0000F162
		public bool visible
		{
			get
			{
				return this._visible;
			}
			set
			{
				this._visible = value;
				this._narration.sceneVisible = this._visible;
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00010F7C File Offset: 0x0000F17C
		private void Start()
		{
			this._narration = Scene<GameBase>.instance.uiManager.narration;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00010F93 File Offset: 0x0000F193
		public void OnDisable()
		{
			this._narration.sceneVisible = false;
		}

		// Token: 0x04000451 RID: 1105
		private bool _visible;

		// Token: 0x04000452 RID: 1106
		private Narration _narration;
	}
}
