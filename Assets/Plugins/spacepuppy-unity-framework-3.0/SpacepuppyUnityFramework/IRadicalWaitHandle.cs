﻿

namespace com.spacepuppy
{

    /// <summary>
    /// A yield instruction used by RadicalCoroutine that can also have a callback manually attached to it through the 'IRadicalWaitHandle.OnComplete' method.
    /// </summary>
    public interface IRadicalWaitHandle : IRadicalYieldInstruction
    {

        /// <summary>
        /// The completion of the wait handle was the result of it being cancelled.
        /// </summary>
        bool Cancelled { get; }

        /// <summary>
        /// Called when the wait handle completed. This includes being cancelled. Check 'Cancelled' to know why it completed.
        /// </summary>
        /// <param name="callback"></param>
        void OnComplete(System.Action<IRadicalWaitHandle> callback);

    }

    /// <summary>
    /// Base implemenation of IRadicalWaitHandle.
    /// </summary>
    public class RadicalWaitHandle : IRadicalWaitHandle, IPooledYieldInstruction
    {

        #region Fields

        private bool _complete;
        private System.Action<IRadicalWaitHandle> _callback;

        #endregion

        #region CONSTRUCTOR

        protected RadicalWaitHandle()
        {

        }

        #endregion

        #region Methods

        public void SignalCancelled()
        {
            if (_complete) return;

            _complete = true;
            this.Cancelled = true;
            if (_callback != null) _callback(this);
        }

        public void SignalComplete()
        {
            if (_complete) return;

            _complete = true;
            if (_callback != null) _callback(this);
        }

        public void Reset()
        {
            _complete = false;
            this.Cancelled = false;
            _callback = null;
        }

        protected virtual bool Tick(out object yieldObject)
        {
            yieldObject = null;
            return !_complete;
        }

        #endregion

        #region IRadicalWaitHandle Interface

        public bool Cancelled
        {
            get;
            private set;
        }

        public bool IsComplete
        {
            get { return _complete; }
        }

        public void OnComplete(System.Action<IRadicalWaitHandle> callback)
        {
            if (callback == null) throw new System.ArgumentNullException("callback");
            if (_complete) throw new System.InvalidOperationException("Can not wait for complete on an already completed IRadicalWaitHandle.");
            _callback += callback;
        }

        bool IRadicalYieldInstruction.Tick(out object yieldObject)
        {
            if (_complete)
            {
                yieldObject = null;
                return false;
            }

            return this.Tick(out yieldObject);
        }

        #endregion

        #region IPooledYieldInstruction Interface

        void System.IDisposable.Dispose()
        {
            this.Reset();
            if (this.GetType() == typeof(RadicalWaitHandle))
            {
                _pool.Release(this);
            }
        }

        #endregion

        #region Static Interface

        private static com.spacepuppy.Collections.ObjectCachePool<RadicalWaitHandle> _pool = new com.spacepuppy.Collections.ObjectCachePool<RadicalWaitHandle>(-1, () => new RadicalWaitHandle());

        public static IRadicalWaitHandle Null
        {
            get
            {
                return NullYieldInstruction.Null;
            }
        }

        public static RadicalWaitHandle Create()
        {
            return _pool.GetInstance();
        }

        #endregion

    }

}
