using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x0200042E RID: 1070
public class MySmi : MyAttributeManager<StateMachine.Instance>
{
	// Token: 0x0600167C RID: 5756 RVA: 0x00075548 File Offset: 0x00073748
	public static void Init()
	{
		MyAttributes.Register(new MySmi(new Dictionary<Type, MethodInfo>
		{
			{
				typeof(MySmiGet),
				typeof(MySmi).GetMethod("FindSmi")
			},
			{
				typeof(MySmiReq),
				typeof(MySmi).GetMethod("RequireSmi")
			}
		}));
	}

	// Token: 0x0600167D RID: 5757 RVA: 0x000755AC File Offset: 0x000737AC
	public MySmi(Dictionary<Type, MethodInfo> attributeMap) : base(attributeMap, null)
	{
	}

	// Token: 0x0600167E RID: 5758 RVA: 0x000755B8 File Offset: 0x000737B8
	public static StateMachine.Instance FindSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		StateMachineController component = c.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetSMI<T>();
		}
		return null;
	}

	// Token: 0x0600167F RID: 5759 RVA: 0x000755E4 File Offset: 0x000737E4
	public static StateMachine.Instance RequireSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		if (isStart)
		{
			StateMachine.Instance instance = MySmi.FindSmi<T>(c, isStart);
			Debug.Assert(instance != null, string.Format("{0} '{1}' requires a StateMachineInstance of type {2}!", c.GetType().ToString(), c.name, typeof(T)));
			return instance;
		}
		return MySmi.FindSmi<T>(c, isStart);
	}
}
