using System;
using UnityEngine;

namespace Housing.BuildLevelAction
{
	// Token: 0x0200014E RID: 334
	[RequireComponent(typeof(BuildLevel))]
	public class SetActiveObject : BuildLevelAction
	{
		// Token: 0x060006AC RID: 1708 RVA: 0x000134A7 File Offset: 0x000116A7
		protected override void Run()
		{
			this._target.SetActive(this._active);
		}

		// Token: 0x040004E0 RID: 1248
		[SerializeField]
		private GameObject _target;

		// Token: 0x040004E1 RID: 1249
		[SerializeField]
		private bool _active = true;
	}
}
