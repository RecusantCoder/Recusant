using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    public GameObject[] sectionPrefabs;
    private Transform playerTransform;
    private Vector2 spawn = new Vector2(0f, 0f);
    private float sectionLength = 10.24f;
    //private int numOfSectionsOnScreen = 3;
    private bool startPoint = false;
    
    enum SectionDirection {Right, TopRight, Top, TopLeft, Left, BottomLeft, Bottom, BottomRight, Centre}
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnSection(SectionDirection.Centre);
        
        SpawnSection(SectionDirection.Right);
        
        SpawnSection(SectionDirection.TopRight);
        SpawnSection(SectionDirection.Top);
        SpawnSection(SectionDirection.TopLeft);
        
        SpawnSection(SectionDirection.Left);
        
        SpawnSection(SectionDirection.BottomLeft);
        SpawnSection(SectionDirection.Bottom);
        SpawnSection(SectionDirection.BottomRight);
        
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
    }

    //Sets the spawn point of Sections properly as the player moves
    private void TrackPlayer()
    {
        if (playerTransform.position.x > (spawn.x + sectionLength))
        {
            spawn.x += sectionLength;
            SpawnSection(SectionDirection.TopRight);
            SpawnSection(SectionDirection.Right);
            SpawnSection(SectionDirection.BottomRight);
        }
        if (playerTransform.position.x < (spawn.x - sectionLength))
        {
            spawn.x -= sectionLength;
            SpawnSection(SectionDirection.TopLeft);
            SpawnSection(SectionDirection.Left);
            SpawnSection(SectionDirection.BottomLeft);
        }
        if (playerTransform.position.y > (spawn.y + sectionLength))
        {
            spawn.y += sectionLength;
            SpawnSection(SectionDirection.TopLeft);
            SpawnSection(SectionDirection.Top);
            SpawnSection(SectionDirection.TopRight);
        }
        if (playerTransform.position.y < (spawn.y - sectionLength))
        {
            spawn.y -= sectionLength;
            SpawnSection(SectionDirection.BottomLeft);
            SpawnSection(SectionDirection.Bottom);
            SpawnSection(SectionDirection.BottomRight);
        }
        //Debug.Log("X: " + playerTransform.position.x + "  Y: " + playerTransform.position.y);
        //Debug.Log("SpawnX: " + spawn.x + "  SpawnY: " + spawn.y);
    }

    private void SpawnSection(SectionDirection sd)
    {
        if (!startPoint)
        {
            GameObject centre;
            centre = Instantiate(sectionPrefabs[0]) as GameObject;
            centre.transform.SetParent(transform);
            spawn = playerTransform.position;
            centre.transform.position = spawn;
            startPoint = true;
        }
        else
        {
            Vector2 spawnTemp = spawn;
            switch (sd)
            {
                case SectionDirection.Right:
                //right
                    GameObject right;
                    right = Instantiate(sectionPrefabs[0]) as GameObject;
                    right.transform.SetParent(transform);
                    spawn.x += sectionLength;
                    right.transform.position = spawn;
                    spawn = spawnTemp;
                    break;
                case SectionDirection.TopRight:
                    //top right
                    GameObject topRight;
                    topRight = Instantiate(sectionPrefabs[0]) as GameObject;
                    topRight.transform.SetParent(transform);
                    spawn.x += sectionLength;
                    spawn.y += sectionLength;
                    topRight.transform.position = spawn;
                    spawn = spawnTemp;
                    break;
                case SectionDirection.Top:
                    //top
                    GameObject top;
                    top = Instantiate(sectionPrefabs[0]) as GameObject;
                    top.transform.SetParent(transform);
                    spawn.y += sectionLength;
                    top.transform.position = spawn;
                    spawn = spawnTemp;
                    break;
                case SectionDirection.TopLeft:
                    //top left
                    GameObject topLeft;
                    topLeft = Instantiate(sectionPrefabs[0]) as GameObject;
                    topLeft.transform.SetParent(transform);
                    spawn.x -= sectionLength;
                    spawn.y += sectionLength;
                    topLeft.transform.position = spawn;
                    spawn = spawnTemp;
                    break;
                case SectionDirection.Left:
                    //left
                    GameObject left;
                    left = Instantiate(sectionPrefabs[0]) as GameObject;
                    left.transform.SetParent(transform);
                    spawn.x -= sectionLength;
                    left.transform.position = spawn;
                    spawn = spawnTemp;
                    break;
                case SectionDirection.BottomLeft:
                    //bottom left
                    GameObject bottomLeft;
                    bottomLeft = Instantiate(sectionPrefabs[0]) as GameObject;
                    bottomLeft.transform.SetParent(transform);
                    spawn.x -= sectionLength;
                    spawn.y -= sectionLength;
                    bottomLeft.transform.position = spawn;
                    spawn = spawnTemp;
                    break;
                case SectionDirection.Bottom:
                    //bottom
                    GameObject bottom;
                    bottom = Instantiate(sectionPrefabs[0]) as GameObject;
                    bottom.transform.SetParent(transform);
                    spawn.y -= sectionLength;
                    bottom.transform.position = spawn;
                    spawn = spawnTemp;
                    break;
                case SectionDirection.BottomRight:    
                    //bottom right
                    GameObject bottomRight;
                    bottomRight = Instantiate(sectionPrefabs[0]) as GameObject;
                    bottomRight.transform.SetParent(transform);
                    spawn.x += sectionLength;
                    spawn.y -= sectionLength;
                    bottomRight.transform.position = spawn;
                    spawn = spawnTemp;
                    break;
                default:
                    Debug.Log("SectionDirection Default");
                    break;
            }
        }
    }
}
