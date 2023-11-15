using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class Skill_Player8_Wave : MonoBehaviour
{
	// Token: 0x06000321 RID: 801 RVA: 0x000132A7 File Offset: 0x000114A7
	private void Awake()
	{
		base.gameObject.name = "BlastWave";
	}

	// Token: 0x06000322 RID: 802 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x06000323 RID: 803 RVA: 0x000132BC File Offset: 0x000114BC
	public void Init(float scale, Color color, double dmg, float critChance, float critEffect, EnumShapeType shapeType, bool ifFromEnemy = false, bool ifTriggerDoubleSword = true)
	{
		this.ifTriggerDoubleSword = ifTriggerDoubleSword;
		if (MyTool.DecimalToBool(critChance))
		{
			this.ifCrit = true;
			dmg *= (double)critEffect;
			scale *= Mathf.Sqrt(critEffect);
			color = color.SetValue(1f).SetSaturation(0.54f);
		}
		else
		{
			this.ifCrit = false;
		}
		scale = Mathf.Min(scale, this.maxScale);
		this.scale = scale;
		this.damage = dmg;
		if (!ifFromEnemy)
		{
			this.DetectColli_FromPlayerToEnemy();
		}
		else
		{
			this.DetectColli_FromEnemyToPlayer();
		}
		float num = color.Value();
		float num2 = color.Saturation();
		Color color2 = color.SetValue(num * this.facValue).SetSaturation(num2 * this.facSaturation).SetAlpha(this.facAlpha);
		this.part.Skill_Destroy_Init(shapeType, color2, scale);
		this.partSystem.Play();
		base.StartCoroutine(this.Fade());
	}

	// Token: 0x06000324 RID: 804 RVA: 0x0001339D File Offset: 0x0001159D
	private IEnumerator Fade()
	{
		yield return new WaitForSeconds(this.lifeTime);
		ObjectPool.inst.BlastWave_GoPool(base.gameObject);
		yield break;
	}

	// Token: 0x06000325 RID: 805 RVA: 0x000133AC File Offset: 0x000115AC
	private void DetectColli_FromPlayerToEnemy()
	{
		List<Enemy> list = new List<Enemy>();
		foreach (Enemy enemy in BattleManager.inst.listEnemies)
		{
			float lastScale = enemy.unit.lastScale;
			if ((enemy.transform.position - base.transform.position).magnitude - lastScale <= this.scale / 2f)
			{
				list.Add(enemy);
			}
		}
		int num = 0;
		while (list.Count > 0)
		{
			num++;
			if (num > 9000)
			{
				Debug.LogError("Error_死循环！");
				return;
			}
			Enemy enemy2 = list[0];
			enemy2.unit.GetHurt(this.damage, Player.inst.unit, Vector2.zero, this.ifCrit, enemy2.transform.position, this.ifTriggerDoubleSword);
			list.Remove(enemy2);
		}
	}

	// Token: 0x06000326 RID: 806 RVA: 0x000134C0 File Offset: 0x000116C0
	private void DetectColli_FromEnemyToPlayer()
	{
		Player inst = Player.inst;
		if (inst == null)
		{
			return;
		}
		float lastScale = inst.unit.lastScale;
		if ((inst.transform.position - base.transform.position).magnitude - lastScale <= this.scale / 2f)
		{
			float num = (float)(TempData.inst.diffiOptFlag[8] ? 3 : 1);
			inst.unit.GetHurt((double)num, null, Vector2.zero, false, inst.transform.position, this.ifTriggerDoubleSword);
		}
	}

	// Token: 0x040002D1 RID: 721
	[SerializeField]
	[CustomLabel("存在时间")]
	private float lifeTime = 0.5f;

	// Token: 0x040002D2 RID: 722
	[SerializeField]
	[Range(0f, 1f)]
	private float facValue = 1f;

	// Token: 0x040002D3 RID: 723
	[SerializeField]
	[Range(0f, 1f)]
	private float facSaturation = 1f;

	// Token: 0x040002D4 RID: 724
	[SerializeField]
	[Range(0f, 1f)]
	private float facAlpha = 1f;

	// Token: 0x040002D5 RID: 725
	[SerializeField]
	private Particle part;

	// Token: 0x040002D6 RID: 726
	[SerializeField]
	private ParticleSystem partSystem;

	// Token: 0x040002D7 RID: 727
	[SerializeField]
	public double damage;

	// Token: 0x040002D8 RID: 728
	[SerializeField]
	public bool ifCrit;

	// Token: 0x040002D9 RID: 729
	[SerializeField]
	public float maxScale = 33f;

	// Token: 0x040002DA RID: 730
	[SerializeField]
	private float scale;

	// Token: 0x040002DB RID: 731
	[SerializeField]
	private bool ifTriggerDoubleSword = true;
}
