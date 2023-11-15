using System;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x0200125F RID: 4703
	public class DualFinish : MonoBehaviour
	{
		// Token: 0x06005D3A RID: 23866 RVA: 0x00112685 File Offset: 0x00110885
		public void OnEnable()
		{
			this.SetPosition();
			this.SetRotation();
		}

		// Token: 0x06005D3B RID: 23867 RVA: 0x00112694 File Offset: 0x00110894
		private void SetPosition()
		{
			Vector3 translation = UnityEngine.Random.insideUnitSphere * this._noise;
			if (MMMaths.RandomBool())
			{
				this._clockWise.transform.Translate(translation);
				this._counterClockWise.transform.localPosition = Vector2.zero;
				return;
			}
			this._clockWise.transform.localPosition = Vector2.zero;
			this._counterClockWise.transform.Translate(translation);
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x00112710 File Offset: 0x00110910
		private void SetRotation()
		{
			this._clockWise.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(this._angleRange.x, this._angleRange.y));
			this._counterClockWise.transform.rotation = Quaternion.Euler(0f, 0f, 180f - UnityEngine.Random.Range(this._angleRange.x, this._angleRange.y));
		}

		// Token: 0x04004AD8 RID: 19160
		[SerializeField]
		[Header("Position")]
		private float _noise = 2f;

		// Token: 0x04004AD9 RID: 19161
		[Header("Rotation")]
		[SerializeField]
		[MinMaxSlider(0f, 90f)]
		private Vector2 _angleRange;

		// Token: 0x04004ADA RID: 19162
		[SerializeField]
		private GameObject _clockWise;

		// Token: 0x04004ADB RID: 19163
		[SerializeField]
		private GameObject _counterClockWise;
	}
}
