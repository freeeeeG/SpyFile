using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BC3 RID: 3011
[AddComponentMenu("KMonoBehaviour/scripts/PopFX")]
public class PopFX : KMonoBehaviour
{
	// Token: 0x06005E7F RID: 24191 RVA: 0x0022B148 File Offset: 0x00229348
	public void Recycle()
	{
		this.icon = null;
		this.text = "";
		this.targetTransform = null;
		this.lifeElapsed = 0f;
		this.trackTarget = false;
		this.startPos = Vector3.zero;
		this.IconDisplay.color = Color.white;
		this.TextDisplay.color = Color.white;
		PopFXManager.Instance.RecycleFX(this);
		this.canvasGroup.alpha = 0f;
		base.gameObject.SetActive(false);
		this.isLive = false;
		this.isActiveWorld = false;
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
	}

	// Token: 0x06005E80 RID: 24192 RVA: 0x0022B1FC File Offset: 0x002293FC
	public void Spawn(Sprite Icon, string Text, Transform TargetTransform, Vector3 Offset, float LifeTime = 1.5f, bool TrackTarget = false)
	{
		this.icon = Icon;
		this.text = Text;
		this.targetTransform = TargetTransform;
		this.trackTarget = TrackTarget;
		this.lifetime = LifeTime;
		this.offset = Offset;
		if (this.targetTransform != null)
		{
			this.startPos = this.targetTransform.GetPosition();
			int num;
			int num2;
			Grid.PosToXY(this.startPos, out num, out num2);
			if (num2 % 2 != 0)
			{
				this.startPos.x = this.startPos.x + 0.5f;
			}
		}
		this.TextDisplay.text = this.text;
		this.IconDisplay.sprite = this.icon;
		this.canvasGroup.alpha = 1f;
		this.isLive = true;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		this.Update();
	}

	// Token: 0x06005E81 RID: 24193 RVA: 0x0022B2E8 File Offset: 0x002294E8
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		if (this.isLive)
		{
			this.SetWorldActive(tuple.first);
		}
	}

	// Token: 0x06005E82 RID: 24194 RVA: 0x0022B310 File Offset: 0x00229510
	private void SetWorldActive(int worldId)
	{
		int num = Grid.PosToCell((this.trackTarget && this.targetTransform != null) ? this.targetTransform.position : (this.startPos + this.offset));
		this.isActiveWorld = (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] == worldId);
	}

	// Token: 0x06005E83 RID: 24195 RVA: 0x0022B374 File Offset: 0x00229574
	private void Update()
	{
		if (!this.isLive)
		{
			return;
		}
		if (!PopFXManager.Instance.Ready())
		{
			return;
		}
		this.lifeElapsed += Time.unscaledDeltaTime;
		if (this.lifeElapsed >= this.lifetime)
		{
			this.Recycle();
		}
		if (this.trackTarget && this.targetTransform != null)
		{
			Vector3 v = PopFXManager.Instance.WorldToScreen(this.targetTransform.GetPosition() + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * this.lifeElapsed));
			v.z = 0f;
			base.gameObject.rectTransform().anchoredPosition = v;
		}
		else
		{
			Vector3 v2 = PopFXManager.Instance.WorldToScreen(this.startPos + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * (this.lifeElapsed / 2f)));
			v2.z = 0f;
			base.gameObject.rectTransform().anchoredPosition = v2;
		}
		this.canvasGroup.alpha = (this.isActiveWorld ? (1.5f * ((this.lifetime - this.lifeElapsed) / this.lifetime)) : 0f);
	}

	// Token: 0x04003FB9 RID: 16313
	private float Speed = 2f;

	// Token: 0x04003FBA RID: 16314
	private Sprite icon;

	// Token: 0x04003FBB RID: 16315
	private string text;

	// Token: 0x04003FBC RID: 16316
	private Transform targetTransform;

	// Token: 0x04003FBD RID: 16317
	private Vector3 offset;

	// Token: 0x04003FBE RID: 16318
	public Image IconDisplay;

	// Token: 0x04003FBF RID: 16319
	public LocText TextDisplay;

	// Token: 0x04003FC0 RID: 16320
	public CanvasGroup canvasGroup;

	// Token: 0x04003FC1 RID: 16321
	private Camera uiCamera;

	// Token: 0x04003FC2 RID: 16322
	private float lifetime;

	// Token: 0x04003FC3 RID: 16323
	private float lifeElapsed;

	// Token: 0x04003FC4 RID: 16324
	private bool trackTarget;

	// Token: 0x04003FC5 RID: 16325
	private Vector3 startPos;

	// Token: 0x04003FC6 RID: 16326
	private bool isLive;

	// Token: 0x04003FC7 RID: 16327
	private bool isActiveWorld;
}
