using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework.Constraints;
using RoadExp.Measurements.Common.Tests.Data;
using RoadExp.Measurements.Core;

namespace RoadExp.Measurements.Common.Tests
{
    //[TestFixture]
    //public class DataStorageTests
    //{
    //    [Test]
    //    public async Task StartSaving_EnableSaving_ShouldSavingStart()
    //    {
    //        // Arrange
    //        var array = new byte[] { 2 };
    //        using var waitHandle = new AutoResetEvent(false);
    //        var data = new MeasurementBatch(1, array);
    //        var writerMock = new Mock<IDataWriter>();

    //        // При вызове Write сигнализируем потоку теста
    //        writerMock
    //            .Setup(x => x.Write(It.IsAny<MeasurementBatch>()))
    //            .Callback(() => waitHandle.Set());

    //        var storage = new DataStorage(100, writerMock.Object, NullLogger<DataStorage>.Instance);

    //        // Act & Assert 1: Запись выключена, метод вызываться не должен
    //        storage.Add(data);
    //        // Ждем 50 мс. Метод возвращает false, если сигнал НЕ пришел за это время
    //        bool isCalledBeforeStart = waitHandle.WaitOne(50);
    //        Assert.That(isCalledBeforeStart, Is.False, "Метод Write не должен вызываться до StartSaving");

    //        // Act & Assert 2: Включаем запись, метод должен быть вызван
    //        storage.StartSaving();
    //        storage.Add(data);

    //        // Ждем сигнала до 5 секунд (тест пойдет дальше сразу же, как только другой поток вызовет Write)
    //        bool isCalledAfterStart = waitHandle.WaitOne(TimeSpan.FromSeconds(5));
    //        Assert.That(isCalledAfterStart, Is.True, "Метод Write должен был вызваться после StartSaving");

    //        // Act & Assert 3: Выключаем запись обратно
    //        waitHandle.Reset(); // Сбрасываем состояние объекта в несигнальное
    //        storage.StopSaving();
    //        storage.Add(data);

    //        bool isCalledAfterStop = waitHandle.WaitOne(50);
    //        Assert.That(isCalledAfterStop, Is.False, "Метод Write не должен вызываться после StopSaving");
    //    }

    //    [Test]
    //    public async Task ProcessQueueAsync_WhenWriterThrows_ShouldLogErrorAndContinue()
    //    {
    //        // Arrange
    //        var array = new byte[] { 100 };
    //        using var waitHandle = new AutoResetEvent(false);
    //        var testData = new MeasurementBatch(1, array);
    //        var expectedException = new InvalidOperationException("Ошибка диска или файла");

    //        var writerMock = new Mock<IDataWriter>();

    //        // 1. Настраиваем писатель так, чтобы он выкидывал исключение
    //        writerMock
    //            .Setup(x => x.Write(It.IsAny<MeasurementBatch>()))
    //            .Callback(() => waitHandle.Set()) // Сигнализируем тесту, что Callback отработал
    //            .Throws(expectedException);

    //        // 2. Создаем Mock для ILogger
    //        var loggerMock = new Mock<ILogger<DataStorage>>();

    //        await using var storage = new DataStorage(10, writerMock.Object, loggerMock.Object);

    //        // Act
    //        storage.StartSaving();
    //        storage.Add(testData);

    //        // Ждем, пока фоновый поток дойдет до вызова Write и выбросит исключение
    //        bool isCalled = waitHandle.WaitOne(TimeSpan.FromSeconds(2));
    //        Assert.That(isCalled, Is.True, "Метод Write не был вызван.");

    //        // Небольшая задержка, чтобы фоновый поток успел передать исключение в логгер
    //        await Task.Delay(50);

    //        // Assert: Проверяем, что логгер перехватил ошибку со статусом LogLevel.Error
    //        loggerMock.Verify(
    //            x => x.Log(
    //                LogLevel.Error,                                      // Проверяем уровень лога
    //                It.IsAny<EventId>(),                                 // Любой ID события
    //                It.Is<It.IsAnyType>((v, t) => true),                // Проверяем состояние (форматированный текст)
    //                expectedException,                                   // Проверяем, что передано именно наше исключение
    //                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), // Функция форматирования
    //            Times.Once,
    //            "Логгер должен был зафиксировать ошибку LogLevel.Error ровно один раз."
    //        );
    //    }

