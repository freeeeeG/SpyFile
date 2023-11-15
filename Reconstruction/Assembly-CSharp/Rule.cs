using System;

// Token: 0x0200014E RID: 334
public abstract class Rule
{
	// Token: 0x17000333 RID: 819
	// (get) Token: 0x060008F7 RID: 2295
	public abstract RuleName RuleName { get; }

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x060008F8 RID: 2296 RVA: 0x0001894F File Offset: 0x00016B4F
	public virtual bool Add
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00018954 File Offset: 0x00016B54
	public string Description
	{
		get
		{
			return GameMultiLang.GetTraduction(this.RuleName.ToString());
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0001897A File Offset: 0x00016B7A
	public virtual void BeforeGameLoad()
	{
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0001897C File Offset: 0x00016B7C
	public virtual void OnGameLoad()
	{
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0001897E File Offset: 0x00016B7E
	public virtual void OnGameInit()
	{
	}
}
