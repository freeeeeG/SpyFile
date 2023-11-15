using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug
{
	// Token: 0x020015E7 RID: 5607
	[TaskDescription("LogFormat is analgous to Debug.LogFormat().\nIt takes format string, substitutes arguments supplied a '{0-4}' and returns success.\nAny fields or arguments not supplied are ignored.It can be used for debugging.")]
	[TaskIcon("{SkinColor}LogIcon.png")]
	public class LogFormat : Action
	{
		// Token: 0x06006B4A RID: 27466 RVA: 0x001338C8 File Offset: 0x00131AC8
		public override TaskStatus OnUpdate()
		{
			object[] args = this.buildParamsArray();
			if (this.logError.Value)
			{
				Debug.LogErrorFormat(this.textFormat.Value, args);
			}
			else
			{
				Debug.LogFormat(this.textFormat.Value, args);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B4B RID: 27467 RVA: 0x00133910 File Offset: 0x00131B10
		private object[] buildParamsArray()
		{
			object[] array;
			if (this.isValid(this.arg3))
			{
				array = new object[]
				{
					null,
					null,
					null,
					this.arg3.GetValue()
				};
				array[2] = this.arg2.GetValue();
				array[1] = this.arg1.GetValue();
				array[0] = this.arg0.GetValue();
			}
			else if (this.isValid(this.arg2))
			{
				array = new object[]
				{
					null,
					null,
					this.arg2.GetValue()
				};
				array[1] = this.arg1.GetValue();
				array[0] = this.arg0.GetValue();
			}
			else if (this.isValid(this.arg1))
			{
				array = new object[]
				{
					null,
					this.arg1.GetValue()
				};
				array[0] = this.arg0.GetValue();
			}
			else
			{
				if (!this.isValid(this.arg0))
				{
					return null;
				}
				array = new object[]
				{
					this.arg0.GetValue()
				};
			}
			return array;
		}

		// Token: 0x06006B4C RID: 27468 RVA: 0x00133A0B File Offset: 0x00131C0B
		private bool isValid(SharedVariable sv)
		{
			return sv != null && !sv.IsNone;
		}

		// Token: 0x06006B4D RID: 27469 RVA: 0x00133A1B File Offset: 0x00131C1B
		public override void OnReset()
		{
			this.textFormat = string.Empty;
			this.logError = false;
			this.arg0 = null;
			this.arg1 = null;
			this.arg2 = null;
			this.arg3 = null;
		}

		// Token: 0x04005706 RID: 22278
		[Tooltip("Text format with {0}, {1}, etc")]
		public SharedString textFormat;

		// Token: 0x04005707 RID: 22279
		[Tooltip("Is this text an error?")]
		public SharedBool logError;

		// Token: 0x04005708 RID: 22280
		public SharedVariable arg0;

		// Token: 0x04005709 RID: 22281
		public SharedVariable arg1;

		// Token: 0x0400570A RID: 22282
		public SharedVariable arg2;

		// Token: 0x0400570B RID: 22283
		public SharedVariable arg3;
	}
}
