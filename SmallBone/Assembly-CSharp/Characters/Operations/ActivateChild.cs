using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DCB RID: 3531
	public class ActivateChild : CharacterOperation
	{
		// Token: 0x060046ED RID: 18157 RVA: 0x000CDE2C File Offset: 0x000CC02C
		public override void Run(Character owner)
		{
			if (this._interval == 0f)
			{
				using (IEnumerator enumerator = this._parent.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						((Transform)obj).gameObject.SetActive(true);
					}
					goto IL_64;
				}
			}
			base.StartCoroutine(this.CRun(owner.chronometer.master));
			IL_64:
			if (this._duration > 0f)
			{
				base.StartCoroutine(this.CExpire(owner.chronometer.master));
			}
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x000CDED4 File Offset: 0x000CC0D4
		private IEnumerator CRun(Chronometer chronometer)
		{
			foreach (object obj in this._parent)
			{
				((Transform)obj).gameObject.SetActive(true);
				yield return chronometer.WaitForSeconds(this._interval);
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x000CDEEA File Offset: 0x000CC0EA
		private IEnumerator CExpire(Chronometer chronometer)
		{
			yield return chronometer.WaitForSeconds(this._duration);
			this.Stop();
			yield break;
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x000CDF00 File Offset: 0x000CC100
		public override void Stop()
		{
			base.StopAllCoroutines();
			foreach (object obj in this._parent)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
		}

		// Token: 0x040035D1 RID: 13777
		[SerializeField]
		private Transform _parent;

		// Token: 0x040035D2 RID: 13778
		[SerializeField]
		[Information("duration이 0이면 Operation이 끝날 때 Deactivate됨", InformationAttribute.InformationType.Info, false)]
		private float _duration;

		// Token: 0x040035D3 RID: 13779
		[SerializeField]
		private float _interval;
	}
}
