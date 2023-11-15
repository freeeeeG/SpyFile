using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class Obj_PlacementEffect : MonoBehaviour
{
	// Token: 0x0600049A RID: 1178 RVA: 0x000127F7 File Offset: 0x000109F7
	public void Initialize(Transform target)
	{
		this.materialControl = base.gameObject.AddComponent<ObjectMaterialControl>();
		this.materialControl.Initialize(target);
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00012816 File Offset: 0x00010A16
	private void Update()
	{
		this.node_RangeRingScale.transform.position = this.node_RangeRingScale.transform.position.WithY(0.1f);
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00012842 File Offset: 0x00010A42
	public void SetStatus(Obj_PlacementEffect.eStatus status)
	{
		if (status == Obj_PlacementEffect.eStatus.ORIGINAL)
		{
			this.materialControl.SwitchMaterial(ObjectMaterialControl.eMaterialType.ORIGINAL);
		}
		else if (this.IsStatusAvaliable(status))
		{
			this.materialControl.SwitchMaterial(ObjectMaterialControl.eMaterialType.PLACEMENT_MODE);
		}
		else
		{
			this.materialControl.SwitchMaterial(ObjectMaterialControl.eMaterialType.DISABLED);
		}
		this.SetRingType(status);
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0001287F File Offset: 0x00010A7F
	private bool IsStatusAvaliable(Obj_PlacementEffect.eStatus status)
	{
		return status == Obj_PlacementEffect.eStatus.AVALIABLE;
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00012885 File Offset: 0x00010A85
	public void SetRingRange(float range)
	{
		this.node_RangeRingScale.localScale = Vector3.one * range * 2f;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x000128A8 File Offset: 0x00010AA8
	public void SetRingType(Obj_PlacementEffect.eStatus status)
	{
		bool flag = this.IsStatusAvaliable(status);
		this.spin.rotationsPerSecond = (flag ? this.ringSpinSpeed_Fast : this.ringSpinSpeed_Slow);
		this.node_DottedRing_Blue.gameObject.SetActive(flag);
		this.node_DottedRing_Red.gameObject.SetActive(!flag);
	}

	// Token: 0x04000476 RID: 1142
	[SerializeField]
	private Transform node_RangeRingScale;

	// Token: 0x04000477 RID: 1143
	[SerializeField]
	private Transform node_DottedRing_Blue;

	// Token: 0x04000478 RID: 1144
	[SerializeField]
	private Transform node_DottedRing_Red;

	// Token: 0x04000479 RID: 1145
	[SerializeField]
	private Spin spin;

	// Token: 0x0400047A RID: 1146
	[SerializeField]
	private Vector3 ringSpinSpeed_Fast;

	// Token: 0x0400047B RID: 1147
	[SerializeField]
	private Vector3 ringSpinSpeed_Slow;

	// Token: 0x0400047C RID: 1148
	[SerializeField]
	[Header("材質控制功能")]
	private ObjectMaterialControl materialControl;

	// Token: 0x02000234 RID: 564
	public enum eStatus
	{
		// Token: 0x04000AF5 RID: 2805
		ORIGINAL,
		// Token: 0x04000AF6 RID: 2806
		AVALIABLE,
		// Token: 0x04000AF7 RID: 2807
		BLOCKED_PATH,
		// Token: 0x04000AF8 RID: 2808
		OVERLAP_OBJECT,
		// Token: 0x04000AF9 RID: 2809
		NO_SUITABLE_BASE,
		// Token: 0x04000AFA RID: 2810
		DISABLED,
		// Token: 0x04000AFB RID: 2811
		NO_GROUND
	}
}
