using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects
{
    public class MeshUtils
    {
        public static int[] GetTris(int count)
        {
            int[] tris = new int[count];
            for (int i = 0; i < count; i++)
            {
                tris[i] = i;//i assume this is just the id of the triangle
            }
            return tris;
        }

        public static Vector3[] createWorldMesh(BlockClasses blocks)
        {
            List<Vector3> worldVertexes = new List<Vector3>();
            foreach (Block block in blocks.world)
            {
                foreach (BlockVec.Direction dir in blocks.GetEmptyAround(block.pos))
                {
                    worldVertexes.AddRange(createFace(block.pos.ToUnity(), dir));
                }
            }
            return worldVertexes.ToArray();
        }

        public static Vector3[] createFace(Vector3 location, BlockVec.Direction dir)
        {
            Vector3 start = location;
            Vector3 end = location;
            switch (dir)
            {
                case BlockVec.Direction.XPos:
                case BlockVec.Direction.XNeg:
                    BlockVec modifiedDirVecX = BlockVec.DirectionAsVec(dir);
                    modifiedDirVecX.x -= 1;
                    modifiedDirVecX.x = (int)(modifiedDirVecX.x * 0.5f);
                    start += new Vector3(modifiedDirVecX.x, -1, -1);
                    end += new Vector3(modifiedDirVecX.x, 0, 0);
                    break;
                case BlockVec.Direction.YPos:
                case BlockVec.Direction.YNeg:
                    BlockVec modifiedDirVecY = BlockVec.DirectionAsVec(dir);
                    modifiedDirVecY.y -= 1;
                    modifiedDirVecY.y = (int)(modifiedDirVecY.y * 0.5f);
                    start += new Vector3(-1, modifiedDirVecY.y, -1);
                    end += new Vector3(0, modifiedDirVecY.y, 0);
                    break;
                case BlockVec.Direction.ZPos:
                case BlockVec.Direction.ZNeg:
                    BlockVec modifiedDirVecZ = BlockVec.DirectionAsVec(dir);
                    modifiedDirVecZ.z -= 1;
                    modifiedDirVecZ.z = (int)(modifiedDirVecZ.z * 0.5f);
                    start += new Vector3(-1, -1, modifiedDirVecZ.z);
                    end += new Vector3(0, 0, modifiedDirVecZ.z);
                    break;
                
                /*
                case BlockVec.Direction.XPos:
                case BlockVec.Direction.XNeg:
                    start += new Vector3(BlockVec.DirectionAsVec(dir).x, -1, -1);
                    end += new Vector3(BlockVec.DirectionAsVec(dir).x, 1, 1);
                    break;
                case BlockVec.Direction.YPos:
                case BlockVec.Direction.YNeg:
                    start += new Vector3(-1, BlockVec.DirectionAsVec(dir).y, -1);
                    end += new Vector3(1, BlockVec.DirectionAsVec(dir).y, 1);
                    break;
                case BlockVec.Direction.ZPos:
                case BlockVec.Direction.ZNeg:
                    start += new Vector3(-1, -1, BlockVec.DirectionAsVec(dir).z);
                    end += new Vector3(1, 1, BlockVec.DirectionAsVec(dir).z);
                    break;
                */
            }
            return createFace(start, end, dir);
        }

        public static Vector3[] createFace(Vector3 start, Vector3 end, BlockVec.Direction dir)
        {
            Vector3[] faces = new Vector3[6];
            switch (dir)
            {
                case BlockVec.Direction.YPos:
                    faces[0] = start;
                    faces[1] = end;
                    faces[2] = new Vector3(end.x, start.y, start.z);
                    faces[3] = new Vector3(start.x, start.y, end.z);
                    faces[4] = end;
                    faces[5] = start;
                    break;
                case BlockVec.Direction.YNeg:
                    faces[5] = start;
                    faces[4] = end;
                    faces[3] = new Vector3(end.x, start.y, start.z);
                    faces[2] = new Vector3(start.x, start.y, end.z);
                    faces[1] = end;
                    faces[0] = start;
                    break;
                case BlockVec.Direction.XNeg:
                    faces[0] = start;
                    faces[1] = end;
                    faces[2] = new Vector3(start.x, end.y, start.z);
                    faces[3] = new Vector3(start.x, start.y, end.z);
                    faces[4] = end;
                    faces[5] = start;
                    break;
                case BlockVec.Direction.XPos:
                    faces[5] = start;
                    faces[4] = end;
                    faces[3] = new Vector3(start.x, end.y, start.z);
                    faces[2] = new Vector3(start.x, start.y, end.z);
                    faces[1] = end;
                    faces[0] = start;
                    break;
                case BlockVec.Direction.ZPos:
                    faces[0] = start;
                    faces[1] = end;
                    faces[2] = new Vector3(start.x, end.y, start.z);
                    faces[3] = new Vector3(end.x, start.y, start.z);
                    faces[4] = end;
                    faces[5] = start;
                    break;
                case BlockVec.Direction.ZNeg:
                    faces[5] = start;
                    faces[4] = end;
                    faces[3] = new Vector3(start.x, end.y, start.z);
                    faces[2] = new Vector3(end.x, start.y, start.z);
                    faces[1] = end;
                    faces[0] = start;
                    break;
                /*
                case BlockVec.Direction.YPos:
                    faces[0] = start;
                    faces[1] = start + end;
                    faces[2] = start + new Vector3(end.x, 0, 0);
                    faces[3] = start + new Vector3(0, 0, end.z);
                    faces[4] = start + end;
                    faces[5] = start;
                    break;
                case BlockVec.Direction.YNeg:
                    faces[5] = start;
                    faces[4] = start + end;
                    faces[3] = start + new Vector3(end.x, 0, 0);
                    faces[2] = start + new Vector3(0, 0, end.z);
                    faces[1] = start + end;
                    faces[0] = start;
                    break;
                case BlockVec.Direction.XNeg:
                    faces[0] = start;
                    faces[1] = start + end;
                    faces[2] = start + new Vector3(0, end.y, 0);
                    faces[3] = start + new Vector3(0, 0, end.z);
                    faces[4] = start + end;
                    faces[5] = start;
                    break;
                case BlockVec.Direction.XPos:
                    faces[5] = start;
                    faces[4] = start + end;
                    faces[3] = start + new Vector3(0, end.y, 0);
                    faces[2] = start + new Vector3(0, 0, end.z);
                    faces[1] = start + end;
                    faces[0] = start;
                    break;
                case BlockVec.Direction.ZPos:
                    faces[0] = start;
                    faces[1] = start + end;
                    faces[2] = start + new Vector3(0, end.y, 0);
                    faces[3] = start + new Vector3(end.x, 0, 0);
                    faces[4] = start + end;
                    faces[5] = start;
                    break;
                case BlockVec.Direction.ZNeg:
                    faces[5] = start;
                    faces[4] = start + end;
                    faces[3] = start + new Vector3(0, end.y, 0);
                    faces[2] = start + new Vector3(end.x, 0, 0);
                    faces[1] = start + end;
                    faces[0] = start;
                    break;
                */
            }
            return faces;
        }
    }
}
