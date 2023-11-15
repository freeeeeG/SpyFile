using System;

// Token: 0x02000279 RID: 633
public class AssignComponentRecursiveAttribute : AssignComponentAttribute
{
	// Token: 0x06000BC8 RID: 3016 RVA: 0x0003DB62 File Offset: 0x0003BF62
	public AssignComponentRecursiveAttribute(Visibility _vis) : base(_vis)
	{
		this.Type = AssignComponentAttribute.LocatorType.Recursive;
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x0003DB72 File Offset: 0x0003BF72
	public AssignComponentRecursiveAttribute(Editorbility _e = Editorbility.NonEditable) : base(_e)
	{
		this.Type = AssignComponentAttribute.LocatorType.Recursive;
	}
}
