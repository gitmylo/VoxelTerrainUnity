using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockClasses : MonoBehaviour
{
    public List<Block> world;

    public Vector2 startChunkPos, endChunkPos;

    public void GenerateWorld(int seed, float scale, int maxHeight, Vector2 startPos, Vector2 endPos, ChunkGenVariables vars)
    {
        startChunkPos = startPos;
        endChunkPos = endPos;
        if (seed == 0)
        {
            seed = 1;
        }
        
        world = new List<Block>();
        float startX = (int)Math.Min(startPos.x, endPos.x);
        float endX = (int)Math.Max(startPos.x, endPos.x);
        float startZ = (int)Math.Min(startPos.y, endPos.y);
        float endZ = (int)Math.Max(startPos.y, endPos.y);
        
        for (float x = startX; x < endX; x++)
        {
            for (float z = startZ; z < endZ; z++)
            {
                //float seedModX = seed * 24;
                //float seedModY = seed * 43;
                float height = GenerateXZ(x, z, seed, scale, vars);//Mathf.PerlinNoise((x + seedModX) * scale * 0.031f, (z + seedModY) * scale * 0.031f); 
                //AddBlock(new Block(new BlockVec((int)x, (int)(height * maxHeight), (int)z)), false);
                if (height >= 0)
                {
                    PlaceBlocks(new Block(new BlockVec((int)x, vars.bottom - 1, (int)z)), new BlockVec((int)x + 1, (int)(height * maxHeight) + vars.bottom + vars.minHeight, (int)z + 1), false);
                }
            }
        }
    }

    public float GenerateXZ(float x, float z, int seed, float scale, ChunkGenVariables vars)
    {
        float outValue = 0;
        for (int i = 0; i < vars.complexity; i++)
        {
            float seedModX = seed * (24 * i);
            float seedModY = seed * (43 * i);
            float value = Mathf.PerlinNoise((x + seedModX) * scale * 0.031f, (z + seedModY) * scale * 0.031f);
            
            outValue += value;
        }

        outValue /= vars.complexity;

        return outValue;
    }

    public void PlaceBlocks(Block blockAndStartPos, BlockVec endPos, bool replace)
    {
        int startX = Math.Min(blockAndStartPos.pos.x, endPos.x);
        int endX = Math.Max(blockAndStartPos.pos.x, endPos.x);
        int startY = Math.Min(blockAndStartPos.pos.y, endPos.y);
        int endY = Math.Max(blockAndStartPos.pos.y, endPos.y);
        int startZ = Math.Min(blockAndStartPos.pos.z, endPos.z);
        int endZ = Math.Max(blockAndStartPos.pos.z, endPos.z);
        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                for (int z = startZ; z < endZ; z++)
                {
                    Block blockToPlace = (Block)blockAndStartPos.Clone();
                    blockToPlace.pos = new BlockVec(x, y, z);
                    AddBlock(blockToPlace, replace);
                }
            }
        }
    }
    
    public void AddBlock(Block block, bool replace)
    {
        Block samePosBlock = GetBlockByPos(block.pos);
        if (samePosBlock != null)
        {
            if (replace)
                world[world.IndexOf(samePosBlock)] = block;
        }
        else
        {
            world.Add(block);
        }
    }

    public void RemoveBlock(BlockVec pos)
    {
        Block targetBlock = GetBlockByPos(pos);
        if (targetBlock != null)
            world.Remove(targetBlock);
    }
    public Block GetBlockByPos(BlockVec pos)
    {
        foreach (Block block in world)
        {
            if (pos.x == block.pos.x && pos.y == block.pos.y && pos.z == block.pos.z)
            {
                return block;
            }
        }

        return null;
    }

    public List<BlockVec.Direction> GetEmptyAround(BlockVec pos)
    {
        
        List<BlockVec.Direction> outDirs = new List<BlockVec.Direction>();
        foreach (BlockVec.Direction dir in Enum.GetValues(typeof(BlockVec.Direction)))
        {
            BlockVec checkPos = pos.Add(BlockVec.DirectionAsVec(dir));
            /*
            print(pos.x + " " + pos.y + " " + pos.z);
            print(checkPos.x + " " + checkPos.y + " " + checkPos.z);
            print(BlockVec.DirectionAsVec(dir).x + " " + BlockVec.DirectionAsVec(dir).y + " " + BlockVec.DirectionAsVec(dir).z);
            */
            if (GetBlockByPos(checkPos) == null)//when fixed, also check isInRange(checkPos, startChunkPos, endChunkPos)
            {
                outDirs.Add(dir);
            }
        }
        return outDirs;
    }

    public bool isInRange(BlockVec checkPos, Vector2 startPos, Vector2 endPos)
    {
        return (checkPos.x > startPos.x && checkPos.x < endPos.x && checkPos.z > startPos.y && checkPos.z < endPos.y);
    }
}

public class Block : ICloneable
{
    public BlockVec pos;

    public Block(BlockVec pos)
    {
        this.pos = pos;
    }
    
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class BlockVec
{
    public enum Direction
    {
        XPos, XNeg, YPos, YNeg, ZPos, ZNeg
    }

    public static BlockVec DirectionAsVec(Direction dir)
    {
        BlockVec outputVec = new BlockVec(0,0,0);
        switch (dir)
        {
            case Direction.XPos:
                outputVec = new BlockVec(1, 0, 0);
                break;
            case Direction.XNeg:
                outputVec = new BlockVec(-1, 0, 0);
                break;
            case Direction.YPos:
                outputVec = new BlockVec(0, 1, 0);
                break;
            case Direction.YNeg:
                outputVec = new BlockVec(0, -1, 0);
                break;
            case Direction.ZPos:
                outputVec = new BlockVec(0, 0, 1);
                break;
            case Direction.ZNeg:
                outputVec = new BlockVec(0, 0, -1);
                break;
        }
        return outputVec;
    }
    
    public int x, y, z;

    public BlockVec(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static BlockVec FromUnity(Vector3 unityVec)
    {
        return new BlockVec((int)unityVec.x, (int)unityVec.y, (int)unityVec.z);
    }

    public BlockVec Add(BlockVec addVec)
    {
        BlockVec outVec = new BlockVec(this.x, this.y, this.z);
        outVec.x += addVec.x;
        outVec.y += addVec.y;
        outVec.z += addVec.z;
        return outVec;
    }
    public BlockVec Sub(BlockVec addVec)
    {
        BlockVec outVec = new BlockVec(this.x, this.y, this.z);
        outVec.x -= addVec.x;
        outVec.y -= addVec.y;
        outVec.z -= addVec.z;
        return outVec;
    }

    public Vector3 ToUnity()
    {
        return new Vector3(x,y,z);
    }
}

[System.Serializable]
public class ChunkGenVariables
{
    public int bottom = 0;
    public int minHeight = 2;
    public int complexity = 1;
    
    public ChunkGenVariables()
    {
        
    }
}
