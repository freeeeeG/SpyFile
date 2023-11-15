using System;
using Characters;
using UnityEngine;

namespace Level
{
	// Token: 0x02000499 RID: 1177
	public class ChangeMapLight : MonoBehaviour
	{
		// Token: 0x0600166C RID: 5740 RVA: 0x000466F8 File Offset: 0x000448F8
		private bool CheckPlayer(GameObject target)
		{
			Character component = target.GetComponent<Character>();
			return !(component == null) && component.type == Character.Type.Player;
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00046723 File Offset: 0x00044923
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!this.CheckPlayer(collision.gameObject))
			{
				return;
			}
			Map.Instance.ChangeLight(this._color, this._intensity, this._changingTime);
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00046750 File Offset: 0x00044950
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (!this.CheckPlayer(collision.gameObject))
			{
				return;
			}
			Map.Instance.RestoreLight(this._changingTime);
		}

		// Token: 0x040013AB RID: 5035
		[SerializeField]
		[Information("레이어를 Interaction으로 설정하고 트리거 콜라이더를 넣어주세요.", InformationAttribute.InformationType.Info, false)]
		private Color _color = Color.white;

		// Token: 0x040013AC RID: 5036
		[SerializeField]
		private float _intensity = 1f;

		// Token: 0x040013AD RID: 5037
		[SerializeField]
		private float _changingTime = 1f;
	}
}
