using System;
using System.Collections;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B7 RID: 439
	public class LineThunderAttackTowadsCursorAction : Action
	{
		// Token: 0x06000A13 RID: 2579 RVA: 0x0002792A File Offset: 0x00025B2A
		public override void Init()
		{
			this.TG = ThunderGenerator.SharedInstance;
			this.SC = ShootingCursor.Instance;
			this.player = PlayerController.Instance;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00027950 File Offset: 0x00025B50
		public override void Activate(GameObject target)
		{
			Vector2 normalized = this.GetDirectionToCursor().normalized;
			Vector2 startPos = this.player.transform.position + normalized * 0.5f;
			this.player.StartCoroutine(this.ThunderAttackCR(normalized, startPos));
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x000279A8 File Offset: 0x00025BA8
		private Vector2 GetDirectionToCursor()
		{
			Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
			Vector2 b = this.player.transform.position;
			return a - b;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x000279F0 File Offset: 0x00025BF0
		private IEnumerator ThunderAttackCR(Vector2 direction, Vector2 startPos)
		{
			int numStrikes = 10;
			float distancePerStrike = 0.8f;
			int num;
			for (int i = 0; i < numStrikes; i = num + 1)
			{
				Vector2 v = startPos + direction * distancePerStrike * (float)i;
				this.TG.GenerateAt(v, 22);
				Vector2 v2 = startPos + direction.Rotate(15f) * distancePerStrike * (float)i;
				this.TG.GenerateAt(v2, 22);
				Vector2 v3 = startPos + direction.Rotate(-15f) * distancePerStrike * (float)i;
				this.TG.GenerateAt(v3, 22);
				yield return new WaitForSeconds(0.03f);
				num = i;
			}
			yield break;
		}

		// Token: 0x0400071A RID: 1818
		[NonSerialized]
		private ThunderGenerator TG;

		// Token: 0x0400071B RID: 1819
		[NonSerialized]
		private ShootingCursor SC;

		// Token: 0x0400071C RID: 1820
		[NonSerialized]
		private PlayerController player;
	}
}
