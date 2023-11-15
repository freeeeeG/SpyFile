using System;
using UnityEngine;

// Token: 0x02000532 RID: 1330
public class TubeTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06001FB9 RID: 8121 RVA: 0x000A931D File Offset: 0x000A751D
	public TubeTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.tube_traveller = navigator.GetSMI<TubeTraveller.Instance>();
		if (this.tube_traveller != null && navigator.CurrentNavType == NavType.Tube && !this.tube_traveller.inTube)
		{
			this.tube_traveller.OnTubeTransition(true);
		}
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x000A935C File Offset: 0x000A755C
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.tube_traveller.OnPathAdvanced(null);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube)
		{
			int cell = Grid.PosToCell(navigator);
			this.entrance = this.GetEntrance(cell);
			return;
		}
		this.entrance = null;
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x000A93AC File Offset: 0x000A75AC
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube && this.entrance)
		{
			this.entrance.ConsumeCharge(navigator.gameObject);
			this.entrance = null;
		}
		this.tube_traveller.OnTubeTransition(transition.end == NavType.Tube);
	}

	// Token: 0x06001FBC RID: 8124 RVA: 0x000A940C File Offset: 0x000A760C
	private TravelTubeEntrance GetEntrance(int cell)
	{
		if (!Grid.HasUsableTubeEntrance(cell, this.tube_traveller.prefabInstanceID))
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
			if (component != null && component.isSpawned)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x040011B8 RID: 4536
	private TubeTraveller.Instance tube_traveller;

	// Token: 0x040011B9 RID: 4537
	private TravelTubeEntrance entrance;
}
