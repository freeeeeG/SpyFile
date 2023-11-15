using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
public static class EntityTemplateExtensions
{
	// Token: 0x06000222 RID: 546 RVA: 0x0000F354 File Offset: 0x0000D554
	public static DefType AddOrGetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController stateMachineController = go.AddOrGet<StateMachineController>();
		DefType defType = stateMachineController.GetDef<DefType>();
		if (defType == null)
		{
			defType = Activator.CreateInstance<DefType>();
			stateMachineController.AddDef(defType);
			defType.Configure(go);
		}
		return defType;
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000F398 File Offset: 0x0000D598
	public static ComponentType AddOrGet<ComponentType>(this GameObject go) where ComponentType : Component
	{
		ComponentType componentType = go.GetComponent<ComponentType>();
		if (componentType == null)
		{
			componentType = go.AddComponent<ComponentType>();
		}
		return componentType;
	}
}
