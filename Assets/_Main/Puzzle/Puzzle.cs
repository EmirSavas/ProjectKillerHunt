using DG.Tweening;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour, IInteractable
{
    public abstract void Interact(CharacterMechanic characterMechanic, CharacterMovement characterMovement);

    protected virtual void CameraChange(CharacterMovement movement, Vector3 movePosition)
    {
        movement.enabled = !movement.enabled;
        movement.cam.transform.DOMove(movePosition, 0.3f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                movement.cam.transform.LookAt(transform);
            });
    }
}
