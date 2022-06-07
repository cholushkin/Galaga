using Galaga.System;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace Galaga.Game
{
    public class PrefabHolder : Singleton<PrefabHolder>
    {
        public GroupHolder Entities;
        public GroupHolder Topology;

        protected override void Awake()
        {
            base.Awake();
            Entities.Init();
            Topology.Init();
        }
    }

    // simple creation helpers
    public static class Factory
    {
        public enum Group
        {
            Entity,
            Topology
        }

        public static GameObject Create(string name, Transform parent, Vector3 position, float duration = -1f, Group resGroup = Group.Entity )
        {
            // choose group
            GroupHolder groupHolder = null;
            if (resGroup == Group.Entity)
                groupHolder = PrefabHolder.Instance.Entities;
            else if (resGroup == Group.Topology)
                groupHolder = PrefabHolder.Instance.Topology;
            Assert.IsNotNull(groupHolder);

            var gObj = Object.Instantiate(groupHolder[name], parent) as GameObject;
            gObj.name = name;
            gObj.transform.position = position;

            if (duration > 0f)
                Object.Destroy(gObj, duration);

            return gObj;
        }

        public static GameObject Create(string name, Transform parent, Group resGroup)
        {
            return Create(name, parent, Vector3.zero, -1f, resGroup);
        }
    }
}
