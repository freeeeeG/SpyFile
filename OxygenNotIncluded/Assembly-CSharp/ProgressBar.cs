using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BC9 RID: 3017
[AddComponentMenu("KMonoBehaviour/scripts/ProgressBar")]
public class ProgressBar : KMonoBehaviour
{
	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x06005EBB RID: 24251 RVA: 0x0022CA9E File Offset: 0x0022AC9E
	// (set) Token: 0x06005EBC RID: 24252 RVA: 0x0022CAAB File Offset: 0x0022ACAB
	public Color barColor
	{
		get
		{
			return this.bar.color;
		}
		set
		{
			this.bar.color = value;
		}
	}

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x06005EBD RID: 24253 RVA: 0x0022CAB9 File Offset: 0x0022ACB9
	// (set) Token: 0x06005EBE RID: 24254 RVA: 0x0022CAC6 File Offset: 0x0022ACC6
	public float PercentFull
	{
		get
		{
			return this.bar.fillAmount;
		}
		set
		{
			this.bar.fillAmount = value;
		}
	}

	// Token: 0x06005EBF RID: 24255 RVA: 0x0022CAD4 File Offset: 0x0022ACD4
	public void SetVisibility(bool visible)
	{
		this.lastVisibilityValue = visible;
		this.RefreshVisibility();
	}

	// Token: 0x06005EC0 RID: 24256 RVA: 0x0022CAE4 File Offset: 0x0022ACE4
	private void RefreshVisibility()
	{
		int myWorldId = base.gameObject.GetMyWorldId();
		bool flag = this.lastVisibilityValue;
		flag &= (!this.hasBeenInitialize || myWorldId == ClusterManager.Instance.activeWorldId);
		flag &= (!this.autoHide || SimDebugView.Instance == null || SimDebugView.Instance.GetMode() == OverlayModes.None.ID);
		base.gameObject.SetActive(flag);
		if (this.updatePercentFull == null || this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005EC1 RID: 24257 RVA: 0x0022CB80 File Offset: 0x0022AD80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hasBeenInitialize = true;
		if (this.autoHide)
		{
			this.overlayUpdateHandle = Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
			if (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() != OverlayModes.None.ID)
			{
				base.gameObject.SetActive(false);
			}
		}
		Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		base.enabled = (this.updatePercentFull != null);
		this.RefreshVisibility();
	}

	// Token: 0x06005EC2 RID: 24258 RVA: 0x0022CC34 File Offset: 0x0022AE34
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		this.SetWorldActive(tuple.first);
	}

	// Token: 0x06005EC3 RID: 24259 RVA: 0x0022CC54 File Offset: 0x0022AE54
	private void SetWorldActive(int worldId)
	{
		this.RefreshVisibility();
	}

	// Token: 0x06005EC4 RID: 24260 RVA: 0x0022CC5C File Offset: 0x0022AE5C
	public void SetUpdateFunc(Func<float> func)
	{
		this.updatePercentFull = func;
		base.enabled = (this.updatePercentFull != null);
	}

	// Token: 0x06005EC5 RID: 24261 RVA: 0x0022CC74 File Offset: 0x0022AE74
	public virtual void Update()
	{
		if (this.updatePercentFull != null && !this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			this.PercentFull = this.updatePercentFull();
		}
	}

	// Token: 0x06005EC6 RID: 24262 RVA: 0x0022CCA1 File Offset: 0x0022AEA1
	public virtual void OnOverlayChanged(object data = null)
	{
		this.RefreshVisibility();
	}

	// Token: 0x06005EC7 RID: 24263 RVA: 0x0022CCAC File Offset: 0x0022AEAC
	public void Retarget(GameObject entity)
	{
		Vector3 vector = entity.transform.GetPosition() + Vector3.down * 0.5f;
		Building component = entity.GetComponent<Building>();
		if (component != null)
		{
			vector -= Vector3.right * 0.5f * (float)(component.Def.WidthInCells % 2);
		}
		else
		{
			vector -= Vector3.right * 0.5f;
		}
		base.transform.SetPosition(vector);
	}

	// Token: 0x06005EC8 RID: 24264 RVA: 0x0022CD37 File Offset: 0x0022AF37
	protected override void OnCleanUp()
	{
		if (this.overlayUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.overlayUpdateHandle);
		}
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		base.OnCleanUp();
	}

	// Token: 0x06005EC9 RID: 24265 RVA: 0x0022CD73 File Offset: 0x0022AF73
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x06005ECA RID: 24266 RVA: 0x0022CD7C File Offset: 0x0022AF7C
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x06005ECB RID: 24267 RVA: 0x0022CD88 File Offset: 0x0022AF88
	public static ProgressBar CreateProgressBar(GameObject entity, Func<float> updateFunc)
	{
		ProgressBar progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
		progressBar.SetUpdateFunc(updateFunc);
		progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
		progressBar.name = ((entity != null) ? (entity.name + "_") : "") + " ProgressBar";
		progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
		progressBar.Update();
		progressBar.Retarget(entity);
		return progressBar;
	}

	// Token: 0x04003FFA RID: 16378
	public Image bar;

	// Token: 0x04003FFB RID: 16379
	private Func<float> updatePercentFull;

	// Token: 0x04003FFC RID: 16380
	private int overlayUpdateHandle = -1;

	// Token: 0x04003FFD RID: 16381
	public bool autoHide = true;

	// Token: 0x04003FFE RID: 16382
	private bool lastVisibilityValue = true;

	// Token: 0x04003FFF RID: 16383
	private bool hasBeenInitialize;
}
