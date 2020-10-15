using System;
using System.Collections.Generic;

//order of values with the same priority is undefined
public class PriorityQueue<T>
{
  private readonly struct Item
  {
    public Item(T itemValue, int priority)
    {
      Value = itemValue;
      Priority = priority;
    }

    public T Value { get; }
    public int Priority { get; }
  }

  private List<Item> items = new List<Item>();
  public int Count { get; private set; } = 0;

  //higher priority -> pops earlier
  public void Push(T itemValue, int priority)
  {
    int curr = Count++;

    if(items.Count < Count)
    {
      items.Add(new Item(itemValue, priority));
    }

    while(curr != 0 && priority > items[(curr - 1) / 2].Priority)
    {
      items[curr] = items[(curr - 1) / 2];
      curr = (curr - 1) / 2;
    }

    items[curr] = new Item(itemValue, priority);
  }

  public T Pop()
  {
    if(Count == 0)
    {
      return default;
    }

    Count--;
    T result = items[0].Value;
    items[0] = items[Count];
    FixHeap();

    items.RemoveAt(Count);
    if(items.Capacity / 4 > Count)
    {
      items.Capacity = Count * 2;
    }

    return result;
  }

  public T Peek()
  {
    if (Count == 0)
    {
      throw new NotSupportedException("Queue is empty!");
    }

    return items[0].Value;
  }

  //assumes heap is correct expect the head of heap
  private void FixHeap()
  {
    int curr = 0;
    int currPriority = items[curr].Priority;
    Item item = items[curr];

    while (2 * curr + 1 < Count)
    {
      int leftChildPrio = items[2 * curr + 1].Priority;
      if (2 * curr + 2 == Count)
      {
        if (leftChildPrio > currPriority)
        {
          items[curr] = items[2 * curr + 1];
          curr = 2 * curr + 1;
        }

        break;
      } 
      else
      {
        int rightChildPrio = items[2 * curr + 2].Priority;
        int highestPriorityIndex 
          = leftChildPrio > rightChildPrio ? 2 * curr + 1 : 2 * curr + 2;
        if(items[highestPriorityIndex].Priority > currPriority)
        {
          items[curr] = items[highestPriorityIndex];
          curr = highestPriorityIndex;
        } 
        else
        {
          //curr has higher priority than both childs
          break;
        }
      }
    }

    items[curr] = item;
  }
}
