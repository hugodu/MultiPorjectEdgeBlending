using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CreateMesh : MonoBehaviour
{

    public GameObject meshPrefabs;
    public float moveDistance = 0.01f;
    public MoveMode moveMode;
    public int c_layerMask;
    


    Mesh mesh;
    GameObject g;
    MeshFilter meshFilter;

    Vector3[] vertices;
    List<Node> cubesNodes = new List<Node>();
    public List<Node> currentSelected = new List<Node>();

    private bool isActive = false;

    delegate float moveDirection();
    delegate void updateNodeNum();


    public int num {
        get {
            return Num;
        }
       
        set {
           Num = clamp(value, 0, (int)Mathf.Pow((float)NodeRes,2)-1);
        }
    }

    private int Num;

    public int NodeRes;
    // Start is called before the first frame update
    public void initilization(int CameraNum, float Xtiling, float Ytiling,float Xoffset,float Yoffset)
    {
        c_layerMask = CameraNum + 9;

        

        this.gameObject.layer = c_layerMask;

       // Debug.Log(this.gameObject.layer);

        GetComponent<Camera>().cullingMask = 1 << this.gameObject.layer;
        GetComponent<Camera>().targetDisplay = CameraNum + 1;

        DrawMesh(meshPrefabs,Xtiling,Ytiling,Xoffset,Yoffset);
        UpdateNodePosition(() => num =0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }


    void DrawMesh(GameObject gameObject, float Xtiling, float Ytiling, float Xoffset, float Yoffset) {
        g = Instantiate(meshPrefabs);
        g.transform.SetParent(this.transform);

        //Debug.Log(Xtiling + " " + Ytiling + " " + Xoffset + " " + Yoffset);

        g.layer = g.transform.parent.gameObject.layer;

        meshFilter = g.GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        vertices = mesh.vertices;




        int id = 0;
        foreach (var item in vertices)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(this.transform);
            cube.layer = cube.transform.parent.gameObject.layer;
            Node node =  cube.AddComponent<Node>();
            id++;
            node.initializtion(id,this);

            cubesNodes.Add(node);
            cube.transform.position = item;
            cube.transform.localScale = Vector3.one * 0.1f;
            cube.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        cubesNodes[num].GetComponent<MeshRenderer>().material.color = Color.red;
        NodeRes =(int)Mathf.Sqrt(cubesNodes.Count);
        Settitling(Xtiling, Ytiling);
        SetOffset(Xoffset, Yoffset);
    }


    void UpdateMesh() {
        if (isActive) {
            if (Input.GetKey(KeyCode.RightArrow))
            {

                movement(() => vertices[num].x - moveDistance, KeyCode.RightArrow);

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                movement(() => vertices[num].x + moveDistance, KeyCode.LeftArrow);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                movement(() => vertices[num].z - moveDistance, KeyCode.UpArrow);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                movement(() => vertices[num].z + moveDistance, KeyCode.DownArrow);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                UpdateNodePosition(() => num++);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                UpdateNodePosition(() => num--);
            }

            else if (Input.GetKeyDown(KeyCode.W))
            {
                UpdateNodePosition(() => num = num + NodeRes);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {


                UpdateNodePosition(() => num = num - NodeRes);
            }

            if (moveMode == MoveMode.Move_H && Input.GetKeyDown(KeyCode.F1))
            {
                cubesNodes[num].setVerticeAlignment_H(num, currentSelected);
            }
            else if (moveMode == MoveMode.Move_V && Input.GetKeyDown(KeyCode.F1))
            {
                cubesNodes[num].setVerticeAlignment_V(num, currentSelected);
            }
            else if (Input.GetKeyDown(KeyCode.F1)) {

                AlignmentAll();
            }
        }

    }


    private List<Node> GetRow(int rowNun) {
        List<Node> temp = new List<Node>();


        for (int j = 0; j < NodeRes; j++)
        {
            int _num = rowNun * NodeRes + j;

            temp.Add(cubesNodes[_num]);

            //cubesNodes[_num].SetColor(Color.black);
        }
        return temp;
    }


    private List<Node> GetColumn(int ColumnNum) {
        List<Node> temp = new List<Node>();

        for (int j = 0; j < NodeRes; j++)
        {
            int _num = j * NodeRes + ColumnNum;

            temp.Add(cubesNodes[_num]);

            //cubesNodes[_num].SetColor(Color.black);
        }
        return temp;
    }

    delegate List<Node> getRowOrCloumn(int val);
    enum xz { x,z}
   
    private float ReturnMinORMax(bool b, getRowOrCloumn _getRowOrCloumn, xz _xz)
    {
        List<Node> tempFirst = new List<Node>();
        List<float> values = new List<float>();

        if (b) {
            tempFirst = _getRowOrCloumn(0);
        }
        else {
            tempFirst = _getRowOrCloumn(NodeRes - 1);
        }
           
        foreach (Node item in tempFirst)
        {
            switch (_xz)
            {
                case xz.x:
                    values.Add(item.transform.position.x);
                    break;
                case xz.z:
                    values.Add(item.transform.position.z);
                    break;
                default:
                    break;
            }
          
        }



        values.Sort();

        if (b)
        {

            return values[0];
        }
        else {
            values.Reverse();
            
            return values[0];
        }
    }




    private void AlignmentAll() {
        List<Node> temp = new List<Node>();
        float minZ, maxZ;
        getRowOrCloumn getROW = new getRowOrCloumn(GetRow);
        getRowOrCloumn getCOLUMN = new getRowOrCloumn(GetColumn);

        minZ= Mathf.Abs( ReturnMinORMax(true, getROW,xz.z));
        maxZ =-Mathf.Abs( ReturnMinORMax(false, getROW,xz.z));

       

        float value =(maxZ -minZ)/(NodeRes-1);



        for (int i = 0; i < NodeRes; i++)
        {
            temp = GetRow(i);

            foreach (var item in temp)
            {
                Vector3 M_vector3 = new Vector3(item.transform.position.x, item.transform.position.y, minZ + value * i);

                item.SetVertices(M_vector3, item.Id - 1);
            }
        }


        float minX, maxX;
        minX = ReturnMinORMax(true, getCOLUMN, xz.x);
        maxX = ReturnMinORMax(false, getCOLUMN, xz.x);
        value = (maxX - minX) / (NodeRes - 1);

        for (int i = 0; i < NodeRes; i++)
        {
            temp = GetColumn(i);

            foreach (var item in temp)
            {
                Vector3 M_vector3 = new Vector3(minX+value*i , item.transform.position.y, item.transform.position.z);

                item.SetVertices(M_vector3, item.Id - 1);
            }
        }
    }



    void movement(moveDirection MoveDirection,KeyCode keyCode)
    {

        Vector3 v = vertices[num];
        if (keyCode == KeyCode.RightArrow || keyCode == KeyCode.LeftArrow) {
            v.x = MoveDirection();
        }
        else if(keyCode == KeyCode.UpArrow || keyCode == KeyCode.DownArrow)
        {
            v.z = MoveDirection();
        }




        cubesNodes[num].SetVertices(v,num);

        //cubesNodes[num].SetPosition(vertices[num]);
    }

    void UpdateNodePosition(updateNodeNum UpdateNodeNum) {
        foreach (Node item in currentSelected)
        {
            item.SetColor(Color.green);
        }

        cubesNodes[num].SetColor(Color.green);
        UpdateNodeNum();
        cubesNodes[num].SetColor(Color.blue);

        //updateNodeNearBy
        currentSelected = cubesNodes[num].GetNodes(num);
        foreach (Node item in currentSelected)
        {
            item.SetColor(Color.red);
        }

        UpdateWeight(currentSelected, cubesNodes[num]);

    }

    private void UpdateWeight(List<Node> nodes,Node nodeSelect) {
        foreach (Node item in nodes)
        {
            float weight = 0.7f/(item.transform.position - nodeSelect.transform.position).magnitude;

            item.setWeight(weight);
           
        }
    }

    public int clamp(int val, int min,int max) {

        if (val < min) {
            return min;
        } else if(val>max){
            return max;
        }

        return val;
    }


    public bool getIsActive() {
        return isActive;
    }

    public void setIsActive(bool b) {
        isActive = b;
        if (b)
        {
            foreach (var item in cubesNodes)
            {
                item.SetColor(Color.green);
            }
        }
        else {
            foreach (var item in cubesNodes)
            {
                item.SetColor(Color.grey);
            }
        }

    }

    public void Toggle_Left_Blending(bool b)
    {
        g.GetComponent<MeshRenderer>().material.SetInt("_L_Enable_Blending", Convert.ToInt32(b));
    }

    public void Toggle_Right_Blending(bool b)
    {
        g.GetComponent<MeshRenderer>().material.SetInt("_R_Enable_Blending", Convert.ToInt32(b));
    }
    public void Toggle_Top_Blending(bool b)
    {
        g.GetComponent<MeshRenderer>().material.SetInt("_T_Enable_Blending", Convert.ToInt32(b));
    }

    public void Toggle_Bottom_Blending(bool b)
    {
        g.GetComponent<MeshRenderer>().material.SetInt("_B_Enable_Blending", Convert.ToInt32(b));
    }


    public void Settitling(float xtitling, float ytitling) {
        g.GetComponent<MeshRenderer>().material.SetVector("_Tiling",new Vector4(xtitling, ytitling, 0,0));

    }
    public void SetOffset(float xOffset, float yOffset)
    {
        g.GetComponent<MeshRenderer>().material.SetVector("_Offset", new Vector4(xOffset, yOffset, 0, 0));

    }



    public class Node : MonoBehaviour {
        public int Id;
        public float weights;
        CreateMesh createMesh;

        //Vector3 dis; 

        public List<Node> GetNodes(int _num)
        {
            List<Node> theNodeNearBy = new List<Node>();
            if (createMesh.moveMode == MoveMode.singleMode)
            {
                theNodeNearBy.Clear();
            }
            else if (createMesh.moveMode == MoveMode.AreaMode) {
                theNodeNearBy.Clear();

                int currentC = ((_num + 1) % createMesh.NodeRes) == 0 ? createMesh.NodeRes : (_num + 1) % createMesh.NodeRes;
                int currentR = (Mathf.FloorToInt((_num + 1) / createMesh.NodeRes)) == createMesh.NodeRes ? createMesh.NodeRes - 1 : Mathf.FloorToInt((_num + 0.9999f) / createMesh.NodeRes);
                for (int row = -2; row <=2; row++)
                {
                    for (int column = -2; column <= 2; column++)
                    {
                       // Debug.Log(currentR);
                        if (currentC + column > 0 && currentC + column <= createMesh.NodeRes) {
                            if (currentR + row >= 0 && currentR + row < createMesh.NodeRes) {
                                int val = _num + column + createMesh.NodeRes * row;
                                if (val != _num)
                                {
                                    if (val >= 0)
                                    {
                                        theNodeNearBy.Add(createMesh.cubesNodes[val]);
                                        createMesh.cubesNodes[val].SetColor(Color.red);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (createMesh.moveMode == MoveMode.Move_H)
            {
                theNodeNearBy.Clear();
                int currentR = (Mathf.FloorToInt((_num + 1) / createMesh.NodeRes)) == createMesh.NodeRes ? createMesh.NodeRes - 1 : Mathf.FloorToInt((_num + 0.9999f) / createMesh.NodeRes);
                for (int i = currentR*createMesh.NodeRes; i < currentR * createMesh.NodeRes+ createMesh.NodeRes; i++)
                {
                    theNodeNearBy.Add(createMesh.cubesNodes[i]);
                    createMesh.cubesNodes[i].SetColor(Color.red);
                }

            }
            else if (createMesh.moveMode == MoveMode.Move_V)
            {
                theNodeNearBy.Clear();
                int currentC = ((_num + 1) % createMesh.NodeRes) == 0 ? createMesh.NodeRes : (_num + 1) % createMesh.NodeRes;
                for (int i = currentC-1; i < currentC+createMesh.NodeRes* (createMesh.NodeRes-1); i=i+createMesh.NodeRes)
                {
                    theNodeNearBy.Add(createMesh.cubesNodes[i]);
                    createMesh.cubesNodes[i].SetColor(Color.red);
                }
            }


            return theNodeNearBy;
        }

        public void initializtion(int id,CreateMesh _createMesh) {
            Id = id;
            weights = 1f;
            createMesh = _createMesh;
        }

        public void setWeight(float _weight) {
            weights = _weight;
        }


        public void SetVertices(Vector3 vector3,int _num) {

            switch (createMesh.moveMode)
            {
                case MoveMode.singleMode:
                    setVerticeSingle(vector3, _num);
                    break;
                case MoveMode.AreaMode:
                    setVerticeArea(vector3, _num, createMesh.currentSelected);

                    break;

                case MoveMode.Move_H:
                    setVerticeV_H(vector3, _num, createMesh.currentSelected);
                    break;

                case MoveMode.Move_V:
                    setVerticeV_H(vector3, _num, createMesh.currentSelected);
                    break;


                default:
                    break;
            }
        }






        private void setVerticeSingle(Vector3 vector3, int _num) {
            weights = 1f;
            createMesh.vertices[_num] = vector3 * weights;
            renderMesh();

            //cubePosition
            this.transform.position = createMesh.vertices[_num] * weights;


        }

        private void setVerticeArea(Vector3 vector3, int _num, List<Node> nodes) {
            weights = 1f;

            Vector3 dir = vector3 - createMesh.vertices[_num];
            createMesh.vertices[_num] = vector3 * weights;
            this.transform.position = createMesh.vertices[_num] * weights;

            foreach (var item in nodes)
            {
                createMesh.vertices[item.Id - 1] += dir * item.weights;
                item.transform.position += dir* item.weights;
            }

            renderMesh();
        }

        private void setVerticeV_H(Vector3 vector3, int _num,List<Node> nodes) {

            Vector3 dir = vector3 - createMesh.vertices[_num];
            foreach (Node item in nodes)
            {
                item.weights = 1;          
                createMesh.vertices[item.Id - 1] += dir * item.weights;
                item.transform.position += dir * item.weights;
            }
            renderMesh();
        }

        public void setVerticeAlignment_H(int _num, List<Node> nodes)
        {
            float z=0;

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].weights = 1;
                z += nodes[i].transform.position.z;
            }

            z = z / nodes.Count;

            foreach (Node item in nodes)
            {
                createMesh.vertices[item.Id - 1] = new Vector3(createMesh.vertices[item.Id - 1].x, createMesh.vertices[item.Id - 1].y, z);
                item.transform.position = new Vector3(createMesh.vertices[item.Id - 1].x, createMesh.vertices[item.Id - 1].y, z);
            }

            renderMesh();
        }



        public void setVerticeAlignment_V(int _num, List<Node> nodes)
        {
            float x = 0;

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].weights = 1;
                x += nodes[i].transform.position.x;
            }

            x = x / nodes.Count;

            foreach (Node item in nodes)
            {
                createMesh.vertices[item.Id - 1] = new Vector3(x, createMesh.vertices[item.Id - 1].y, createMesh.vertices[item.Id - 1].z);
                item.transform.position = new Vector3(x, createMesh.vertices[item.Id - 1].y, createMesh.vertices[item.Id - 1].z);
            }

            renderMesh();
        }






        public void renderMesh() {
            createMesh.mesh.vertices = createMesh.vertices;
            createMesh.mesh.RecalculateNormals();
            createMesh.mesh.MarkDynamic();
            createMesh.meshFilter.mesh = createMesh.mesh;
        }

        public void SetColor(Color color) {
         this.GetComponent<MeshRenderer>().material.color = color;

        }

    }


}
