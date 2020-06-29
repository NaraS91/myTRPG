using System.Collections.Generic;

public class StaticUtils
{
  //returns false when stack doesn't change
  public static bool PushCollaction<T>(Stack<T> stack, ICollection<T> items)
  {
    if(items.Count == 0)
    {
      return false;
    }

    foreach(T item in items)
    {
      stack.Push(item);
    }

    return true;
  }

  //returns false when stack doesn't change
  public static bool PushArray<T>(Stack<T> stack, T[] items)
  {
    if(items.Length == 0)
    {
      return false;
    }

    foreach(T item in items)
    {
      stack.Push(item);
    }

    return true;
  }
}
