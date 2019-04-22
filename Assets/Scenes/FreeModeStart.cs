using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class FreeModeStart : MonoBehaviour {

    public Transform StartPoint;

    GameObject mrCameraParent;
    GameObject cam;

    public PostProcessingProfile camPostProcess;


    private void Start()
    {
        GameObject.Find("MixedRealityCamera").AddComponent<PostProcessingBehaviour>();
        GameObject.Find("MixedRealityCamera").GetComponent<PostProcessingBehaviour>().profile = camPostProcess;


        mrCameraParent = GameObject.Find("MixedRealityCameraParent");
        mrCameraParent.transform.parent = StartPoint;
        mrCameraParent.transform.localPosition = Vector3.zero;
        GameObject sContent = GameObject.Find("SceneContent");
        sContent.transform.parent.gameObject.SetActive(false);
        sContent.SetActive(false);
        cam = GameObject.Find("MixedRealityCamera");


        //cam.AddComponent<SphereCollider>().radius = 0.4f;
        //cam.GetComponent<SphereCollider>().center = new Vector3(0, 0.8f, 0);
        //cam.AddComponent<Rigidbody>();

        if (mrCameraParent.GetComponent<Collider>() == null)
        {
            //mrCameraParent.AddComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, 0.5f);
            mrCameraParent.AddComponent<CapsuleCollider>().radius = 0.3f;
            mrCameraParent.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.2f, 0);
        }
        mrCameraParent.AddComponent<Rigidbody>();
        mrCameraParent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

    }

    private void Update()
    {
        if (Input.GetButton(XboxControllerMapping.XboxLeftBumper)
            ||
            Input.GetButton(XboxControllerMapping.XboxRightBumper))
        {
            // 움직인다 //
            Debug.Log("움직인다.");
            //cam.transform.localPosition += new Vector3(0, 0, 5 * Time.deltaTime);
            //mrCameraParent.transform.localPosition += new Vector3(0, 0, 5 * Time.deltaTime);
            Vector3 aForce = cam.transform.rotation.eulerAngles;
            mrCameraParent.transform.localPosition += new Vector3(Mathf.Sin(aForce.y / 180f * Mathf.PI) * Time.deltaTime*2f, 0, Mathf.Cos(aForce.y / 180f * Mathf.PI) * Time.deltaTime*2f);
            //cam.GetComponent<Rigidbody>().AddForce(new Vector3(5, 0, 5),ForceMode.Force);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(mrCameraParent);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

}
