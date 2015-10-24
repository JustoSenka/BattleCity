using UnityEngine;
using System.Collections;

public class ArgsPointer<T> {

    public T[] Args { set; get; }

    public ArgsPointer(params T[] Args)
    {
        this.Args = Args;
    }

    public T this[int index]
    {
        get
        {
            return Args[index];
        }
        set
        {
            Args[index] = value;
        }
    }
}
