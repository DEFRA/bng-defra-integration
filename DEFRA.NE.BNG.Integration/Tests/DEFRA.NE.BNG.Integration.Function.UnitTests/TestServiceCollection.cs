using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace DEFRA.NE.BNG.Integration.Function.Tests
{
    public class TestServiceCollection : IServiceCollection
    {
        private readonly List<ServiceDescriptor> services = [];

        public ServiceDescriptor this[int index] { get => services[index]; set => services[index] = value; }

        public int Count => services.Count;

        public bool IsReadOnly => true;

        public void Add(ServiceDescriptor item)
        {
            services.Add(item);
        }

        public void Clear()
        {
            services.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return services.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {

        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return services.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return services.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            services.Insert(index, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            return services.Remove(item);
        }

        public void RemoveAt(int index)
        {
            services.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return services.GetEnumerator();
        }
    }
}