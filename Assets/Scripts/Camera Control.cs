using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public float zoomSensivity = 1.0f;
    public float mobile_zoomSensivity = 0.01f;
    public float mobile_moveSensivity = 0.1f;
    public float mobile_rotateSensivity = 1.0f;
    public float moveSensivity = 1.0f;
    public float rotateSensivity = 1.0f;


    float rotateSense = 0;
    float zoomSense = 0;
    float zoom = 0;
    float forward = 0;
    float left = 0;

    Vector2 startMouseInput;
    Vector2 rotationDelta;
    Vector2 touch0_pos;
    Vector2 touch1_pos;

    void Start()
    {
        rotateSense = rotateSensivity;
        zoomSense = zoomSensivity;
    }

    // Update is called once per frame
    void Update()
    {
        zoom = 0;

        if (Input.mouseScrollDelta.magnitude > 0)
        {
            zoomSense = zoomSensivity;
            zoom = Input.mouseScrollDelta.y;
        }

        forward = Input.GetAxis("Vertical");
        left = Input.GetAxis("Horizontal");

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touch0_pos = Input.GetTouch(0).position;
            }
            if (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)
            {
                touch1_pos = Input.GetTouch(1).position;
            }
        }

        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                var direction = Vector2.Dot(Input.GetTouch(0).deltaPosition, Input.GetTouch(1).deltaPosition);

                if (direction < 0)
                {
                    zoomSense = mobile_zoomSensivity;
                    zoom = direction;
                    zoom *= MathF.Sign((touch0_pos - touch1_pos).magnitude - (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude);

                }
                else
                {
                    rotateSense = mobile_rotateSensivity;
                    rotationDelta = Input.GetTouch(0).deltaPosition;
                }

                touch0_pos = Input.GetTouch(0).position;
                touch1_pos = Input.GetTouch(1).position;
            }
        }

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var offset = Input.GetTouch(0).deltaPosition;

                forward = -offset.y * mobile_moveSensivity;
                left = -offset.x * mobile_moveSensivity;
            }
        }

        if (MathF.Abs(zoom) > 0)
        {
            var followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            var zoomDir = followOffset.normalized;

            var newZoom = followOffset - zoomDir * Mathf.Sign(zoom);
            if (newZoom.y > 0 && newZoom.y < 2.5)
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(followOffset, newZoom, Time.deltaTime * zoomSense);
        }


        if (Mathf.Abs(forward) > 0 || Mathf.Abs(left) > 0)
        {
            Vector3 newPos = transform.position + (transform.forward * forward * moveSensivity)
                                                + (transform.right * left * moveSensivity);

            transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
        }

        if (Input.GetMouseButtonDown(1))
        {
            startMouseInput = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            rotationDelta = (Vector2)Input.mousePosition - startMouseInput;
        }

        print(transform.eulerAngles);

        if (transform.eulerAngles.x + -rotationDelta.y * rotateSense <= 40 || transform.eulerAngles.x + -rotationDelta.y * rotateSense >= 360 - 10)
        {
            transform.eulerAngles += new Vector3(-rotationDelta.y * rotateSense, rotationDelta.x * rotateSense, 0);
        }

        startMouseInput = Input.mousePosition;
        rotationDelta = Vector2.Lerp(rotationDelta, new Vector2(), Time.deltaTime);

    }
}
