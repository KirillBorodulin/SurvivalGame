using UnityEngine;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    [SerializeField]
    private InputActionReference placeAction;
    [SerializeField]
    private InputActionReference rotateAction;
    [SerializeField]
    private InputActionReference nextBuildingAction;
    [SerializeField]
    private InputActionReference prevBuildingAction;

    [Space]
    [SerializeField]
    private float placeDistance = 5f;
    [SerializeField]
    private LayerMask groundLayer;

    [Space]
    [SerializeField]
    private GameObject[] buildings;

    private Camera playerCamera;
    private int selectedBuilding = 0;
    private GameObject preview;
    private Material previewMaterial;
    private bool isBuildMode = false;

    public void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();

        previewMaterial = new Material(Shader.Find("Standard"));
        previewMaterial.color = new Color(0, 1, 0, 0.5f);

        placeAction.action.performed += OnPlace;
        rotateAction.action.performed += OnRotate;
        nextBuildingAction.action.performed += OnNextBuilding;
        prevBuildingAction.action.performed += OnPrevBuilding;
    }

    public void OnEnable()
    {
        placeAction.action.Enable();
        rotateAction.action.Enable();
        nextBuildingAction.action.Enable();
        prevBuildingAction.action.Enable();
    }

    public void OnDisable()
    {
        placeAction.action.Disable();
        rotateAction.action.Disable();
        nextBuildingAction.action.Disable();
        prevBuildingAction.action.Disable();
    }

    public void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            ToggleBuildMode();
        }

        if (isBuildMode)
        {
            UpdatePreviewPosition();
        }
    }

    private void OnNextBuilding(InputAction.CallbackContext context)
    {
        if (!isBuildMode) return;

        selectedBuilding++;
        if (selectedBuilding >= buildings.Length)
            selectedBuilding = 0;

        UpdatePreview();
    }

    private void OnPrevBuilding(InputAction.CallbackContext context)
    {
        if (!isBuildMode) return;

        selectedBuilding--;
        if (selectedBuilding < 0)
            selectedBuilding = buildings.Length - 1;

        UpdatePreview();
    }

    public void ToggleBuildMode()
    {
        isBuildMode = !isBuildMode;

        if (isBuildMode)
        {
            UpdatePreview();
        }
        else
        {
            if (preview != null)
                Destroy(preview);
        }
    }

    public void UpdatePreview()
    {
        if (preview != null)
            Destroy(preview);

        if (buildings.Length > 0 && buildings[selectedBuilding] != null)
        {
            preview = Instantiate(buildings[selectedBuilding]);

            Renderer[] renderers = preview.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
                r.material = previewMaterial;

            Collider[] colliders = preview.GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
                c.enabled = false;
        }
    }

    public void UpdatePreviewPosition()
    {
        if (preview == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, placeDistance, groundLayer))
        {
            preview.transform.position = hit.point;
        }
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        if (!isBuildMode || preview == null) return;

        Instantiate(buildings[selectedBuilding], preview.transform.position, preview.transform.rotation);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (isBuildMode && preview != null)
        {
            preview.transform.Rotate(0, 45, 0);
        }
    }
}