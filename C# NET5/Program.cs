using C__NET5.async_await;
using C__NET5.keywords;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace C__NET5;

internal class Program
{
    #region by reference
    class Contact
    {
        public int X { get; set; }
    }

    static void SetContact(Contact contact)
    {
        contact.X = 1;
    }
    #endregion

    #region async await
    private static string str = "initial";

    async static Task<string> Delay()
    {
        Thread.Sleep(5000);
        //for (long i = 0; i < 10000000000; i++) i++; // delay 7000

        str = "after sleep";
        return "default";
    }

    static string Delay2()
    {
        Thread.Sleep(5000);
        str = "after sleep";
        return "default";
    }

    static Task Delay3_()
    {
        Task.Delay(5000); // паузы не будет. следующая строка тут же сработает
        str = "after sleep";
        return Task.CompletedTask;
    }

    static async Task Delay3()
    {
        await Task.Delay(5000); // будет пауза, если в вызывающем коде - await
        str = "after sleep";
    }
    
    static async Task Delay3NoAwait()
    {
        Task.Delay(5000); // паузы не будет, т.к. нет await следующая строка тут же сработает
        str = "after sleep";
    }

    static Task Delay4()
    {
        for (long i = 0; i < 10000000000; i++) i++; // delay 7000
        str = "after sleep";
        return Task.CompletedTask;
    }

    static async Task Delay5()
    {
        await Task.Delay(5000); // паузы не будет
    }
    
    static Task DelayNoAsyncInSignature()
    {
        Task.Delay(5000); // паузы не будет. await нельзя
        return Task.CompletedTask;
    }

    // habr
    static async Task ExecuteOperation() // задача внутри задачи?
    {
        Console.WriteLine($"Before: {Thread.CurrentThread.ManagedThreadId}"); // ManagedThreadId не меняется
        await Task.Run(() =>
        {
            Console.WriteLine($"Inside before sleep: {Thread.CurrentThread.ManagedThreadId}"); // ManagedThreadId меняется
            Thread.Sleep(1000);
            Console.WriteLine($"Inside after sleep: {Thread.CurrentThread.ManagedThreadId}");
        });
        Console.WriteLine($"After: {Thread.CurrentThread.ManagedThreadId}");
    }
    
    static async Task ExecuteOperationNoAwait()
    {
        Console.WriteLine($"Before: {Thread.CurrentThread.ManagedThreadId}");
        Task.Run(() => // без await результат другой. хоть в вызывающем методе await - Task.Run ... не дожидается выполнения
        {
            Console.WriteLine($"Inside before sleep: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
            Console.WriteLine($"Inside after sleep: {Thread.CurrentThread.ManagedThreadId}");
        });
        Console.WriteLine($"After: {Thread.CurrentThread.ManagedThreadId}");
    }

    // отличия
    async Task AsyncTask() { } // async - модификатор, означающий, что метод выполняется асинхронно; Task - возвращаемое значение
    Task Task_() { return null; }

    static async Task ExecuteOperationTaskDelay()
    {
        Console.WriteLine($"Before: {Thread.CurrentThread.ManagedThreadId}"); // ManagedThreadId не меняется
        await Task.Run(() =>
        {
            Console.WriteLine($"Inside before sleep: {Thread.CurrentThread.ManagedThreadId}"); // ManagedThreadId меняется
            Task.Delay(5000); // паузы нет
            Console.WriteLine($"Inside after sleep: {Thread.CurrentThread.ManagedThreadId}");
        });
        Console.WriteLine($"After: {Thread.CurrentThread.ManagedThreadId}");
    }
    
    static async Task ExecuteOperationTaskDelayAsyncLambda()
    {
        Console.WriteLine($"Before: {Thread.CurrentThread.ManagedThreadId}"); // ManagedThreadId не меняется
        await Task.Run(async () => // можно без async. тогда надо убрать await
        {
            Console.WriteLine($"Inside before sleep: {Thread.CurrentThread.ManagedThreadId}"); // ManagedThreadId меняется
            await Task.Delay(5000); // можно без await. тогда паузы не будет
            Console.WriteLine($"Inside after sleep: {Thread.CurrentThread.ManagedThreadId}");
        });
        Console.WriteLine($"After: {Thread.CurrentThread.ManagedThreadId}");
    }
    #endregion
    static void PrintName(string name)
    {
        //Thread.Sleep(3000);     // имитация продолжительной работы
        Console.WriteLine(name); // зависает
    }

    static async Task Main()
    {
        new Generic.Covariance();
        new Generic.Contravariance();
        new Generic.CovarianceContravariance();

        new Lazy();
        new Lazy2();

        #region #virtual #override
        double r = 3.0, h = 5.0;

        //new Square(r).Area();
        //new Circle(r).Area();
        //new Sphere(r).Area();
        //new Cylinder(r, h).Area();

        //Shape s = new Shape(r, h);
        //s.Area(); // используем именно эту переменную во всей программе. вызовется у Shape
        //s = new Square(r);
        //s.Area(); // где-то в рантайме специализировали. вызовется у Square
        // итог: работает полиморфизм. не надо создавать новую переменную, плодить разные, но похожие методы
        #endregion

        new LINQ();

        #region #async #await
        var asyncAwait = new AsyncAwait();
        await asyncAwait.Main1();
        //await ExecuteOperation();
        //ExecuteOperation(); // выполняется меньше кода
        //await ExecuteOperationNoAwait(); // выполняется больше кода. по любому строки вне Task.Run
        //ExecuteOperationNoAwait(); // выполняются синхронные строки Before, After, хоть и не дожидаемся ничего
        //await ExecuteOperationTaskDelay();
        //await ExecuteOperationTaskDelayAsyncLambda();
        //var breakpoint = 0;
        //Delay5();
        //DelayNoAsyncInSignature();

        //asyncAwait.AsyncVoidExceptions_CannotBeCaughtByCatch();
        //asyncAwait.AsyncVoidExceptions();
        //AsyncAwait.f(); // неудачный пример
        //AsyncAwait.g();

        // Delay3();
        //await Delay3();
        //await Delay3_();

        // результат идентичный
        //Delay4();
        //Delay4().Wait();
        //await Delay4();

        //Delay2();

        //Task.Run(() => Delay2());
        //var t = Task.Run(() => Delay2()); t.Wait();
        //await Task.Run(() => Delay2());


        //AsyncAndAwaitInCSharp.Main1();

        //Test t = new Test();
        //try
        //{
        //    //t.Call();
        //    t.Call();
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}

        var results = new List<int>();
        for (int i = 0; i < 30; i++)
        {
            var result = ConcurrencyInCsharpCookBook.ParallelSum(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }); // 15
            results.Add(result);
        }
        //var resultsArr = results.ToArray().ToString();
        #endregion

