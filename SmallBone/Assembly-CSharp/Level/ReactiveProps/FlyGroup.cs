using System;
using System.Collections;
using UnityEngine;

namespace Level.ReactiveProps
{
	// Token: 0x0200056C RID: 1388
	public class FlyGroup : MonoBehaviour
	{
		// Token: 0x06001B43 RID: 6979 RVA: 0x00054B7C File Offset: 0x00052D7C
		private void Start()
		{
			ReactiveProp[] array = this._group.GetComponentsInChildren<AlwaysFly>();
			this._reactiveProps = array;
			array = this._reactiveProps;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x00054BC0 File Offset: 0x00052DC0
		public void Activate()
		{
			FlyGroup.StartPositionType startPositionType = this._startPositionType;
			if (startPositionType != FlyGroup.StartPositionType.Consistent)
			{
				if (startPositionType == FlyGroup.StartPositionType.RandomInBounds)
				{
					this.StartRandomPosition();
				}
			}
			else
			{
				this.StartCosistentPosition();
			}
			base.StartCoroutine(this.Spawn());
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00054BF8 File Offset: 0x00052DF8
		private IEnumerator Spawn()
		{
			foreach (ReactiveProp fly in this._reactiveProps)
			{
				int waitForRandomAnimationLength = UnityEngine.Random.Range(1, 3);
				int num;
				for (int i = 0; i < waitForRandomAnimationLength; i = num + 1)
				{
					yield return null;
					num = i;
				}
				fly.ResetPosition();
				fly.gameObject.SetActive(true);
				fly = null;
			}
			ReactiveProp[] array = null;
			yield break;
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00054C07 File Offset: 0x00052E07
		private void StartRandomPosition()
		{
			this._group.transform.position = MMMaths.RandomPointWithinBounds(this._startBounds.bounds);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x00054C2E File Offset: 0x00052E2E
		private void StartCosistentPosition()
		{
			this._group.transform.position = this._startPoint.position;
		}

		// Token: 0x0400176B RID: 5995
		[SerializeField]
		private FlyGroup.StartPositionType _startPositionType;

		// Token: 0x0400176C RID: 5996
		[SerializeField]
		private Transform _startPoint;

		// Token: 0x0400176D RID: 5997
		[SerializeField]
		private Collider2D _startBounds;

		// Token: 0x0400176E RID: 5998
		[SerializeField]
		private Transform _group;

		// Token: 0x0400176F RID: 5999
		private ReactiveProp[] _reactiveProps;

		// Token: 0x0200056D RID: 1389
		private enum StartPositionType
		{
			// Token: 0x04001771 RID: 6001
			Consistent,
			// Token: 0x04001772 RID: 6002
			RandomInBounds
		}
	}
}
