using System;
using System.Collections.Generic;

// Token: 0x02000389 RID: 905
public class ObjectPool<T>
{
	// Token: 0x060012AF RID: 4783 RVA: 0x00064148 File Offset: 0x00062348
	public ObjectPool(Func<T> instantiator, int initial_count = 0)
	{
		this.instantiator = instantiator;
		this.unused = new Stack<T>(initial_count);
		for (int i = 0; i < initial_count; i++)
		{
			this.unused.Push(instantiator());
		}
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x0006418C File Offset: 0x0006238C
	public virtual T GetInstance()
	{
		T result = default(T);
		if (this.unused.Count > 0)
		{
			result = this.unused.Pop();
		}
		else
		{
			result = this.instantiator();
		}
		return result;
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x000641CA File Offset: 0x000623CA
	public void ReleaseInstance(T instance)
	{
		if (object.Equals(instance, null))
		{
			return;
		}
		this.unused.Push(instance);
	}

	// Token: 0x04000A37 RID: 2615
	protected Stack<T> unused;

	// Token: 0x04000A38 RID: 2616
	protected Func<T> instantiator;
}
