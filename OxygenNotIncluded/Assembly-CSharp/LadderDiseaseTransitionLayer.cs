using System;
using Klei;
using UnityEngine;

// Token: 0x02000533 RID: 1331
public class LadderDiseaseTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06001FBD RID: 8125 RVA: 0x000A945F File Offset: 0x000A765F
	public LadderDiseaseTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x000A9468 File Offset: 0x000A7668
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (transition.end == NavType.Ladder)
		{
			int cell = Grid.PosToCell(navigator);
			GameObject gameObject = Grid.Objects[cell, 1];
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component != null)
				{
					PrimaryElement component2 = navigator.GetComponent<PrimaryElement>();
					if (component2 != null)
					{
						SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
						invalid.idx = component2.DiseaseIdx;
						invalid.count = (int)((float)component2.DiseaseCount * 0.005f);
						SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
						invalid2.idx = component.DiseaseIdx;
						invalid2.count = (int)((float)component.DiseaseCount * 0.005f);
						component2.ModifyDiseaseCount(-invalid.count, "Navigator.EndTransition");
						component.ModifyDiseaseCount(-invalid2.count, "Navigator.EndTransition");
						if (invalid.count > 0)
						{
							component.AddDisease(invalid.idx, invalid.count, "TransitionDriver.EndTransition");
						}
						if (invalid2.count > 0)
						{
							component2.AddDisease(invalid2.idx, invalid2.count, "TransitionDriver.EndTransition");
						}
					}
				}
			}
		}
	}
}
