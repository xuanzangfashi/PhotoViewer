using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPButton : ButtonBase {
    public int lr;
    protected override void onclick()
    {
        base.onclick();
        mainCom.leftRightButtonClick(this.lr);
        mainCom.ResetImgTrans();
    }
}
