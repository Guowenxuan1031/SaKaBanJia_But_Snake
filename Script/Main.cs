using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Main : MonoBehaviour
{
    private static int XMax = 22;
    private static int YMax = 12;
    private static int fishLength = 9;

    private int appleX;
    private int appleY;
    private int[,] GameGrid = new int [XMax,YMax];
    private int[] order = new int[XMax*YMax];
    private int[,] GamePos = new int[XMax*YMax+1,2];
    private float timeCounterUpdate = 0f;
    private float timeDelta = 0.3f;
    private GameObject[] GameObj = new GameObject[XMax*YMax];
    public GameObject Square;
    public GameObject Apple;
    public GameObject LoseSquare;

    public TextMeshProUGUI textObject;

    public TextMeshProUGUI textObject1;


    private bool rotationLock = false;
    private bool loseLock = false;
    private bool moveLock = false;
    // public Transform pos;
    // Start is called before the first frame update
    //        下   左    右    上
    // 尾巴   1    2     3     4
    // 身体   11   12    13    14
    // 转弯  21下左 22左上 23上右 24右下
    // 头     31   32    33    34
    // 转弯2 
    void Start()
    {

        for(int r = 0;r < fishLength;r++)
        {
            if(r == 0)
            {
                order[0] = 32;
                GameGrid[5,5] = 32;
            }else if(r == fishLength-1)
            {
                order[r] = 2;
                GameGrid[5+r,5] = 2;
            }else
            {
                order[r] = 12;
                GameGrid[5+r,5] = 12;
            }

            
            
            
            
            GamePos[r,0] = 5+r;
            GamePos[r,1] = 5;
            GameObj[r] = GameObject.Instantiate(Square, new Vector3((r+5)*30, 5*30, 0),new Quaternion(0, 0, 0, 0)) as GameObject;

            LoadPicture(GameObj[r], order[r].ToString());
            MoveApple();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove_KeyTransform();
        if(!loseLock && Time.time - timeCounterUpdate >= timeDelta)
        {
            UpdateGame();
            timeCounterUpdate = Time.time;
        }
    }

    void UpdateGame()
    {
        StringBuilder builder = new StringBuilder(1500);

        
        int lastX = 0;
        int lastY = 0;
        for(int h = fishLength-1;h >= 0;h--)
        {
            if(h != 0)
            {
                if(h == fishLength - 1)
                {
                    lastX = GamePos[h,0];
                    lastY = GamePos[h,1];
                }
                GamePos[h,0] = GamePos[h-1,0];
                GamePos[h,1] = GamePos[h-1,1];
                
            }else
            {
                if(order[0] == 32)
                {
                    GamePos[0,0] = GamePos[0,0] - 1;
                }else if(order[0] == 31)
                {
                    GamePos[0,1] = GamePos[0,1] - 1;
                }else if(order[0] == 33)
                {
                    GamePos[0,0] = GamePos[0,0] + 1;
                }else if(order[0] == 34)
                {
                    GamePos[0,1] = GamePos[0,1] + 1;
                }

                if(GamePos[0,0] < 1 || GamePos[0,0] > XMax-2 || GamePos[0,1] < 1 || GamePos[0,1] > YMax-2)
                {
                    GameLose();
                    loseLock = true;
                }
                
                if(GameGrid[GamePos[0,0],GamePos[0,1]] == 100)
                {
                    GameGrid[lastX, lastY] = 99;
                    moveLock = false;
                    MoveApple();
                    GameGrid[GamePos[0,0],GamePos[0,1]] = 0;
                    GameObj[fishLength] = GameObject.Instantiate(Square, new Vector3(lastX*30, lastY*30, 0),new Quaternion(0, 0, 0, 0)) as GameObject;
                    GamePos[fishLength,0] = lastX;
                    GamePos[fishLength,1] = lastY;
                    order[fishLength] = order[fishLength-1];
                    order[fishLength-1] = 10 + order[fishLength-1]%10;
                    fishLength += 1;
                }else if(GameGrid[GamePos[0,0],GamePos[0,1]] != 0)
                {
                    GameLose();
                    loseLock = true;
                }
            }
            
            GameGrid[GamePos[h,0],GamePos[h,1]] = order[h];
            GameGrid[lastX, lastY] = 0;
            GameObj[h].transform.position = new Vector3(GamePos[h,0]*30, GamePos[h,1]*30, 0);
            // print(GamePos[h,0] + " ");
            // print(GamePos[h,1] + " ");
            // LoadPicture(GameObj[h], order[h].ToString());

            if((order[h] / 10 == 2) || (order[h] / 10 == 4))
            {
                if(order[h+1] > 5)
                {
                    order[h+1] = order[h];
                }else
                {
                    order[h+1] = order[h] % 10;
                }
                order[h] = 10 + order[h] % 10;
                
                // print("yes");
                
            }
            
        }
        rotationLock = false;

        // for(int a = 0;a < fishLength;a++)
        // {
            
        //     if(a != 0 && order[a]%10 != order[a-1]%10)
        //     {
        //         if(a-1 == 0)
        //         {
        //             order[a] = 20 + order[a-1] % 10;
        //         }
        //         // break;
        //     }


        //     // print(order)
            
        // }
        if(order[0]%10 != order[1]%10)
        {
            if(order[0]%10 == 1)
            {
                if(order[1]%10 == 2)
                {
                    order[1] = 41;
                }else if(order[1]%10 == 3)
                {
                    order[1] = 21;
                }
            }
            if(order[0]%10 == 2)
            {
                if(order[1]%10 == 1)
                {
                    order[1] = 42;
                }else if(order[1]%10 == 4)
                {
                    order[1] = 22;
                }
            }
            if(order[0]%10 == 3)
            {
                if(order[1]%10 == 1)
                {
                    order[1] = 43;
                }else if(order[1]%10 == 4)
                {
                    order[1] = 23;
                }
            }
            if(order[0]%10 == 4)
            {
                if(order[1]%10 == 2)
                {
                    order[1] = 44;
                }else if(order[1]%10 == 3)
                {
                    order[1] = 24;
                }
            }
            
            // if(order[0]%10 > order[1]%10 || (order[0]%10 == 4 && order[1]%10 == 1))
            // {
            //     order[1] = 40 + order[0] % 10;
            // }else
            // {
            //     order[1] = 20 + order[0] % 10;
            // }
        }

        // if((order[fishLength-1]%10 != order[fishLength-2]%10) && (order[fishLength-2] / 10 != 2) && (order[fishLength-2] / 10 != 4))
        // {
        //     order[fishLength - 1] = order[fishLength - 2]%10;
        // }

        for(int y=0;y < fishLength;y++)
        {
            // print(order[y]);
            LoadPicture(GameObj[y], order[y].ToString());
        }
        
        // print(" ");




        // GameObj[GamePos[fishLength-1,0],GamePos[fishLength-1,1]] = order[fishLength-1];
        // for(int j = 0;j < YMax;j++)
        
        // {
        //     for(int i = 0;i < XMax;i++)
        //     {
        //         builder.Append(GameGrid[i,j].ToString() + " ");
                    
        //     }
        //     builder.Append("\n");
        // }
        builder.Append("Points:");
        builder.Append(fishLength.ToString());
        textObject.text = builder.ToString();
    }

    void GameLose()
    {
        order[0] = order[0] % 10 + 300;
        LoseSquare.active = true;
        StringBuilder builder1 = new StringBuilder(1500);
        print("Lose!");
        if(PlayerPrefs.GetInt("HIGH",0) < fishLength)
        {
            PlayerPrefs.SetInt("HIGH",fishLength);
            builder1.Append("NEW RECORD!!\n");
        }else
        {
            builder1.Append("HIGHEST:" +PlayerPrefs.GetInt("HIGH",0).ToString() + "\n");
        }
        builder1.Append("GAME OVER\nYOUR SCORE\n" + fishLength.ToString());
        textObject1.text = builder1.ToString();
    }

    void LoadPicture(GameObject obj,string str)
    {
        SpriteRenderer sr_des = obj.transform.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load(str, typeof(Sprite)) as Sprite;
        sr_des.sprite = sprite;
    }

    public void PlayerMove_KeyTransform()
    {
        if (!rotationLock && (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.UpArrow)) && order[0] != 31) //前
        {
            order[0] = 34;
            rotationLock = true;
        }
        if (!rotationLock && (Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.DownArrow)) && order[0] != 34) //后
        {
            order[0] = 31;
            rotationLock = true;
        }
        if (!rotationLock && (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)) && order[0] != 33) //左
        {
            order[0] = 32;
            rotationLock = true;
        }
        if (!rotationLock && (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow)) && order[0] != 32) //右
        {
            order[0] = 33;
            rotationLock = true;
        }
    }

    void MoveApple()
    {
        for(int g = 0;g < 30;g++)
        {
            appleX = Random.Range(1, XMax-2);
            appleY = Random.Range(1, YMax-2);
            if(!moveLock && GameGrid[appleX,appleY] == 0)
            {
                GameGrid[appleX,appleY] = 100;
                print("yes");
                Apple.transform.position = new Vector3(appleX*30,appleY*30, 0);
                moveLock = true;
                return;
                // break;
            }
            
        }
        for(int i = 1;i < XMax-1;i++)
        {
            for(int j = 1;j < YMax-1;j++)
            {
                if(!moveLock && GameGrid[i,j] == 0)
                {
                    appleX = i;
                    appleY = j;
                    GameGrid[appleX,appleY] = 100;
                    print("yes");
                    Apple.transform.position = new Vector3(appleX*30,appleY*30, 0);
                    moveLock = true;
                    return;
                    // break;
                }
            }
        }
        
    }
}
