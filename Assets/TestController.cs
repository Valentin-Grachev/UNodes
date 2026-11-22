using UnityEngine;
using UNodes;

public class TestController : IUpdateHandler
{
    void IUpdateHandler.Update()
    {
        Debug.Log("Test");
    }
}
