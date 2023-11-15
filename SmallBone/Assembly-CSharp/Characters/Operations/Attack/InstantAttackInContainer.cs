using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F8F RID: 3983
	public sealed class InstantAttackInContainer : CharacterOperation
	{
		// Token: 0x06004D48 RID: 19784 RVA: 0x000E63D5 File Offset: 0x000E45D5
		private void Awake()
		{
			this._instantAttack.Initialize();
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x000E63E4 File Offset: 0x000E45E4
		public override void Run(Character owner)
		{
			foreach (Collider2D range in this._container.GetComponentsInChildren<Collider2D>())
			{
				this._instantAttack.range = range;
				this._instantAttack.Run(owner);
			}
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x000E6427 File Offset: 0x000E4627
		public override void Stop()
		{
			base.Stop();
			this._instantAttack.Stop();
		}

		// Token: 0x04003D0D RID: 15629
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(InstantAttack))]
		private InstantAttack _instantAttack;

		// Token: 0x04003D0E RID: 15630
		[SerializeField]
		private Transform _container;
	}
}
