using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Albarnie.InputManager;

namespace Albarnie.Interaction2D
{
    public class Interaction2D : MonoBehaviour
    {
        public List<IInteractable2D> interactables;
        public IInteractable2D currentInteractable;

        float currentInteractTime = 0;

        private void OnEnable()
        {
            interactables = new List<IInteractable2D>();

            InputManager.InputManager.manager.AddEvent("Interact", OnInteractionStarted, InputType.OnStarted);
            InputManager.InputManager.manager.AddEvent("Interact", OnInteractionCancelled, InputType.OnCancelled);
        }

        private void Update()
        {
            if (currentInteractable != null)
            {
                currentInteractTime += Time.deltaTime;
                if (currentInteractTime >= currentInteractable.InteractionTime)
                {
                    FinishInteract();
                }
            }
        }

        void OnInteractionStarted()
        {
            if (interactables.Count > 0)
                StartInteract(interactables[0]);
        }

        void OnInteractionCancelled()
        {
            if (currentInteractable != null)
                CancelInteract();
        }

        void StartInteract(IInteractable2D interactable)
        {
            currentInteractTime = 0;

            if (currentInteractable != null)
                CancelInteract();

            currentInteractable = interactable;

            interactable.OnStartInteract(this);
            if (interactable.InteractionTime <= 0)
            {
                FinishInteract();
                return;
            }
            interactables.Remove(interactable);
        }

        void FinishInteract()
        {
            currentInteractable.OnFinishInteract(this);
            currentInteractable = null;
        }

        void CancelInteract()
        {
            currentInteractable.OnCancelInteract(this);
            interactables.Add(currentInteractable);
            currentInteractable = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Interactable"))
            {
                IInteractable2D interactable = collision.GetComponent<IInteractable2D>();
                interactables.Add(interactable);
                interactable.OnBeginFocus(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Interactable"))
            {
                IInteractable2D interactable = collision.GetComponent<IInteractable2D>();
                interactables.Remove(interactable);
                interactable.OnEndFocus(this);
            }
        }

    }
}