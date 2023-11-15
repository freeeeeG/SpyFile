using System;
using UnityEngine;

// Token: 0x0200092F RID: 2351
public class TechItem : Resource
{
	// Token: 0x06004444 RID: 17476 RVA: 0x0017F023 File Offset: 0x0017D223
	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] dlcIds) : base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.dlcIds = dlcIds;
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06004445 RID: 17477 RVA: 0x0017F04E File Offset: 0x0017D24E
	public Tech ParentTech
	{
		get
		{
			return Db.Get().Techs.Get(this.parentTechId);
		}
	}

	// Token: 0x06004446 RID: 17478 RVA: 0x0017F065 File Offset: 0x0017D265
	public Sprite UISprite()
	{
		return this.getUISprite("ui", false);
	}

	// Token: 0x06004447 RID: 17479 RVA: 0x0017F078 File Offset: 0x0017D278
	public bool IsComplete()
	{
		return this.ParentTech.IsComplete();
	}

	// Token: 0x04002D43 RID: 11587
	public string description;

	// Token: 0x04002D44 RID: 11588
	public Func<string, bool, Sprite> getUISprite;

	// Token: 0x04002D45 RID: 11589
	public string parentTechId;

	// Token: 0x04002D46 RID: 11590
	public string[] dlcIds;
}
