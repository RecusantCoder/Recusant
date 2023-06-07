using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    public GameObject[] sectionPrefabs;
    public GameObject[] collidePrefabs;
    private Transform playerTransform;
    private Vector2 spawn = new Vector2(0f, 0f);
    private float sectionLength = 10.24f;
    private bool startPoint = false;
    private Dictionary<Vector2, GameObject> spawnedSections = new Dictionary<Vector2, GameObject>();

    enum SectionDirection { Right, TopRight, Top, TopLeft, Left, BottomLeft, Bottom, BottomRight, Centre }

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

    // Sets the spawn point of Sections properly as the player moves
    private void TrackPlayer()
    {
        // Calculate the position of the player in section coordinates
        int playerSectionX = Mathf.FloorToInt(playerTransform.position.x / sectionLength);
        int playerSectionY = Mathf.FloorToInt(playerTransform.position.y / sectionLength);

        // Iterate through all the spawned sections
        foreach (var kvp in spawnedSections)
        {
            Vector2 sectionPosition = kvp.Key;
            GameObject section = kvp.Value;

            // Calculate the position of the section in section coordinates
            int sectionX = Mathf.FloorToInt(sectionPosition.x / sectionLength);
            int sectionY = Mathf.FloorToInt(sectionPosition.y / sectionLength);

            // Calculate the distance between the player section and the current section
            int distanceX = Mathf.Abs(playerSectionX - sectionX);
            int distanceY = Mathf.Abs(playerSectionY - sectionY);

            // Check if the section is within the desired distance range (e.g., radius of 2 tiles)
            if (distanceX <= 2 && distanceY <= 2)
            {
                // Enable the section
                section.SetActive(true);
            }
            else
            {
                // Disable the section
                section.SetActive(false);
            }
        }
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
            spawnedSections.Add(spawn, centre);
        }
        else
        {
            Vector2 spawnTemp = spawn;
            switch (sd)
            {
                case SectionDirection.Right:
                    // right
                    spawn.x += sectionLength;
                    break;
                case SectionDirection.TopRight:
                    // top right
                    spawn.x += sectionLength;
                    spawn.y += sectionLength;
                    break;
                case SectionDirection.Top:
                    // top
                    spawn.y += sectionLength;
                    break;
                case SectionDirection.TopLeft:
                    // top left
                    spawn.x -= sectionLength;
                    spawn.y += sectionLength;
                    break;
                case SectionDirection.Left:
                    // left
                    spawn.x -= sectionLength;
                    break;
                case SectionDirection.BottomLeft:
                    // bottom left
                    spawn.x -= sectionLength;
                    spawn.y -= sectionLength;
                    break;
                case SectionDirection.Bottom:
                    // bottom
                    spawn.y -= sectionLength;
                    break;
                case SectionDirection.BottomRight:
                    // bottom right
                    spawn.x += sectionLength;
                    spawn.y -= sectionLength;
                    break;
                default:
                    Debug.Log("SectionDirection Default");
                    break;
            }

            if (spawnedSections.ContainsKey(spawn))
            {
                GameObject section = spawnedSections[spawn];
                section.SetActive(true);
            }
            else
            {
                GameObject randomSectionPrefab = sectionPrefabs[Random.Range(0, sectionPrefabs.Length)];
                GameObject spawnedSection = Instantiate(randomSectionPrefab) as GameObject;
                spawnedSection.transform.SetParent(transform);
                spawnedSection.transform.position = spawn;
                spawnedSections.Add(spawn, spawnedSection);
                
                // Spawn 2-5 random collidePrefabs within the area of the sectionPrefab
                int numberOfCollidePrefabs = Random.Range(2, 6);
                for (int i = 0; i < numberOfCollidePrefabs; i++)
                {
                    GameObject randomCollidePrefab = collidePrefabs[Random.Range(0, collidePrefabs.Length)];
                    GameObject spawnedCollidePrefab = Instantiate(randomCollidePrefab) as GameObject;
                    spawnedCollidePrefab.transform.SetParent(spawnedSection.transform);
                    // Set the position of the spawned collidePrefab within the area of the sectionPrefab
                    Vector2 randomPosition = new Vector2(Random.Range(-sectionLength / 2f, sectionLength / 2f), Random.Range(-sectionLength / 2f, sectionLength / 2f));
                    spawnedCollidePrefab.transform.localPosition = randomPosition;
                }
            }

            spawn = spawnTemp;
        }
    }
}
