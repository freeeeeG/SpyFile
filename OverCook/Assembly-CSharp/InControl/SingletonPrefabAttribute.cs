using System;

namespace InControl
{
	// Token: 0x02000346 RID: 838
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class SingletonPrefabAttribute : Attribute
	{
		// Token: 0x06000FEC RID: 4076 RVA: 0x0005C70C File Offset: 0x0005AB0C
		public SingletonPrefabAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x04000C18 RID: 3096
		public readonly string Name;
	}
}
