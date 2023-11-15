using System;
using Characters;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014BA RID: 5306
	public class CanChase : Conditional
	{
		// Token: 0x0600673B RID: 26427 RVA: 0x0012AE7E File Offset: 0x0012907E
		public override void OnAwake()
		{
			this._characterValue = this._character.Value;
		}

		// Token: 0x0600673C RID: 26428 RVA: 0x0012AE91 File Offset: 0x00129091
		public override TaskStatus OnUpdate()
		{
			if (!Precondition.CanChase(this._characterValue, this._target.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x04005350 RID: 21328
		[SerializeField]
		[Tooltip("행동 주체")]
		private SharedCharacter _character;

		// Token: 0x04005351 RID: 21329
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x04005352 RID: 21330
		private Character _characterValue;
	}
}
