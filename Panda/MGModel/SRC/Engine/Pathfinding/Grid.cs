using System;
using System.Collections.Generic;
using System.Linq;

namespace Panda.MGModel;

// For A*, credit to https://www.redblobgames.com/pathfinding/a-star/implementation.html and https://www.youtube.com/watch?v=3Dw5d7PlcTM
// For merge sort, credit to https://www.geeksforgeeks.org/python-program-for-merge-sort/?ref=lbp for the merge sort algorithm kinda
// For the bfs, credit to https://www.redblobgames.com/pathfinding/a-star/implementation.html#python-breadth-first, https://www.youtube.com/watch?v=jAZB-3sFGS4, and https://www.youtube.com/watch?v=JtiK0DOeI4A 
public sealed class Grid
{
    // The size of the slots and the grid in slots
    public Vector2 Locsize { get; set; }
    public Vector2 GridSize { get; private set; }

    // The start position and the total size of the grid
    private Vector2 Startpos { get; set; }
    private Vector2 TotalSize { get; set; }

    // The grid slots
    public readonly GridLoc[,] Locs;

    // the eight directions
    private readonly Vector2[] directions =
    [
        -Vector2.UnitX,
        Vector2.UnitX,
        -Vector2.UnitY,
        Vector2.UnitY,
        -Vector2.One,
        new Vector2(1, -1),
        new Vector2(-1, 1),
        Vector2.One
    ];

    // The list of neighbours
    private List<GridLoc> neighbours = [];

    public Grid(Vector2 LOCSIZE, Vector2 STARTPOS, Vector2 TOTALSIZE)
    {
        // sets the size of the slots, the start position and the total size of the grid
        Locsize = LOCSIZE;
        Startpos = STARTPOS;
        TotalSize = TOTALSIZE;

        // creates the grid
        Locs = new GridLoc[(int)(TotalSize.X / Locsize.X), (int)(TotalSize.Y / Locsize.Y)];


        // sets the base grid
        // sets the grid size to the total size divided by the loc size
        GridSize = new Vector2((int)(TotalSize.X / Locsize.X), (int)(TotalSize.Y / Locsize.Y));

        // creates the grid slots
        for (var i = 0; i < GridSize.X; i++)
        for (var j = 0; j < GridSize.Y; j++)
            Locs[i, j] = new GridLoc(false)
            {
                pos = GetPosFromLoc(new Vector2(i, j))
            };
    }


    // returns the loc from the relative position if it is within the grid boundaries otherwise returns null
    public GridLoc GetLocFromRelPos(Vector2 LOC)
    {
        if (LOC is { X: >= 0, Y: >= 0 } && LOC.X < Locs.GetLength(0) && LOC.Y < Locs.GetLength(1))
            return Locs[(int)LOC.X, (int)LOC.Y];

        return null;
    }

    // returns the loc from the position if it is within the grid boundaries otherwise returns null
    public GridLoc GetLocFromPos(Vector2 POS)
    {
        var pos = GetRelPosFromPos(POS);
        var node = GetLocFromRelPos(pos);
        node.pos = GetPosFromLoc(pos);
        return node;
    }

    // returns the position from the slot location
    public Vector2 GetPosFromLoc(Vector2 LOC)
    {
        return Startpos + new Vector2((int)LOC.X * Locsize.X, (int)LOC.Y * Locsize.Y) + Locsize / 2;
    }

    // returns the relative position from the position
    public Vector2 GetRelPosFromPos(Vector2 POS)
    {
        return new Vector2(Math.Min(Math.Max(0, (int)((POS - Startpos).X / Locsize.X)), Locs.GetLength(0) - 1),
            Math.Min(Math.Max(0, (int)((POS - Startpos).Y / Locsize.Y)), Locs.GetLength(1) - 1));
    }


    public List<GridLoc> BreadthFirstSearch(Vector2 STARTPOS, Vector2 ENDPOS)
    {
        // clears the list of neighbours
        neighbours.Clear();


        // gets the start and end nodes
        var startNode = GetLocFromPos(STARTPOS);
        var endNode = GetLocFromPos(ENDPOS);

        // creates the linked list and the previous node dictionary
        LinkedList<GridLoc> linkedList = [];
        Dictionary<GridLoc, GridLoc> prevNode = new();

        // adds the start node to the linked list and sets the previous node to null
        linkedList.AddLast(startNode);
        prevNode[startNode] = null;

        while (linkedList.Count > 0)
        {
            // gets the first node from the linked list and removes it
            if (linkedList.First == null) continue;
            var current = linkedList.First.Value;
            linkedList.RemoveFirst();

            // if the current node is the end node
            if (current == endNode)
            {
                // creates the path list and adds the current node to it
                var path = new List<GridLoc>();
                while (current != null)
                {
                    path.Add(current);
                    current = prevNode[current];
                }

                // reverses the path list and returns it
                path.Reverse();
                return path;
            }

            // gets the neighbours of the current node
            neighbours = GetNeighbours(current);

            // cycles through the neighbours
            for (var i = neighbours.Count - 1; i >= 0; i--)
            {
                // for each neighbour, if the previous node is not set, adds the neighbour to the prevnode and the linked list
                var neighbour = neighbours[i];
                if (!prevNode.TryAdd(neighbour, current)) continue;
                linkedList.AddLast(neighbour);
            }
        }

        // Returns null if no path is found
        return null;
    }

