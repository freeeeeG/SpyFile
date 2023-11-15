using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class UnrealStructure : ElementSkill
{
	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06000370 RID: 880 RVA: 0x0000A1C2 File Offset: 0x000083C2
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				2,
				4
			};
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06000371 RID: 881 RVA: 0x0000A1DE File Offset: 0x000083DE
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000372 RID: 882 RVA: 0x0000A1E8 File Offset: 0x000083E8
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0000A213 File Offset: 0x00008413
	public override void Build()
	{
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0000A218 File Offset: 0x00008418
	public override void Composite()
	{
		ElementSkill elementSkill = TurretSkillFactory.GetElementSkill(new List<int>
		{
			Random.Range(0, 5),
			Random.Range(0, 5),
			Random.Range(0, 5)
		});
		ElementSkill elementSkill2 = TurretSkillFactory.GetElementSkill(new List<int>
		{
			Random.Range(0, 5),
			Random.Range(0, 5),
			Random.Range(0, 5)
		});
		elementSkill.Elements = elementSkill2.Elements;
		elementSkill.IsException = true;
		this.strategy.TurretSkills.Remove(this);
		this.strategy.AddElementSkill(elementSkill);
		for (int i = 0; i < 3; i++)
		{
			((RefactorStrategy)this.strategy).Compositions[i].elementRequirement = elementSkill.Elements[i];
		}
		elementSkill.Composite();
	}
}
