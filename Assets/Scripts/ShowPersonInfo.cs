using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ShowPersonInfo : MonoBehaviour
{
    [SerializeField]
    protected Text textMesh;

    [SerializeField]
    protected Image image;
    [SerializeField]
    protected Transform statusImage;


    private void Start()
    {
        var person = GetComponentInParent<Person>();
        image.sprite = person.icon;
    }

    public void ShowStatus(PersonStatus personStatus)
    {
        GameObject icon;

        if (statusImage.childCount > 0)
        {
            for (int i = 0; i < statusImage.childCount; i++)
            {
                var temp = statusImage.GetChild(i);
                Destroy(temp.gameObject);
            }

        }


        if (GameManagerr.instance.statusSprites.TryGetValue(personStatus, out icon))
        {
            textMesh.gameObject.SetActive(false);
            statusImage.gameObject.SetActive(true);
            Instantiate(icon, statusImage);
        }
        else
        {
            statusImage.gameObject.SetActive(false);
            textMesh.gameObject.SetActive(true);
        }
    }

    private void Update()
    {

    }

}
