using System.Collections.Generic;

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

  private List<LinkedList<Item>> items = new List<LinkedList<Item>>();
  public int Count { get; private set; } = 0;
  private int elementsInList = 0;

  public void Push(T itemValue, int priority)
  {
    int curr = elementsInList++;
    Count++;

    while(curr != 0 && priority > items[curr / 2].First.Value.Priority)
    {
      items[curr] = items[curr / 2];
      curr /= 2;
    }

    if(items[curr / 2].First.Value.Priority == priority)
    {
      items[curr / 2].AddLast(new Item(itemValue, priority));
      fixHeap();
    } else
    {
      items[curr] = new LinkedList<Item>();
      items[curr].AddLast(new Item(itemValue, priority));
    }
  }

  private void fixHeap()
  {
    if(items[elementsInList] == null)
    {
      return;
    }

    int curr = elementsInList;
    LinkedList<Item> prev = null;

    while(items[curr] != items[curr / 2])
    {
      LinkedList<Item> temp = items[curr];
      items[curr] = prev;
      prev = temp;
      curr /= 2;
    }
  }

}
