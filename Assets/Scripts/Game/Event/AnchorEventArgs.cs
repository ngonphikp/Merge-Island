using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class AnchorEventArgs : BaseEventArgs<AnchorEventArgs> {

    public AnchorEventArgs() {
    }

    public static AnchorEventArgs Create() {
        var eventArgs = new AnchorEventArgs();
        return eventArgs;
    }
}