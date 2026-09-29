namespace RoadExp.Utils
{
    /// <summary>
    /// Реализация очереди для организации байтового буфера с задаваемой шириной буфера.
    /// Позволяет вносить в конец очереди массив байт и забирать с начала очереди нужное количество байт
    /// </summary>
    public class ByteQueueBuffer
    {
        private byte[] _array;
        private int _head;
        private int _tail;
        private int _size;

        private int _growFactor = 200;

        private const int _MinimumGrow = 16;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity">Ширина буфера</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ByteQueueBuffer(int capacity = 1024)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException("Длина очереди должна быть больше нуля");

            _array = new byte[capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        /// <summary>
        /// Получить длину очереди
        /// </summary>
        public int Length
        {
            get => _size;
        }

        /// <summary>
        /// Очистить очередь
        /// </summary>
        public void Clear()
        {
            _head = 0;
            _tail = 0;
            _size = 0;
        }


        /// <summary>
        /// Добавить элемент в конец очереди
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Push(byte value)
        {
            if (_size == _array.Length)
            {
                int newcapacity = (int)(_array.Length * (long)_growFactor / 100);
                if (newcapacity < _array.Length + _MinimumGrow)
                    newcapacity = _array.Length + _MinimumGrow;
                SetCapacity(newcapacity);
            }
            _array[_tail] = value;
            _tail = (_tail + 1) % _array.Length;
            _size++;
        }

        /// <summary>
        /// Добавить массив элементов в конец очереди
        /// </summary>
        /// <param name="value"></param>
        public void Push(byte[] value)
        {
            if (_size + value.Length > _array.Length)
            {
                int adjust = Math.Max(value.Length, _array.Length);
                int newcapacity = (int)(adjust * (long)_growFactor / 100);
                if (newcapacity < _array.Length + _MinimumGrow)
                    newcapacity = _array.Length + _MinimumGrow;
                SetCapacity(newcapacity);
            }

            int numToCopy = value.Length;
            int firstPart = _array.Length - _tail < numToCopy ? _array.Length - _tail : numToCopy;
            Array.Copy(value, 0, _array, _tail, firstPart);
            numToCopy -= firstPart;
            if (numToCopy > 0) Array.Copy(value, _array.Length - _tail, _array, 0, numToCopy);

            _tail = (_tail + value.Length) % _array.Length;
            _size += value.Length;
        }

        /// <summary>
        /// Посмотреть первый элемент очереди
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public byte Peek()
        {
            if (_size == 0) throw new InvalidOperationException("Пустая очередь");
            return _array[_head];
        }


        /// <summary>
        /// Посмотреть начало очереди длинной len
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public byte[] Peek(int len)
        {
            if (len > _size) throw new InvalidOperationException("Выход за границы очереди");
            byte[] data = new byte[len];
            int numToCopy = len;
            int firstPart = _array.Length - _head < numToCopy ? _array.Length - _head : numToCopy;
            Array.Copy(_array, _head, data, 0, firstPart);
            numToCopy -= firstPart;
            if (numToCopy > 0) Array.Copy(_array, 0, data, _array.Length - _head, numToCopy);
            return data;
        }

        /// <summary>
        /// Забрать первый элемент очереди
        /// </summary>
        /// <returns></returns>
        public byte Pop()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("no data in buf");
            }
            byte removed = _array[_head];
            //_array[_head] = 0; 
            _head = (_head + 1) % _array.Length;
            _size--;
            return removed;
        }

        /// <summary>
        /// Забрать массив длинною len с начала очереди
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] Pop(int len)
        {
            byte[] data = Peek(len);
            _head = (_head + len) % _array.Length;
            _size -= len;
            return data;
        }

        /// <summary>
        /// Добавить в конец очередь
        /// </summary>
        /// <param name="buf"></param>
        public void Append(ByteQueueBuffer buf)
        {
            byte[] data = buf.Peek(buf._size);
            Push(data);
        }

        /// <summary>
        /// изменить длину очереди
        /// </summary>
        /// <param name="capacity"></param>
        private void SetCapacity(int capacity)
        {
            byte[] newarray = new byte[capacity];
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, newarray, 0, _size);
                }
                else
                {
                    Array.Copy(_array, _head, newarray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, newarray, _array.Length - _head, _tail);
                }
            }
            _array = newarray;
            _head = 0;
            _tail = _size == capacity ? 0 : _size;
        }
    }
}
