using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject grid;
    public float startX;
    public float startY;
    private float grab = 1f;
    private GameObject parent;
    private MyGrid[,] map;
    private int row = 10;
    private int col = 20;

    public MyGrid grid1 = null;
    public MyGrid grid2 = null;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        parent = GameObject.Find("parent");
        map = new MyGrid[col, row];
        for(int i = 0; i < col; i++)
        {
            for(int j = 0; j < row; j++)
            {
                Vector2 pos = new Vector2(startX + grab * i, startY - grab * j);
                GameObject go = Instantiate(grid, pos, Quaternion.identity) as GameObject;
                go.transform.SetParent(parent.transform);
                map[i, j] = go.GetComponent<MyGrid>();
                map[i, j].SetInfo(i, j, pos);
                if (i == 0 || j == 0 || i == col - 1 || j == row - 1)
                {
                    map[i, j].SetWall();
                }
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetOrigin();
        }
    }

    void SetOrigin()
    {
        grid1 = null;
        grid2 = null;
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (i == 0 || i == col - 1 || j == 0 || j == row - 1)
                    continue;
                else
                    map[i, j].SetOrigin();
            }
        }
    }

    public void Astar(MyGrid start, MyGrid end)
    {
        List<MyGrid> open = new List<MyGrid>();
        List<MyGrid> close = new List<MyGrid>();
        open.Add(start);
        while(open.Count > 0)
        {
            MyGrid cur = GetMin(open);
            open.Remove(cur);
            if(cur == end)
            {
                PaintRoad(start, end);
                return;
            }
            close.Add(cur);
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    int newX = cur.x + i;
                    int newY = cur.y + j;
                    MyGrid temp = map[newX, newY];
                    if (temp.iswall || close.Contains(temp))
                        continue;
                    int newCost = cur.gCost + GetDis(cur.x, cur.y, temp.x, temp.y);
                    if(newCost < temp.gCost || !open.Contains(temp))
                    {
                        temp.gCost = newCost;
                        temp.hCost = GetDis(temp.x, temp.y, end.x, end.y);
                        temp.parent = cur;
                        if(!open.Contains(temp))
                        {
                            open.Add(temp);
                        }
                    }
                }
            }
        }
        SetOrigin();
    }

    MyGrid GetMin(List<MyGrid> open)
    {
        MyGrid cur = open[0];
        foreach(var node in open)
        {
            if(node.fCost <= cur.fCost && node.hCost < cur.hCost)
            {
                cur = node;
            }
        }
        return cur;
    }

    int GetDis(int sx, int sy, int ex, int ey)
    {
        int disx = Mathf.Abs(sx - ex);
        int disy = Mathf.Abs(sy - ey);
        if(disx >= disy)
        {
            return (disx - disy) * 10 + disy * 14;
        }
        else
        {
            return (disy - disx) * 10 + disx * 14;
        }
    }

    void PaintRoad(MyGrid start, MyGrid end)
    {
        while(end != start)
        {
            end.SetRoad();
            end = end.parent;
        }
        end.SetRoad();
    }

    public void SetGrid(MyGrid grid)
    {
        if(grid1 == null)
        {
            grid1 = grid;
            grid1.SetStart();
            return;
        }
        if(grid1 == grid)
        {
            grid1.SetOrigin();
            grid1 = null;
            return;
        }
        grid2 = grid;
        Astar(grid1, grid2);
        grid1 = null;
        grid2 = null;
    }
}
