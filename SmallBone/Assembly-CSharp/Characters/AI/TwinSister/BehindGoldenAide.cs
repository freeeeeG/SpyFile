using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.TwinSister
{
	// Token: 0x0200114E RID: 4430
	public class BehindGoldenAide : MonoBehaviour
	{
		// Token: 0x0600568B RID: 22155 RVA: 0x00002191 File Offset: 0x00000391
		private void Start()
		{
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x0010131C File Offset: 0x000FF51C
		public IEnumerator CIntroOut()
		{
			this.Show(this._introSource.position);
			this._introOut.TryStart();
			while (this._introOut.running)
			{
				yield return null;
			}
			this.Hide();
			yield break;
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x0010132B File Offset: 0x000FF52B
		public IEnumerator CIn()
		{
			this.Show(this._inStart.position);
			this._in.TryStart();
			Vector2 v = this._character.transform.position;
			Vector2 v2 = this._inDest.position;
			yield return this.MoveToDestination(v, v2, this._in, this._inDuration);
			yield break;
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x0010133A File Offset: 0x000FF53A
		public IEnumerator COut()
		{
			this._out.TryStart();
			Vector2 v = this._character.transform.position;
			Vector2 v2 = this._inStart.position;
			yield return this.MoveToDestination(v, v2, this._out, this._inDuration);
			this.Hide();
			yield break;
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x00101349 File Offset: 0x000FF549
		public void Hide()
		{
			this._character.@base.gameObject.SetActive(false);
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x00101361 File Offset: 0x000FF561
		private void Show(Vector3 startPoint)
		{
			this._character.transform.position = startPoint;
			this._character.@base.gameObject.SetActive(true);
		}

		// Token: 0x06005691 RID: 22161 RVA: 0x0010138A File Offset: 0x000FF58A
		private IEnumerator MoveToDestination(Vector3 source, Vector3 dest, Characters.Actions.Action action, float duration)
		{
			float elapsed = 0f;
			Character.LookingDirection direction = this._character.lookingDirection;
			while (action.running)
			{
				yield return null;
				Vector2 v = Vector2.Lerp(source, dest, elapsed / duration);
				this._character.transform.position = v;
				elapsed += this._character.chronometer.master.deltaTime;
				if (elapsed > duration)
				{
					this._character.CancelAction();
					break;
				}
				if ((source - dest).magnitude < 0.1f)
				{
					this._character.CancelAction();
					break;
				}
			}
			this._character.transform.position = dest;
			this._character.lookingDirection = direction;
			yield break;
		}

		// Token: 0x04004590 RID: 17808
		[SerializeField]
		private Character _character;

		// Token: 0x04004591 RID: 17809
		[Space]
		[SerializeField]
		[Header("Intro")]
		private Characters.Actions.Action _introOut;

		// Token: 0x04004592 RID: 17810
		[SerializeField]
		private Transform _introSource;

		// Token: 0x04004593 RID: 17811
		[SerializeField]
		private Transform _introDest;

		// Token: 0x04004594 RID: 17812
		[Header("InGame")]
		[SerializeField]
		[Space]
		private Characters.Actions.Action _in;

		// Token: 0x04004595 RID: 17813
		[SerializeField]
		private Transform _inStart;

		// Token: 0x04004596 RID: 17814
		[SerializeField]
		private Transform _inDest;

		// Token: 0x04004597 RID: 17815
		[SerializeField]
		private float _inDuration;

		// Token: 0x04004598 RID: 17816
		[SerializeField]
		private Characters.Actions.Action _out;
	}
}
