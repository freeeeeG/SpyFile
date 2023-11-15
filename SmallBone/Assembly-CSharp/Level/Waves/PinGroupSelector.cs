using System;
using System.Collections.Generic;
using Characters;
using Hardmode;
using Singletons;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000565 RID: 1381
	public sealed class PinGroupSelector : GroupSelector
	{
		// Token: 0x06001B31 RID: 6961 RVA: 0x0005477C File Offset: 0x0005297C
		public override ICollection<Character> Load()
		{
			this._currentStep = Singleton<HardmodeManager>.Instance.GetEnemyStep();
			switch (this._currentStep)
			{
			case HardmodeManager.EnemyStep.Normal:
				this._selectedGroup = this._groupNormal;
				UnityEngine.Object.Destroy(this._groupA.gameObject);
				UnityEngine.Object.Destroy(this._groupB.gameObject);
				UnityEngine.Object.Destroy(this._groupC.gameObject);
				break;
			case HardmodeManager.EnemyStep.A:
				this._selectedGroup = this._groupA;
				UnityEngine.Object.Destroy(this._groupNormal.gameObject);
				UnityEngine.Object.Destroy(this._groupB.gameObject);
				UnityEngine.Object.Destroy(this._groupC.gameObject);
				break;
			case HardmodeManager.EnemyStep.B:
				this._selectedGroup = this._groupB;
				UnityEngine.Object.Destroy(this._groupNormal.gameObject);
				UnityEngine.Object.Destroy(this._groupA.gameObject);
				UnityEngine.Object.Destroy(this._groupC.gameObject);
				break;
			case HardmodeManager.EnemyStep.C:
				this._selectedGroup = this._groupC;
				UnityEngine.Object.Destroy(this._groupNormal.gameObject);
				UnityEngine.Object.Destroy(this._groupA.gameObject);
				UnityEngine.Object.Destroy(this._groupB.gameObject);
				break;
			}
			this._selectedGroup.gameObject.SetActive(true);
			return this._selectedGroup.Load();
		}

		// Token: 0x04001759 RID: 5977
		[SerializeField]
		private PinGroup _groupNormal;

		// Token: 0x0400175A RID: 5978
		[SerializeField]
		[Header("하드모드에만 설정")]
		private PinGroup _groupA;

		// Token: 0x0400175B RID: 5979
		[SerializeField]
		private PinGroup _groupB;

		// Token: 0x0400175C RID: 5980
		[SerializeField]
		private PinGroup _groupC;

		// Token: 0x0400175D RID: 5981
		private HardmodeManager.EnemyStep _currentStep;

		// Token: 0x0400175E RID: 5982
		private PinGroup _selectedGroup;
	}
}
