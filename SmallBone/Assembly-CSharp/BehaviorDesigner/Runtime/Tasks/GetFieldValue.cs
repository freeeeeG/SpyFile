using System;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001474 RID: 5236
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	[TaskCategory("Reflection")]
	[TaskDescription("Gets the value from the field specified. Returns success if the field was retrieved.")]
	public class GetFieldValue : Action
	{
		// Token: 0x0600661D RID: 26141 RVA: 0x0012728C File Offset: 0x0012548C
		public override TaskStatus OnUpdate()
		{
			if (this.fieldValue == null)
			{
				Debug.LogWarning("Unable to get field - field value is null");
				return TaskStatus.Failure;
			}
			Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.Value);
			if (typeWithinAssembly == null)
			{
				Debug.LogWarning("Unable to get field - type is null");
				return TaskStatus.Failure;
			}
			Component component = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				Debug.LogWarning("Unable to get the field with component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			FieldInfo field = component.GetType().GetField(this.fieldName.Value);
			this.fieldValue.SetValue(field.GetValue(component));
			return TaskStatus.Success;
		}

		// Token: 0x0600661E RID: 26142 RVA: 0x0012733A File Offset: 0x0012553A
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.componentName = null;
			this.fieldName = null;
			this.fieldValue = null;
		}

		// Token: 0x04005219 RID: 21017
		[Tooltip("The GameObject to get the field on")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400521A RID: 21018
		[Tooltip("The component to get the field on")]
		public SharedString componentName;

		// Token: 0x0400521B RID: 21019
		[Tooltip("The name of the field")]
		public SharedString fieldName;

		// Token: 0x0400521C RID: 21020
		[Tooltip("The value of the field")]
		[RequiredField]
		public SharedVariable fieldValue;
	}
}
