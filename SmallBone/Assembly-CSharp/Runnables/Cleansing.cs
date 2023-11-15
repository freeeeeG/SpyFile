using System;
using Characters.Abilities;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000314 RID: 788
	public class Cleansing : Runnable
	{
		// Token: 0x06000F49 RID: 3913 RVA: 0x0002EB4D File Offset: 0x0002CD4D
		public override void Run()
		{
			this._target.character.playerComponents.savableAbilityManager.Remove(SavableAbilityManager.Name.Curse);
		}

		// Token: 0x04000C9F RID: 3231
		[SerializeField]
		private Target _target;
	}
}
