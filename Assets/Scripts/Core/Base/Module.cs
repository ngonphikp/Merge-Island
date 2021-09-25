using System;

namespace Core {
    public abstract class Module {
        public virtual int Priority => 100;

        public virtual void OnInit() {
        }

        public abstract void OnClose();
    }
}