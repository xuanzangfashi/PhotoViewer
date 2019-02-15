using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : ButtonBase {
    
    protected override void onclick()
    {
        mainCom.ResetImgTrans();
    }

}
