using System;

// Token: 0x020003A9 RID: 937
public interface IWorkerPrioritizable
{
	// Token: 0x06001393 RID: 5011
	bool GetWorkerPriority(Worker worker, out int priority);
}
