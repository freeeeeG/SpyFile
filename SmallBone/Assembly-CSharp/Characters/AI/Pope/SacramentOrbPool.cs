using System;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011FB RID: 4603
	public class SacramentOrbPool : MonoBehaviour
	{
		// Token: 0x06005A5E RID: 23134 RVA: 0x0010C3C0 File Offset: 0x0010A5C0
		public void Initialize(Character character)
		{
			this._originPositions = new Vector3[this._width * this._height];
			for (int i = 0; i < this._height; i++)
			{
				for (int j = 0; j < this._width; j++)
				{
					SacramentOrb sacramentOrb = UnityEngine.Object.Instantiate<SacramentOrb>(this._orbPrefab, base.transform);
					sacramentOrb.transform.position = new Vector2(this._leftTop.position.x + this._distance * (float)j, this._leftTop.position.y + this._distance * (float)i);
					sacramentOrb.Initialize(character);
					sacramentOrb.gameObject.SetActive(false);
					this._originPositions[this._width * i + j] = sacramentOrb.transform.position;
				}
			}
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x0010C4A4 File Offset: 0x0010A6A4
		public void Run()
		{
			int num = 0;
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				transform.position = this._originPositions[num++];
				transform.Translate(UnityEngine.Random.insideUnitSphere * this._noise);
				transform.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x0010C530 File Offset: 0x0010A730
		public void Hide()
		{
			foreach (object obj in base.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
		}

		// Token: 0x040048F6 RID: 18678
		[SerializeField]
		private Transform _leftTop;

		// Token: 0x040048F7 RID: 18679
		[SerializeField]
		private SacramentOrb _orbPrefab;

		// Token: 0x040048F8 RID: 18680
		[Information("홀수", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private int _width = 5;

		// Token: 0x040048F9 RID: 18681
		[SerializeField]
		private int _height = 5;

		// Token: 0x040048FA RID: 18682
		[SerializeField]
		private float _distance = 5f;

		// Token: 0x040048FB RID: 18683
		[SerializeField]
		private float _noise = 2f;

		// Token: 0x040048FC RID: 18684
		private Vector3[] _originPositions;
	}
}
