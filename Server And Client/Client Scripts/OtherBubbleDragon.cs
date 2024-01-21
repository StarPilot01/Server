using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class OtherBubbleDragon : BubbleDragon
{

    private void Update()
    {
        base.Update();

        Debug.Log(_moveDir);
    }



}
