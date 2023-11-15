using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class SpecialBullet_SmallLaser : MonoBehaviour
{
	// Token: 0x06000899 RID: 2201 RVA: 0x00031C83 File Offset: 0x0002FE83
	private void Awake()
	{
		base.StartCoroutine(this.DieAfter3Frames());
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00031C92 File Offset: 0x0002FE92
	private void Update()
	{
		if (!BattleManager.inst.GameOn)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00031CAC File Offset: 0x0002FEAC
	public void LaserInit(Color cl, bool ifColli, Vector2 dir)
	{
		if (TempData.inst.jobId != 11)
		{
			Debug.LogError("Error_不是CPU");
			Object.Destroy(base.gameObject);
			return;
		}
		this.laserCollider2D.enabled = ifColli;
		Factor playerFactorTotal = Player.inst.unit.playerFactorTotal;
		this.length = Mathf.Sqrt(playerFactorTotal.bulletRng);
		if (ifColli)
		{
			float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(3).facs;
			this.damage = playerFactorTotal.bulletDmg * (double)playerFactorTotal.fireSpd * (double)facs[4];
			this.ifCrit = MyTool.DecimalToBool(playerFactorTotal.critChc);
			if (this.ifCrit)
			{
				this.damage *= (double)playerFactorTotal.critDmg;
			}
		}
		Color color = cl.SetValue(1f).SetSaturation(0.54f);
		this.spr_Body.color = color.SetAlpha(0.6f);
		this.spr_Body_Outline.color = color.SetAlpha(0.3f);
		SpriteRenderer[] sprs = new SpriteRenderer[]
		{
			this.spr_Body,
			this.spr_Body_Outline
		};
		this.bloom.InitMat(sprs, ResourceLibrary.Inst.GlowIntensity_Bullet, true);
		base.transform.localScale = new Vector2(this.length, this.width);
		base.transform.position += dir.normalized * this.length / 2f;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x00031E29 File Offset: 0x00030029
	private IEnumerator DieAfter3Frames()
	{
		int num;
		for (int i = 0; i < 9; i = num + 1)
		{
			yield return 0;
			num = i;
		}
		Debug.LogError("Error_小激光残留、摧毁自己");
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000711 RID: 1809
	[Header("参数们")]
	[SerializeField]
	private SpriteRenderer spr_Body;

	// Token: 0x04000712 RID: 1810
	[SerializeField]
	private SpriteRenderer spr_Body_Outline;

	// Token: 0x04000713 RID: 1811
	[SerializeField]
	private float length;

	// Token: 0x04000714 RID: 1812
	[SerializeField]
	private float width = 1f;

	// Token: 0x04000715 RID: 1813
	[SerializeField]
	public double damage = 1.0;

	// Token: 0x04000716 RID: 1814
	[SerializeField]
	public bool ifCrit;

	// Token: 0x04000717 RID: 1815
	[Header("碰撞")]
	[SerializeField]
	private Collider2D laserCollider2D;

	// Token: 0x04000718 RID: 1816
	[SerializeField]
	private BloomMaterialControl bloom;
}
