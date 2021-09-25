using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class ModuleMgr {
        private static readonly Dictionary<int, Module> m_AllModules =
            new Dictionary<int, Module>();

        private static List<IUpdate> m_AllUpdates = new List<IUpdate>();
        private static List<IFixedUpdate> m_AllFixedUpdates = new List<IFixedUpdate>();
        private static List<IImGui> _allImGuis = new List<IImGui>();

        public static T GetModule<T>() where T : Module, new() {
            return (T) GetModule(typeof(T));
        }

        public static void Init() {
            var orderResult = m_AllModules.OrderBy(x => x.Value.Priority);
            foreach (var item in orderResult) {
                item.Value.OnInit();
            }

            GameCore.Event.FireNow(null, GameModuleInitializedEventArgs.Create());
        }

        public static void Update() {
            for (int i = 0; i < m_AllUpdates.Count; i++) {
                m_AllUpdates[i].OnUpdate();
            }
        }

        public static void FixedUpdate() {
            for (int i = 0; i < m_AllFixedUpdates.Count; i++) {
                m_AllFixedUpdates[i].OnFixedUpdate();
            }
        }

        public static void ImGui() {
            for (int i = 0; i < _allImGuis.Count; i++) {
                _allImGuis[i].OnImGui();
            }
        }

        public static void ShutDown() {
            var orderResult = m_AllModules.OrderBy(x => x.Value.Priority);
            foreach (var item in orderResult) {
                item.Value.OnClose();
            }

            m_AllUpdates.Clear();
            m_AllFixedUpdates.Clear();
            m_AllModules.Clear();
        }

        private static Module GetModule(Type type) {
            int hashCode = type.GetHashCode();
            Module module = null;
            if (m_AllModules.TryGetValue(hashCode, out module)) {
                return module;
            }

            module = CreateModule(type);
            return module;
        }

        private static Module CreateModule(Type type) {
            int hashCode = type.GetHashCode();
            Module module = (Module) Activator.CreateInstance(type);
            m_AllModules[hashCode] = module;

            var update = module as IUpdate;
            if (update != null) {
                m_AllUpdates.Add(update);
            }

            var fixedUpdate = module as IFixedUpdate;
            if (fixedUpdate != null) {
                m_AllFixedUpdates.Add(fixedUpdate);
            }

            var imgui = module as IImGui;
            if (imgui != null) {
                _allImGuis.Add(imgui);
            }

            return module;
        }
    }
}