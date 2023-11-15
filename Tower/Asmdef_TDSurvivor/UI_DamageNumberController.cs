using System;
using Lean.Pool;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class UI_DamageNumberController : MonoBehaviour
{
	// Token: 0x0600091A RID: 2330 RVA: 0x000228E7 File Offset: 0x00020AE7
	private void OnEnable()
	{
		EventMgr.Register<Vector3, int, bool, eDamageType>(eGameEvents.UI_ShowDamageNumber, new Action<Vector3, int, bool, eDamageType>(this.OnShowDamageNumber));
		EventMgr.Register<Vector3, string>(eGameEvents.UI_ShowBuffApplyText, new Action<Vector3, string>(this.OnShowBuffApplyText));
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00022919 File Offset: 0x00020B19
	private void OnDisable()
	{
		EventMgr.Remove<Vector3, int, bool, eDamageType>(eGameEvents.UI_ShowDamageNumber, new Action<Vector3, int, bool, eDamageType>(this.OnShowDamageNumber));
		EventMgr.Remove<Vector3, string>(eGameEvents.UI_ShowBuffApplyText, new Action<Vector3, string>(this.OnShowBuffApplyText));
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0002294C File Offset: 0x00020B4C
	private void OnShowBuffApplyText(Vector3 worldPos, string content)
	{
		Vector3 vector = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(worldPos);
		vector = Singleton<CameraManager>.Instance.EnsureUIStaysInLRBorder(vector, this.prefab_Obj_BuffApplyText.Width);
		GameObject gameObject = LeanPool.Spawn(this.prefab_Obj_BuffApplyText.gameObject, vector, Quaternion.identity, base.transform);
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		gameObject.GetComponent<UI_BuffApplyText>().Trigger(worldPos, content);
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x000229B8 File Offset: 0x00020BB8
	private void OnShowDamageNumber(Vector3 worldPos, int value, bool isCrit, eDamageType damageType = eDamageType.NONE)
	{
		Vector3 vector = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(worldPos);
		vector = Singleton<CameraManager>.Instance.EnsureUIStaysInLRBorder(vector, this.prefab_Obj_Score.Width);
		GameObject gameObject = LeanPool.Spawn(this.prefab_Obj_Score.gameObject, vector, Quaternion.identity, base.transform);
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		gameObject.GetComponent<UI_DamageNumber>().Trigger(worldPos, value, isCrit, damageType);
	}

	// Token: 0x0400073C RID: 1852
	[SerializeField]
	private UI_DamageNumber prefab_Obj_Score;

	// Token: 0x0400073D RID: 1853
	[SerializeField]
	private UI_BuffApplyText prefab_Obj_BuffApplyText;
}
