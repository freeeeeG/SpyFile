using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations.Attack;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.TwinSister
{
	// Token: 0x0200119B RID: 4507
	public class RisingPierce : MonoBehaviour
	{
		// Token: 0x06005884 RID: 22660 RVA: 0x00107F85 File Offset: 0x00106185
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x00107F90 File Offset: 0x00106190
		private void Initialize()
		{
			int y = this._countRange.y;
			this._cachedDistance = new float[y];
			this._sweepAttack.Initialize();
			for (int i = 0; i < y; i++)
			{
				GameObject gameObject = new GameObject();
				BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
				boxCollider2D.size = new Vector2(this._rangePartsOrigin.bounds.size.x, this._rangePartsOrigin.bounds.size.y);
				boxCollider2D.offset = new Vector2(this._rangePartsOrigin.offset.x, this._rangePartsOrigin.offset.y);
				boxCollider2D.usedByComposite = true;
				gameObject.transform.SetParent(this._range.transform);
				gameObject.SetActive(false);
			}
		}

		// Token: 0x06005886 RID: 22662 RVA: 0x00108068 File Offset: 0x00106268
		private void MakeAttackRange(float startX, float y, int count)
		{
			float x = this._rangePartsOrigin.bounds.size.x;
			float x2 = this._rangePartsOrigin.bounds.extents.x;
			for (int i = 0; i < count; i++)
			{
				Component child = this._range.transform.GetChild(i);
				float x3 = startX + (x * (float)i + x2) + (float)i * this._cachedDistance[i];
				child.transform.position = new Vector2(x3, y);
			}
			this._range.GenerateGeometry();
		}

		// Token: 0x06005887 RID: 22663 RVA: 0x001080FC File Offset: 0x001062FC
		private float GetStartPointX(Character character)
		{
			float x = character.movement.controller.collisionState.lastStandingCollider.bounds.min.x;
			float num = UnityEngine.Random.Range(this._startNoise.x, this._startNoise.y);
			return x + num;
		}

		// Token: 0x06005888 RID: 22664 RVA: 0x0010814E File Offset: 0x0010634E
		public IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Bounds platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			float startX = this.GetStartPointX(character);
			float sizeX = this._rangePartsOrigin.bounds.size.x;
			float extentsX = this._rangePartsOrigin.bounds.extents.x;
			int count = UnityEngine.Random.Range(this._countRange.x, this._countRange.y);
			this._ready.TryStart();
			while (this._ready.running)
			{
				yield return null;
			}
			this._motion.TryStart();
			for (int i = 0; i < count; i++)
			{
				float num = UnityEngine.Random.Range(this._distanceNoise.x, this._distanceNoise.y);
				this._cachedDistance[i] = num;
				float x = startX + (sizeX * (float)i + extentsX) + (float)i * num;
				this._rangePartsOrigin.transform.position = new Vector3(x, platformBounds.max.y);
				this._spawnAttackSign.Run(character);
			}
			this.MakeAttackRange(startX, platformBounds.max.y, count);
			yield return character.chronometer.master.WaitForSeconds(this._attackDelay);
			this._sweepAttack.Run(character);
			for (int j = 0; j < count; j++)
			{
				float num2 = this._cachedDistance[j];
				float x2 = startX + (sizeX * (float)j + extentsX) + (float)j * num2;
				this._rangePartsOrigin.transform.position = new Vector3(x2, platformBounds.max.y);
				this._sweepAttackEffect.Run(character);
			}
			yield break;
		}

		// Token: 0x0400476A RID: 18282
		[SerializeField]
		private Characters.Actions.Action _ready;

		// Token: 0x0400476B RID: 18283
		[SerializeField]
		private Characters.Actions.Action _motion;

		// Token: 0x0400476C RID: 18284
		[SerializeField]
		private Collider2D _rangePartsOrigin;

		// Token: 0x0400476D RID: 18285
		[Subcomponent(typeof(SpawnEffect))]
		[SerializeField]
		private SpawnEffect _spawnAttackSign;

		// Token: 0x0400476E RID: 18286
		[Subcomponent(typeof(SweepAttack2))]
		[SerializeField]
		private SweepAttack2 _sweepAttack;

		// Token: 0x0400476F RID: 18287
		[SerializeField]
		[Subcomponent(typeof(SpawnEffect))]
		private SpawnEffect _sweepAttackEffect;

		// Token: 0x04004770 RID: 18288
		[SerializeField]
		private float _attackDelay = 1f;

		// Token: 0x04004771 RID: 18289
		[SerializeField]
		private CompositeCollider2D _range;

		// Token: 0x04004772 RID: 18290
		[SerializeField]
		[MinMaxSlider(0f, 5f)]
		private Vector2 _startNoise;

		// Token: 0x04004773 RID: 18291
		[SerializeField]
		[MinMaxSlider(0f, 5f)]
		private Vector2 _distanceNoise;

		// Token: 0x04004774 RID: 18292
		[MinMaxSlider(6f, 8f)]
		[SerializeField]
		private Vector2Int _countRange;

		// Token: 0x04004775 RID: 18293
		private float[] _cachedDistance;
	}
}
