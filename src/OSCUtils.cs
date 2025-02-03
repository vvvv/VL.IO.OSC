using OSC;
using System;
using System.Text;
using VL.Lib.Collections;

namespace VL.IO.OSC
{
    public static class OSCUtils
    {
        const int align = 4;

        public static unsafe void UnpackString(Spread<byte> input, int startIndex, out string value, out int nextIndex)
        {
            const byte delimiter = 0;
            var s = input.AsMemory().Span.Slice(startIndex);
            var length = s.IndexOf(delimiter);
            if (length > 0)
            {
                fixed (byte* x = s)
                {
                    value = Encoding.UTF8.GetString(x, length);
                }
            }
            else
            {
                value = string.Empty;
            }
            nextIndex = Align(startIndex + length + 1);
        }

        public static int Align(int offset) => (offset + (align - 1)) & -align;

        public static bool MatchAddress(string senderAddress, string receiverAddress)
        {
            const char separator = '/';

            var count = senderAddress.Count(separator);
            if (count != receiverAddress.Count(separator))
                return false;

            var s = senderAddress.AsSpan();
            var r = receiverAddress.AsSpan();
            var se = s.Split(separator);
            var re = r.Split(separator);
            while (se.MoveNext() && re.MoveNext())
            {
                var sl = s.Slice(se.Current);
                var rl = r.Slice(re.Current);
                if (!MessagePattern.IsMatch(sl, rl))
                    return false;
            }

            return true;
        }

        static ReadOnlySpan<char> Slice(this ReadOnlySpan<char> s, (int start, int end) range)
        {
            return s.Slice(range.start, range.end - range.start);
        }

        static int Count(this string s, char character)
        {
            var count = 0;
            foreach (var c in s)
                if (c == character) count++;
            return count;
        }
    }

    // Taken from https://github.com/dotnet/runtime/issues/934
    static partial class MemoryExtensions
    {
        public static SpanSplitEnumerator<char> Split(this ReadOnlySpan<char> span, char separator)
            => new SpanSplitEnumerator<char>(span, separator);
    }


    ref struct SpanSplitEnumerator<T> where T : IEquatable<T>
    {
        private readonly ReadOnlySpan<T> _buffer;

        private readonly T _separator;

        private readonly int _separatorLength;

        private readonly bool _isInitialized;

        private int _startCurrent;
        private int _endCurrent;
        private int _startNext;

        public SpanSplitEnumerator<T> GetEnumerator() => this;

        public (int start, int end) Current => (_startCurrent, _endCurrent);

        internal SpanSplitEnumerator(ReadOnlySpan<T> span, T separator)
        {
            _isInitialized = true;
            _buffer = span;
            _separator = separator;
            _separatorLength = 1;
            _startCurrent = 0;
            _endCurrent = 0;
            _startNext = 0;
        }

        public bool MoveNext()
        {
            if (!_isInitialized || _startNext > _buffer.Length)
            {
                return false;
            }

            ReadOnlySpan<T> slice = _buffer.Slice(_startNext);
            _startCurrent = _startNext;

            int separatorIndex = slice.IndexOf(_separator);
            int elementLength = (separatorIndex != -1 ? separatorIndex : slice.Length);

            _endCurrent = _startCurrent + elementLength;
            _startNext = _endCurrent + _separatorLength;
            return true;
        }
    }
}
