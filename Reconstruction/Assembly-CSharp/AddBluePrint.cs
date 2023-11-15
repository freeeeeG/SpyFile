using System;
using System.Collections.Generic;

// Token: 0x02000259 RID: 601
public class AddBluePrint : GuideEvent
{
	// Token: 0x06000F19 RID: 3865 RVA: 0x00027E0C File Offset: 0x0002600C
	public override void Trigger()
	{
		RefactorStrategy specificStrategyByString = ConstructHelper.GetSpecificStrategyByString(this.TurretName, this.Elements, this.Qualities, 1);
		specificStrategyByString.AddElementSkill(TurretSkillFactory.GetElementSkill(this.Elements));
		Singleton<GameManager>.Instance.AddBluePrint(specificStrategyByString);
	}

	// Token: 0x04000781 RID: 1921
	public string TurretName;

	// Token: 0x04000782 RID: 1922
	public List<int> Elements;

	// Token: 0x04000783 RID: 1923
	public List<int> Qualities;
}
