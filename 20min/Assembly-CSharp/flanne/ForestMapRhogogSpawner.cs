using System;
using flanne.Core;
using flanne.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace flanne
{
	// Token: 0x020000C8 RID: 200
	public class ForestMapRhogogSpawner : MonoBehaviour
	{
		// Token: 0x06000652 RID: 1618 RVA: 0x0001CF8C File Offset: 0x0001B18C
		private void Start()
		{
			this.timer = GameTimer.SharedInstance;
			this.AddObserver(new Action<object, object>(this.OnOneSecondLeft), GameTimer.OneSecondLeftNotification);
			this.treeHealths = base.GetComponentsInChildren<Health>();
			Health[] array = this.treeHealths;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].onDeath.AddListener(new UnityAction(this.OnTreeKilled));
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001CFF8 File Offset: 0x0001B1F8
		private void OnTreeKilled()
		{
			if (this.treesActivated)
			{
				return;
			}
			this.treesActivated = true;
			foreach (Health health in this.treeHealths)
			{
				if (health.gameObject.activeSelf)
				{
					health.gameObject.SetActive(false);
					Object.Instantiate<GameObject>(this.walkingTreePrefab).transform.position = health.transform.position;
				}
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001D068 File Offset: 0x0001B268
		private void OnOneSecondLeft(object sender, object args)
		{
			if (!this.treesActivated || SelectedMap.MapData.endless)
			{
				return;
			}
			this.timer.Stop();
			GameObject gameObject = Object.Instantiate<GameObject>(this.rhogogPrefab);
			Vector3 position = PlayerController.Instance.transform.position + new Vector3(20f, 0f, 0f);
			gameObject.transform.position = position;
			gameObject.GetComponent<Health>().onDeath.AddListener(new UnityAction(this.OnRhogogDefeated));
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001D0F0 File Offset: 0x0001B2F0
		private void OnRhogogDefeated()
		{
			if (!SaveSystem.data.gunUnlocks.unlocks[8])
			{
				this.sporeGunUnlockedUI.Show();
				this.sporeGunUIConfirm.onClick.AddListener(new UnityAction(this.OnSporeGunUIConfirm));
				PauseController.SharedInstance.Pause();
				SaveSystem.data.gunUnlocks.unlocks[8] = true;
				return;
			}
			this.timer.Start();
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001D15F File Offset: 0x0001B35F
		private void OnSporeGunUIConfirm()
		{
			this.sporeGunUnlockedUI.Hide();
			PauseController.SharedInstance.UnPause();
			this.timer.Start();
		}

		// Token: 0x0400041C RID: 1052
		[SerializeField]
		private GameObject rhogogPrefab;

		// Token: 0x0400041D RID: 1053
		[SerializeField]
		private GameObject arenaMonsterPrefab;

		// Token: 0x0400041E RID: 1054
		[SerializeField]
		private GameObject walkingTreePrefab;

		// Token: 0x0400041F RID: 1055
		[SerializeField]
		private Panel sporeGunUnlockedUI;

		// Token: 0x04000420 RID: 1056
		[SerializeField]
		private Button sporeGunUIConfirm;

		// Token: 0x04000421 RID: 1057
		private bool treesActivated;

		// Token: 0x04000422 RID: 1058
		private Health[] treeHealths;

		// Token: 0x04000423 RID: 1059
		private GameTimer timer;
	}
}
