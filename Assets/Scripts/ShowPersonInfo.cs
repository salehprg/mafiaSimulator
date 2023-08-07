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

    Person person;
    GameManagerr gamemanager;

    GameObject currentStatusImg;
    PersonStatus status;

    private void Start()
    {
        gamemanager = GameManagerr.instance;
        person = GetComponentInParent<Person>();
        image.sprite = person.icon;
    }

    private void Update()
    {
        if (person != null)
        {
            GameObject icon;
            if (status != person.personStatus && gamemanager.statusSprites.TryGetValue(person.personStatus, out icon))
            {
                status = person.personStatus;
                
                if (currentStatusImg != null)
                    Destroy(currentStatusImg);

                textMesh.gameObject.SetActive(false);
                statusImage.gameObject.SetActive(true);
                currentStatusImg = Instantiate(icon, statusImage);
            }
            else if(!gamemanager.statusSprites.ContainsKey(person.personStatus))
            {
                status = person.personStatus;

                if (currentStatusImg != null)
                    Destroy(currentStatusImg);

                statusImage.gameObject.SetActive(false);
                textMesh.gameObject.SetActive(true);
            }

            textMesh.text = person.personStatus.ToString().ToLowercaseNamingConvention();
        }
    }

}