        //[Test]
        //public async Task Add_WhenBufferIsFull_ShouldDropOldestItem()
        //{
        //    //Как работает этот тест:
        //    //1. ManualResetEventSlim используется для жесткой координации потоков.
        //    //   Тест не начнет спамить буфер, пока первый элемент гарантированно не уйдет на обработку.

        //    //2. Изоляция буфера: Как только item1 ушел в работу, внутри Channel освободилось место.
        //    //   Мы забиваем его item2, item3 и item4. Буфер полон.

        //    //3. Проверка вытеснения:
        //    //   При добавлении item5 встроенный механизм BoundedChannelFullMode.DropOldest на уровне самого класса Channel видит переполнение,
        //    //   находит в голове очереди item2 и молча стирает его, освобождая место для item5.

        //    //4. storage.DisposeAsync() в конце — это красивый способ дождаться,
        //    //   пока воркер штатно завершит обработку оставшихся элементов после того,
        //    //   как мы сняли блокировку(blockWorkerSignal.Set()).

        //    // Arrange
        //    const int bufferCapacity = 3;

        //    // Используем ManualResetEventSlim, чтобы заблокировать фоновый воркер
        //    using var blockWorkerSignal = new ManualResetEventSlim(false);
        //    // Сигнал для теста, что воркер зашел в метод Write для первого элемента
        //    using var workerStartedSignal = new ManualResetEventSlim(false);

        //    // Список, куда мы будем записывать фактически дошедшие до хранилища элементы
        //    var writtenItems = new List<IntData>();

        //    var writerMock = new Mock<IDataWriter>();

        //    writerMock
        //        .Setup(x => x.Write(It.IsAny<MeasurementBatch>()))
        //        .Callback<IntData>(item =>
        //        {
        //            lock (writtenItems)
        //            {
        //                writtenItems.Add(item);
        //            }
        //            workerStartedSignal.Set();     // Сообщаем тесту: первый элемент начали обрабатывать
        //            blockWorkerSignal.Wait();      // Искусственно подвешиваем фоновый поток
        //        });

        //    var loggerMock = new Mock<ILogger<DataStorage>>();

        //    // Создаем хранилище с маленьким буфером размера 3
        //    var storage = new DataStorage(bufferCapacity, writerMock.Object, loggerMock.Object);
        //    storage.StartSaving();

        //    // 1. Добавляем элемент #1. Вокер тут же заберет его из канала и заблокируется внутри Write
        //    var item1 = new IntData(1, 10);
        //    storage.Add(item1);

        //    // Гарантированно ждем, пока воркер заберет первый элемент и заблокируется
        //    Assert.That(workerStartedSignal.Wait(TimeSpan.FromSeconds(2)), Is.True);

        //    // Теперь канал (буфер) пуст, но воркер занят. Заполняем буфер до максимума (3 элемента)
        //    var item2 = new IntData(2, 20); // Станет самым старым ВНУТРИ канала
        //    var item3 = new IntData(3, 30);
        //    var item4 = new IntData(4, 40); // Заполнил буфер целиком

        //    storage.Add(item2);
        //    storage.Add(item3);
        //    storage.Add(item4);

        //    // 2. Добавляем элемент #5 при ПЕРЕПОЛНЕННОМ буфере.
        //    // Стратегия DropOldest должна удалить из канала item2 и вставить вместо него item5.
        //    var item5 = new IntData(5, 50);
        //    storage.Add(item5);

        //    // Act
        //    blockWorkerSignal.Set(); // Разблокируем воркер, позволяя ему дочитать канал
        //    await storage.DisposeAsync(); // Корректно завершаем работу и дожидаемся вычитывания хвостов

        //    // Assert
        //    // Всего должно быть записано 4 элемента: 
        //    // item1 (который обрабатывался сразу) + item3, item4, item5 (из канала).
        //    // item2 должен быть выброшен (Dropped).
        //    Assert.That(writtenItems.Count, Is.EqualTo(4), "Количество записанных элементов не совпадает.");

        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(writtenItems, Contains.Item(item1), "Первый элемент должен быть записан.");
        //        Assert.That(writtenItems, Contains.Item(item3), "Третий элемент должен быть записан.");
        //        Assert.That(writtenItems, Contains.Item(item4), "Четвертый элемент должен быть записан.");
        //        Assert.That(writtenItems, Contains.Item(item5), "Пятый элемент должен быть записан (он вытеснил второй).");

        //        Assert.That(writtenItems, Does.Not.Contain(item2), "Второй элемент должен был быть выброшен из-за переполнения.");
        //    });
        //}

    //}
}
