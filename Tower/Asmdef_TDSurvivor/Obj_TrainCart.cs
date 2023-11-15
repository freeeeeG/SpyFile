using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
[SelectionBase]
public class Obj_TrainCart : MonoBehaviour, IDynamicPlacementTarget
{
	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060004DB RID: 1243 RVA: 0x000137C5 File Offset: 0x000119C5
	public bool IsAnimPlaying
	{
		get
		{
			return this.isAnimPlaying;
		}
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x000137CD File Offset: 0x000119CD
	private void Start()
	{
		this.lastFramePosition = base.transform.position;
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x000137E0 File Offset: 0x000119E0
	private void Update()
	{
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x000137E2 File Offset: 0x000119E2
	private void OnMouseEnter()
	{
		if (this.attachedTower != null)
		{
			return;
		}
		EventMgr.SendEvent<IDynamicPlacementTarget>(eGameEvents.RegisterDynamicPlacementObject, this);
		EventMgr.SendEvent<Renderer, OutlineController.eOutlineType>(eGameEvents.RequestAddOutline, this.renderer_Cart, OutlineController.eOutlineType.BASIC);
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00013818 File Offset: 0x00011A18
	private void OnMouseExit()
	{
		if (this.attachedTower != null)
		{
			return;
		}
		EventMgr.SendEvent<IDynamicPlacementTarget>(eGameEvents.UnregisterDynamicPlacementObject, this);
		EventMgr.SendEvent<Renderer>(eGameEvents.RequestRemoveOutline, this.renderer_Cart);
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0001384D File Offset: 0x00011A4D
	public Transform GetPlacementTransform()
	{
		return this.node_PlacementPosition;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00013858 File Offset: 0x00011A58
	public void UpdateCartDirection()
	{
		if (base.transform.position != this.lastFramePosition)
		{
			Vector3 b = base.transform.position - this.lastFramePosition;
			base.transform.forward = Vector3.Lerp(base.transform.forward, b, Time.deltaTime * this.forwardDirectionLerpSpeed);
			this.lastFramePosition = base.transform.position;
		}
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x000138D0 File Offset: 0x00011AD0
	public void PlaceTower(ABaseTower tower)
	{
		if (this.attachedTower != null)
		{
			Debug.LogError("試圖在已經有砲塔的IDynamicPlacementTarget上放置物件!!");
			return;
		}
		EventMgr.SendEvent<IDynamicPlacementTarget>(eGameEvents.UnregisterDynamicPlacementObject, this);
		EventMgr.SendEvent<Renderer>(eGameEvents.RequestRemoveOutline, this.renderer_Cart);
		this.attachedTower = tower;
		if (this.buffType != Obj_TrainCart.eTrainCartBuffType.NONE)
		{
			this.ApplyEffectToTower(tower);
		}
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00013930 File Offset: 0x00011B30
	public void ToggleCartAnimation(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
		this.isAnimPlaying = isOn;
		if (isOn)
		{
			this.particle_TrackFlare.Play();
			return;
		}
		this.particle_TrackFlare.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00013966 File Offset: 0x00011B66
	protected void ApplyEffectToTower(ABaseTower tower)
	{
		this.particle_ApplyBuff.Play();
		SoundManager.PlaySound("Block", "BuffGrid_TowerOn", -1f, -1f, -1f);
		tower.SettingData.AddBuffMultiplier(this.buffStat);
	}

	// Token: 0x040004A8 RID: 1192
	[SerializeField]
	private Obj_TrainCart.eTrainCartBuffType buffType;

	// Token: 0x040004A9 RID: 1193
	[SerializeField]
	private Animator animator;

	// Token: 0x040004AA RID: 1194
	[SerializeField]
	private ParticleSystem particle_TrackFlare;

	// Token: 0x040004AB RID: 1195
	[SerializeField]
	private Renderer renderer_Cart;

	// Token: 0x040004AC RID: 1196
	[SerializeField]
	private Transform node_PlacementPosition;

	// Token: 0x040004AD RID: 1197
	[SerializeField]
	private float forwardDirectionLerpSpeed = 10f;

	// Token: 0x040004AE RID: 1198
	[SerializeField]
	private ABaseTower attachedTower;

	// Token: 0x040004AF RID: 1199
	[SerializeField]
	private TowerStats buffStat;

	// Token: 0x040004B0 RID: 1200
	[SerializeField]
	private ParticleSystem particle_ApplyBuff;

	// Token: 0x040004B1 RID: 1201
	private Vector3 lastFramePosition;

	// Token: 0x040004B2 RID: 1202
	private bool isAnimPlaying;

	// Token: 0x0200023D RID: 573
	public enum eTrainCartBuffType
	{
		// Token: 0x04000B15 RID: 2837
		NONE,
		// Token: 0x04000B16 RID: 2838
		DAMAGE_UP,
		// Token: 0x04000B17 RID: 2839
		RANGE_UP,
		// Token: 0x04000B18 RID: 2840
		SHOOT_SPEED_UP
	}
}
