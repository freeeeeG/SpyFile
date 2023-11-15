using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200130C RID: 4876
	public class Fly : Move
	{
		// Token: 0x06006061 RID: 24673 RVA: 0x0011A26B File Offset: 0x0011846B
		private void Awake()
		{
			this._flyableZone = this._flyableZoneCollider.bounds;
		}

		// Token: 0x06006062 RID: 24674 RVA: 0x0011A27E File Offset: 0x0011847E
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			base.result = Behaviour.Result.Doing;
			base.StartCoroutine(base.CExpire(controller, this._duration));
			if (this.wander)
			{
				this.direction = UnityEngine.Random.insideUnitSphere;
			}
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				character.movement.move = this.direction;
				if (Mathf.Abs(this._flyableZone.min.x - character.transform.position.x) < 1f && this.direction.x < 0f)
				{
					this.direction.Set(-this.direction.x, this.direction.y);
				}
				if (Mathf.Abs(this._flyableZone.max.x - character.transform.position.x) < 1f && this.direction.x > 0f)
				{
					this.direction.Set(-this.direction.x, this.direction.y);
				}
				if (Mathf.Abs(this._flyableZone.min.y - character.transform.position.y) < 1f && this.direction.y < 0f)
				{
					this.direction.Set(this.direction.x, -this.direction.y);
				}
				if (Mathf.Abs(this._flyableZone.max.y - character.transform.position.y) < 1f && this.direction.y > 0f)
				{
					this.direction.Set(this.direction.x, -this.direction.y);
				}
			}
			this.idle.CRun(controller);
			yield break;
		}

		// Token: 0x04004DA0 RID: 19872
		[MinMaxSlider(0f, 10f)]
		[SerializeField]
		private Vector2 _duration;

		// Token: 0x04004DA1 RID: 19873
		[SerializeField]
		private Collider2D _flyableZoneCollider;

		// Token: 0x04004DA2 RID: 19874
		private Bounds _flyableZone;
	}
}
