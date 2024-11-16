using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimStateManager : MonoBehaviour
{
    #region States
    [Header("States")]
    public AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();
    [HideInInspector] public Animator anim;
    #endregion

    #region Camera
    [Header("Camera")]
    public float xAxis, yAxis;
    [SerializeField]public float mouseSense = 1;
    [SerializeField] Transform camFollowPos;
    [HideInInspector] public CinemachineVirtualCamera vCam;
    public float aimFov = 40;
    [HideInInspector] public float hipFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10;
    public Camera mainCamera;
    float xFollowPos;
    float yFollowPos, ogYPos;
    [SerializeField] float crouchCameraHeight = 0.6f;
    [SerializeField] float shoulderSwapSpeed = 10;
    MovementStateManager moving;
    #endregion

    #region Aiming
    [Header("Aiming")]
    public Transform aimPos;
    //[HideInInspector] public Vector3 actualAimPos;
    [SerializeField] float aimSmoothSpeed = 20;
    [SerializeField] LayerMask aimMask;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        moving = GetComponentInParent<MovementStateManager>();
        xFollowPos = camFollowPos.localPosition.x;
        ogYPos = camFollowPos.localPosition.y;
        yFollowPos = ogYPos;
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        hipFov = vCam.m_Lens.FieldOfView;
        anim = GetComponent<Animator>();
        SwitchState(Hip);
    }

    // Update is called once per frame
    void Update()
    {
        // Update camera rotation based on mouse position 
        xAxis +=(Input.GetAxisRaw("Mouse X") * mouseSense);
        yAxis -=(Input.GetAxisRaw("Mouse Y") * mouseSense);
        // Clamp the camera rotation
        yAxis = Mathf.Clamp(yAxis,-80,80);
        // Make the zoom transition for aim smoother 
        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);
        // Get the screen centre
        Vector2 screenCentre = new Vector2(Screen.width/2, Screen.height/2);
        // Get the ray from the camera to the screen centre
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);
        // Check if the ray hits the aim mask
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask)){
            // Smoothly move the aim position
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
            //actualAimPos = hit.point;
        }
        // Move the camera follow position
        MoveCamera();
        // Update the current state
        currentState.UpdateState(this);


    }
    void LateUpdate(){
        // Rotate the camera follow position
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z); //yAxis.Value
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z); //xAxis.Value
    }

    public void SwitchState(AimBaseState state){
        currentState = state;
        currentState.EnterState(this);
    }

    void MoveCamera(){
        // Shoulder swap camera on key press
        if (Input.GetKeyDown(KeyCode.LeftAlt)) xFollowPos = -xFollowPos;
        // Move camera down if crouching
        if (moving.currentState == moving.Crouch) yFollowPos = crouchCameraHeight;
        else yFollowPos = ogYPos;
        // Smoothly move the camera follow position
        Vector3 newFollowPos = new Vector3(xFollowPos, yFollowPos, camFollowPos.localPosition.z);
        camFollowPos.localPosition = Vector3.Lerp(camFollowPos.localPosition, newFollowPos, shoulderSwapSpeed * Time.deltaTime);
    }
}