    public List<GridLoc> AStarSearch(Vector2 STARTPOS, Vector2 ENDPOS)
    {
        // clears the list of neighbours
        neighbours.Clear();

        // gets the start and end nodes
        var startNode = GetLocFromPos(STARTPOS);
        var endNode = GetLocFromPos(ENDPOS);

        // creates the previous node dictionary, the gscore and fscore dictionaries
        Dictionary<GridLoc, GridLoc> prevNode = new();
        Dictionary<GridLoc, float> gScore = new();
        Dictionary<GridLoc, float> fScore = new();

        // creates the open set list and adds the start node to it
        var openSet = new LinkedList<GridLoc>();
        openSet.AddLast(startNode);

        // sets the gscore of the start node to 0 and the fscore to the heuristic
        gScore[startNode] = 0;
        fScore[startNode] = Heuristic(startNode, endNode);

        while (openSet.Count > 0)
        {
            // Sort the openSet using MergeSort
            openSet = new LinkedList<GridLoc>(MergeSort(openSet.ToList(), fScore));

            // gets the first node from the open set and removes it
            var current = openSet.First.Value;
            openSet.RemoveFirst();

            // if the current node is the end node
            if (current == endNode)
            {
                // creates the path list and adds the current node to it
                List<GridLoc> path = [];
                while (current != null)
                {
                    path.Add(current);
                    prevNode.TryGetValue(current, out current);
                }

                // reverses the path list and returns it
                path.Reverse();
                return path;
            }

            // gets the neighbours of the current node
            neighbours = GetNeighbours(current);

            // cycles through the neighbours
            for (var i = neighbours.Count - 1; i >= 0; i--)
            {
                // for each neighbour, if the gscore is less than the current gscore, sets the previous node to the current node and updates the gscore and fscore
                var potentialgscore = gScore[current] + Heuristic(current, neighbours[i]);

                if (potentialgscore >= gScore.GetValueOrDefault(neighbours[i], float.MaxValue)) continue;
                prevNode[neighbours[i]] = current;
                gScore[neighbours[i]] = potentialgscore;
                fScore[neighbours[i]] = gScore[neighbours[i]] + Heuristic(neighbours[i], endNode);

                if (!openSet.Contains(neighbours[i])) openSet.AddLast(neighbours[i]);
            }
        }

        // Returns null if no path is found
        return null;
    }

    private List<GridLoc> MergeSort(List<GridLoc> LOCS, IReadOnlyDictionary<GridLoc, float> FSCORE)
    {
        // If the list has only one element, returns it
        if (LOCS.Count <= 1) return LOCS;

        // Splits the list in two
        var left = LOCS.GetRange(0, LOCS.Count / 2);
        var right = LOCS.GetRange(LOCS.Count / 2, LOCS.Count - LOCS.Count / 2);

        // Sorts the two lists
        return Merge(MergeSort(left, FSCORE), MergeSort(right, FSCORE), FSCORE);
    }

    private List<GridLoc> Merge(IReadOnlyList<GridLoc> LEFT, IReadOnlyList<GridLoc> RIGHT,
        IReadOnlyDictionary<GridLoc, float> FSCORE)
    {
        // creates the result list and sets the indexes to 0
        var result = new List<GridLoc>();
        var indexLeft = 0;
        var indexRight = 0;

        // cycles through the two lists
        while (indexLeft < LEFT.Count && indexRight < RIGHT.Count)
            // adds the smallest element to the result list
            if (FSCORE[LEFT[indexLeft]] <= FSCORE[RIGHT[indexRight]])
            {
                result.Add(LEFT[indexLeft]);
                indexLeft++;
            }
            else
            {
                result.Add(RIGHT[indexRight]);
                indexRight++;
            }

        // adds the remaining elements to the result list
        while (indexLeft < LEFT.Count)
        {
            result.Add(LEFT[indexLeft]);
            indexLeft++;
        }

        while (indexRight < RIGHT.Count)
        {
            result.Add(RIGHT[indexRight]);
            indexRight++;
        }

        return result;
    }


    // uses the Chebyshev distance as the heuristic
    private float Heuristic(GridLoc A, GridLoc B)
    {
        return Math.Max(Math.Abs(A.pos.X - B.pos.X), Math.Abs(A.pos.Y - B.pos.Y));
    }

    private List<GridLoc> GetNeighbours(GridLoc NODE)
    {
        // clears the list of neighbours
        neighbours.Clear();

        // gets the relative position of the node
        var nodePos = GetRelPosFromPos(NODE.pos);

        // cycles through the eight directions
        for (var i = directions.Length - 1; i >= 0; i--)
        {
            // gets the relative position of the neighbour
            var neighbourPos = nodePos + directions[i];

            // Checks if the neighbour is within the grid boundaries
            if (neighbourPos.X < 0 || neighbourPos.Y < 0 || neighbourPos.X >= Locs.GetLength(0) ||
                neighbourPos.Y >= Locs.GetLength(1)) continue;

            var neighbour = Locs[(int)neighbourPos.X, (int)neighbourPos.Y];

            // Checks if the neighbour is not filled
            if (neighbour.IsFilled) continue;

            // If the direction is diagonal, checks if the two adjacent cells are not blocked
            if (Math.Abs(Math.Abs(directions[i].X) - 1) < 1e-9f && Math.Abs(Math.Abs(directions[i].Y) - 1) < 1e-9f)
            {
                // If the two adjacent cells are not blocked, adds the neighbour to the list of neighbours
                if (!Locs[(int)(nodePos.X + directions[i].X), (int)nodePos.Y].IsFilled &&
                    !Locs[(int)nodePos.X, (int)(nodePos.Y + directions[i].Y)].IsFilled) neighbours.Add(neighbour);
            }
            else
            {
                neighbours.Add(neighbour);
            }
        }

        // returns the list of neighbours
        return neighbours;
    }
}