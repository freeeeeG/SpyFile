using System;

// Token: 0x02000743 RID: 1859
public class Death : Resource
{
	// Token: 0x06003354 RID: 13140 RVA: 0x00111232 File Offset: 0x0010F432
	public Death(string id, ResourceSet parent, string name, string description, string pre_anim, string loop_anim) : base(id, parent, name)
	{
		this.preAnim = pre_anim;
		this.loopAnim = loop_anim;
		this.description = description;
	}

	// Token: 0x04001ECE RID: 7886
	public string preAnim;

	// Token: 0x04001ECF RID: 7887
	public string loopAnim;

	// Token: 0x04001ED0 RID: 7888
	public string sound;

	// Token: 0x04001ED1 RID: 7889
	public string description;
}
