using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    public int x;
    public int y;
    public Vector2 pos;
    public MyGrid parent;
    public bool iswall = false;
    public int gCost;
    public int hCost;
    public int fCost
    {
        get { return gCost + hCost; }
    }

    public void SetInfo(int x, int y, Vector2 pos)
    {
        this.x = x;
        this.y = y;
        this.pos = pos;
        iswall = false;
        parent = null;
        gCost = 0;
        hCost = 0;
    }

    public void SetWall()
    {
        iswall = true;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void SetRoad()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void SetOrigin()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        iswall = false;
    }

    public void SetStart()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    private void OnMouseDown()
    {
        if(Input.GetKey(KeyCode.W))
        {
            if (!iswall)
                SetWall();
            else
                SetOrigin();
            return;
        }
        if (iswall)
            return;
        GameManager.Instance.SetGrid(this);
    }
}
