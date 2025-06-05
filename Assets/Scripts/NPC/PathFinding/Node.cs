using UnityEngine;

public class Node : IheapItem<Node>
{
    //Variables
    public Node parent; //Parent Node
    public bool walkable; //Indicates whether the node can be traversed
    public Vector2 worldPos; //Position
    public int movementPenalty; //Movement Penalty

    public int gridX, gridY; //Node size

    public int gCost; //movement cost
    public int hCost; //heuristic cost

    private int heapIndex; //Index in the heap

    //Properties
    public int FCost => gCost + hCost; //Calculate total cost

    public int HeapIndex
    {
        get => heapIndex;

        set => heapIndex = value;
    }

    // This method compares two nodes based on their FCost.
    // If the costs are equal, it compares their hCost.
    // The return value is negated to prioritize lower costs in the heap (which usually means smaller FCost should come first).
    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost); //Compare the total cost
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost); //Compare the heuristic cost
        }

        return -compare; //negates the comparison result to create a max-heap instead of a min-heap
    }

    //Constructor
    //Initializes the properties of a node.
    //It determines if a node is walkable, sets its position, grid coordinates,
    //and any movement penalty associated with it
    public Node(bool walkable, Vector2 worldPos, int gridX, int gridY, int movementPenalty)
    {
        this.walkable = walkable;
        this.worldPos = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
        this.movementPenalty = movementPenalty;
    }
}