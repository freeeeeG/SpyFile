using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C9 RID: 5321
	[TaskCategory("Reflection")]
	[TaskDescription("Compares the field value to the value specified. Returns success if the values are the same.")]
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	public class CompareFieldValue : Conditional
	{
		// Token: 0x06006777 RID: 26487 RVA: 0x0012B59C File Offset: 0x0012979C
		public override TaskStatus OnUpdate()
		{
			if (this.compareValue == null)
			{
				Debug.LogWarning("Unable to compare field - compare value is null");
				return TaskStatus.Failure;
			}
			Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.Value);
			if (typeWithinAssembly == null)
			{
				Debug.LogWarning("Unable to compare field - type is null");
				return TaskStatus.Failure;
			}
			Component component = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				Debug.LogWarning("Unable to compare the field with component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			object value = component.GetType().GetField(this.fieldName.Value).GetValue(component);
			if (value == null && this.compareValue.GetValue() == null)
			{
				return TaskStatus.Success;
			}
			if (!value.Equals(this.compareValue.GetValue()))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006778 RID: 26488 RVA: 0x0012B665 File Offset: 0x00129865
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.componentName = null;
			this.fieldName = null;
			this.compareValue = null;
		}

		// Token: 0x0400537D RID: 21373
		[Tooltip("The GameObject to compare the field on")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400537E RID: 21374
		[Tooltip("The component to compare the field on")]
		public SharedString componentName;

		// Token: 0x0400537F RID: 21375
		[Tooltip("The name of the field")]
		public SharedString fieldName;

		// Token: 0x04005380 RID: 21376
		[Tooltip("The value to compare to")]
		public SharedVariable compareValue;
	}
}
