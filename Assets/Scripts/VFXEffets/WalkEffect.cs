using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.InputSystem;

public class WalkEffect : MonoBehaviour
{
    [SerializeField]
    private VisualEffect effect;
    [SerializeField]
    private InputActionReference moveAction;

    public void Update()
    {
        Vector2 move = moveAction.action.ReadValue<Vector2>();
        effect.enabled = move.magnitude > 0;
    }
}
