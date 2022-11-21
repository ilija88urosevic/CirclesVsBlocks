﻿using UnityEngine;
using System.Collections.Generic;
using com.spacepuppy.Dynamic;
using com.spacepuppy.Project;
using com.spacepuppy.Utils;

namespace com.spacepuppy.Events
{

    public sealed class EventTriggerEvaluator : EventTriggerEvaluator.IEvaluator
    {

        #region Singleton Interface

        private static EventTriggerEvaluator _default = new EventTriggerEvaluator();
        private static IEvaluator _evaluator;

        public static EventTriggerEvaluator Default
        {
            get { return _default; }
        }

        public static IEvaluator Current
        {
            get { return _evaluator; }
        }

        public static void SetCurrentEvaluator(IEvaluator ev)
        {
            _evaluator = ev ?? _default;
        }

        static EventTriggerEvaluator()
        {
            _evaluator = _default;
        }

        #endregion

        #region Methods

        private ITriggerable[] GetCache(GameObject go)
        {
            //we don't trigger inactive GameObjects unless they are prefabs

            EventTriggerCache cache;
            if (go.activeInHierarchy)
            {
                cache = go.AddOrGetComponent<EventTriggerCache>();
                return cache.Targets ?? cache.RefreshCache();
            }
            else if (go.HasComponent<PrefabToken>())
            {
                cache = go.GetComponent<EventTriggerCache>();
                if (cache != null) return cache.Targets ?? cache.RefreshCache();

                return go.GetComponents<ITriggerable>();
            }
            else
            {
                return ArrayUtil.Empty<ITriggerable>();
            }
        }

        public void GetAllTriggersOnTarget(object target, List<ITriggerable> outputColl)
        {
            if (target is IProxy) target = (target as IProxy).GetTarget();
            var go = GameObjectUtil.GetGameObjectFromSource(target);
            if (go != null)
            {
                outputColl.AddRange(this.GetCache(go));
            }
            else if (target is ITriggerable)
                outputColl.Add(target as ITriggerable);
        }

        public void TriggerAllOnTarget(object target, object sender, object arg)
        {
            if (target is IProxy) target = (target as IProxy).GetTarget();
            var go = GameObjectUtil.GetGameObjectFromSource(target);
            if (go != null)
            {
                var arr = this.GetCache(go);

                foreach (var t in arr)
                {
                    if (t.CanTrigger)
                    {
                        t.Trigger(sender, arg);
                    }
                }
            }
            else
            {
                var targ = target as ITriggerable;
                if (targ != null && targ.CanTrigger)
                    targ.Trigger(sender, arg);
            }
        }

        public void TriggerSelectedTarget(object target, object sender, object arg)
        {
            if (target is IProxy) target = (target as IProxy).GetTarget();
            if (target != null && target is ITriggerable)
            {
                var t = target as ITriggerable;
                if (t.CanTrigger) t.Trigger(sender, arg);
            }
        }

        public void SendMessageToTarget(object target, string message, object arg)
        {
            var go = GameObjectUtil.GetGameObjectFromSource(target, true);
            if (go != null && message != null)
            {
                go.SendMessage(message, arg, SendMessageOptions.DontRequireReceiver);
            }
        }

        public void CallMethodOnSelectedTarget(object target, string methodName, VariantReference[] methodArgs)
        {
            if (target is IProxy) target = (target as IProxy).GetTarget();
            if (methodName != null)
            {
                //CallMethod does not support using the passed in arg
                //var args = (from a in this._triggerableArgs select (a != null) ? a.Value : null).ToArray();

                object[] args = null;
                if (methodArgs != null && methodArgs.Length > 0)
                {
                    args = new object[methodArgs.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (methodArgs[i] != null) args[i] = methodArgs[i].Value;
                    }
                }

                if (args != null && args.Length == 1)
                {
                    DynamicUtil.SetValue(target, methodName, args[0]);
                }
                else
                {
                    DynamicUtil.InvokeMethod(target, methodName, args);
                }
            }
        }

        public void EnableTarget(object target, EnableMode mode)
        {
            var go = GameObjectUtil.GetGameObjectFromSource(target, true);
            if (go != null)
            {
                switch (mode)
                {
                    case EnableMode.Disable:
                        go.SetActive(false);
                        break;
                    case EnableMode.Enable:
                        go.SetActive(true);
                        break;
                    case EnableMode.Toggle:
                        go.SetActive(!go.activeSelf);
                        break;
                }
            }
        }

        public void DestroyTarget(object target)
        {
            if (target is IProxy) target = (target as IProxy).GetTarget();
            var go = GameObjectUtil.GetGameObjectFromSource(target);
            if (go != null)
            {
                ObjUtil.SmartDestroy(go);
            }
            else if (target is UnityEngine.Object)
            {
                ObjUtil.SmartDestroy(target as UnityEngine.Object);
            }
        }

        #endregion

        #region Special Types

        private class EventTriggerCache : MonoBehaviour
        {

            #region Fields

            private ITriggerable[] _targets;

            #endregion

            #region Properties

            public ITriggerable[] Targets
            {
                get { return _targets; }
            }

            #endregion

            #region Methods

            private void Awake()
            {
                this.RefreshCache();
            }

            public ITriggerable[] RefreshCache()
            {
                _targets = this.gameObject.GetComponents<ITriggerable>();
                if (_targets.Length > 1)
                    System.Array.Sort(_targets, TriggerableOrderComparer.Default);
                return _targets;
            }

            #endregion

        }

        public interface IEvaluator
        {

            void GetAllTriggersOnTarget(object target, List<ITriggerable> outputColl);

            void TriggerAllOnTarget(object target, object sender, object arg);
            void TriggerSelectedTarget(object target, object sender, object arg);
            void SendMessageToTarget(object target, string message, object arg);
            void CallMethodOnSelectedTarget(object target, string methodName, VariantReference[] methodArgs);
            void EnableTarget(object target, EnableMode mode);
            void DestroyTarget(object target);

        }

        #endregion

    }

}
