using System;
using System.Collections;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FE0 RID: 4064
	public sealed class MultidimensionalPrism : CharacterOperation
	{
		// Token: 0x06004E8D RID: 20109 RVA: 0x000EB548 File Offset: 0x000E9748
		public override void Run(Character owner)
		{
			Character character;
			if (this._targetLayer == 512)
			{
				character = Singleton<Service>.Instance.levelManager.player;
			}
			else
			{
				character = TargetFinder.FindClosestTarget(this._center.position, this._detectRange.value, this._targetLayer);
			}
			float num = (float)UnityEngine.Random.Range(0, this._maxAngle);
			if (character != null)
			{
				Vector2 vector = character.transform.position - this._center.position;
				num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			}
			int num2 = UnityEngine.Random.Range(this._laserCountRange.x, this._laserCountRange.y);
			int num3 = this._maxAngle / num2;
			float num4 = num;
			Transform[] laser = this._laser;
			for (int i = 0; i < laser.Length; i++)
			{
				laser[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < num2; j++)
			{
				this._effectInfo.Spawn(this._center.position, owner, num4, 1f);
				this._laser[j].gameObject.SetActive(true);
				this._laser[j].rotation = Quaternion.Euler(0f, 0f, num4);
				num4 = num + (float)((j + 1) * num3) + (float)UnityEngine.Random.Range(-15, 15);
			}
			base.StartCoroutine(this.CGenerateCollider());
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x000EB6CB File Offset: 0x000E98CB
		private IEnumerator CGenerateCollider()
		{
			yield return null;
			this._collider.GenerateGeometry();
			yield break;
		}

		// Token: 0x04003E9E RID: 16030
		[Header("Laser 각도 설정")]
		[SerializeField]
		[MinMaxSlider(1f, 30f)]
		private Vector2Int _laserCountRange;

		// Token: 0x04003E9F RID: 16031
		[SerializeField]
		private Transform[] _laser;

		// Token: 0x04003EA0 RID: 16032
		[SerializeField]
		private LayerMask _targetLayer = 1024;

		// Token: 0x04003EA1 RID: 16033
		[SerializeField]
		private CustomFloat _detectRange;

		// Token: 0x04003EA2 RID: 16034
		[SerializeField]
		private Transform _center;

		// Token: 0x04003EA3 RID: 16035
		[SerializeField]
		private CompositeCollider2D _collider;

		// Token: 0x04003EA4 RID: 16036
		[Range(0f, 360f)]
		[SerializeField]
		private int _maxAngle;

		// Token: 0x04003EA5 RID: 16037
		[Header("Laser 이펙트 설정")]
		[SerializeField]
		private EffectInfo _effectInfo;
	}
}
