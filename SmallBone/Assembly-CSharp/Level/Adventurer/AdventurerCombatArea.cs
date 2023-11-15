using System;
using Characters;
using Characters.AI.Adventurer;
using UnityEngine;

namespace Level.Adventurer
{
	// Token: 0x02000691 RID: 1681
	public class AdventurerCombatArea : MonoBehaviour
	{
		// Token: 0x0600219A RID: 8602 RVA: 0x00064F2E File Offset: 0x0006312E
		private void Awake()
		{
			this._enemyWave.onClear += this.DisableSideWall;
			this._leftWall.SetActive(false);
			this._rightWall.SetActive(true);
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x00064F60 File Offset: 0x00063160
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
			if (component.collider.bounds.min.x < this._startTrigger.bounds.min.x - 0.5f)
			{
				component.movement.force += new Vector2(1f, 0f);
			}
			this.EnableSideWall();
			if (this._commander != null)
			{
				this._commander.StartIntro();
			}
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x00065004 File Offset: 0x00063204
		private void EnableSideWall()
		{
			this._startTrigger.enabled = false;
			this._leftWall.SetActive(true);
			this._rightWall.SetActive(true);
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x0006502A File Offset: 0x0006322A
		public void DisableSideWall()
		{
			this._startTrigger.enabled = false;
			this._leftWall.SetActive(false);
			this._rightWall.SetActive(false);
		}

		// Token: 0x04001CAA RID: 7338
		[SerializeField]
		private Commander _commander;

		// Token: 0x04001CAB RID: 7339
		[SerializeField]
		private EnemyWave _enemyWave;

		// Token: 0x04001CAC RID: 7340
		[GetComponent]
		[SerializeField]
		private BoxCollider2D _startTrigger;

		// Token: 0x04001CAD RID: 7341
		[SerializeField]
		private GameObject _leftWall;

		// Token: 0x04001CAE RID: 7342
		[SerializeField]
		private GameObject _rightWall;
	}
}
