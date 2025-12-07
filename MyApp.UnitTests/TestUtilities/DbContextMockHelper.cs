using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace MyApp.UnitTests.TestUtilities
{
    public static class DbContextMockHelper
    {
        // 1. Mock للـ FirstOrDefaultAsync (مبسط)
        public static void SetupFirstOrDefaultAsync<T>(this Mock<DbSet<T>> mockSet, T returnValue)
            where T : class
        {
            var data = new List<T> { returnValue }.AsQueryable();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider)
                  .Returns(new TestAsyncQueryProvider<T>(data.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression)
                  .Returns(data.Expression);

            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType)
                  .Returns(data.ElementType);

            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
                  .Returns(data.GetEnumerator());
        }

        // 2. Mock للـ Where (مبسط)
        public static void SetupWhere<T>(this Mock<DbSet<T>> mockSet, List<T> filteredData)
            where T : class
        {
            var queryableData = filteredData.AsQueryable();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider)
                  .Returns(queryableData.Provider);

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression)
                  .Returns(queryableData.Expression);

            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType)
                  .Returns(queryableData.ElementType);

            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
                  .Returns(queryableData.GetEnumerator());
        }

        // 3. Helper class للـ async operations (مصحح)
        internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            public TestAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
            {
                // هذا النوع من التحويل مطلوب للـ async operations
                var expectedResultType = typeof(TResult).GetGenericArguments()[0];
                var executionResult = typeof(IQueryProvider)
                    .GetMethod(
                        name: "Execute",
                        genericParameterCount: 1,
                        types: new[] { typeof(Expression) })
                    .MakeGenericMethod(expectedResultType)
                    .Invoke(this, new object[] { expression });

                return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                    .MakeGenericMethod(expectedResultType)
                    .Invoke(null, new object[] { executionResult });
            }
        }

        // 4. TestAsyncEnumerable (مصحح)
        internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

            public TestAsyncEnumerable(Expression expression) : base(expression) { }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return this.AsEnumerable().GetEnumerator();
            }
        }

        // 5. TestAsyncEnumerator (مصحح)
        internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public T Current => _inner.Current;

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_inner.MoveNext());
            }

            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return ValueTask.CompletedTask;
            }
        }
    }
}