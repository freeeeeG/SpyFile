using System;
using System.Collections;
using System.Globalization;
using Characters.Controllers;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using Scenes;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x0200039B RID: 923
	public class EndingGameResult : MonoBehaviour
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00032025 File Offset: 0x00030225
		// (set) Token: 0x060010F2 RID: 4338 RVA: 0x0003202D File Offset: 0x0003022D
		public bool animationFinished { get; private set; }

		// Token: 0x060010F3 RID: 4339 RVA: 0x00032038 File Offset: 0x00030238
		private void OnEnable()
		{
			PlayerInput.blocked.Attach(this);
			Scene<GameBase>.instance.uiManager.pauseEventSystem.PushEmpty();
			Chronometer.global.AttachTimeScale(this, 0f);
			base.StartCoroutine(this.CAnimate());
			this._playTime.text = new TimeSpan(0, 0, GameData.Progress.playTime).ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture);
			this._deaths.text = GameData.Progress.deaths.ToString();
			this._kills.text = GameData.Progress.kills.ToString();
			this._eliteKills.text = GameData.Progress.eliteKills.ToString();
			this._darkcites.text = GameData.Currency.darkQuartz.income.ToString();
			this.UpdateGearList();
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00032115 File Offset: 0x00030315
		private void OnDisable()
		{
			PlayerInput.blocked.Detach(this);
			Scene<GameBase>.instance.uiManager.pauseEventSystem.PopEvent();
			Chronometer.global.DetachTimeScale(this);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00032144 File Offset: 0x00030344
		private void UpdateGearList()
		{
			this._gearListContainer.Empty();
			Inventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory;
			WeaponInventory weapon = inventory.weapon;
			QuintessenceInventory quintessence = inventory.quintessence;
			ItemInventory item = inventory.item;
			for (int i = 0; i < weapon.weapons.Length; i++)
			{
				Weapon weapon2 = weapon.weapons[i];
				if (weapon2 != null)
				{
					GearImageContainer gearImageContainer = UnityEngine.Object.Instantiate<GearImageContainer>(this._gearContainerPrefab, this._gearListContainer);
					gearImageContainer.image.sprite = weapon2.icon;
					gearImageContainer.image.SetNativeSize();
				}
			}
			for (int j = 0; j < quintessence.items.Count; j++)
			{
				Quintessence quintessence2 = quintessence.items[j];
				if (quintessence2 != null)
				{
					GearImageContainer gearImageContainer2 = UnityEngine.Object.Instantiate<GearImageContainer>(this._gearContainerPrefab, this._gearListContainer);
					gearImageContainer2.image.sprite = quintessence2.icon;
					gearImageContainer2.image.SetNativeSize();
				}
			}
			for (int k = 0; k < item.items.Count; k++)
			{
				Item item2 = item.items[k];
				if (item2 != null)
				{
					GearImageContainer gearImageContainer3 = UnityEngine.Object.Instantiate<GearImageContainer>(this._gearContainerPrefab, this._gearListContainer);
					gearImageContainer3.image.sprite = item2.icon;
					gearImageContainer3.image.SetNativeSize();
				}
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0003229D File Offset: 0x0003049D
		private IEnumerator CAnimate()
		{
			this.animationFinished = false;
			float time = 0f;
			Vector3 targetPosition = this._container.transform.position;
			Vector3 position = targetPosition;
			position.y += 200f;
			while (time < 1f)
			{
				this._container.transform.position = Vector3.LerpUnclamped(position, targetPosition, this._curve.Evaluate(time));
				yield return null;
				time += Time.unscaledDeltaTime;
			}
			this._container.transform.position = targetPosition;
			this.animationFinished = true;
			yield break;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0001913A File Offset: 0x0001733A
		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x000075E7 File Offset: 0x000057E7
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000DD2 RID: 3538
		[SerializeField]
		private GameObject _container;

		// Token: 0x04000DD3 RID: 3539
		[SerializeField]
		private AnimationCurve _curve;

		// Token: 0x04000DD4 RID: 3540
		[SerializeField]
		private TextMeshProUGUI _playTime;

		// Token: 0x04000DD5 RID: 3541
		[SerializeField]
		private TextMeshProUGUI _deaths;

		// Token: 0x04000DD6 RID: 3542
		[SerializeField]
		private TextMeshProUGUI _kills;

		// Token: 0x04000DD7 RID: 3543
		[SerializeField]
		private TextMeshProUGUI _eliteKills;

		// Token: 0x04000DD8 RID: 3544
		[SerializeField]
		private TextMeshProUGUI _darkcites;

		// Token: 0x04000DD9 RID: 3545
		[SerializeField]
		private TextMeshProUGUI _title;

		// Token: 0x04000DDA RID: 3546
		[SerializeField]
		private TextMeshProUGUI _subTitle;

		// Token: 0x04000DDB RID: 3547
		[SerializeField]
		private TextMeshProUGUI _yourEnemy;

		// Token: 0x04000DDC RID: 3548
		[SerializeField]
		private TextMeshProUGUI _stageName;

		// Token: 0x04000DDD RID: 3549
		[SerializeField]
		private Transform _gearListContainer;

		// Token: 0x04000DDE RID: 3550
		[SerializeField]
		private GearImageContainer _gearContainerPrefab;
	}
}
