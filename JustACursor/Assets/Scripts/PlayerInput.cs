using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    
    private Camera camera;

    [Header("Action Keys")]
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private KeyCode shootKey;

    private HashSet<InputAction> requestedActions = new HashSet<InputAction>();

    void Start() {
        camera = Camera.main;
    }
    
    private void Update() {
        CaptureInput();
    }
    
    private void CaptureInput() {
        if (Input.GetKeyDown(dashKey)) requestedActions.Add(InputAction.Dash);
        else if (Input.GetKeyUp(dashKey)) requestedActions.Remove(InputAction.Dash);

        if (Input.GetKeyDown(shootKey)) requestedActions.Add(InputAction.Shoot);
        else if (Input.GetKeyUp(shootKey)) requestedActions.Remove(InputAction.Shoot);
    }
    
    public float GetAxisRaw(Axis axis) {
        return Input.GetAxisRaw(axis == Axis.X ? "Horizontal" : "Vertical");
    }

    public Vector2 GetMousePos() {
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }

    public bool GetActionPressed(InputAction action) {
        return requestedActions.Contains(action);
    }
}
