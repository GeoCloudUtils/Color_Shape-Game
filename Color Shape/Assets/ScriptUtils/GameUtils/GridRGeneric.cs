using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptUtils.GameUtils;
using ScriptUtils.Common;
using UnityEditor;

namespace ScriptUtils.GameUtils
{
    public class GridRGeneric<T1, T2> : MonoBehaviour where T1 : UnityEngine.Object where T2 : MultiDimensionalArr<T1>, IEnumerable
    {

        /// <summary>
        /// Array of MultiDimensionalArrays
        /// </summary>
        [SerializeField]
        public T2[] grid;
        /// <summary>
        /// Ranomizer rules used for <seealso cref="EduUtils.GameUtil.GridRepGenereic{T1, T2}.getRandomPoint"/>
        /// </summary>
        private Randomizer rowRandom;
        /// <summary>
        /// Ranomizer rules used for <seealso cref="EduUtils.GameUtil.GridRepGenereic{T1, T2}.getRandomPoint"/>
        /// </summary>
        private Randomizer cellRandom;
        /// <summary>
        /// Set up grid if its necessary.
        /// </summary>
        protected virtual void Awake()
        {
            bool gridExists = false;
            if (grid != null)
            {
                if (grid.Length != 0)
                    gridExists = true;
            }
            if (!gridExists)
            {
                int rows = transform.childCount;
                int cols = transform.GetChild(0).childCount;
                grid = new T2[rows];
                for (int i = 0; i < rows; i++)
                {
                    grid[i] = System.Activator.CreateInstance(typeof(T2), cols) as T2;
                    for (int j = 0; j < cols; j++)
                    {
                        if (typeof(T1) == typeof(GameObject))
                        {
                            grid[i][j] = transform.GetChild(i).GetChild(j).gameObject as T1;
                        }
                        else
                        {
                            grid[i][j] = transform.GetChild(i).GetChild(j).gameObject.GetComponent<T1>();
                        }
                        //Debug.Log(transform.GetChild(i).GetChild(j).gameObject.name);
                    }
                }
            }
            rowRandom = Randomizer.CreateRandomizer<SemiRandom>(1, grid.Length);
            cellRandom = Randomizer.CreateRandomizer<SemiRandom>(1, grid[0].Length);
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }
        /// <summary>
        /// Returs a random point in the cell
        /// </summary>
        /// <returns></returns>
        public GridPoint getRandomPoint()
        {
            return new GridPoint(rowRandom.getRandom() - 1, cellRandom.getRandom() - 1);
        }
        /// <summary>
        /// Determines if two points neighbor each other on grid (diagonal neighbors are ignored)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool isNeighbor(GridPoint a, GridPoint b)
        {
            GridPoint[] neighbors = getNeighbors(a);
            foreach (GridPoint c in neighbors)
            {
                if (c == b)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Gets all neighbors of a given point.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="includeDiagonal">If true will include diagonal neighbors. Default is false.</param>
        /// <returns></returns>
        public GridPoint[] getNeighbors(GridPoint position, bool includeDiagonal = false)
        {
            if (!includeDiagonal)
            {
                List<GridPoint> neighbors = new List<GridPoint>();
                int pozX = (int)position.x;
                int pozY = (int)position.y;
                if (hasAbove(position))
                {
                    neighbors.Add(new GridPoint(pozX - 1, pozY));
                }
                if (hasBelow(position))
                {
                    neighbors.Add(new GridPoint(pozX + 1, pozY));
                }
                if (hasToLeft(position))
                {
                    neighbors.Add(new GridPoint(pozX, pozY - 1));
                }
                if (hasToRight(position))
                {
                    neighbors.Add(new GridPoint(pozX, pozY + 1));
                }
                return neighbors.ToArray();
            }
            else
            {
                List<GridPoint> neighbors = new List<GridPoint>();
                int pozX = (int)position.x;
                int pozY = (int)position.y;
                if (hasAbove(position))
                {
                    neighbors.Add(new GridPoint(pozX - 1, pozY));
                    if (hasToLeft(position))
                        neighbors.Add(new GridPoint(pozX - 1, pozY - 1));
                    if (hasToRight(position))
                        neighbors.Add(new GridPoint(pozX - 1, pozY + 1));
                }
                if (hasBelow(position))
                {
                    neighbors.Add(new GridPoint(pozX + 1, pozY));
                    if (hasToLeft(position))
                        neighbors.Add(new GridPoint(pozX + 1, pozY - 1));
                    if (hasToRight(position))
                        neighbors.Add(new GridPoint(pozX + 1, pozY + 1));
                }
                if (hasToLeft(position))
                {
                    neighbors.Add(new GridPoint(pozX, pozY - 1));
                }
                if (hasToRight(position))
                {
                    neighbors.Add(new GridPoint(pozX, pozY + 1));
                }
                return neighbors.ToArray();
            }
        }
        /// <summary>
        /// Checks if a given Point denotes a position that exists in the grid.
        /// </summary>
        /// <param name="poz"></param>
        /// <returns></returns>
        public bool Exists(GridPoint poz)
        {
            return poz.x >= 0 && poz.y >= 0 && poz.x <= rows - 1 && poz.y <= cols - 1;
        }

        public bool Exists(T1 cell)
        {
            foreach (T1 item in this)
            {
                if (item == cell)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if given cell has a neighbor above it.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool hasAbove(T1 cell)
        {
            if (!Exists(cell))
                throw (new System.Exception("Not a child!"));
            return hasAbove(getPositionOfDirectChild(cell));
        }
        /// <summary>
        /// Checks if given cell has a neighbor below it.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool hasBelow(T1 cell)
        {
            if (!Exists(cell))
                throw (new System.Exception("Not a child!"));
            return hasBelow(getPositionOfDirectChild(cell));
        }
        /// <summary>
        /// Checks if given cell has a neighbor to its left.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool hasToLeft(T1 cell)
        {
            if (!Exists(cell))
                throw (new System.Exception("Not a child!"));
            return hasToLeft(getPositionOfDirectChild(cell));
        }
        /// <summary>
        /// Checks if given cell has a neighbor to its right.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool hasToRight(T1 cell)
        {
            if (!Exists(cell))
                throw (new System.Exception("Not a child!"));
            return hasToRight(getPositionOfDirectChild(cell));
        }
        /// <summary>
        /// Checks if given posiiton has a neighbor above it.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool hasAbove(GridPoint poz)
        {
            return poz.x > 0;
        }
        /// <summary>
        /// Checks if given posiiton has a neighbor below it.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool hasBelow(GridPoint poz)
        {
            return poz.x < rows - 1;
        }
        /// <summary>
        /// Checks if given cell has a position to its left.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool hasToLeft(GridPoint poz)
        {
            return poz.y > 0;
        }
        /// <summary>
        /// Checks if given cell has a position to its right.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool hasToRight(GridPoint poz)
        {
            return poz.y < cols - 1;
        }
        /// <summary>
        /// Gets the coordinates of neighbor above given cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public GridPoint getPositionAbove(T1 cell)
        {
            GridPoint poz = getPositionOfDirectChild(cell);
            return new GridPoint(poz.x - 1, poz.y);
        }
        /// <summary>
        /// Gets the coordinates of neighbor below given cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public GridPoint getPositionBelow(T1 cell)
        {
            GridPoint poz = getPositionOfDirectChild(cell);
            return new GridPoint(poz.x + 1, poz.y);
        }
        /// <summary>
        /// Gets the coordinates of neighbor to the left of given cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public GridPoint getPositionToLeft(T1 cell)
        {
            GridPoint poz = getPositionOfDirectChild(cell);
            return new GridPoint(poz.x, poz.y - 1);
        }
        /// <summary>
        /// Gets the coordinates of neighbor to the right of given cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public GridPoint getPositionToRight(T1 cell)
        {
            GridPoint poz = getPositionOfDirectChild(cell);
            return new GridPoint(poz.x, poz.y + 1);
        }
        /// <summary>
        /// Gets the coordinates of neighbor above given position.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public GridPoint getPositionAbove(GridPoint poz)
        {
            return new GridPoint(poz.x - 1, poz.y);
        }
        /// <summary>
        /// Gets the coordinates of neighbor below given position.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public GridPoint getPositionBelow(GridPoint poz)
        {
            return new GridPoint(poz.x + 1, poz.y);
        }
        /// <summary>
        /// Gets the coordinates of neighbor to the left of given position.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public GridPoint getPositionToLeft(GridPoint poz)
        {
            return new GridPoint(poz.x, poz.y - 1);
        }
        /// <summary>
        /// Gets the coordinates of neighbor to the right of given position.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public GridPoint getPositionToRight(GridPoint poz)
        {
            return new GridPoint(poz.x, poz.y + 1);
        }
        /// <summary>
        /// Get child at given position
        /// </summary>
        /// <param name="poz"></param>
        /// <returns></returns>
        public T1 getChild(GridPoint poz)
        {
            try
            {
                return this[poz.x][poz.y];
            }
            catch (System.IndexOutOfRangeException)
            {
                return null;
            }
        }
        /// <summary>
        /// Gets an array of children located in given positions in the same order as positions where given.
        /// (This is an overload method, it will convert the point list to an array)
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public T1[] getChildren(List<GridPoint> positions)
        {
            return getChildren(positions.ToArray());
        }

        /// <summary>
        /// Gets an array of children located in given positions in the same order as positions where given.
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public T1[] getChildren(GridPoint[] positions)
        {
            List<T1> children = new List<T1>();
            for (int i = 0; i < positions.Length; i++)
                children.Add(getChild(positions[i]));
            return children.ToArray();
        }
        /// <summary>
        /// Gets the neighboring child above given position.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public T1 getAbove(GridPoint poz)
        {
            return getChild(getPositionAbove(poz));
        }
        /// <summary>
        /// Gets the neighboring child below given position.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public T1 getBelow(GridPoint poz)
        {
            if (!hasBelow(poz))
                return null;
            return getChild(getPositionBelow(poz));
        }
        /// <summary>
        /// Gets the neighboring child to the left of given position.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public T1 getToLeft(GridPoint poz)
        {
            if (!hasToLeft(poz))
                return null;
            return getChild(getPositionToLeft(poz));
        }
        /// <summary>
        /// Gets the neighboring child to the right of given position.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public T1 getToRight(GridPoint poz)
        {
            if (!hasToRight(poz))
                return null;
            return getChild(getPositionToRight(poz));
        }

        /// <summary>
        /// Gets the neighboring child above the given child.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public T1 getAbove(T1 cell)
        {
            if (!hasAbove(cell))
                return null;
            return getChild(getPositionAbove(cell));
        }
        /// <summary>
        /// Gets the neighboring child below the given child.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public T1 getBelow(T1 cell)
        {
            if (!hasBelow(cell))
                return null;
            return getChild(getPositionBelow(cell));
        }
        /// <summary>
        /// Gets the neighboring child to the left of the given child.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public T1 getToLeft(T1 cell)
        {
            if (!hasToLeft(cell))
                return null;
            return getChild(getPositionToLeft(cell));
        }
        /// <summary>
        /// Gets the neighboring child to the right of the given child.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public T1 getToRight(T1 cell)
        {
            if (!hasToRight(cell))
                return null;
            return getChild(getPositionToRight(cell));
        }
        /// <summary>
        /// Gets the position of the specified child.
        /// If child is not found returns position -1,-1
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public GridPoint getPositionOfDirectChild(T1 child)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid[i][j] == child)
                        return new GridPoint(i, j);
                }
            }
            return new GridPoint(-1, -1);
        }
        /// <summary>
        /// Number of rowns
        /// </summary>
        [SerializeField]
        public int rows
        {
            get { return this.Length; }
        }
        /// <summary>
        /// Number of colums
        /// </summary>
        [SerializeField]
        public int cols
        {
            get { return grid[0].Length; }
        }

        /// <summary>
        /// Number of rowns
        /// </summary>
        public int Length
        {
            get
            {
                return grid.Length;
            }
        }
        /// <summary>
        /// Indexer used to acccess the rows.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T2 this[int index]
        {
            get
            {
                return grid[index];
            }
            set
            {
                grid[index] = value;
            }
        }

        #region IEnumerable implementation
        /// <summary>
        /// Enumarator will go from 0,0 to 0,1 etc.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    yield return grid[i][j];
                }
            }
        }

        #endregion
    }
}