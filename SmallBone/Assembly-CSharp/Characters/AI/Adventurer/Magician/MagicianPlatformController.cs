using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Adventurer.Magician
{
	// Token: 0x020013F7 RID: 5111
	public class MagicianPlatformController : MonoBehaviour
	{
		// Token: 0x060064A7 RID: 25767 RVA: 0x00123DA4 File Offset: 0x00121FA4
		private void OnEnable()
		{
			this._left = new HashSet<MagicianPlatform>(this._leftPlatforms);
			this._right = new HashSet<MagicianPlatform>(this._rightPlatforms);
			foreach (MagicianPlatform magicianPlatform in this._left)
			{
				magicianPlatform.Initialize(this);
			}
			foreach (MagicianPlatform magicianPlatform2 in this._right)
			{
				magicianPlatform2.Initialize(this);
			}
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x060064A8 RID: 25768 RVA: 0x00123E64 File Offset: 0x00122064
		private IEnumerator CRun()
		{
			while (!this._owner.health.dead)
			{
				this.NextSpawn();
				yield return Chronometer.global.WaitForSeconds(this._interval);
			}
			yield break;
		}

		// Token: 0x060064A9 RID: 25769 RVA: 0x00123E74 File Offset: 0x00122074
		private void NextSpawn()
		{
			MagicianPlatform magicianPlatform = this._left.Random<MagicianPlatform>();
			magicianPlatform.Show();
			this._left.Remove(magicianPlatform);
			magicianPlatform = this._right.Random<MagicianPlatform>();
			magicianPlatform.Show();
			this._right.Remove(magicianPlatform);
		}

		// Token: 0x060064AA RID: 25770 RVA: 0x00123EBF File Offset: 0x001220BF
		public void AddPlatform(MagicianPlatform platform, bool left)
		{
			if (left)
			{
				this._left.Add(platform);
				return;
			}
			this._right.Add(platform);
		}

		// Token: 0x04005129 RID: 20777
		[SerializeField]
		private Character _owner;

		// Token: 0x0400512A RID: 20778
		[SerializeField]
		private float _interval;

		// Token: 0x0400512B RID: 20779
		[SerializeField]
		private MagicianPlatform[] _leftPlatforms;

		// Token: 0x0400512C RID: 20780
		[SerializeField]
		private MagicianPlatform[] _rightPlatforms;

		// Token: 0x0400512D RID: 20781
		private HashSet<MagicianPlatform> _left;

		// Token: 0x0400512E RID: 20782
		private HashSet<MagicianPlatform> _right;
	}
}
