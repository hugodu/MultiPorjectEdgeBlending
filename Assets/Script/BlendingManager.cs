using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendingManager : MonoBehaviour
{
    public int CameraNum;
    public GameObject G_Camera;
    public int Xdivide;
    public int Ydivide;
    public List<CreateMesh> createMeshes = new List<CreateMesh>();
    public float BlendPresent;

    // Start is called before the first frame update
    void Start()
    {
        float xTiling = 1f / Xdivide + BlendPresent;
        float StartUnit = 1f / Xdivide - BlendPresent;
        float midUnit = 1f / Xdivide - 2 * BlendPresent;
        Debug.Log("xTiling " + xTiling);
        for (int i = 0; i < CameraNum; i++)
        {
            GameObject g = Instantiate(G_Camera);


            // float yTiling = 1 / Ydivide + BlendPresent;



            float xoffset = 0;
            if (i == 0) {
                xoffset = 0;
            } else if (i == 1) {
                xoffset = StartUnit;

            }
            else {
                xoffset = StartUnit + (i - 1) * (midUnit + 2 * BlendPresent);
            }

       
            //   float yoffset = i * yTiling - BlendPresent;
            Debug.Log(xoffset);
            g.GetComponent<CreateMesh>().initilization(i, xTiling, 1,xoffset,0);
            createMeshes.Add(g.GetComponent<CreateMesh>());
            g.name = "Camera" + i.ToString();


        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
