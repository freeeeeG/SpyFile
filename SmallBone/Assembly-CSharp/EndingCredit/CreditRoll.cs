using System;
using System.Collections;
using Services;
using Singletons;
using UI.Pause;
using UnityEngine;

namespace EndingCredit
{
	// Token: 0x02000196 RID: 406
	public class CreditRoll : MonoBehaviour
	{
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00019108 File Offset: 0x00017308
		public bool active
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00019115 File Offset: 0x00017315
		public IEnumerator CRun(bool resetGameScene = true)
		{
			this._resetGameScene = resetGameScene;
			this._pauseEventSystem.PushEvent(this._pauseEvent);
			Vector3 vector = this._destination.transform.position - this._lastSupporterList.transform.position;
			while (vector.y > 0f)
			{
				yield return null;
				this._target.transform.Translate(this._input.speed * Chronometer.global.deltaTime * Vector2.up);
				vector = this._destination.transform.position - this._lastSupporterList.transform.position;
			}
			yield return Chronometer.global.WaitForSeconds(this._delay);
			yield return this.CLoadScene();
			yield break;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001912B File Offset: 0x0001732B
		public IEnumerator CLoadScene()
		{
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			yield return Chronometer.global.WaitForSeconds(2f);
			this.Hide();
			if (this._resetGameScene)
			{
				Singleton<Service>.Instance.ResetGameScene();
			}
			yield break;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001913A File Offset: 0x0001733A
		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x000075E7 File Offset: 0x000057E7
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00019148 File Offset: 0x00017348
		private void OnDisable()
		{
			Singleton<Service>.Instance.fadeInOut.FadeIn();
			this.Hide();
		}

		// Token: 0x04000701 RID: 1793
		[PauseEvent.SubcomponentAttribute]
		[SerializeField]
		private PauseEvent _pauseEvent;

		// Token: 0x04000702 RID: 1794
		[SerializeField]
		private PauseEventSystem _pauseEventSystem;

		// Token: 0x04000703 RID: 1795
		[SerializeField]
		private Input _input;

		// Token: 0x04000704 RID: 1796
		[SerializeField]
		private Transform _target;

		// Token: 0x04000705 RID: 1797
		[SerializeField]
		private Transform _destination;

		// Token: 0x04000706 RID: 1798
		[SerializeField]
		private Transform _lastSupporterList;

		// Token: 0x04000707 RID: 1799
		[SerializeField]
		private float _delay;

		// Token: 0x04000708 RID: 1800
		private bool _resetGameScene;
	}
}
