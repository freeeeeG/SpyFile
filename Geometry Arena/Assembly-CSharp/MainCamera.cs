using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class MainCamera : MonoBehaviour
{
	// Token: 0x06000478 RID: 1144 RVA: 0x0001C07F File Offset: 0x0001A27F
	private void Awake()
	{
		MainCamera.inst = this;
		if (this.camShake_Trans == null)
		{
			Debug.LogError("camShake_Trans null!");
		}
		if (this.camera_Comp == null)
		{
			Debug.LogError("cameraComp null!");
		}
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0001C0B7 File Offset: 0x0001A2B7
	public void CameraMove_RefVeloReset()
	{
		this.camMove_RefVelo = Vector2.zero;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0001C0C4 File Offset: 0x0001A2C4
	private void FixedUpdate()
	{
		this.CameraShake_RecouverInFixedUpdate();
		this.CameraScaling();
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0001C0D4 File Offset: 0x0001A2D4
	private void CameraMove_PosFollowPlayer()
	{
		if (Player.inst == null)
		{
			Debug.LogWarning("Player Null!");
			return;
		}
		Player player = Player.inst;
		Vector2 physics_Speed = player.unit.physics_Speed;
		Vector2 current = base.gameObject.transform.position;
		Vector2 target = player.gameObject.transform.position + physics_Speed / this.camMove_SpeedPredict;
		base.transform.position = Vector2.SmoothDamp(current, target, ref this.camMove_RefVelo, this.camMove_SmoothTime);
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0001C16C File Offset: 0x0001A36C
	private void CameraScaling()
	{
		if (Player.inst == null)
		{
			return;
		}
		float target = this.scaling_TargetSizeSingleSize * GameParameters.Inst.WorldSize;
		float orthographicSize = Mathf.SmoothDamp(this.camera_Comp.orthographicSize, target, ref this.scaling_RefVelo, this.scaling_SmoothTime);
		this.camera_Comp.orthographicSize = orthographicSize;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0001C1C3 File Offset: 0x0001A3C3
	public void CameraShake_ShakeOnce(float strenthFactor)
	{
		Debug.Log("震屏一次");
		base.StartCoroutine(this.CameraShake_Coroutine(this.camShake_TimeTotal, strenthFactor * this.camShake_Strenth_StdInitValue));
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0001C1EA File Offset: 0x0001A3EA
	private IEnumerator CameraShake_Coroutine(float duration, float strength)
	{
		this.camShake_CurCount += 1f;
		Vector3 oldPos = this.camShake_Trans.localPosition;
		while (duration > 0f)
		{
			Vector3 target = oldPos + Random.insideUnitSphere * strength;
			this.camShake_Trans.localPosition = Vector3.SmoothDamp(oldPos, target, ref this.camShake_RefVelo, this.camShake_Speed);
			duration -= Time.deltaTime;
			strength = Mathf.SmoothDamp(strength, 0f, ref this.camShake_Strenth_RefVelo, this.camShake_Strenth_DecaySpeed);
			yield return new WaitForEndOfFrame();
		}
		this.camShake_CurCount -= 1f;
		yield break;
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0001C208 File Offset: 0x0001A408
	private void CameraShake_RecouverInFixedUpdate()
	{
		if (this.camShake_CurCount > 0f)
		{
			return;
		}
		this.camShake_Trans.localPosition = Vector3.SmoothDamp(this.camShake_Trans.localPosition, Vector3.zero, ref this.recover_RefVelo, this.recover_SmoothTime);
		if (this.camShake_Trans.localPosition.magnitude < this.recover_Threshold)
		{
			this.camShake_Trans.localPosition = Vector3.zero;
		}
	}

	// Token: 0x040003CA RID: 970
	public static MainCamera inst;

	// Token: 0x040003CB RID: 971
	[SerializeField]
	private Camera camera_Comp;

	// Token: 0x040003CC RID: 972
	[SerializeField]
	private Transform camShake_Trans;

	// Token: 0x040003CD RID: 973
	[Header("CameraMove")]
	private Vector2 camMove_RefVelo = Vector2.zero;

	// Token: 0x040003CE RID: 974
	[SerializeField]
	[Tooltip("相机移动速度，越小越快")]
	private float camMove_SmoothTime = 0.85f;

	// Token: 0x040003CF RID: 975
	[SerializeField]
	private float camMove_SpeedPredict = 1.75f;

	// Token: 0x040003D0 RID: 976
	[Header("CameraShake")]
	[SerializeField]
	private float camShake_TimeTotal = 0.3f;

	// Token: 0x040003D1 RID: 977
	[SerializeField]
	private float camShake_Speed = 0.05f;

	// Token: 0x040003D2 RID: 978
	[SerializeField]
	private float camShake_Strenth_StdInitValue = 1f;

	// Token: 0x040003D3 RID: 979
	[SerializeField]
	private float camShake_Strenth_DecaySpeed = 0.7f;

	// Token: 0x040003D4 RID: 980
	[SerializeField]
	private float camShake_CurCount;

	// Token: 0x040003D5 RID: 981
	private float camShake_Strenth_RefVelo;

	// Token: 0x040003D6 RID: 982
	private Vector3 camShake_RefVelo = Vector3.zero;

	// Token: 0x040003D7 RID: 983
	[Header("CameraScaling")]
	[SerializeField]
	private float scaling_SmoothTime = 0.2f;

	// Token: 0x040003D8 RID: 984
	private float scaling_RefVelo;

	// Token: 0x040003D9 RID: 985
	[SerializeField]
	private float scaling_TargetSizeSingleSize = 16f;

	// Token: 0x040003DA RID: 986
	[Header("Recover")]
	[SerializeField]
	private float recover_SmoothTime = 0.3f;

	// Token: 0x040003DB RID: 987
	private Vector3 recover_RefVelo = Vector3.zero;

	// Token: 0x040003DC RID: 988
	[SerializeField]
	[Tooltip("强制清零的阈值")]
	private float recover_Threshold = 0.1f;
}
