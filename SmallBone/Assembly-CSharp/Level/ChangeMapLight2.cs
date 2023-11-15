using System;
using Characters;
using UnityEngine;

namespace Level
{
	// Token: 0x0200049A RID: 1178
	public class ChangeMapLight2 : MonoBehaviour
	{
		// Token: 0x06001670 RID: 5744 RVA: 0x0004679C File Offset: 0x0004499C
		private void OnTriggerEnter2D(Collider2D collision)
		{
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (component.type != Character.Type.Player)
			{
				return;
			}
			Map.Instance.ChangeLight(this._color, this._intensity, this._changingTime);
		}

		// Token: 0x040013AE RID: 5038
		[Information("레이어를 Interaction으로 설정하고 트리거 콜라이더를 넣어주세요.", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Color _color = Color.white;

		// Token: 0x040013AF RID: 5039
		[SerializeField]
		private float _intensity = 1f;

		// Token: 0x040013B0 RID: 5040
		[SerializeField]
		private float _changingTime = 1f;
	}
}
