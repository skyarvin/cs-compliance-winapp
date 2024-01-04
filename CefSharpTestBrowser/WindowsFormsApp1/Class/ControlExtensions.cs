// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;

namespace CSTool.Class
{
    public static class ControlExtensions
    {
        /// CefSharp.WinForms.Internals.ControlExtensions.InvokeOnUiThreadIfRequired is now internal,
        /// it was never intended to be part of the public API
        /// https://github.com/cefsharp/CefSharp/issues/2983

        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// No action will be performed if the control doesn't have a valid handle or the control is Disposed/Disposing.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
        {
            //No action
            if (control.Disposing || control.IsDisposed || !control.IsHandleCreated)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke((Action)(() =>
                {
                    //No action
                    if (control.Disposing || control.IsDisposed || !control.IsHandleCreated)
                    {
                        return;
                    }

                    action();
                }));
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
