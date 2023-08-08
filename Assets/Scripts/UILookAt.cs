using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILookAt : MonoBehaviour
{
    private void LateUpdate() {
        try{
        transform.LookAt(Camera.main.transform.position);
        }
        catch(Exception){}
    }
}
