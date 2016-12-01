namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;
    
    /// <summary>
    /// Interface to implement to be notified of asynchronous completion via
    /// CefCookieManager::DeleteCookies().
    /// </summary>
    public abstract unsafe partial class CefDeleteCookiesCallback
    {
        private void on_complete(cef_delete_cookies_callback_t* self, int num_deleted)
        {
            CheckSelf(self);
            throw new NotImplementedException(); // TODO: CefDeleteCookiesCallback.OnComplete
        }
        
        /// <summary>
        /// Method that will be called upon completion. |num_deleted| will be the
        /// number of cookies that were deleted or -1 if unknown.
        /// </summary>
        // protected abstract void OnComplete(int num_deleted);
        
    }
}