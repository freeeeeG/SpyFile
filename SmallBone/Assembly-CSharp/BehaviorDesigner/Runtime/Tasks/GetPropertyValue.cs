using System;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001475 RID: 5237
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	[TaskCategory("Reflection")]
	[TaskDescription("Gets the value from the property specified. Returns success if the property was retrieved.")]
	public class GetPropertyValue : Action
	{
		// Token: 0x06006620 RID: 26144 RVA: 0x00127358 File Offset: 0x00125558
		public override TaskStatus OnUpdate()
		{
			if (this.propertyValue == null)
			{
				Debug.LogWarning("Unable to get property - property value is null");
				return TaskStatus.Failure;
			}
			Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.Value);
			if (typeWithinAssembly == null)
			{
				Debug.LogWarning("Unable to get property - type is null");
				return TaskStatus.Failure;
			}
			Component component = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				Debug.LogWarning("Unable to get the property with component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			PropertyInfo property = component.GetType().GetProperty(this.propertyName.Value);
			this.propertyValue.SetValue(property.GetValue(component, null));
			return TaskStatus.Success;
		}

		// Token: 0x06006621 RID: 26145 RVA: 0x00127407 File Offset: 0x00125607
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.componentName = null;
			this.propertyName = null;
			this.propertyValue = null;
		}

		// Token: 0x0400521D RID: 21021
		[Tooltip("The GameObject to get the property of")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400521E RID: 21022
		[Tooltip("The component to get the property of")]
		public SharedString componentName;

		// Token: 0x0400521F RID: 21023
		[Tooltip("The name of the property")]
		public SharedString propertyName;

		// Token: 0x04005220 RID: 21024
		[RequiredField]
		[Tooltip("The value of the property")]
		public SharedVariable propertyValue;
	}
}
