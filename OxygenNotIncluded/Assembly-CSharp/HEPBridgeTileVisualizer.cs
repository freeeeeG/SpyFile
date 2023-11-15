using System;

// Token: 0x02000613 RID: 1555
public class HEPBridgeTileVisualizer : KMonoBehaviour, IHighEnergyParticleDirection
{
	// Token: 0x06002720 RID: 10016 RVA: 0x000D4980 File Offset: 0x000D2B80
	protected override void OnSpawn()
	{
		base.Subscribe<HEPBridgeTileVisualizer>(-1643076535, HEPBridgeTileVisualizer.OnRotateDelegate);
		this.OnRotate();
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x000D4999 File Offset: 0x000D2B99
	public void OnRotate()
	{
		Game.Instance.ForceOverlayUpdate(true);
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06002722 RID: 10018 RVA: 0x000D49A8 File Offset: 0x000D2BA8
	// (set) Token: 0x06002723 RID: 10019 RVA: 0x000D49F5 File Offset: 0x000D2BF5
	public EightDirection Direction
	{
		get
		{
			EightDirection result = EightDirection.Right;
			Rotatable component = base.GetComponent<Rotatable>();
			if (component != null)
			{
				switch (component.Orientation)
				{
				case Orientation.Neutral:
					result = EightDirection.Left;
					break;
				case Orientation.R90:
					result = EightDirection.Up;
					break;
				case Orientation.R180:
					result = EightDirection.Right;
					break;
				case Orientation.R270:
					result = EightDirection.Down;
					break;
				}
			}
			return result;
		}
		set
		{
		}
	}

	// Token: 0x04001674 RID: 5748
	private static readonly EventSystem.IntraObjectHandler<HEPBridgeTileVisualizer> OnRotateDelegate = new EventSystem.IntraObjectHandler<HEPBridgeTileVisualizer>(delegate(HEPBridgeTileVisualizer component, object data)
	{
		component.OnRotate();
	});
}
