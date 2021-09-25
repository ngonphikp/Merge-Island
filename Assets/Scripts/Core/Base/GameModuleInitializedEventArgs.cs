using System;

namespace Core {
    public class GameModuleInitializedEventArgs : BaseEventArgs<GameModuleInitializedEventArgs> {
        public GameModuleInitializedEventArgs() {
        }

        public static GameModuleInitializedEventArgs Create() {
            GameModuleInitializedEventArgs eventArgs = new GameModuleInitializedEventArgs();
            return eventArgs;
        }
    }
}