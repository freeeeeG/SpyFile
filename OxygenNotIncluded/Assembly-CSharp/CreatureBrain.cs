using System;

// Token: 0x0200039D RID: 925
public class CreatureBrain : Brain
{
	// Token: 0x0600135C RID: 4956 RVA: 0x000659B4 File Offset: 0x00063BB4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			component.SetAbilities(new CreaturePathFinderAbilities(component));
		}
	}

	// Token: 0x04000A60 RID: 2656
	public string symbolPrefix;

	// Token: 0x04000A61 RID: 2657
	public Tag species;
}
