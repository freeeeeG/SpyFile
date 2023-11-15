using System;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DF8 RID: 3576
	public class SetEmbraceLaser : CharacterOperation
	{
		// Token: 0x06004792 RID: 18322 RVA: 0x000CFDB0 File Offset: 0x000CDFB0
		private void Awake()
		{
			this._signs = new LineEffect[this._signContainer.childCount];
			this._lasers = new LineEffect[this._laserContainer.childCount];
			for (int i = 0; i < this._signContainer.childCount; i++)
			{
				this._signs[i] = this._signContainer.GetChild(i).GetComponent<LineEffect>();
				this._lasers[i] = this._laserContainer.GetChild(i).GetComponent<LineEffect>();
			}
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x000CFE34 File Offset: 0x000CE034
		public override void Run(Character owner)
		{
			float num = (float)UnityEngine.Random.Range(0, 360);
			if (this._lasers == null || this._lasers.Length == 0)
			{
				Debug.LogError("LineEffects is null or length is lower than or equals zero");
				return;
			}
			this._signs[0].transform.rotation = Quaternion.AngleAxis(num, Vector3.forward);
			this._lasers[0].transform.rotation = Quaternion.AngleAxis(num, Vector3.forward);
			this._lasers[0].gameObject.SetActive(true);
			float num2 = (float)(360 / this._lasers.Length);
			for (int i = 1; i < this._lasers.Length; i++)
			{
				float num3 = UnityEngine.Random.Range(this._radianRange.x, this._radianRange.y);
				this._signs[i].transform.rotation = Quaternion.AngleAxis(num + num3, Vector3.forward);
				this._lasers[i].transform.rotation = Quaternion.AngleAxis(num + num3, Vector3.forward);
				num += num2;
				this._lasers[i].gameObject.SetActive(true);
			}
			this._range.GenerateGeometry();
			LineEffect[] lasers = this._lasers;
			for (int j = 0; j < lasers.Length; j++)
			{
				lasers[j].gameObject.SetActive(false);
			}
		}

		// Token: 0x04003692 RID: 13970
		[MinMaxSlider(0f, 180f)]
		[SerializeField]
		private Vector2 _radianRange;

		// Token: 0x04003693 RID: 13971
		[SerializeField]
		private Transform _signContainer;

		// Token: 0x04003694 RID: 13972
		[SerializeField]
		private Transform _laserContainer;

		// Token: 0x04003695 RID: 13973
		[SerializeField]
		private CompositeCollider2D _range;

		// Token: 0x04003696 RID: 13974
		private LineEffect[] _signs;

		// Token: 0x04003697 RID: 13975
		private LineEffect[] _lasers;
	}
}
