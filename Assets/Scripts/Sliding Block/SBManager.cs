using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBManager : MonoBehaviour
{
    public Tile tilePrefab;

    private List<Tile> tileSet = new List<Tile>();
    private List<Vector3> tilePositions = new List<Vector3>();
    private HashSet<int> randomNumberSet = new HashSet<int>();

    private Vector2 startPosition = new Vector2(-4.60f, 2.65f); // Modify this to adjust where the top-left most tile starts
    private Vector2 offset = new Vector2(3.038f, 1.735f); // Modify this to change how much the tiles are spaced apart

    public LayerMask collisionMask; // For moving the tiles

    Ray leftRay, rightRay, upRay, downRay;
    RaycastHit hit;

    private BoxCollider collider;
    private Vector2 colliderSize;
    private Vector2 colliderCenter;

    private bool puzzleCompleted;
    private bool textBoxCompleted = false;

    public GameObject fullPicture;

    public bool blurEnabled;
    public GameObject blurBarrier;

    // Start is called before the first frame update
    void Start()
    {
        blurEnabled = true;
        fullPicture.SetActive(false);
        SpawnPuzzle(15);
        SetStartPositions();
        ApplySprite();
        ShuffleTiles();
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleCompleted)
        {
            if (textBoxCompleted)
            {
                GameObject.FindGameObjectsWithTag("Music")[0].GetComponent<AudioSource>().Stop();
                return;
            }
            else
            {
                StartCoroutine(EndGame());
            }
        }
        MoveTiles();
        CheckTilePositions();
        blurBarrier.SetActive(blurEnabled);

        if (Input.GetKeyDown(KeyCode.B))
        {
            PressBToWin();
        }
    }

    void SpawnPuzzle(int numTiles)
    {
        for (int i = 0; i < numTiles; i++)
        {
            Tile newTile = Instantiate(tilePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as Tile;
            newTile.transform.GetChild(0).gameObject.SetActive(false);
            tileSet.Add(newTile);
        }
    }

    void SetStartPositions()
    {
        // First line
        tileSet[0].transform.position = new Vector3(startPosition.x, startPosition.y, 0.0f);
        tileSet[1].transform.position = new Vector3(startPosition.x + offset.x, startPosition.y, 0.0f);
        tileSet[2].transform.position = new Vector3(startPosition.x + (2 * offset.x), startPosition.y, 0.0f);

        // Second line
        tileSet[3].transform.position = new Vector3(startPosition.x, startPosition.y - offset.y, 0.0f);
        tileSet[4].transform.position = new Vector3(startPosition.x + offset.x, startPosition.y - offset.y, 0.0f);
        tileSet[5].transform.position = new Vector3(startPosition.x + (2 * offset.x), startPosition.y - offset.y, 0.0f);
        tileSet[6].transform.position = new Vector3(startPosition.x + (3 * offset.x), startPosition.y - offset.y, 0.0f);

        // Third line
        tileSet[7].transform.position = new Vector3(startPosition.x, startPosition.y - (2 * offset.y), 0.0f);
        tileSet[8].transform.position = new Vector3(startPosition.x + offset.x, startPosition.y - (2 * offset.y), 0.0f);
        tileSet[9].transform.position = new Vector3(startPosition.x + (2 * offset.x), startPosition.y - (2 * offset.y), 0.0f);
        tileSet[10].transform.position = new Vector3(startPosition.x + (3 * offset.x), startPosition.y - (2 * offset.y), 0.0f);

        // Fourth line
        tileSet[11].transform.position = new Vector3(startPosition.x, startPosition.y - (3 * offset.y), 0.0f);
        tileSet[12].transform.position = new Vector3(startPosition.x + offset.x, startPosition.y - (3 * offset.y), 0.0f);
        tileSet[13].transform.position = new Vector3(startPosition.x + (2 * offset.x), startPosition.y - (3 * offset.y), 0.0f);
        tileSet[14].transform.position = new Vector3(startPosition.x + (3 * offset.x), startPosition.y - (3 * offset.y), 0.0f);
    }

    void MoveTiles()
    {
        foreach(Tile tile in tileSet)
        {
            tile.moveAmount = offset;
            if (tile.clicked)
            {
                collider = tile.GetComponent<BoxCollider>();
                colliderSize = collider.size;
                colliderCenter = collider.center;

                float moveAmount = offset.x;
                float direction = Mathf.Sign(moveAmount);

                // Draw vertical rays
                float xRay = (tile.transform.position.x + colliderCenter.x - colliderSize.x / 2) + (colliderSize.x / 2);
                float yUp = tile.transform.position.y + colliderCenter.y - colliderSize.y / 2 * direction;
                float yDown = tile.transform.position.y + colliderCenter.y - colliderSize.y / 2 * -direction;
                upRay = new Ray(new Vector2(xRay, yUp + 2.0f), new Vector2(0f, direction));
                downRay = new Ray(new Vector2(xRay, yDown - 2.0f), new Vector2(0f, -direction));
                Debug.DrawRay(upRay.origin, upRay.direction, Color.green, 10, false);
                Debug.DrawRay(downRay.origin, downRay.direction, Color.green, 10, false);

                // Draw horizontal rays
                float yRay = (tile.transform.position.y + colliderCenter.y - colliderSize.y / 2) + (colliderSize.y / 2);
                float xLeft = tile.transform.position.x + colliderCenter.x - colliderSize.x / 2;
                float xRight = tile.transform.position.x + colliderCenter.x - colliderSize.x / 2 * -1;
                leftRay = new Ray(new Vector2(xLeft, yRay), new Vector2(-direction, 0f));
                rightRay = new Ray(new Vector2(xRight, yRay), new Vector2(direction, 0f));
                Debug.DrawRay(leftRay.origin, leftRay.direction, Color.green, 10, false);
                Debug.DrawRay(rightRay.origin, rightRay.direction, Color.green, 10, false);

                bool upPossible = Physics.Raycast(upRay, out hit, 1.0f, collisionMask);
                bool downPossible = Physics.Raycast(downRay, out hit, 1.0f, collisionMask);
                bool leftPossible = Physics.Raycast(leftRay, out hit, 1.0f, collisionMask);
                bool rightPossible = Physics.Raycast(rightRay, out hit, 1.0f, collisionMask);

                // Check ray collisions
                if (!upPossible && (tile.moved == false) && (tile.transform.position.y < startPosition.y))
                {
                    tile.goUp = true;
                }
                if (!downPossible && (tile.moved == false) && (tile.transform.position.y > (startPosition.y - (2.95 * offset.y))))
                {
                    tile.goDown = true;
                }
                if (!leftPossible && (tile.moved == false) && (tile.transform.position.x > startPosition.x))
                {

                    tile.goLeft = true;
                }
                if (!rightPossible && (tile.moved == false) && (tile.transform.position.x < (startPosition.x + (3 * offset.x))))
                {
                    tile.goRight = true;
                }
            }
        }
    }

    void ApplySprite()
    {
        string filePath = "Sliding Block/";
        for (int i = 0; i < tileSet.Count; i++)
        {
            filePath = "Sliding Block/";
            if (i > 2)
            {
                filePath += (i + 1).ToString();
            } 
            else
            {
                filePath += i.ToString();
            }
            Sprite sprite2d = Resources.Load(filePath, typeof(Sprite)) as Sprite;
            tileSet[i].GetComponent<SpriteRenderer>().sprite = sprite2d;
        }
    }

    void ShuffleTiles()
    {
        foreach(Tile tile in tileSet)
        {
            tilePositions.Add(tile.transform.position);
        }
        foreach(Tile tile in tileSet)
        {
            int number = Random.Range(0, tileSet.Count);
            while (randomNumberSet.Contains(number))
            {
                number = Random.Range(0, tileSet.Count);
            }
            randomNumberSet.Add(number);
            tile.transform.position = tilePositions[number];
        }
    }

    void CheckTilePositions()
    {
        int correctPositions = 0;
        for (int i = 0; i < tilePositions.Count; i++)
        {
            if (tilePositions[i] == tileSet[i].transform.position)
            {
                tileSet[i].transform.GetChild(0).gameObject.SetActive(true);
                correctPositions++;
            } 
            else
            {
                tileSet[i].transform.GetChild(0).gameObject.SetActive(false);
                correctPositions--;
                if (correctPositions < 0)
                {
                    correctPositions = 0;
                }
            }
        }
        if (correctPositions == tilePositions.Count)
        {
            puzzleCompleted = true;
            blurEnabled = false;
            fullPicture.SetActive(true);
        }
    }

    void PressBToWin()
    {
        for (int i = 0; i < tilePositions.Count; i++)
        {
            tileSet[i].transform.position = tilePositions[i];
        }
    }

    IEnumerator EndGame()
    {
        textBoxCompleted = true;
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectsWithTag("EventSystem")[0].GetComponent<InteractionManager>().FireEvent(2);
    }
}
