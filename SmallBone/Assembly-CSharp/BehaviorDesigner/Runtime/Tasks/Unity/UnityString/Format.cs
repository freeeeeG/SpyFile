using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x0200164A RID: 5706
	[TaskCategory("Unity/String")]
	[TaskDescription("Stores a string with the specified format.")]
	public class Format : Action
	{
		// Token: 0x06006CD6 RID: 27862 RVA: 0x001372DE File Offset: 0x001354DE
		public override void OnAwake()
		{
			this.variableValues = new object[this.variables.Length];
		}

		// Token: 0x06006CD7 RID: 27863 RVA: 0x001372F4 File Offset: 0x001354F4
		public override TaskStatus OnUpdate()
		{
			for (int i = 0; i < this.variableValues.Length; i++)
			{
				this.variableValues[i] = this.variables[i].Value.value.GetValue();
			}
			try
			{
				this.storeResult.Value = string.Format(this.format.Value, this.variableValues);
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006CD8 RID: 27864 RVA: 0x00137378 File Offset: 0x00135578
		public override void OnReset()
		{
			this.format = "";
			this.variables = null;
			this.storeResult = null;
		}

		// Token: 0x0400589E RID: 22686
		[Tooltip("The format of the string")]
		public SharedString format;

		// Token: 0x0400589F RID: 22687
		[Tooltip("Any variables to appear in the string")]
		public SharedGenericVariable[] variables;

		// Token: 0x040058A0 RID: 22688
		[Tooltip("The result of the format")]
		[RequiredField]
		public SharedString storeResult;

		// Token: 0x040058A1 RID: 22689
		private object[] variableValues;
	}
}
