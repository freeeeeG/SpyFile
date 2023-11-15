using System;

// Token: 0x02000118 RID: 280
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ExecutionDependency : Attribute
{
	// Token: 0x06000522 RID: 1314 RVA: 0x000293FF File Offset: 0x000277FF
	public ExecutionDependency(Type _dependency)
	{
		this.Dependency = _dependency;
	}

	// Token: 0x04000485 RID: 1157
	public Type Dependency;
}
