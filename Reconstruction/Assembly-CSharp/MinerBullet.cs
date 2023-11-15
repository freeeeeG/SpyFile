using System;
using UnityEngine;

// Token: 0x02000213 RID: 531
public class MinerBullet : GroundBullet
{
	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06000D36 RID: 3382 RVA: 0x0002220C File Offset: 0x0002040C
	// (set) Token: 0x06000D37 RID: 3383 RVA: 0x00022214 File Offset: 0x00020414
	public BasicTile TargetTile { get; set; }

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06000D38 RID: 3384 RVA: 0x0002221D File Offset: 0x0002041D
	// (set) Token: 0x06000D39 RID: 3385 RVA: 0x00022225 File Offset: 0x00020425
	public float AttackIncreasePerSecond { get; set; }

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06000D3A RID: 3386 RVA: 0x0002222E File Offset: 0x0002042E
	// (set) Token: 0x06000D3B RID: 3387 RVA: 0x00022236 File Offset: 0x00020436
	public float MaxAttackIncrease { get; set; }

	// Token: 0x06000D3C RID: 3388 RVA: 0x00022240 File Offset: 0x00020440
	public override void TriggerDamage()
	{
		Mine mine = Singleton<ObjectPool>.Instance.Spawn(this.mine) as Mine;
		mine.attackIncreasePerSecond = this.AttackIncreasePerSecond;
		mine.maxDmgIncreased = this.MaxAttackIncrease;
		mine.transform.position = this.TargetPos;
		mine.SetAtt(this);
		this.ReclaimBullet();
	}

	// Token: 0x04000668 RID: 1640
	[SerializeField]
	private Mine mine;
}
