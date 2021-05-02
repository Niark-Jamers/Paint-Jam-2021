using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderNinjaScript : MonoBehaviour
{
    public TomatoNinja TMNScript;

    public void SliceEnd()
    {
        TMNScript.NinjaSliceOver();
    }

    public void FuiteEnd()
    {
        TMNScript.NinjaFuiteOver();
    }

}
