using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    [SerializeField] GameObject scripts=null;

    gUIController gUIController;
    private void Awake()
    {
        gUIController = scripts.GetComponent<gUIController>();
    }

    public void TurnTextChangeEvent()
    {
        gUIController.TurnCountTextChange();
    }

}
