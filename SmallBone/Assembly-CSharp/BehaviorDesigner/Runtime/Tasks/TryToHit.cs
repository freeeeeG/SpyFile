using System;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001489 RID: 5257
	[TaskIcon("{SkinColor}StackedActionIcon.png")]
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	public sealed class TryToHit : BehaviorDesigner.Runtime.Tasks.Action
	{
		// Token: 0x0600666E RID: 26222 RVA: 0x0012868F File Offset: 0x0012688F
		public override void OnAwake()
		{
			this._characterValue = this._character.Value;
			this._attackValue = this._attack.Value;
		}

		// Token: 0x0600666F RID: 26223 RVA: 0x001286B3 File Offset: 0x001268B3
		public override void OnStart()
		{
			this._hit = false;
			Character characterValue = this._characterValue;
			characterValue.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(characterValue.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			this._beforeTaskStatus = TaskStatus.Inactive;
		}

		// Token: 0x06006670 RID: 26224 RVA: 0x001286EC File Offset: 0x001268EC
		public override TaskStatus OnUpdate()
		{
			if (this._hit)
			{
				this._beforeTaskStatus = TaskStatus.Success;
				return this._beforeTaskStatus;
			}
			if (this._attackValue.running)
			{
				this._beforeTaskStatus = TaskStatus.Running;
				return TaskStatus.Running;
			}
			if (this._beforeTaskStatus == TaskStatus.Running)
			{
				return TaskStatus.Failure;
			}
			if (this._beforeTaskStatus != TaskStatus.Inactive)
			{
				return TaskStatus.Running;
			}
			if (this._attackValue.TryStart())
			{
				this._beforeTaskStatus = TaskStatus.Running;
				return TaskStatus.Running;
			}
			this._beforeTaskStatus = TaskStatus.Inactive;
			return TaskStatus.Running;
		}

		// Token: 0x06006671 RID: 26225 RVA: 0x00128758 File Offset: 0x00126958
		public override void OnEnd()
		{
			Character characterValue = this._characterValue;
			characterValue.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(characterValue.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
		}

		// Token: 0x06006672 RID: 26226 RVA: 0x00128781 File Offset: 0x00126981
		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (string.IsNullOrEmpty(this._key))
			{
				this._hit = true;
				return;
			}
			if (string.Equals(originalDamage.key, this._key, StringComparison.OrdinalIgnoreCase))
			{
				this._hit = true;
			}
		}

		// Token: 0x0400527A RID: 21114
		[SerializeField]
		private SharedCharacter _character;

		// Token: 0x0400527B RID: 21115
		[SerializeField]
		private SharedCharacterAction _attack;

		// Token: 0x0400527C RID: 21116
		[SerializeField]
		private string _key;

		// Token: 0x0400527D RID: 21117
		private Character _characterValue;

		// Token: 0x0400527E RID: 21118
		private Characters.Actions.Action _attackValue;

		// Token: 0x0400527F RID: 21119
		private bool _hit;

		// Token: 0x04005280 RID: 21120
		private TaskStatus _beforeTaskStatus;
	}
}
