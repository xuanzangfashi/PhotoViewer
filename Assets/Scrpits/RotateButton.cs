using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButton : ButtonBase {

    protected override void onclick()
    {
        base.onclick();
        mainCom.RotateImg90();
    }
}
