using System;
using Characters;
using Characters.Abilities.Darks;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200146B RID: 5227
	public sealed class IntroDarkEnemy : Action
	{
		// Token: 0x06006605 RID: 26117 RVA: 0x00126CBE File Offset: 0x00124EBE
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
		}

		// Token: 0x06006606 RID: 26118 RVA: 0x00126CD4 File Offset: 0x00124ED4
		public override void OnStart()
		{
			this._darkEnemy = this._ownerValue.GetComponent<DarkEnemy>();
			if (this._darkEnemy == null)
			{
				Debug.LogError("Not has DarkEnemy Component");
				return;
			}
			if (!this._darkEnemy.enabled)
			{
				Debug.LogError("Disabled DarkEnemy Component");
			}
		}

		// Token: 0x06006607 RID: 26119 RVA: 0x00126D22 File Offset: 0x00124F22
		public override TaskStatus OnUpdate()
		{
			this._darkEnemy.RunIntro();
			return TaskStatus.Success;
		}

		// Token: 0x040051FD RID: 20989
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040051FE RID: 20990
		private Character _ownerValue;

		// Token: 0x040051FF RID: 20991
		private DarkEnemy _darkEnemy;
	}
}
