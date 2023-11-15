using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001477 RID: 5239
	[TaskDescription("Sets the field to the value specified. Returns success if the field was set.")]
	[TaskCategory("Reflection")]
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	public class SetFieldValue : Action
	{
		// Token: 0x06006626 RID: 26150 RVA: 0x001275C8 File Offset: 0x001257C8
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
				Debug.LogWarning("Unable to set field - type is null");
				return TaskStatus.Failure;
			}
			Component component = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				Debug.LogWarning("Unable to set the field with component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			component.GetType().GetField(this.fieldName.Value).SetValue(component, this.fieldValue.GetValue());
			return TaskStatus.Success;
		}

		// Token: 0x06006627 RID: 26151 RVA: 0x00127674 File Offset: 0x00125874
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.componentName = null;
			this.fieldName = null;
			this.fieldValue = null;
		}

		// Token: 0x04005229 RID: 21033
		[Tooltip("The GameObject to set the field on")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400522A RID: 21034
		[Tooltip("The component to set the field on")]
		public SharedString componentName;

		// Token: 0x0400522B RID: 21035
		[Tooltip("The name of the field")]
		public SharedString fieldName;

		// Token: 0x0400522C RID: 21036
		[Tooltip("The value to set")]
		public SharedVariable fieldValue;
	}
}
