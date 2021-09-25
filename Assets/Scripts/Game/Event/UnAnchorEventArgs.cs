using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class UnAnchorEventArgs : BaseEventArgs<UnAnchorEventArgs> {

    public UnAnchorEventArgs() {
    }

    public static UnAnchorEventArgs Create() {
        var eventArgs = new UnAnchorEventArgs();
        return eventArgs;
    }
}