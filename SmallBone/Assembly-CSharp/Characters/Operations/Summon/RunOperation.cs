using System;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F31 RID: 3889
	[Serializable]
	public class RunOperation : IBDCharacterSetting
	{
		// Token: 0x06004BBC RID: 19388 RVA: 0x000DEBE7 File Offset: 0x000DCDE7
		public void ApplyTo(Character character)
		{
			this._operation.Initialize();
			if (!this._operation.gameObject.activeSelf)
			{
				this._operation.gameObject.SetActive(true);
			}
			this._operation.Run(character);
		}

		// Token: 0x04003AEF RID: 15087
		[SerializeField]
		private OperationInfos _operation;
	}
}
