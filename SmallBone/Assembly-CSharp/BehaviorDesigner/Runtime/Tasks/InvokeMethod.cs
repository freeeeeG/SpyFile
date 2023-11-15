using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001476 RID: 5238
	[TaskDescription("Invokes the specified method with the specified parameters. Can optionally store the return value. Returns success if the method was invoked.")]
	[TaskCategory("Reflection")]
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	public class InvokeMethod : Action
	{
		// Token: 0x06006623 RID: 26147 RVA: 0x00127428 File Offset: 0x00125628
		public override TaskStatus OnUpdate()
		{
			Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.Value);
			if (typeWithinAssembly == null)
			{
				Debug.LogWarning("Unable to invoke - type is null");
				return TaskStatus.Failure;
			}
			Component component = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				Debug.LogWarning("Unable to invoke method with component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			List<object> list = new List<object>();
			List<Type> list2 = new List<Type>();
			int num = 0;
			SharedVariable sharedVariable;
			while (num < 4 && (sharedVariable = (base.GetType().GetField("parameter" + (num + 1).ToString()).GetValue(this) as SharedVariable)) != null)
			{
				list.Add(sharedVariable.GetValue());
				list2.Add(sharedVariable.GetType().GetProperty("Value").PropertyType);
				num++;
			}
			MethodInfo method = component.GetType().GetMethod(this.methodName.Value, list2.ToArray());
			if (method == null)
			{
				Debug.LogWarning("Unable to invoke method " + this.methodName.Value + " on component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			object value = method.Invoke(component, list.ToArray());
			if (this.storeResult != null)
			{
				this.storeResult.SetValue(value);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006624 RID: 26148 RVA: 0x0012758C File Offset: 0x0012578C
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.componentName = null;
			this.methodName = null;
			this.parameter1 = null;
			this.parameter2 = null;
			this.parameter3 = null;
			this.parameter4 = null;
			this.storeResult = null;
		}

		// Token: 0x04005221 RID: 21025
		[Tooltip("The GameObject to invoke the method on")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005222 RID: 21026
		[Tooltip("The component to invoke the method on")]
		public SharedString componentName;

		// Token: 0x04005223 RID: 21027
		[Tooltip("The name of the method")]
		public SharedString methodName;

		// Token: 0x04005224 RID: 21028
		[Tooltip("The first parameter of the method")]
		public SharedVariable parameter1;

		// Token: 0x04005225 RID: 21029
		[Tooltip("The second parameter of the method")]
		public SharedVariable parameter2;

		// Token: 0x04005226 RID: 21030
		[Tooltip("The third parameter of the method")]
		public SharedVariable parameter3;

		// Token: 0x04005227 RID: 21031
		[Tooltip("The fourth parameter of the method")]
		public SharedVariable parameter4;

		// Token: 0x04005228 RID: 21032
		[Tooltip("Store the result of the invoke call")]
		public SharedVariable storeResult;
	}
}
