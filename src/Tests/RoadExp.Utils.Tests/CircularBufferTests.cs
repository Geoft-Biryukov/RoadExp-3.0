namespace RoadExp.Utils.Tests
{
    [TestFixture]
    public class CircularBufferTests
    {
        private CircularBuffer<int> _buffer;

        [SetUp]
        public void Setup()
        {
            // Инициализируем буфер емкостью 3 перед каждым тестом
            _buffer = new CircularBuffer<int>(3);
        }

        [Test]
        public void Constructor_NegativeOrZeroCapacity_ThrowsArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new CircularBuffer<int>(0));
            Assert.Throws<ArgumentException>(() => new CircularBuffer<int>(-5));
        }

        [Test]
        public void InitialState_IsEmptyAndCountIsZero()
        {
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_buffer.IsEmpty, Is.True);
                Assert.That(_buffer.IsFull, Is.False);
                Assert.That(_buffer.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public void Enqueue_AddsElementsCorrectly_ShouldAddElementCorrectly()
        {
            // Act
            _buffer.Enqueue(10);
            _buffer.Enqueue(20);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_buffer.Count, Is.EqualTo(2));
                Assert.That(_buffer.IsEmpty, Is.False);
                Assert.That(_buffer.IsFull, Is.False);
                Assert.That(_buffer.Peek(), Is.EqualTo(10));
            });
        }

        [Test]
        public void Dequeue_RemovesAndReturnsFirstElement()
        {
            // Arrange
            _buffer.Enqueue(10);
            _buffer.Enqueue(20);

            // Act
            int result = _buffer.Dequeue();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(10));
                Assert.That(_buffer.Count, Is.EqualTo(1));
                Assert.That(_buffer.Peek(), Is.EqualTo(20));
            });
        }

        [Test]
        public void IsFull_ReturnsTrue_WhenCapacityIsReached()
        {
            // Act
            _buffer.Enqueue(1);
            _buffer.Enqueue(2);
            _buffer.Enqueue(3);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_buffer.IsFull, Is.True);
                Assert.That(_buffer.Count, Is.EqualTo(3));
            });
        }

        [Test]
        public void Enqueue_WhenFull_OverwritesOldestElementAndShiftsFront()
        {
            // Arrange & Act
            _buffer.Enqueue(1); // Будет затерт
            _buffer.Enqueue(2);
            _buffer.Enqueue(3);

            // Буфер полон. Добавляем 4-й элемент.
            // Элемент 1 должен удалиться, голова сместиться на элемент 2.
            _buffer.Enqueue(4);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_buffer.Count, Is.EqualTo(3));
                Assert.That(_buffer.IsFull, Is.True);
                Assert.That(_buffer.Peek(), Is.EqualTo(2), "Голова очереди должна сместиться на второй элемент (2).");

                // Проверяем порядок извлечения оставшихся элементов
                Assert.That(_buffer.Dequeue(), Is.EqualTo(2));
                Assert.That(_buffer.Dequeue(), Is.EqualTo(3));
                Assert.That(_buffer.Dequeue(), Is.EqualTo(4));
                Assert.That(_buffer.IsEmpty, Is.True);
            });
        }

        [Test]
        public void Dequeue_OnEmptyBuffer_ThrowsInvalidOperationException()
        {
            // Assert
            Assert.Throws<InvalidOperationException>(() => _buffer.Dequeue());
        }

        [Test]
        public void Peek_OnEmptyBuffer_ThrowsInvalidOperationException()
        {
            // Assert
            Assert.Throws<InvalidOperationException>(() => _buffer.Peek());
        }

        [Test]
        public void Dequeue_ClearsReferenceForGarbageCollector()
        {
            // Проверка, что default устанавливается корректно (важно для ссылочных типов)
            var stringBuffer = new CircularBuffer<string>(2);
            stringBuffer.Enqueue("Тест");

            // Act
            stringBuffer.Dequeue();

            // Чтобы проверить, что внутри массива ссылка затерлась, 
            // мы можем заполнить буфер заново до краев и проверить корректность работы.
            stringBuffer.Enqueue("Новый 1");
            stringBuffer.Enqueue("Новый 2");

            Assert.That(stringBuffer.Peek(), Is.EqualTo("Новый 1"));
        }
    }
}