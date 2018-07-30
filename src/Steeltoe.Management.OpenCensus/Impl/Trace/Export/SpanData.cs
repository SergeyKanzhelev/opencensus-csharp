﻿using System;
using System.Collections.Generic;
using System.Text;
using Steeltoe.Management.Census.Common;

namespace Steeltoe.Management.Census.Trace.Export
{
    public sealed class SpanData : ISpanData
    {
        public static ISpanData Create(
                        ISpanContext context,
                        ISpanId parentSpanId,
                        bool? hasRemoteParent,
                        string name,
                        ITimestamp startTimestamp,
                        IAttributes attributes,
                        ITimedEvents<IAnnotation> annotations,
                        ITimedEvents<IMessageEvent> messageOrNetworkEvents,
                        ILinks links,
                        uint? childSpanCount,
                        Status status,
                        ITimestamp endTimestamp)
        {
            if (messageOrNetworkEvents == null)
            {
                throw new ArgumentNullException(nameof(messageOrNetworkEvents));
            }
            IList<ITimedEvent<IMessageEvent>> messageEventsList = new List<ITimedEvent<IMessageEvent>>();
            foreach (ITimedEvent<IMessageEvent> timedEvent in messageOrNetworkEvents.Events)
            {
                messageEventsList.Add(timedEvent);
            }
            ITimedEvents<IMessageEvent> messageEvents = TimedEvents<IMessageEvent>.Create(messageEventsList, messageOrNetworkEvents.DroppedEventsCount);
            return new SpanData(
                context,
                parentSpanId,
                hasRemoteParent,
                name,
                startTimestamp,
                attributes,
                annotations,
                messageEvents,
                links,
                childSpanCount,
                status,
                endTimestamp);
        }
        internal SpanData(
            ISpanContext context,
            ISpanId parentSpanId,
            bool? hasRemoteParent,
            string name,
            ITimestamp startTimestamp,
            IAttributes attributes,
            ITimedEvents<IAnnotation> annotations,
            ITimedEvents<IMessageEvent> messageEvents,
            ILinks links,
            uint? childSpanCount,
            Status status,
            ITimestamp endTimestamp)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            this.Context = context;
            this.ParentSpanId = parentSpanId;
            this.HasRemoteParent = hasRemoteParent;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.StartTimestamp = startTimestamp ?? throw new ArgumentNullException(nameof(startTimestamp));
            this.Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.Annotations = annotations ?? throw new ArgumentNullException(nameof(annotations));
            this.MessageEvents = messageEvents ?? throw new ArgumentNullException(nameof(messageEvents));
            this.Links = links ?? throw new ArgumentNullException(nameof(links));
            this.ChildSpanCount = childSpanCount;
            this.Status = status;
            this.EndTimestamp = endTimestamp;
        }
        public ISpanContext Context { get; }

        public ISpanId ParentSpanId { get; }

        public bool? HasRemoteParent { get; }

        public string Name { get; }

        public ITimestamp Timestamp { get; }

        public IAttributes Attributes { get; }

        public ITimedEvents<IAnnotation> Annotations { get; }

        public ITimedEvents<IMessageEvent> MessageEvents { get; }

        public ILinks Links { get; }

        public uint? ChildSpanCount { get; }

        public Status Status { get; }

        public ITimestamp EndTimestamp { get; }

        public ITimestamp StartTimestamp { get; }


        public override string ToString()
        {
            return "SpanData{"
                + "context=" + Context + ", "
                + "parentSpanId=" + ParentSpanId + ", "
                + "hasRemoteParent=" + HasRemoteParent + ", "
                + "name=" + Name + ", "
                + "startTimestamp=" + StartTimestamp + ", "
                + "attributes=" + Attributes + ", "
                + "annotations=" + Annotations + ", "
                + "messageEvents=" + MessageEvents + ", "
                + "links=" + Links + ", "
                + "childSpanCount=" + ChildSpanCount + ", "
                + "status=" + Status + ", "
                + "endTimestamp=" + EndTimestamp
                + "}";
        }

        public override bool Equals(Object o)
        {
            if (o == this)
            {
                return true;
            }
            if (o is SpanData)
            {
                SpanData that = (SpanData)o;
                return (this.Context.Equals(that.Context))
                     && ((this.ParentSpanId == null) ? (that.ParentSpanId == null) : this.ParentSpanId.Equals(that.ParentSpanId))
                     && (this.HasRemoteParent.Equals(that.HasRemoteParent))
                     && (this.Name.Equals(that.Name))
                     && (this.StartTimestamp.Equals(that.StartTimestamp))
                     && (this.Attributes.Equals(that.Attributes))
                     && (this.Annotations.Equals(that.Annotations))
                     && (this.MessageEvents.Equals(that.MessageEvents))
                     && (this.Links.Equals(that.Links))
                     && ((this.ChildSpanCount == null) ? (that.ChildSpanCount == null) : this.ChildSpanCount.Equals(that.ChildSpanCount))
                     && ((this.Status == null) ? (that.Status == null) : this.Status.Equals(that.Status))
                     && ((this.EndTimestamp == null) ? (that.EndTimestamp == null) : this.EndTimestamp.Equals(that.EndTimestamp));
            }
            return false;
        }

        public override int GetHashCode()
        {
            int h = 1;
            h *= 1000003;
            h ^= this.Context.GetHashCode();
            h *= 1000003;
            h ^= (ParentSpanId == null) ? 0 : this.ParentSpanId.GetHashCode();
            h *= 1000003;
            h ^= (HasRemoteParent == null) ? 0 : this.HasRemoteParent.GetHashCode();
            h *= 1000003;
            h ^= this.Name.GetHashCode();
            h *= 1000003;
            h ^= this.StartTimestamp.GetHashCode();
            h *= 1000003;
            h ^= this.Attributes.GetHashCode();
            h *= 1000003;
            h ^= this.Annotations.GetHashCode();
            h *= 1000003;
            h ^= this.MessageEvents.GetHashCode();
            h *= 1000003;
            h ^= this.Links.GetHashCode();
            h *= 1000003;
            h ^= (ChildSpanCount == null) ? 0 : this.ChildSpanCount.GetHashCode();
            h *= 1000003;
            h ^= (Status == null) ? 0 : this.Status.GetHashCode();
            h *= 1000003;
            h ^= (EndTimestamp == null) ? 0 : this.EndTimestamp.GetHashCode();
            return h;
        }

    }
}
