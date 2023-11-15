using System;

// Token: 0x020006F6 RID: 1782
internal class TaskDivision<Task, SharedData> where Task : DivisibleTask<SharedData>, new()
{
	// Token: 0x060030E0 RID: 12512 RVA: 0x001031E0 File Offset: 0x001013E0
	public TaskDivision(int taskCount)
	{
		this.tasks = new Task[taskCount];
		for (int num = 0; num != this.tasks.Length; num++)
		{
			this.tasks[num] = Activator.CreateInstance<Task>();
		}
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x00103223 File Offset: 0x00101423
	public TaskDivision() : this(CPUBudget.coreCount)
	{
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x00103230 File Offset: 0x00101430
	public void Initialize(int count)
	{
		int num = count / this.tasks.Length;
		for (int num2 = 0; num2 != this.tasks.Length; num2++)
		{
			this.tasks[num2].start = num2 * num;
			this.tasks[num2].end = this.tasks[num2].start + num;
		}
		DebugUtil.Assert(this.tasks[this.tasks.Length - 1].end + count % this.tasks.Length == count);
		this.tasks[this.tasks.Length - 1].end = count;
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x001032F4 File Offset: 0x001014F4
	public void Run(SharedData sharedData)
	{
		Task[] array = this.tasks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Run(sharedData);
		}
	}

	// Token: 0x04001D68 RID: 7528
	public Task[] tasks;
}
