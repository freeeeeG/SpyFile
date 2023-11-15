using System;
using Characters;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000563 RID: 1379
	public class PinEnemySetting : MonoBehaviour
	{
		// Token: 0x06001B2C RID: 6956 RVA: 0x00054668 File Offset: 0x00052868
		public void ApplyTo(Character character)
		{
			IPinEnemyOption[] settings = this._settings;
			for (int i = 0; i < settings.Length; i++)
			{
				settings[i].ApplyTo(character);
			}
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x00054693 File Offset: 0x00052893
		public void CopySettingsFromPin(bool setTargetToPlayer, bool idleUntilFindTarget, bool staticMovement)
		{
			this._settings = new IPinEnemyOption[]
			{
				new SetEnemyBehaviorOption(setTargetToPlayer, idleUntilFindTarget, staticMovement)
			};
		}

		// Token: 0x04001756 RID: 5974
		[SerializeReference]
		[SubclassSelector]
		private IPinEnemyOption[] _settings = new IPinEnemyOption[]
		{
			new SetEnemyBehaviorOption(false, false, false)
		};
	}
}
