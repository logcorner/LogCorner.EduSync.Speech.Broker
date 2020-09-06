using System;
using System.Reflection;

namespace LogCorner.EduSync.Speech.ReadModel.SpeechAggregate
{
    public class Invoker
    {
        public static T CreateInstanceOfAggregateRoot<T>()
        {
            return (T)typeof(T)
                .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new Type[0],
                    new ParameterModifier[0])
                ?.Invoke(new object[0]);
        }
    }
}