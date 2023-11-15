using System;

// Token: 0x02000278 RID: 632
public class AssignChildComponentAttribute : AssignComponentAttribute
{
	// Token: 0x06000BC6 RID: 3014 RVA: 0x0003DB42 File Offset: 0x0003BF42
	public AssignChildComponentAttribute(Visibility _vis) : base(_vis)
	{
		this.Type = AssignComponentAttribute.LocatorType.DirectChild;
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0003DB52 File Offset: 0x0003BF52
	public AssignChildComponentAttribute(Editorbility _e = Editorbility.NonEditable) : base(_e)
	{
		this.Type = AssignComponentAttribute.LocatorType.DirectChild;
	}
}
