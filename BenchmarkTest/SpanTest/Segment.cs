using System;
using System.Buffers;

namespace SpanTest
{
    public class Segment<T> : ReadOnlySequenceSegment<T>
    {
        public Segment(ReadOnlyMemory<T> memory)
        {
            Memory = memory;
        }

        public Segment<T> Add(ReadOnlyMemory<T> mem)
        {
            var segment = new Segment<T>(mem);
            segment.RunningIndex = RunningIndex + mem.Length;

            Next = segment;

            return segment;
        }
    }
}
