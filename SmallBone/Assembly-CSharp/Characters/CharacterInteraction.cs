using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Controllers;
using Level;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006C4 RID: 1732
	public class CharacterInteraction : MonoBehaviour
	{
		// Token: 0x060022CA RID: 8906 RVA: 0x00068DD2 File Offset: 0x00066FD2
		public bool IsInteracting(InteractiveObject interactiveObject)
		{
			return !(this._objectToInteract == null) && this._objectToInteract.Equals(interactiveObject);
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x00068DF0 File Offset: 0x00066FF0
		private void OnTriggerEnter2D(Collider2D collision)
		{
			InteractiveObject component = collision.GetComponent<InteractiveObject>();
			if (component != null)
			{
				this._interactiveObjects.Add(component);
				return;
			}
			IPickupable component2 = collision.GetComponent<IPickupable>();
			if (component2 == null)
			{
				return;
			}
			component2.PickedUpBy(this._character);
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x00068E34 File Offset: 0x00067034
		private void OnTriggerExit2D(Collider2D collision)
		{
			InteractiveObject component = collision.GetComponent<InteractiveObject>();
			if (component != null)
			{
				this._interactiveObjects.Remove(component);
			}
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x00068E60 File Offset: 0x00067060
		private void Update()
		{
			for (int i = this._interactiveObjects.Count - 1; i >= 0; i--)
			{
				InteractiveObject interactiveObject = this._interactiveObjects[i];
				if (interactiveObject == null || !interactiveObject.isActiveAndEnabled)
				{
					this._interactiveObjects.RemoveAt(i);
				}
			}
			if (this._interactiveObjects.Count == 0 || PlayerInput.blocked.value)
			{
				if (this._objectToInteract != null)
				{
					this._objectToInteract.ClosePopup();
				}
				this._pressingTime = 0f;
				base.StopCoroutine("CPressing");
				this._objectToInteract = null;
				return;
			}
			if (this._objectToInteract != null && !this._objectToInteract.activated)
			{
				this._objectToInteract = null;
			}
			float positionX = base.transform.position.x;
			this._interactiveObjects.Sort(delegate(InteractiveObject i1, InteractiveObject i2)
			{
				if (i1 is Potion)
				{
					return -1;
				}
				return Mathf.Abs(positionX - i1.transform.position.x).CompareTo(Mathf.Abs(positionX - i2.transform.position.x));
			});
			int j = 0;
			while (j < this._interactiveObjects.Count)
			{
				InteractiveObject interactiveObject2 = this._interactiveObjects[j];
				if (interactiveObject2.activated && interactiveObject2.interactable)
				{
					if (interactiveObject2.autoInteract)
					{
						interactiveObject2.InteractWith(this._character);
					}
					if (this._objectToInteract != interactiveObject2)
					{
						if (this._objectToInteract != null)
						{
							this._pressingTime = 0f;
							base.StopCoroutine("CPressing");
							this._objectToInteract.ClosePopup();
						}
						interactiveObject2.OpenPopupBy(this._character);
						this._objectToInteract = interactiveObject2;
						return;
					}
					break;
				}
				else
				{
					j++;
				}
			}
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x00068FFC File Offset: 0x000671FC
		public void Interact(CharacterInteraction.InteractionType interactionType)
		{
			if (this._objectToInteract == null || !this._objectToInteract.activated || this._objectToInteract.interactionType != interactionType || this._lastInteractedTime + 0.2f > Time.realtimeSinceStartup)
			{
				return;
			}
			this._objectToInteract.InteractWith(this._character);
			this._lastInteractedTime = Time.realtimeSinceStartup;
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x00069064 File Offset: 0x00067264
		public void InteractionKeyWasPressed()
		{
			if (this._objectToInteract == null || !this._objectToInteract.activated)
			{
				return;
			}
			if (this._lastInteractedTime + 0.2f > Time.realtimeSinceStartup)
			{
				return;
			}
			if (this._objectToInteract.interactionType == CharacterInteraction.InteractionType.Normal)
			{
				this._objectToInteract.InteractWith(this._character);
				this._lastInteractedTime = Time.realtimeSinceStartup;
				return;
			}
			if (this._objectToInteract.interactionType == CharacterInteraction.InteractionType.Pressing)
			{
				base.StartCoroutine("CPressing");
				return;
			}
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000690E6 File Offset: 0x000672E6
		private IEnumerator CPressing()
		{
			this._pressingTime = Chronometer.global.deltaTime;
			while (this._pressingTime < 1f)
			{
				yield return null;
				this._pressingTime += Chronometer.global.deltaTime;
				this._objectToInteract.pressingPercent = this._pressingTime / 1f;
			}
			this._objectToInteract.InteractWithByPressing(this._character);
			yield break;
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x000690F8 File Offset: 0x000672F8
		public void InteractionKeyWasReleased()
		{
			if (this._pressingTime == 0f)
			{
				return;
			}
			base.StopCoroutine("CPressing");
			if (this._objectToInteract == null || !this._objectToInteract.activated)
			{
				return;
			}
			if (this._lastInteractedTime + 0.2f > Time.realtimeSinceStartup)
			{
				return;
			}
			this._objectToInteract.pressingPercent = 0f;
			if (this._pressingTime > 0.5f)
			{
				return;
			}
			this._objectToInteract.InteractWith(this._character);
		}

		// Token: 0x04001DA9 RID: 7593
		public const float pressingTimeForPressing = 1f;

		// Token: 0x04001DAA RID: 7594
		private const float _maxPressingTimeForRelease = 0.5f;

		// Token: 0x04001DAB RID: 7595
		private const float _interactInterval = 0.2f;

		// Token: 0x04001DAC RID: 7596
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001DAD RID: 7597
		[GetComponent]
		[SerializeField]
		private PlayerInput _input;

		// Token: 0x04001DAE RID: 7598
		private readonly List<InteractiveObject> _interactiveObjects = new List<InteractiveObject>();

		// Token: 0x04001DAF RID: 7599
		private float _lastInteractedTime;

		// Token: 0x04001DB0 RID: 7600
		private float _pressingTime;

		// Token: 0x04001DB1 RID: 7601
		private InteractiveObject _objectToInteract;

		// Token: 0x020006C5 RID: 1733
		public enum InteractionType
		{
			// Token: 0x04001DB3 RID: 7603
			Normal,
			// Token: 0x04001DB4 RID: 7604
			Pressing
		}
	}
}
