using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
  public GameObject cubeObj;
  public GameObject sphereObj;
  public Renderer ren1;
  public Renderer ren2;
  public Material[] mat1;
  public Material[] mat2;

  public AudioSource ac1;
  public AudioSource ac2;

    // Use this for initialization
  void Start ()
  {
    ren1 = cubeObj.GetComponent<Renderer>();
    ren2 = sphereObj.GetComponent<Renderer>();
    mat1 = ren1.materials;
    mat2 = ren2.materials;
	}

	// Update is called once per frame
	void Update ()
  {
        if (Input.GetKeyDown("q"))
        {
          cubeObj.SetActive(true);
          sphereObj.SetActive(false);
        }

        if (Input.GetKeyDown("w"))
        {
            cubeObj.SetActive(false);
            sphereObj.SetActive(true);
        }

        if (Input.GetKeyDown("a"))
        {
            mat1[0].color = Color.red;
            mat2[0].color = Color.red;

        }

        if (Input.GetKeyDown("s"))
        {
            mat1[0].color = Color.blue;
            mat2[0].color = Color.blue;
        }
	}
}
