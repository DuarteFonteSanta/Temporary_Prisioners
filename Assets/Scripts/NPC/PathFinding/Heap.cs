using UnityEngine;
using System;

public class Heap<T> where T : IheapItem<T>
{
    //Variables
    private T[] items;
    private int currentItemCount; // Number items are currently in the heap

    //Properties
    public int Count => currentItemCount;//Gets the current number of items stored on the Heap

    //Constructor
    //Initializes the heap given maximum size
    public Heap(int maxheapSize) => items = new T[maxheapSize];

    //Adds a new item to the heap
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount; //The items HeapIndex is set to the current item count.
                                           //This ensures the item knows its position within the array

        items[currentItemCount] = item; //The item is placed in the next available spot in the array.
        SortUp(item); //The item is sorted upwards in the heap to maintain the heap property
        currentItemCount++; //The count of items is incremented after successfully adding the item
    }

    //Removes and returns the top (or "first") item from the heap,
    //which is typically the item with the highest priority (depending on the type of heap).
    public T RemoveFirst()
    {
        T firstItem = items[0]; //The item at the top(index 0) is stored in first item to be returned later
        currentItemCount--; //The item count is decremented since we are removing an item.
        items[0] = items[currentItemCount]; //The last item in the heap is moved to the top to fill the gap left by the removed item.
        items[0].HeapIndex = 0; // The moved item's index is updated to reflect its new position at the top.
        SortDown(items[0]); //The moved item is sorted downward to maintain the heap structure.
        return firstItem; //The removed item is returned
    }

    //If an item in the heap has changed (e.g., its priority),
    //this method sorts it back up to ensure the heap maintains the correct order.
    //It assumes the item may have moved up in priority
    public void UpdateItem(T item) => SortUp(item);

    //This checks if a specific item exists in the heap by comparing the items position in the heap (item.HeapIndex)
    //to the actual object at that index
    public bool Contains(T item) => Equals(items[item.HeapIndex], item);

    //This method maintains heap order after removing an item by moving the root element downwards
    private void SortDown(T item)
    {
        while (true)
        {
            //Gets the left child index
            int childIndexLeft = item.HeapIndex * 2 + 1;

            //Gets the right child index
            int childIndexRight = item.HeapIndex * 2 + 2;

            //is initialized to track which child (if any)
            //the current node needs to be swapped with during the sorting process.
            int swapIndex = 0;

            //This checks if the current node (item) has a left child
            //if childIndexLeft is out of bounds (i.e., greater than or equal to currentItemCount),
            //the node has no children, and the method can terminate early.
            if (childIndexLeft < currentItemCount)
            {
                //Stores the child to swap
                swapIndex = childIndexLeft;

                //This checks if the current node (item) has a right child
                if (childIndexRight < currentItemCount)
                {
                    //If the left child is smaller (or less preferred), the swapIndex remains as the left child
                    //If the right child is smaller(or less preferred), swapIndex is updated to the right child
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                //If the parent (item) is smaller than the child it’s being compared to,
                //a swap is performed.
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]); //Swap performed
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }

        }
    }

    //This is the counterpart to SortDown and is called when adding a new item or updating an item in the heap.
    //It moves the item upwards to ensure the heap order is maintained,
    //checking the item's parent and swapping them if necessary.
    private void SortUp(T item)
    {
        //The parent index is calculated using the formula for a binary heap
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            //Gets the parent item
            T parentItem = items[parentIndex];

            //If item is greater than parentItem (indicating the heap property is violated), a swap is needed.
            if (item.CompareTo(parentItem) > 0)
            {
                //Applies the swap
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            //After a swap, the parentIndex is recalculated based on the updated HeapIndex of the item
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    //This swaps two items in the heap, updating both their positions
    //in the array and their HeapIndex
    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

//This interface is essential because it provides the HeapIndex
//and comparison functionality for sorting items in the heap.
public interface IheapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}