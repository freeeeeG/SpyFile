using System;
using System.Collections;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200147A RID: 5242
	[TaskDescription("액션을 실행한다")]
	[TaskIcon("Assets/Behavior Designer/Icon/RunCharacterAction.png")]
	public sealed class RunCharacterAction : BehaviorDesigner.Runtime.Tasks.Action
	{
		// Token: 0x06006630 RID: 26160 RVA: 0x00127814 File Offset: 0x00125A14
		public override void OnAwake()
		{
			this._characterValue = this._character.Value;
			this._actionValue = this._action.Value;
			if (this._characterValue == null || this._actionValue == null)
			{
				return;
			}
			this._actionValue.Initialize(this._characterValue);
			if (this._checkLastMotionStayTime)
			{
				this._lastMotion = this._actionValue.motions[this._actionValue.motions.Length - 1];
				if (this._lastMotion.stay)
				{
					this._lastMotion.onStart += this.CheckLastMotionStayTime;
				}
			}
		}

		// Token: 0x06006631 RID: 26161 RVA: 0x001278BE File Offset: 0x00125ABE
		public override void OnStart()
		{
			if (this._characterValue.stunedOrFreezed)
			{
				return;
			}
			this._trying = this._actionValue.TryStart();
			this._running = true;
		}

		// Token: 0x06006632 RID: 26162 RVA: 0x001278E8 File Offset: 0x00125AE8
		public override TaskStatus OnUpdate()
		{
			if (this._action == null)
			{
				return TaskStatus.Failure;
			}
			if (!this._running)
			{
				if (this._characterValue.stunedOrFreezed)
				{
					return TaskStatus.Running;
				}
				this._running = true;
				this._trying = this._actionValue.TryStart();
				return TaskStatus.Running;
			}
			else if (this._tryUntilSucceed && !this._trying)
			{
				this._trying = this._actionValue.TryStart();
				if (!this._trying)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Running;
			}
			else
			{
				if (this._checkLastMotionStayTime && this._lastMotion.stay && this._stayActionRunning)
				{
					return TaskStatus.Success;
				}
				if (this._actionValue.running)
				{
					return TaskStatus.Running;
				}
				return TaskStatus.Success;
			}
		}

		// Token: 0x06006633 RID: 26163 RVA: 0x0012798C File Offset: 0x00125B8C
		public override void OnEnd()
		{
			base.OnEnd();
			this._stayActionRunning = false;
			this._running = false;
			this._trying = false;
		}

		// Token: 0x06006634 RID: 26164 RVA: 0x001279A9 File Offset: 0x00125BA9
		private void CheckLastMotionStayTime()
		{
			this._coroutineReference.Stop();
			this._coroutineReference = this._characterValue.StartCoroutineWithReference(this.CCheckLastMotionStayTime());
		}

		// Token: 0x06006635 RID: 26165 RVA: 0x001279CD File Offset: 0x00125BCD
		private IEnumerator CCheckLastMotionStayTime()
		{
			this._stayActionRunning = false;
			float t = 0f;
			float length = this._actionValue.motions[this._actionValue.motions.Length - 1].length;
			while (t < length)
			{
				t += this._characterValue.chronometer.animation.deltaTime;
				yield return null;
			}
			this._stayActionRunning = true;
			yield break;
		}

		// Token: 0x04005234 RID: 21044
		[SerializeField]
		private SharedCharacter _character;

		// Token: 0x04005235 RID: 21045
		[SerializeField]
		private SharedCharacterAction _action;

		// Token: 0x04005236 RID: 21046
		[SerializeField]
		private bool _tryUntilSucceed;

		// Token: 0x04005237 RID: 21047
		[SerializeField]
		private bool _checkLastMotionStayTime;

		// Token: 0x04005238 RID: 21048
		private bool _running;

		// Token: 0x04005239 RID: 21049
		private bool _trying;

		// Token: 0x0400523A RID: 21050
		private bool _stayActionRunning;

		// Token: 0x0400523B RID: 21051
		private Character _characterValue;

		// Token: 0x0400523C RID: 21052
		private Characters.Actions.Action _actionValue;

		// Token: 0x0400523D RID: 21053
		private Characters.Actions.Motion _lastMotion;

		// Token: 0x0400523E RID: 21054
		private CoroutineReference _coroutineReference;
	}
}
