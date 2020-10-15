using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Tests
{
  public class PriorityQueueTests
  {
    // A Test behaves as an ordinary method
    [Test]
    public void EmptyOnInitialization()
    {
      PriorityQueue<int> pq = new PriorityQueue<int>();
      Assert.IsTrue(pq.Count == 0);
    }

    [Test]
    public void SizeIsCorrectAfterAddingElements()
    {
      PriorityQueue<int> pq = new PriorityQueue<int>();
      
      for(int i = 0; i < 10000; i++)
      {
        pq.Push(i, i * i % 123);
        Assert.AreEqual(i + 1, pq.Count);
      }
    }

    [Test]
    public void IsPriorityRestrictedSmallTest()
    {
      PriorityQueue<int> pq = new PriorityQueue<int>();
      pq.Push(0, 0);
      pq.Push(6, 6);
      pq.Push(5, 5);
      pq.Push(7, 7);
      pq.Push(1, 1);
      pq.Push(9, 9);
      pq.Push(2, 2);
      pq.Push(3, 3);
      pq.Push(8, 8);
      pq.Push(4, 4);

      for(int i = 0; i < 10; i++)
      {
        Assert.AreEqual(9 - i, pq.Pop());
      }

    }

    [Test]
    public void AddingManyElementsInIncreasingOrderAndCheckingPopingOrder()
    {
      PriorityQueue<int> pq = new PriorityQueue<int>();

      for(int i = 0; i <= 1001000; i++)
      {
        pq.Push(i, i);
      }

      for(int i = 0; i <= 1000; i++)
      {
        Assert.AreEqual(1001000 - i, pq.Pop());
      }

      for(int i = 1000000; i <= 2000000; i++)
      {
        pq.Push(i, i);
      }

      for(int i = 0; i <= 2000000; i++)
      {
        Assert.AreEqual(2000000 - i, pq.Pop());
      }
    }

    [Test]
    public void TestingRandomNumbers()
    {
      PriorityQueue<int> pq = new PriorityQueue<int>();

      var random = new System.Random();
      for (int i = 0; i < 100000; i++)
      {
        int randomInt = random.Next();
        pq.Push(randomInt, randomInt);
      }

      int temp1 = int.MaxValue;
      int temp2 = 0;
      while (pq.Count > 0)
      {
        temp2 = pq.Pop();
        Assert.IsTrue(temp2 <= temp1);
        temp1 = temp2;
      }
    }
  }
}
