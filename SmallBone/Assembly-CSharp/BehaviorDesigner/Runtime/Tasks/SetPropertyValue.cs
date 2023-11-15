using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001478 RID: 5240
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	[TaskCategory("Reflection")]
	[TaskDescription("Sets the property to the value specified. Returns success if the property was set.")]
	public class SetPropertyValue : Action
	{
		// Token: 0x06006629 RID: 26153 RVA: 0x00127694 File Offset: 0x00125894
		public override TaskStatus OnUpdate()
		{
			if (this.propertyValue == null)
			{
				Debug.LogWarning("Unable to get field - field value is null");
				return TaskStatus.Failure;
			}
			Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.Value);
			if (typeWithinAssembly == null)
			{
				Debug.LogWarning("Unable to set property - type is null");
				return TaskStatus.Failure;
			}
			Component component = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				Debug.LogWarning("Unable to set the property with component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			component.GetType().GetProperty(this.propertyName.Value).SetValue(component, this.propertyValue.GetValue(), null);
			return TaskStatus.Success;
		}

		// Token: 0x0600662A RID: 26154 RVA: 0x00127741 File Offset: 0x00125941
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.componentName = null;
			this.propertyName = null;
			this.propertyValue = null;
		}

		// Token: 0x0400522D RID: 21037
		[Tooltip("The GameObject to set the property on")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400522E RID: 21038
		[Tooltip("The component to set the property on")]
		public SharedString componentName;

		// Token: 0x0400522F RID: 21039
		[Tooltip("The name of the property")]
		public SharedString propertyName;

		// Token: 0x04005230 RID: 21040
		[Tooltip("The value to set")]
		public SharedVariable propertyValue;
	}
}
