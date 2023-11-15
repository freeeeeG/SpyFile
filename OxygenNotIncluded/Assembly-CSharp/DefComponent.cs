using System;
using UnityEngine;

// Token: 0x02000426 RID: 1062
[Serializable]
public class DefComponent<T> where T : Component
{
	// Token: 0x06001669 RID: 5737 RVA: 0x0007526C File Offset: 0x0007346C
	public DefComponent(T cmp)
	{
		this.cmp = cmp;
	}

	// Token: 0x0600166A RID: 5738 RVA: 0x0007527C File Offset: 0x0007347C
	public T Get(StateMachine.Instance smi)
	{
		T[] components = this.cmp.GetComponents<T>();
		int num = 0;
		while (num < components.Length && !(components[num] == this.cmp))
		{
			num++;
		}
		return smi.gameObject.GetComponents<T>()[num];
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x000752D7 File Offset: 0x000734D7
	public static implicit operator DefComponent<T>(T cmp)
	{
		return new DefComponent<T>(cmp);
	}

	// Token: 0x04000C80 RID: 3200
	[SerializeField]
	private T cmp;
}
