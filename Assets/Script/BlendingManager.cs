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


    private int currentNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        IniMesh();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int val = currentNum + 1;

            currentNum= Mathf.Clamp(val, 0, createMeshes.Count - 1);

            setMeshActive(currentNum);
        }
        else if (Input.GetKeyDown(KeyCode.Q)) {
            int val = currentNum - 1;

            currentNum = Mathf.Clamp(val, 0, createMeshes.Count - 1);

            setMeshActive(currentNum);
        }
    }

    private void setMeshActive(int _num) {
        foreach (var item in createMeshes)
        {
            if (createMeshes.IndexOf(item) == _num) {
                item.setIsActive(true);
            } else {
                item.setIsActive(false);
            } 
        }
    }

    public void IniMesh()
    {
        float xTiling = 1f / Xdivide + BlendPresent;
        float StartUnit = 1f / Xdivide - BlendPresent;
        float midUnit = 1f / Xdivide - 2 * BlendPresent;
        for (int i = 0; i < CameraNum; i++)
        {
            GameObject g = Instantiate(G_Camera);


            // float yTiling = 1 / Ydivide + BlendPresent;



            float xoffset = 0;
            if (i == 0)
            {
                xoffset = 0;
            }
            else if (i == 1)
            {
                xoffset = StartUnit;

            }
            else
            {
                xoffset = StartUnit + (i - 1) * (midUnit + 2 * BlendPresent);
            }
            g.GetComponent<CreateMesh>().initilization(i, xTiling, 1, xoffset, 0);
            createMeshes.Add(g.GetComponent<CreateMesh>());
            g.name = "Camera" + i.ToString();
        }

        foreach (var item in createMeshes)
        {
            if (createMeshes.IndexOf(item) == 0)
            {
                item.setIsActive(true);
            }
            else
            {
                item.setIsActive(false);
            }
        }
    }


    public void setToSingleMode() {
        createMeshes[currentNum].moveMode = MoveMode.singleMode;
    }

    public void setToAreaMode()
    {
        createMeshes[currentNum].moveMode = MoveMode.AreaMode;
    }

    public void setTo_H_Mode()
    {
        createMeshes[currentNum].moveMode = MoveMode.Move_H;
    }

    public void setTo_V_Mode()
    {
        createMeshes[currentNum].moveMode = MoveMode.Move_V;
    }
}
