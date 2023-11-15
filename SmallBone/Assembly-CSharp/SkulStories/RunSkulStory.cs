using System;
using Scenes;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000118 RID: 280
	public class RunSkulStory : MonoBehaviour
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00010AB4 File Offset: 0x0000ECB4
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00010ABC File Offset: 0x0000ECBC
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

		// Token: 0x06000589 RID: 1417 RVA: 0x00010AD6 File Offset: 0x0000ECD6
		private void Start()
		{
			this._narration = Scene<GameBase>.instance.uiManager.narration;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00010AED File Offset: 0x0000ECED
		public void OnDisable()
		{
			this._narration.sceneVisible = false;
		}

		// Token: 0x04000431 RID: 1073
		[SerializeField]
		private bool _visible;

		// Token: 0x04000432 RID: 1074
		private Narration _narration;
	}
}
