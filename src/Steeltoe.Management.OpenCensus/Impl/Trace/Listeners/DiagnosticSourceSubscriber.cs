﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Steeltoe.Management.Census.Trace;

namespace Steeltoe.Management.Census.Impl.Trace.Listeners
{
    public class DiagnosticSourceSubscriber : IDisposable, IObserver<DiagnosticListener>
    {
        private readonly HashSet<string> sourceNames;
        private ConcurrentDictionary<string, DiagnosticSourceListener> subscriptions;
        private bool disposing;
        private IDisposable subscription;
        private readonly ITracer tracer;

        public DiagnosticSourceSubscriber(HashSet<string> sourceNames, ITracer tracer)
        {
            subscriptions = new ConcurrentDictionary<string, DiagnosticSourceListener>();
            this.sourceNames = sourceNames;
            this.tracer = tracer;
        }

        public void Subscribe()
        {
            if (subscription == null)
            {
                subscription = DiagnosticListener.AllListeners.Subscribe(this);
            }
        }

        public void OnNext(DiagnosticListener value)
        {
            if (!Volatile.Read(ref disposing) && subscriptions != null)
            {
                if (sourceNames.Contains(value.Name))
                {
                    subscriptions.GetOrAdd(value.Name, name =>
                    {
                        var dl = new DiagnosticSourceListener(value.Name, tracer);
                        dl.Subscription = value.Subscribe(dl);
                        return dl;
                    });
                }
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void Dispose()
        {
            Volatile.Write(ref disposing, true);

            var subsCopy = subscriptions;
            subscriptions = null;

            var keys = subsCopy.Keys;
            foreach (var key in keys)
            {
                if (subsCopy.TryRemove(key, out var sub))
                {
                    sub?.Dispose();
                }
            }

            subscription?.Dispose();
            subscription = null;
        }
    }
}
