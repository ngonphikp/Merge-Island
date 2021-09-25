using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class SelectedTowerEventArgs : BaseEventArgs<SelectedTowerEventArgs> {

    public SelectedTowerEventArgs() {
    }

    public static SelectedTowerEventArgs Create() {
        var eventArgs = new SelectedTowerEventArgs();
        return eventArgs;
    }
}