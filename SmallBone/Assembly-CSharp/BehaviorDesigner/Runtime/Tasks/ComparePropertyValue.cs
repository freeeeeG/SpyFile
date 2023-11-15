using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014CA RID: 5322
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	[TaskCategory("Reflection")]
	[TaskDescription("Compares the property value to the value specified. Returns success if the values are the same.")]
	public class ComparePropertyValue : Conditional
	{
		// Token: 0x0600677A RID: 26490 RVA: 0x0012B684 File Offset: 0x00129884
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
				Debug.LogWarning("Unable to compare property - type is null");
				return TaskStatus.Failure;
			}
			Component component = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				Debug.LogWarning("Unable to compare the property with component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			object value = component.GetType().GetProperty(this.propertyName.Value).GetValue(component, null);
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

		// Token: 0x0600677B RID: 26491 RVA: 0x0012B74E File Offset: 0x0012994E
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.componentName = null;
			this.propertyName = null;
			this.compareValue = null;
		}

		// Token: 0x04005381 RID: 21377
		[Tooltip("The GameObject to compare the property of")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005382 RID: 21378
		[Tooltip("The component to compare the property of")]
		public SharedString componentName;

		// Token: 0x04005383 RID: 21379
		[Tooltip("The name of the property")]
		public SharedString propertyName;

		// Token: 0x04005384 RID: 21380
		[Tooltip("The value to compare to")]
		public SharedVariable compareValue;
	}
}
