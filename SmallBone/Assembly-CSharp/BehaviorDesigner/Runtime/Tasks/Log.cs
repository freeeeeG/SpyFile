using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200146C RID: 5228
	[TaskDescription("Log is a simple task which will output the specified text and return success. It can be used for debugging.")]
	[TaskIcon("{SkinColor}LogIcon.png")]
	public class Log : Action
	{
		// Token: 0x06006609 RID: 26121 RVA: 0x00126D30 File Offset: 0x00124F30
		public override TaskStatus OnUpdate()
		{
			if (this.logError.Value)
			{
				Debug.LogError(this.logTime.Value ? string.Format("{0}: {1}", Time.time, this.text) : this.text);
			}
			else
			{
				Debug.Log(this.logTime.Value ? string.Format("{0}: {1}", Time.time, this.text) : this.text);
			}
			return TaskStatus.Success;
		}

		// Token: 0x0600660A RID: 26122 RVA: 0x00126DBF File Offset: 0x00124FBF
		public override void OnReset()
		{
			this.text = "";
			this.logError = false;
			this.logTime = false;
		}

		// Token: 0x04005200 RID: 20992
		[Tooltip("Text to output to the log")]
		public SharedString text;

		// Token: 0x04005201 RID: 20993
		[Tooltip("Is this text an error?")]
		public SharedBool logError;

		// Token: 0x04005202 RID: 20994
		[Tooltip("Should the time be included in the log message?")]
		public SharedBool logTime;
	}
}
