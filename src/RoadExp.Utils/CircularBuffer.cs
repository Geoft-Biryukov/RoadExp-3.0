namespace RoadExp.Utils
{
    public class CircularBuffer<T>
    {
        #region private fields
        private readonly T[] _array;
        private int _front;
        private int _rear;
        private int _size;
        private readonly int _capacity;
        #endregion

        #region constructor
        public CircularBuffer(int capacity) 
        {
            if (capacity <= 0)
                throw new ArgumentException(nameof(capacity));

            _capacity = capacity;
            _array = new T[capacity];
            _front = 0;
            _rear = -1;
            _size = 0;
        }
        #endregion

        #region properties
        public int Count => _size;
        public bool IsFull => _size == _capacity;
        public bool IsEmpty => _size == 0;
        #endregion

        #region public methods
        /// <summary>
        /// Добавляет элемент в конец. При переполнении перезаписывает голову
        /// </summary>
        /// <param name="item">Добавляемый элемент</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Enqueue(T item)
        {
            if (IsFull)
            {
                // Если очередь полная, мы затираем старый элемент в _front.
                // Поэтому сдвигаем указатель начала очереди вперёд.
                _front = (_front + 1) % _capacity;
            }
            else
            {
                // Очередь не полная, значит размер увеличивается
                _size++;
            }

            // Сдвигаем указатель хвоста по кругу и записываем данные
            _rear = (_rear + 1) % _capacity;
            _array[_rear] = item;
        }

        /// <summary>
        /// Удаляет и возвращает самый старый элемент из начала очереди
        /// </summary>
        /// <returns>Элемент из начала очереди</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается в случае пустой очереди</exception>
        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Очередь пуста.");
            }

            T item = _array[_front];
            _array[_front] = default; // Освобождаем ссылку для сборщика мусора (GC)

            _front = (_front + 1) % _capacity;
            _size--;

            return item;
        }

        /// <summary>
        /// Возвращает элемент из начала, не удаляя его
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Выбрасывается в случае пустой очереди</exception>
        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Очередь пуста.");
            }
            return _array[_front];
        }
        #endregion
    }
}
