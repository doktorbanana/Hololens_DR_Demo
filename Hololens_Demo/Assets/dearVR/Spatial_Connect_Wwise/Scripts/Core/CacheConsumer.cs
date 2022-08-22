using System;

namespace SpatialConnect.Wwise.Core
{
    public interface ICacheConsumer<T> where T : class
    {
        T Value { set; }
        
        void Consume();
    }

    public class CacheConsumer<T> : ICacheConsumer<T> where T : class
    {
        private T value_;
        private readonly Action<T> consumeAction_;
        
        public T Value { set => value_ = value; }

        public CacheConsumer(Action<T> consumeAction)
        {
            consumeAction_ = consumeAction;
        }
        
        public void Consume()
        {
            if (value_ == null)
                return;
            consumeAction_.Invoke(value_);
            value_ = null;
        }
    }
}
