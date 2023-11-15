using System;
using Characters;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000567 RID: 1383
	[Serializable]
	public class SetEnemyBehaviorOption : IPinEnemyOption
	{
		// Token: 0x06001B35 RID: 6965 RVA: 0x00054916 File Offset: 0x00052B16
		public SetEnemyBehaviorOption(bool setTargetToPlayer, bool idleUntilFindTarget, bool staticMovement)
		{
			this._setTargetToPlayer = setTargetToPlayer;
			this._idleUntilFindTarget = idleUntilFindTarget;
			this._staticMovement = staticMovement;
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x00054934 File Offset: 0x00052B34
		public void ApplyTo(Character character)
		{
			EnemyCharacterBehaviorOption component = character.GetComponent<EnemyCharacterBehaviorOption>();
			if (component)
			{
				component.SetBehaviorOption(this._setTargetToPlayer, this._idleUntilFindTarget, this._staticMovement);
			}
		}

		// Token: 0x04001761 RID: 5985
		[SerializeField]
		private bool _setTargetToPlayer;

		// Token: 0x04001762 RID: 5986
		[SerializeField]
		private bool _idleUntilFindTarget;

		// Token: 0x04001763 RID: 5987
		[SerializeField]
		private bool _staticMovement;
	}
}
