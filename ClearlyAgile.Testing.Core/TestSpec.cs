using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;

namespace ClearlyAgile.Testing.Core
{
    public class TestSpec<T> where T : class
    {
        private IDictionary<Type, object> mocks = new Dictionary<Type, object>();

        public TType Mock<TType>()
        {
            var type = typeof(TType);

            if (mocks.ContainsKey(type))
            {
                return (TType)mocks[type];
            }

            object mock = Substitute.For(new Type[] { type }, new object[] { });

            return (TType)mock;
        }

        public T Sut()
        {
            var constructorInfos = typeof(T).GetConstructors();

            var constructorInfo = constructorInfos.OrderByDescending(x => x.GetParameters().Length).FirstOrDefault();

            var parameterInfos = constructorInfo.GetParameters();

            var length = parameterInfos.Length;

            var types = new Type[length];

            var constructorParameters = new object[length];

            for (int i = 0; i < length; i++)
            {
                types[i] = parameterInfos[i].ParameterType;
                object mock;

                if (types[i].FullName == typeof(Uri).FullName)
                {
                    mock = new Uri("https://pwc.com");
                }
                else
                {
                    mock = Substitute.For(new Type[]
                    {
                        types[i]
                    }, new object[] { });
                }

                constructorParameters[i] = mock;

                mocks.Add(types[i], mock);
            }

            return (T)Activator.CreateInstance(typeof(T), constructorParameters);
        }
    }
}