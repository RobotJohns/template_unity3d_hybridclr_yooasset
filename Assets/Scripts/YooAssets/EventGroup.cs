using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UniFramework.Event
{
    public class EventGroup
    {
        private readonly Dictionary<System.Type, List<Action<IEventMessage>>> _cachedListener = new Dictionary<System.Type, List<Action<IEventMessage>>>();

        /// <summary>
        /// ����һ������
        /// </summary>
        public void AddListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
        {
            System.Type eventType = typeof(TEvent);
            if (_cachedListener.ContainsKey(eventType) == false)
                _cachedListener.Add(eventType, new List<Action<IEventMessage>>());

            if (_cachedListener[eventType].Contains(listener) == false)
            {
                _cachedListener[eventType].Add(listener);
                UniEvent.AddListener(eventType, listener);
            }
            else
            {
                UniLogger.Warning($"Event listener is exist : {eventType}");
            }
        }

        /// <summary>
        /// �Ƴ����л���ļ���
        /// </summary>
        public void RemoveAllListener()
        {
            foreach (var pair in _cachedListener)
            {
                System.Type eventType = pair.Key;
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    UniEvent.RemoveListener(eventType, pair.Value[i]);
                }
                pair.Value.Clear();
            }
            _cachedListener.Clear();
        }
    }
}