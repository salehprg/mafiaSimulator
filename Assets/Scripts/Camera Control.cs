using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public float zoomSensivity = 1.0f;
    public float moveSensivity = 1.0f;
    public float rotateSensivity = 1.0f;

    Vector2 startMouseInput;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.magnitude > 0)
        {
            var followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            var zoomDir = followOffset.normalized;

            var newZoom = followOffset - zoomDir * Mathf.Sign(Input.mouseScrollDelta.y);

            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(followOffset , newZoom , Time.deltaTime * zoomSensivity);

        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            Vector3 newPos = transform.position + (transform.forward * Input.GetAxis("Vertical") * moveSensivity)
                                                + (transform.right * Input.GetAxis("Horizontal") * moveSensivity);

            transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
        }

        if (Input.GetMouseButtonDown(1))
        {
            startMouseInput = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            var dif = (Vector2)Input.mousePosition - startMouseInput;
            transform.eulerAngles += new Vector3(-dif.y * rotateSensivity, dif.x * rotateSensivity, 0);
            startMouseInput = Input.mousePosition;
        }

    }
}
