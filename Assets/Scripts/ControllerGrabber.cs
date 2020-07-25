using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabber : OVRGrabber
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        EventManager.StartListening("ReleaseObject", ReleaseObject);
    }

    private void ReleaseObject()
    {
        //if (m_grabbedObj)
        //{
        //    m_grabbedObj = null;
            GrabEnd();
        //}
    }

}
