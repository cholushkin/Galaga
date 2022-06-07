using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace Galaga.System
{
    [Serializable]
    public class GroupHolder
    {
        public Object[] Objects;
        Dictionary<string, Object> _name2Obj;

        public Object this[string name]
        {
            get
            {
                Init(); // lazy init
                Assert.IsTrue(_name2Obj.ContainsKey(name));
                return _name2Obj[name];
            }
        }

        public void Init()
        {
            if (_name2Obj != null)
                return;
            _name2Obj = new Dictionary<string, Object>();
            foreach (var obj in Objects)
            {
                if (obj == null)
                    continue;
                if (!_name2Obj.ContainsKey(obj.name))
                    _name2Obj.Add(obj.name, obj);
                else
                    Debug.LogError("Dup prefab in prefab list " + obj.name);
            }
        }
    }
}
