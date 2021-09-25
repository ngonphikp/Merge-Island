using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class DeselectedTowerEventArgs : BaseEventArgs<DeselectedTowerEventArgs> {
    public DeselectedTowerEventArgs() {
    }

    public static DeselectedTowerEventArgs Create() {
        var eventArgs = new DeselectedTowerEventArgs();
        return eventArgs;
    }
}