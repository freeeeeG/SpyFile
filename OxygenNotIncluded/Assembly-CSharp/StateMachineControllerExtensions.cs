using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public static class StateMachineControllerExtensions
{
	// Token: 0x060016C0 RID: 5824 RVA: 0x0007616C File Offset: 0x0007436C
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this StateMachine.Instance smi) where StateMachineInstanceType : StateMachine.Instance
	{
		return smi.gameObject.GetSMI<StateMachineInstanceType>();
	}

	// Token: 0x060016C1 RID: 5825 RVA: 0x00076179 File Offset: 0x00074379
	public static DefType GetDef<DefType>(this Component cmp) where DefType : StateMachine.BaseDef
	{
		return cmp.gameObject.GetDef<DefType>();
	}

	// Token: 0x060016C2 RID: 5826 RVA: 0x00076188 File Offset: 0x00074388
	public static DefType GetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component == null)
		{
			return default(DefType);
		}
		return component.GetDef<DefType>();
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x000761B5 File Offset: 0x000743B5
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
	{
		return cmp.gameObject.GetSMI<StateMachineInstanceType>();
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x000761C4 File Offset: 0x000743C4
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetSMI<StateMachineInstanceType>();
		}
		return default(StateMachineInstanceType);
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x000761F1 File Offset: 0x000743F1
	public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
	{
		return cmp.gameObject.GetAllSMI<StateMachineInstanceType>();
	}

	// Token: 0x060016C6 RID: 5830 RVA: 0x00076200 File Offset: 0x00074400
	public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetAllSMI<StateMachineInstanceType>();
		}
		return new List<StateMachineInstanceType>();
	}
}
