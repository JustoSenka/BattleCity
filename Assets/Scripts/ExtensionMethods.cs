using System;
using System.Collections;
using UnityEngine;


public static class ExtensionMethods
{
    public static T GetByName<T>(this T[] array, string name) where T : UnityEngine.Object
    {
        foreach (T t in array)
        {
            if (t.name.Equals(name)) return t;
        }
        Debug.LogError("Get By Name Returned Null on:" + array.ToString());
        return null;
    }

    public static T GetByNameAndCoords<T>(this T[] array, string name, float x, float y) where T : UnityEngine.Transform
    {
        foreach (T t in array)
        {
            if (t.name.Contains(name) && t.position.x == x && t.position.y == y) return t;
        }
        
        return null;
    }

    public static void NotNull<T>(this T t, Action<T> ac)
    {
        if (t != null)
        {
            ac.Invoke(t);
        }
    }

    public static bool IsIn<T>(this T t, params T[] par) where T : IEquatable<T>
    {
        foreach (T p in par)
        {
            if (t.Equals(p)) return true;
        }
        return false;
    }

    public static void DoAfter<T>(this T t, float delay, Action ac) where T : MonoBehaviour
    {
        t.StartCoroutine(afterEnumerator(delay, ac));
    }

    private static IEnumerator afterEnumerator(float delay, Action ac)
    {
        yield return new WaitForSeconds(delay);
        ac.Invoke();
    }
}

