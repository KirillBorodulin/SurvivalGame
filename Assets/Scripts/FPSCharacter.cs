using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(CharacterController))]
public class FPSCharacter : MonoBehaviour
{
    [SerializeField]
    private Transform head;
    [SerializeField, Range(0, 50)]
    private float sense = 8;
    [SerializeField, Range(0, 50)]
    private float speed = 3;

    [Space]
    [SerializeField]
    private InputActionReference moveAction;
    [SerializeField]
    private InputActionReference lookAction;
    [SerializeField]
    private InputActionReference interactAction;

    private CharacterController character;
    private Inventory inventory;

    private Vector2 headVector;

    private void Awake()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        interactAction.action.Enable();

        inventory = GetComponent<Inventory>();
        character = GetComponent<CharacterController>();

        interactAction.action.performed += OnInteraction_Input;
    }
    private void OnDestroy()
    {
        interactAction.action.performed -= OnInteraction_Input;
    }

    private void Update()
    {
        HeadTorque();
        Walking();
    }

    private void OnInteraction_Input(CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (Physics.Raycast(head.position, head.forward, out var hit))
            {
                hit.collider.SendMessage("Interact", gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void HeadTorque()
    {
        var lookVector = lookAction.action.ReadValue<Vector2>();
        lookVector *= Time.deltaTime;
        headVector += lookVector * sense;
        headVector.y = Mathf.Clamp(headVector.y, -89.9f, 89.9f);

        transform.eulerAngles = new (0, headVector.x, 0);
        head.localEulerAngles = new (-headVector.y, 0, 0);
    }
    private void Walking()
    {
        var input = moveAction.action.ReadValue<Vector2>();
        input *= Time.deltaTime;
        var walkVector = transform.rotation * new Vector3(input.x, 0, input.y);
        walkVector *= speed;
        character.Move(walkVector + Physics.gravity);
    }
}
