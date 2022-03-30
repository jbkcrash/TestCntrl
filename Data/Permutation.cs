namespace TestCntrl.Data;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class ListOfListsPermuter<TValue> : IEnumerable<IEnumerable<TValue>>
{
    private int count;
    private IEnumerable<TValue>[] listOfLists;

    public ListOfListsPermuter(IEnumerable<IEnumerable<TValue>> listOfLists_)
    {
        if (object.ReferenceEquals(listOfLists_, null))
        {
            throw new ArgumentNullException(nameof(listOfLists_));
        }
        listOfLists =listOfLists_.ToArray();
        count = listOfLists.Count();
        for (int i = 0; i < count; i++)
        {
            if (object.ReferenceEquals(listOfLists[i], null))
            {
                throw new NullReferenceException(string.Format("{0}[{1}] is null.", nameof(listOfLists_), i));
            }
        }
    }

    // A variant of Michael Liu's answer in StackOverflow
    // https://stackoverflow.com/questions/2055927/ienumerable-and-recursion-using-yield-return
    public IEnumerator<IEnumerable<TValue>> GetEnumerator()
    {
        TValue[] currentList = new TValue[count];
        int level = 0; 
        var enumerators = new Stack<IEnumerator<TValue>>();
        IEnumerator<TValue> enumerator = listOfLists[level].GetEnumerator();
        try
        {
            while (true)
            {
                if (enumerator.MoveNext())
                {
                    currentList[level] = enumerator.Current;
                    level++;
                    if (level >= count)
                    {
                        level--;
                        yield return currentList;
                    }
                    else
                    {
                        enumerators.Push(enumerator);
                        enumerator = listOfLists[level].GetEnumerator();
                    }
                }
                else
                {
                    if (level == 0)
                    {
                        yield break;
                    }
                    else
                    {
                        enumerator.Dispose();
                        enumerator = enumerators.Pop();
                        level--;
                    }
                }
            }
        }
        finally
        {
            // Clean up in case of an exception.
            enumerator?.Dispose();
            while (enumerators.Count > 0) 
            {
                enumerator = enumerators.Pop();
                enumerator.Dispose();
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}