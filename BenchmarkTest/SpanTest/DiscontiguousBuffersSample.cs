using System;
using System.Buffers;

namespace SpanTest
{
    public class DiscontiguousBuffersSample
    {
        public void SingleSegmentSequences()
        {
            int[] array = { 1, 2, 3, 4, 5 };
            var sequence = new ReadOnlySequence<int>(array, 1, 3);// sequence = { 2, 3, 4 }

            ReadOnlyMemory<int> memory = array;
            sequence = new ReadOnlySequence<int>(memory);
            sequence = sequence.Slice(1, 3);// sequence = { 2, 3, 4 }
        }

        public void LikeSingleSegmentSequences()
        {
            int[] array1 = { 1, 2, 3 };
            int[] array2 = { 4, 5, 6 };

            var startSegment = new Segment<int>(array1);
            var endSegment = startSegment.Add(array2);

            var sequence = new ReadOnlySequence<int>(startSegment, 1, endSegment, 3);
        }
    }
}
