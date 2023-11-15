using System;

// Token: 0x020000B9 RID: 185
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public sealed class AddTypeMenuAttribute : Attribute
{
	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000394 RID: 916 RVA: 0x0000D02C File Offset: 0x0000B22C
	public string MenuName { get; }

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000395 RID: 917 RVA: 0x0000D034 File Offset: 0x0000B234
	public int Order { get; }

	// Token: 0x06000396 RID: 918 RVA: 0x0000D03C File Offset: 0x0000B23C
	public AddTypeMenuAttribute(string menuName, int order = 0)
	{
		this.MenuName = menuName;
		this.Order = order;
	}

	// Token: 0x06000397 RID: 919 RVA: 0x0000D052 File Offset: 0x0000B252
	public string[] GetSplittedMenuName()
	{
		if (string.IsNullOrWhiteSpace(this.MenuName))
		{
			return Array.Empty<string>();
		}
		return this.MenuName.Split(AddTypeMenuAttribute.k_Separeters, StringSplitOptions.RemoveEmptyEntries);
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0000D078 File Offset: 0x0000B278
	public string GetTypeNameWithoutPath()
	{
		string[] splittedMenuName = this.GetSplittedMenuName();
		if (splittedMenuName.Length == 0)
		{
			return null;
		}
		return splittedMenuName[splittedMenuName.Length - 1];
	}

	// Token: 0x040002DB RID: 731
	private static readonly char[] k_Separeters = new char[]
	{
		'/'
	};
}
