using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace HelloWorld
{
    public class Client_Camera : NetworkBehaviour
    {
        public static void Change_Cameras()
        {
            var Main_camera = GameObject.Find("Main Camera");
            var Second_camera = GameObject.Find("Client Camera");
            if (HelloWorldManager.isClient)
            {
                Main_camera.GetComponent<Camera>().enabled = false;
                Second_camera.GetComponent<Camera>().enabled = true;
                Main_camera.GetComponent<AudioListener>().enabled = false;
                Second_camera.GetComponent<AudioListener>().enabled = true;
            }
            else
            {
                Main_camera.GetComponent<Camera>().enabled = true;
                Second_camera.GetComponent<Camera>().enabled = false;
                Second_camera.GetComponent<AudioListener>().enabled = false;
                Main_camera.GetComponent<AudioListener>().enabled = true;
            }
        }
    }
}
