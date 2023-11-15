using System;
using Characters.Controllers;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000352 RID: 850
	public class InteractionInput : Trigger
	{
		// Token: 0x06000FEC RID: 4076 RVA: 0x0002FB40 File Offset: 0x0002DD40
		private void Awake()
		{
			if (Singleton<Service>.Instance.levelManager == null || Singleton<Service>.Instance.levelManager.player == null)
			{
				Debug.LogError("No levelManager or player character.");
				return;
			}
			this._input = Singleton<Service>.Instance.levelManager.player.GetComponent<PlayerInput>();
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0002FB9B File Offset: 0x0002DD9B
		protected override bool Check()
		{
			return this._input == null || this._input.interaction.IsPressed;
		}

		// Token: 0x04000D0B RID: 3339
		private PlayerInput _input;
	}
}