        //OOP.Main_();

        #region immutable class
        var i0 = new object();
        var i1 = new ImmutableClass(1);
        var i2 = new ImmutableClass(1);
        var i3 = new ImmutableClass(2);
        var i1_true = i1.Equals(i2); // true
        var i2_false = i1.Equals(i3); // false
        var i3_true = i1.Equals(i1); // true

        var i4_false = ReferenceEquals(i1, i2); // false
        var i5_false = ReferenceEquals(i1, i3); // false
        var i6_true = ReferenceEquals(i1, i1); // true
        #endregion

        #region object
        var foo1 = new Foo();
        var foo1_ = foo1;
        var foo2 = new Foo();
        
        var hashCode = foo2.GetHashCode(); // при перезапуске проекта всегда такой же

        // все методы сравнения Object - по ссылке. по дефолту не знает как сравнивать по значению - объектов пользователя ещё нет. по значению - надо сравнивать по полям
        // эквивалентно
        var eq0 = Object.Equals(foo1, foo2); // false. не переопредилили.
        var eq0_ = foo1.Equals(foo2); // false
        
        // эквивалентно
        var eq1 = Object.Equals(foo1, foo1_); // true
        var eq1_ = foo1.Equals(foo1_); // true

        var eq2 = Object.ReferenceEquals(foo1, foo2); // false
        var eq2_ = Object.ReferenceEquals(foo1, foo1_); // true
        //foo1.X = 1;
        //foo2.X = 2;
        //foo1 = foo2;
        var eq4 = foo1 == foo2; // подкапот ReferenceEquals

        foo1 = foo2; // обе ссылки указывают на 1 и тот же объект

        var eq5 = Equals(foo1, foo2); // без Object. true
        var eq6 = Object.ReferenceEquals(foo1, foo2); // true
        var eq7 = foo1.Equals(foo2); // true
        var eq8 = foo1 == foo2;

        var eq9 = Object.Equals(foo1, foo1); // true
        var eq10 = Object.ReferenceEquals(foo1, foo1); // true
        var eq11 = foo1.Equals(foo1); // true
        var eq12 = foo1 == foo2; // true

        var foo2_ = new Foo2();
        //foo2 = foo2_; // нельзя

        var eq13 = new Foo1 { X = 1 }.Equals(new Foo2 { X = 1 }); // true
        var eq14 = new Foo1 { X = 1 }.Equals(new Foo2 { X = 2 }); // false
        #endregion

        var f = new Finalize_();
        f = null; // должен вызваться финализатор https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/finalizers

        new Exceptions();



        new Cast().Run();

        Common common = new Common();
        common.Run();
        common.Run3();
        common.Run4();

        new VirtualOverride().Run();

        var ref_ = new Ref();
        #region
        var foo = new Ref.Foo();
        ref_.Bar(foo);
        ref_.Bar2(foo);
        ref_.Bar3(foo); // Name не меняется

        foo = new Ref.Foo();
        ref_.Bar(foo);
        ref_.BarRef2(ref foo);
        ref_.BarRef3(ref foo); // Name = null
        #endregion
        #region
        var testRef = new[] { "0", "1" };
        ref_.Swap(ref testRef[0], ref testRef[1]);
        var test = new[] { "0", "1" };
        ref_.Swap(test[0], test[1]); // не меняет
        #endregion
        ref_.Run();
        Console.WriteLine("name"); // зависает
    }

    // почему основной метод async Task?
    // почему точка входа - статическая?
    static async Task Main_(string[] args) 
    {
        PrintName("xxx");

        #region by reference
        var contact = new Contact();
        SetContact(contact);
        #endregion






        
        //Object_.Main();
        
        Console.WriteLine("Hello World!");
        Console.ReadLine();
        //SafeHandle
    }
}
