using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000018 RID: 24
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/擊中時把怪物抓進來", order = 1)]
public class GrabMonsterOnHitBuff : ABaseBuffSettingData
{
	// Token: 0x0600006D RID: 109 RVA: 0x000031F4 File Offset: 0x000013F4
	protected override void ApplyEffect()
	{
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, "套用 攻擊暈眩 Buff", null);
		this.triggerTimer = this.triggerInterval;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0000320F File Offset: 0x0000140F
	protected override void RemoveEffect()
	{
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00003211 File Offset: 0x00001411
	protected override void TickProc(float delta)
	{
		if (this.canProc)
		{
			return;
		}
		this.triggerTimer -= delta;
		if (this.triggerTimer <= 0f)
		{
			this.canProc = true;
			this.triggerTimer += this.triggerInterval;
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00003251 File Offset: 0x00001451
	public override void OnTowerShoot(ABaseTower tower, AMonsterBase targetMonster)
	{
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00003254 File Offset: 0x00001454
	public override void OnTowerBulletHit(ABaseTower tower, AMonsterBase targetMonster, int shootIndex, int bulletIndex)
	{
		if (!this.canProc && shootIndex != this.lastShootIndex)
		{
			return;
		}
		List<AMonsterBase> monstersInRange = Singleton<MonsterManager>.Instance.GetMonstersInRange(targetMonster.transform.position, this.grabRadius);
		monstersInRange.Sort((AMonsterBase x, AMonsterBase y) => x.RemainingDistance.CompareTo(y.RemainingDistance));
		List<AMonsterBase> list = new List<AMonsterBase>();
		int num = 0;
		while (num < this.grabCount && num < monstersInRange.Count)
		{
			if (!(monstersInRange[num] == targetMonster) && Vector3.Magnitude(targetMonster.transform.position - monstersInRange[num].transform.position) >= 1f && monstersInRange[num].MonsterData.Size != eMonsterSize.BOSS)
			{
				list.Add(monstersInRange[num]);
			}
			num++;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.prefab_GrabEffect);
		Obj_GrabEffect component = gameObject.GetComponent<Obj_GrabEffect>();
		gameObject.transform.SetParent(targetMonster.transform);
		gameObject.transform.localPosition = Vector3.zero;
		component.GrabMonsters(list, this.grabRadius);
		this.lastShootIndex = shootIndex;
		this.canProc = false;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0000337E File Offset: 0x0000157E
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000073 RID: 115 RVA: 0x000033A8 File Offset: 0x000015A8
	public override string GetLocStatsString()
	{
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			(int)this.triggerInterval,
			(int)this.grabRadius,
			this.grabCount
		});
	}

	// Token: 0x04000052 RID: 82
	[SerializeField]
	private float grabRadius = 5f;

	// Token: 0x04000053 RID: 83
	[SerializeField]
	private int grabCount = 5;

	// Token: 0x04000054 RID: 84
	[SerializeField]
	private float triggerInterval = 2f;

	// Token: 0x04000055 RID: 85
	[SerializeField]
	private GameObject prefab_GrabEffect;

	// Token: 0x04000056 RID: 86
	private float triggerTimer;

	// Token: 0x04000057 RID: 87
	private bool canProc = true;

	// Token: 0x04000058 RID: 88
	private int lastShootIndex = -999;
}
