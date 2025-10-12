using System;
using System.Linq;

namespace C__NET5;

internal class Lazy
{
    public Lazy()
    {
        var arr = new[] { 1, /*2, 3, 4, 5*/ };
        var query = arr.Where(x => x == 1); // лямбда (точка останова) сработает столько раз, сколько эл-в в массиве
        var a1 = query.ToArray(); // лямбда срабоает
        var a2 = query.ToArray(); // лямбда сработает - не кэшировано. когда кэширует?
        var a3 = a2; // лямбда не сработает - кэшировано?
    }
}

internal class Lazy2
{
    class Person
    {
        public int Age { get; set; }
    }

    public Lazy2()
    {                 // 0, 1, 2
        var persons = Enumerable.Range(0, 3).Select(i => new Person { Age = i })/*.ToArray()*/; // возвр-т IEnumerable(не массив), отложеннная иниц-я

        foreach (var p in persons) // заново проиниц-т. раз Lazy, Age ещё не проиниц-н
        {
            p.Age++; // эффекта от это строки нет. без неё рез-т тот же
        }

        foreach (var p in persons)
        {
            Console.WriteLine(p.Age); // 0, 1, 2;   1, 2, 3 - c ToArray/ToList
        }
    }
}