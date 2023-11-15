using System;

// Token: 0x02000535 RID: 1333
public class NavTeleportTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06001FC2 RID: 8130 RVA: 0x000A9614 File Offset: 0x000A7814
	public NavTeleportTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000A9620 File Offset: 0x000A7820
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		if (transition.start == NavType.Teleport)
		{
			int num = Grid.PosToCell(navigator);
			int num2;
			int num3;
			Grid.CellToXY(num, out num2, out num3);
			int num4 = navigator.NavGrid.teleportTransitions[num];
			int num5;
			int num6;
			Grid.CellToXY(navigator.NavGrid.teleportTransitions[num], out num5, out num6);
			transition.x = num5 - num2;
			transition.y = num6 - num3;
		}
	}
}
