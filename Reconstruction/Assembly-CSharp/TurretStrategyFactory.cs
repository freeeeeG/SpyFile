using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010A RID: 266
[CreateAssetMenu(menuName = "Factory/BlueprintFactory", fileName = "blueprintFactory")]
public class TurretStrategyFactory : GameObjectFactory
{
	// Token: 0x060006A3 RID: 1699 RVA: 0x00012480 File Offset: 0x00010680
	public RefactorStrategy GetRandomRefactorStrategy(TurretAttribute attribute)
	{
		int[] someRandoms = StaticData.GetSomeRandoms(attribute.minElementLevel, attribute.maxElementLevel, attribute.totalLevel, attribute.elementNumber);
		List<Composition> list = new List<Composition>();
		for (int i = 0; i < attribute.elementNumber; i++)
		{
			int element = (int)Singleton<StaticData>.Instance.ContentFactory.GetRandomElementAttribute().element;
			Composition item = new Composition(someRandoms[i], element);
			list.Add(item);
		}
		return this.FormStrategy(attribute, 1, list);
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x000124F4 File Offset: 0x000106F4
	public RefactorStrategy GetSpecificRefactorStrategy(TurretAttribute attribute, List<int> elements, List<int> qualities, int quality = 1)
	{
		List<Composition> list = new List<Composition>();
		for (int i = 0; i < elements.Count; i++)
		{
			Composition item = new Composition(qualities[i], elements[i]);
			list.Add(item);
		}
		return this.FormStrategy(attribute, quality, list);
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x00012540 File Offset: 0x00010740
	private RefactorStrategy FormStrategy(TurretAttribute att, int quality, List<Composition> compositions)
	{
		RefactorTurretName refactorName = att.RefactorName;
		RefactorStrategy result;
		if (refactorName != RefactorTurretName.Rotary)
		{
			switch (refactorName)
			{
			case RefactorTurretName.Boomerrang:
				return new BoomerangStrategy(att, quality, compositions);
			case RefactorTurretName.Prism:
			case RefactorTurretName.Amplifier:
				return new BuildingStrategy(att, quality, compositions);
			case RefactorTurretName.Chiller:
			case RefactorTurretName.Firer:
				return new FirerStrategy(att, quality, compositions);
			}
			result = new RefactorStrategy(att, quality, compositions);
		}
		else
		{
			result = new RotaryStrategy(att, quality, compositions);
		}
		return result;
	}
}
