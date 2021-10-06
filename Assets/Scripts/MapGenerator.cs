//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform Player;
    public GameObject MovingLimiter;
    public int MaxSpawnedChunks = 10;
    public int StraightChunksAfterTurn = 5;

    // all chunks
    public Chunk[] ChunkPrefabs;

    // chunks with turning
    public Chunk[] ChunkNoTurn;
    public Chunk[] ChunkLTurn;
    public Chunk[] ChunkRTurn;

    public List<Chunk> SpawnedChunks = new List<Chunk>();

    public char RoadDirection = 'f';
    public int DirectionCounter = 0;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // distance between player and limiter
        float Distance1 = Vector3.Distance(Player.transform.position, SpawnedChunks[0].Begin.transform.position);
        //float Distance1 = Vector3.Distance(Player.transform.position, MovingLimiter.transform.position);

        // distance between two last chunks
        float Distance2 = Vector3.Distance(SpawnedChunks[0].Begin.transform.position, SpawnedChunks[2].End.transform.position);
        

        //Debug.Log("D1 = " + Distance1 + " D2 = " + Distance2);

        if (Distance1 > Distance2 + 75.0f && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isAlive)
            //|| Vector3.Distance(Player.transform.position, SpawnedChunks[SpawnedChunks.Count].End.transform.position) 
            //<= Vector3.Distance(SpawnedChunks[SpawnedChunks.Count].Begin.transform.position, SpawnedChunks[SpawnedChunks.Count].End.transform.position))
        {
            SpawnChunk();
            MovingLimiter.transform.position = SpawnedChunks[1].End.transform.position;
        }
    }

    private void SpawnChunk()
    {
        Chunk NewChunk;
        //char LastTurn = 'u';
        //for (int i = SpawnedChunks.Count ; i >=  0; i--)
        //{
        //    if (SpawnedChunks[i].Direction != 'f')
        //    {
        //        LastTurn = SpawnedChunks[i].Direction;
        //    }
        //}

        if (RoadDirection == 'f')
        {
            if (DirectionCounter == 0)
            {
                NewChunk = Instantiate(ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length)]);
            }
            else
            {
                --DirectionCounter;
                NewChunk = Instantiate(ChunkNoTurn[Random.Range(0, ChunkNoTurn.Length)]);
            }
            NewChunk.transform.position = SpawnedChunks[SpawnedChunks.Count - 1].End.position - NewChunk.Begin.localPosition;
            RoadDirection = NewChunk.Direction;
        }
        else if (RoadDirection == 'l')
        {
            if (DirectionCounter == 0)
                DirectionCounter = StraightChunksAfterTurn;
            else
                --DirectionCounter;

            if (DirectionCounter == 0)
            {
                NewChunk = Instantiate(ChunkRTurn[Random.Range(0, ChunkRTurn.Length)]);
                RoadDirection = 'f';
                DirectionCounter = StraightChunksAfterTurn;
            }
            else
                NewChunk = Instantiate(ChunkNoTurn[Random.Range(0, ChunkNoTurn.Length)]);

            NewChunk.transform.rotation = Quaternion.Euler(0, -90, 0);
            NewChunk.transform.position = new Vector3(
                SpawnedChunks[SpawnedChunks.Count - 1].End.position.x + NewChunk.Begin.localPosition.z,
                0,
                SpawnedChunks[SpawnedChunks.Count - 1].End.position.z);
        }
        else if (RoadDirection == 'r')
        {
            if (DirectionCounter == 0)
                DirectionCounter = StraightChunksAfterTurn;
            else
                --DirectionCounter;

            if (DirectionCounter == 0)
            {
                NewChunk = Instantiate(ChunkLTurn[Random.Range(0, ChunkLTurn.Length)]);
                RoadDirection = 'f';
                DirectionCounter = StraightChunksAfterTurn;
            }
            else
            {
                NewChunk = Instantiate(ChunkNoTurn[Random.Range(0, ChunkNoTurn.Length)]);
                //Debug.LogError("Road direction error");
            }

            NewChunk.transform.rotation = Quaternion.Euler(0, 90, 0);
            NewChunk.transform.position = new Vector3(
                SpawnedChunks[SpawnedChunks.Count - 1].End.position.x - NewChunk.Begin.localPosition.z,
                0,
                SpawnedChunks[SpawnedChunks.Count - 1].End.position.z);
        }
        else
        {
            Debug.LogError("Road direction undefined");
            NewChunk = ChunkPrefabs[0];
        }

        SpawnedChunks.Add(NewChunk);

        if (SpawnedChunks.Count > MaxSpawnedChunks)
        {
            Destroy(SpawnedChunks[0].gameObject);
            SpawnedChunks.RemoveAt(0);
        }
    }
}
