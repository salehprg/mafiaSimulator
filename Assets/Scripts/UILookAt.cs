using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILookAt : MonoBehaviour
{
    private void LateUpdate() {
        transform.LookAt(Camera.main.transform.position);
    }
}
