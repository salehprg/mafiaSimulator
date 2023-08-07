using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Camera camera;
    Vector2 startMouseInput;

    public float sensivity = 1.0f;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.magnitude > 0)
        {
            Vector3 newPos = camera.transform.position + (camera.transform.forward * Input.mouseScrollDelta.y);

            camera.transform.position = newPos;
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
        {
            Vector3 newPos = camera.transform.position + (camera.transform.forward * Input.GetAxis("Vertical"));

            camera.transform.position = new Vector3(newPos.x, camera.transform.position.y, newPos.z);
        }

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            Vector3 newPos = camera.transform.position + (camera.transform.right * Input.GetAxis("Horizontal"));

            camera.transform.position = new Vector3(newPos.x, camera.transform.position.y, newPos.z);
        }


        if (Input.GetMouseButton(1))
        {
            var dif = (Vector2)Input.mousePosition - startMouseInput;
            dif = dif.normalized;

            var c = Camera.main.transform;

            var newRot = Vector3.Lerp(c.eulerAngles , new Vector3(c.eulerAngles.x - dif.y, c.eulerAngles.y + dif.x , 0) , Time.time * sensivity);
            c.eulerAngles = newRot;

            startMouseInput = Input.mousePosition; 
        }


        if (Input.GetMouseButtonDown(1))
        {
            startMouseInput = Input.mousePosition;
        }
    }
}
