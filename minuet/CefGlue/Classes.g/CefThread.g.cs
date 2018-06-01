//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;
    
    // Role: PROXY
    public sealed unsafe partial class CefThread : IDisposable
    {
        internal static CefThread FromNative(cef_thread_t* ptr)
        {
            return new CefThread(ptr);
        }
        
        internal static CefThread FromNativeOrNull(cef_thread_t* ptr)
        {
            if (ptr == null) return null;
            return new CefThread(ptr);
        }
        
        private cef_thread_t* _self;
        
        private CefThread(cef_thread_t* ptr)
        {
            if (ptr == null) throw new ArgumentNullException("ptr");
            _self = ptr;
        }
        
        ~CefThread()
        {
            if (_self != null)
            {
                Release();
                _self = null;
            }
        }
        
        public void Dispose()
        {
            if (_self != null)
            {
                Release();
                _self = null;
            }
            GC.SuppressFinalize(this);
        }
        
        internal void AddRef()
        {
            cef_thread_t.add_ref(_self);
        }
        
        internal bool Release()
        {
            return cef_thread_t.release(_self) != 0;
        }
        
        internal bool HasOneRef
        {
            get { return cef_thread_t.has_one_ref(_self) != 0; }
        }
        
        internal cef_thread_t* ToNative()
        {
            AddRef();
            return _self;
        }
    }
}