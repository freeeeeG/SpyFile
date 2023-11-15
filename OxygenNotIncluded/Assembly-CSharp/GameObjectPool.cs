using System;
using UnityEngine;

// Token: 0x0200038A RID: 906
public class GameObjectPool : ObjectPool<GameObject>
{
	// Token: 0x060012B2 RID: 4786 RVA: 0x000641E7 File Offset: 0x000623E7
	public GameObjectPool(Func<GameObject> instantiator, int initial_count = 0) : base(instantiator, initial_count)
	{
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x000641F1 File Offset: 0x000623F1
	public override GameObject GetInstance()
	{
		return base.GetInstance();
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x000641FC File Offset: 0x000623FC
	public void Destroy()
	{
		for (int i = this.unused.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.unused.Pop());
		}
	}
}
