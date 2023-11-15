using System;
using TMPro;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class JumpDamage : ReusableObject
{
	// Token: 0x06000F58 RID: 3928 RVA: 0x00028D64 File Offset: 0x00026F64
	private void Update()
	{
		this.progress += this.jumpSpeed * Time.deltaTime;
		if (this.progress <= 1f)
		{
			base.transform.position = Vector2.Lerp(this.startPos, this.endPos, this.progress);
			return;
		}
		if (this.progress >= 2f)
		{
			this.RecycleObject();
		}
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x00028DD4 File Offset: 0x00026FD4
	private void SetValue(long amount, Vector2 pos, bool isCritical)
	{
		this.progress = 0f;
		TextSet textSet = isCritical ? this.crit : this.normal;
		this.Text.color = textSet.color;
		base.transform.localScale = Vector2.one * Mathf.Clamp(textSet.size * (Mathf.Log10((float)amount) + 1f), 0.1f, 5f);
		this.randomOffset = new Vector2(Random.Range(-this.offset, this.offset), Random.Range(-this.offset, this.offset));
		this.startPos = pos + this.randomOffset;
		this.endPos = this.startPos + Vector2.up * 0.5f;
		base.transform.position = this.startPos;
		this.Text.text = amount.ToString();
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x00028ED5 File Offset: 0x000270D5
	public void Jump(long amount, Vector2 pos, bool isCritical)
	{
		this.SetValue(amount, pos, isCritical);
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x00028EE0 File Offset: 0x000270E0
	public void RecycleObject()
	{
		Singleton<ObjectPool>.Instance.UnSpawn(this);
	}

	// Token: 0x040007BB RID: 1979
	[SerializeField]
	private TextMeshPro Text;

	// Token: 0x040007BC RID: 1980
	private Vector2 randomOffset;

	// Token: 0x040007BD RID: 1981
	private float offset = 0.1f;

	// Token: 0x040007BE RID: 1982
	[SerializeField]
	private TextSet normal;

	// Token: 0x040007BF RID: 1983
	[SerializeField]
	private TextSet crit;

	// Token: 0x040007C0 RID: 1984
	[SerializeField]
	private float jumpSpeed;

	// Token: 0x040007C1 RID: 1985
	private Vector2 startPos;

	// Token: 0x040007C2 RID: 1986
	private Vector2 endPos;

	// Token: 0x040007C3 RID: 1987
	private float progress;
}
