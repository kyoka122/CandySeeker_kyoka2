using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraZoomMobile : MonoBehaviour
{
   
    [SerializeField] GameObject cameraObj = null;
    Transform tf; //Main CameraのTransform
    Camera cam; //Main CameraのCamera
    private int buttonDown = 0;
    Image colorchangeImage;
    [SerializeField] Image[] cameraMoveButton = new Image[6];
    private void Awake()
    {
        tf = cameraObj.GetComponent<Transform>();
        cam = cameraObj.GetComponent<Camera>();
    }
    public void Update()
    {
        if (buttonDown == 0)
        {
            return;
        }
        else if (buttonDown == 1)
        {
            tf.position += new Vector3(0.0f, 3.0f, 0.0f);
        }
        else if (buttonDown == 2)
        {
            tf.position += new Vector3(0.0f, -3.0f, 0.0f);
        }
        else if (buttonDown == 3)
        {
            tf.position += new Vector3(3.0f, 0.0f, 0.0f);
        }
        else if (buttonDown == 4)
        {
            tf.position += new Vector3(-3.0f, 0.0f, 0.0f);
        }
        else if (buttonDown == 5)
        {
            cam.orthographicSize += -3.0f;
        }
        else if (buttonDown == 6)
        {
            cam.orthographicSize += 3.0f;
        }
    }

    public void UpButtonOn()
    {
        buttonDown = 1;
        colorchangeImage = cameraMoveButton[0];
        cameraMoveButton[0].color = new Color(cameraMoveButton[0].color.r, cameraMoveButton[0].color.g, cameraMoveButton[0].color.b, 170f / 250f);
    }
    public void DownButtonOn()
    {
        buttonDown = 2;
        colorchangeImage = cameraMoveButton[1];
        cameraMoveButton[1].color = new Color(cameraMoveButton[1].color.r, cameraMoveButton[1].color.g, cameraMoveButton[1].color.b, 170f / 250f);
    }
    public void RightButtonOn()
    {
        buttonDown = 3;
        colorchangeImage = cameraMoveButton[2];
        cameraMoveButton[2].color = new Color(cameraMoveButton[2].color.r, cameraMoveButton[2].color.g, cameraMoveButton[2].color.b, 170f / 250f);
    }
    public void LeftButtonOn()
    {
        buttonDown = 4;
        colorchangeImage = cameraMoveButton[3];
        cameraMoveButton[3].color = new Color(cameraMoveButton[3].color.r, cameraMoveButton[3].color.g, cameraMoveButton[3].color.b, 170f / 250f);
    }
    public void InButtonOn()
    {
        buttonDown = 5;
        colorchangeImage = cameraMoveButton[4];
        cameraMoveButton[4].color = new Color(cameraMoveButton[4].color.r, cameraMoveButton[4].color.g, cameraMoveButton[4].color.b, 170f / 250f);
    }
    public void OutButtonOn()
    {
        buttonDown = 6;
        colorchangeImage = cameraMoveButton[5];
        cameraMoveButton[5].color = new Color(cameraMoveButton[5].color.r, cameraMoveButton[5].color.g, cameraMoveButton[5].color.b, 170f / 250f);
    }

    public void ButtonOff()
    {
        buttonDown = 0;
        colorchangeImage.color = new Color(colorchangeImage.color.r, colorchangeImage.color.g, colorchangeImage.color.b, 1f);
    }
}
