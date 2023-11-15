using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class TargetDrone : Drone
{
	// Token: 0x0600030D RID: 781 RVA: 0x00012EEF File Offset: 0x000110EF
	private void Awake()
	{
		TargetDrone.inst = this;
		this.UpdateTargetPos();
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00012F00 File Offset: 0x00011100
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.DetectPos_Target();
		this.DetectPos_Wall();
		float z = Time.fixedTime * this.rotateSpd % 360f;
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, z));
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00012F52 File Offset: 0x00011152
	public void UpdateTargetPos()
	{
		this.targetPos = BattleManager.ChooseBattleItemGenePosInScene();
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00012F60 File Offset: 0x00011160
	private void DetectPos_Target()
	{
		if ((this.targetPos - base.transform.position).magnitude <= 1f)
		{
			this.UpdateTargetPos();
		}
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00012FA0 File Offset: 0x000111A0
	private void DetectPos_Wall()
	{
		float num = BattleManager.inst.sceneRadiusSingleSize * GameParameters.Inst.WorldSize * SceneObj.inst.SceneSize - 1f;
		Vector2 vector = base.transform.position;
		if (Mathf.Abs(vector.x) >= num || Mathf.Abs(vector.y) >= num)
		{
			this.UpdateTargetPos();
		}
	}

	// Token: 0x040002C7 RID: 711
	public static TargetDrone inst;

	// Token: 0x040002C8 RID: 712
	[SerializeField]
	private float rotateSpd = 30f;
}
