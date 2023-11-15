using System;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200147C RID: 5244
	[TaskIcon("{SkinColor}StackedActionIcon.png")]
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	public sealed class RunCharacterActions : BehaviorDesigner.Runtime.Tasks.Action
	{
		// Token: 0x0600663D RID: 26173 RVA: 0x00127AA8 File Offset: 0x00125CA8
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
		}

		// Token: 0x0600663E RID: 26174 RVA: 0x00127ABC File Offset: 0x00125CBC
		public override void OnStart()
		{
			if (this._actions == null)
			{
				return;
			}
			if (this._actions.Length == 0)
			{
				return;
			}
			if (this._ownerValue.stunedOrFreezed)
			{
				return;
			}
			this._trying = this._actions[this._index].TryStart();
			this._running = true;
		}

		// Token: 0x0600663F RID: 26175 RVA: 0x00127B0C File Offset: 0x00125D0C
		public override TaskStatus OnUpdate()
		{
			Characters.Actions.Action action = this._actions[this._index];
			if (!this._running)
			{
				if (this._ownerValue.stunedOrFreezed)
				{
					return TaskStatus.Running;
				}
				this._running = true;
				this._index++;
				this._trying = action.TryStart();
				return TaskStatus.Running;
			}
			else
			{
				if (this._tryUntilSucceed && !this._trying)
				{
					this._trying = action.TryStart();
					return TaskStatus.Running;
				}
				if (action.running)
				{
					return TaskStatus.Running;
				}
				if (this._index >= this._actions.Length)
				{
					return TaskStatus.Success;
				}
				return TaskStatus.Running;
			}
		}

		// Token: 0x06006640 RID: 26176 RVA: 0x00127B9C File Offset: 0x00125D9C
		public override void OnEnd()
		{
			base.OnEnd();
			this._running = false;
			this._trying = false;
			this._index = 0;
		}

		// Token: 0x04005244 RID: 21060
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005245 RID: 21061
		[SerializeField]
		private Characters.Actions.Action[] _actions;

		// Token: 0x04005246 RID: 21062
		[SerializeField]
		private bool _tryUntilSucceed;

		// Token: 0x04005247 RID: 21063
		private bool _running;

		// Token: 0x04005248 RID: 21064
		private bool _trying;

		// Token: 0x04005249 RID: 21065
		private int _index;

		// Token: 0x0400524A RID: 21066
		private Character _ownerValue;
	}
}
