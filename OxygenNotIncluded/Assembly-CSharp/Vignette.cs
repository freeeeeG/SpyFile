using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C90 RID: 3216
public class Vignette : KMonoBehaviour
{
	// Token: 0x06006678 RID: 26232 RVA: 0x00263617 File Offset: 0x00261817
	public static void DestroyInstance()
	{
		Vignette.Instance = null;
	}

	// Token: 0x06006679 RID: 26233 RVA: 0x00263620 File Offset: 0x00261820
	protected override void OnSpawn()
	{
		this.looping_sounds = base.GetComponent<LoopingSounds>();
		base.OnSpawn();
		Vignette.Instance = this;
		this.defaultColor = this.image.color;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(1585324898, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-1393151672, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-741654735, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-2062778933, new Action<object>(this.Refresh));
	}

	// Token: 0x0600667A RID: 26234 RVA: 0x002636E2 File Offset: 0x002618E2
	public void SetColor(Color color)
	{
		this.image.color = color;
	}

	// Token: 0x0600667B RID: 26235 RVA: 0x002636F0 File Offset: 0x002618F0
	public void Refresh(object data)
	{
		AlertStateManager.Instance alertManager = ClusterManager.Instance.activeWorld.AlertManager;
		if (alertManager == null)
		{
			return;
		}
		if (alertManager.IsYellowAlert())
		{
			this.SetColor(this.yellowAlertColor);
			if (!this.showingYellowAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("YellowAlert_LP", false), true, false, true);
				this.showingYellowAlert = true;
			}
		}
		else
		{
			this.showingYellowAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
		}
		if (alertManager.IsRedAlert())
		{
			this.SetColor(this.redAlertColor);
			if (!this.showingRedAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("RedAlert_LP", false), true, false, true);
				this.showingRedAlert = true;
			}
		}
		else
		{
			this.showingRedAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		}
		if (!this.showingRedAlert && !this.showingYellowAlert)
		{
			this.Reset();
		}
	}

	// Token: 0x0600667C RID: 26236 RVA: 0x002637E0 File Offset: 0x002619E0
	public void Reset()
	{
		this.SetColor(this.defaultColor);
		this.showingRedAlert = false;
		this.showingYellowAlert = false;
		this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
	}

	// Token: 0x0400469C RID: 18076
	[SerializeField]
	private Image image;

	// Token: 0x0400469D RID: 18077
	public Color defaultColor;

	// Token: 0x0400469E RID: 18078
	public Color redAlertColor = new Color(1f, 0f, 0f, 0.3f);

	// Token: 0x0400469F RID: 18079
	public Color yellowAlertColor = new Color(1f, 1f, 0f, 0.3f);

	// Token: 0x040046A0 RID: 18080
	public static Vignette Instance;

	// Token: 0x040046A1 RID: 18081
	private LoopingSounds looping_sounds;

	// Token: 0x040046A2 RID: 18082
	private bool showingRedAlert;

	// Token: 0x040046A3 RID: 18083
	private bool showingYellowAlert;
}
