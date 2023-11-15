using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A8F RID: 2703
public class AlertVignette : KMonoBehaviour
{
	// Token: 0x06005296 RID: 21142 RVA: 0x001D9489 File Offset: 0x001D7689
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005297 RID: 21143 RVA: 0x001D9494 File Offset: 0x001D7694
	private void Update()
	{
		Color color = this.image.color;
		if (ClusterManager.Instance.GetWorld(this.worldID) == null)
		{
			color = Color.clear;
			this.image.color = color;
			return;
		}
		if (ClusterManager.Instance.GetWorld(this.worldID).IsRedAlert())
		{
			if (color.r != Vignette.Instance.redAlertColor.r || color.g != Vignette.Instance.redAlertColor.g || color.b != Vignette.Instance.redAlertColor.b)
			{
				color = Vignette.Instance.redAlertColor;
			}
		}
		else if (ClusterManager.Instance.GetWorld(this.worldID).IsYellowAlert())
		{
			if (color.r != Vignette.Instance.yellowAlertColor.r || color.g != Vignette.Instance.yellowAlertColor.g || color.b != Vignette.Instance.yellowAlertColor.b)
			{
				color = Vignette.Instance.yellowAlertColor;
			}
		}
		else
		{
			color = Color.clear;
		}
		if (color != Color.clear)
		{
			color.a = 0.2f + (0.5f + Mathf.Sin(Time.unscaledTime * 4f - 1f) / 2f) * 0.5f;
		}
		if (this.image.color != color)
		{
			this.image.color = color;
		}
	}

	// Token: 0x04003729 RID: 14121
	public Image image;

	// Token: 0x0400372A RID: 14122
	public int worldID;
}
