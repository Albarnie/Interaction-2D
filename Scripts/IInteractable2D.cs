using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Albarnie.Interaction2D
{
    public interface IInteractable2D
    {
        float InteractionTime { get; }

        Transform GetTransform();
        void OnFinishInteract(Interaction2D user);
        void OnStartInteract(Interaction2D user);
        void OnCancelInteract(Interaction2D user);

        void OnBeginFocus(Interaction2D user);
        void OnEndFocus(Interaction2D user);
    }
}