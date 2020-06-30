using System;
using System.Collections.Generic;
//used for debug.log
using UnityEngine;

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


  private void testPriorityQueue()
  {
    PriorityQueue<int> pQueue = new PriorityQueue<int>();
    for (int i = 0; i < 10; i++)
    {
      pQueue.Push(i, 10 - i);
    }

    Debug.Log("increasing");
    while (pQueue.Count > 0)
    {
      Debug.Log(String.Format("value: {0}", pQueue.Pop()));
    }

    for (int i = 0; i < 10; i++)
    {
      pQueue.Push(i, i);
    }

    Debug.Log("decreasing");
    while (pQueue.Count > 0)
    {
      Debug.Log(String.Format("value: {0}", pQueue.Pop()));
    }

    for (int i = 0; i < 10; i++)
    {
      pQueue.Push(i, i % 2);
    }

    Debug.Log("odd first");
    while (pQueue.Count > 0)
    {
      Debug.Log(String.Format("value: {0}", pQueue.Pop()));
    }

    var random = new System.Random();
    for (int i = 0; i < 100; i++)
    {
      int randomInt = random.Next();
      pQueue.Push(randomInt, randomInt);
    }

    int temp1 = int.MaxValue;
    int temp2 = 0;
    while (pQueue.Count > 0)
    {
      temp2 = pQueue.Pop();
      if (temp2 > temp1)
      {
        Debug.Log("BUG");
      }
      temp1 = temp2;
    }
  }
}
