using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] GameObject cameraObj = null;
    Transform tf; //Main CameraのTransform
    Camera cam; //Main CameraのCamera

    private void Awake()
    {
        tf = cameraObj.GetComponent<Transform>();
        cam = cameraObj.GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.I)) //Iキーが押されていれば
        {
            cam.orthographicSize += -3.0f; //ズームイン。
        }
        else if (Input.GetKey(KeyCode.O)) //Oキーが押されていれば
        {
            cam.orthographicSize += 3.0f; //ズームアウト。
        }
        if (Input.GetKey(KeyCode.W)) //上キーが押されていれば
        {
            tf.position += new Vector3(0.0f, 3.0f, 0.0f); //カメラを上へ移動。
        }
        else if (Input.GetKey(KeyCode.S)) //下キーが押されていれば
        {
            tf.position += new Vector3(0.0f, -3.0f, 0.0f); //カメラを下へ移動。
        }
        if (Input.GetKey(KeyCode.A)) //左キーが押されていれば
        {
            tf.position += new Vector3(-3.0f, 0.0f, 0.0f); //カメラを左へ移動。
        }
        else if (Input.GetKey(KeyCode.D)) //右キーが押されていれば
        {
            tf.position += new Vector3(3.0f, 0.0f, 0.0f); //カメラを右へ移動。
        }
    }






}
