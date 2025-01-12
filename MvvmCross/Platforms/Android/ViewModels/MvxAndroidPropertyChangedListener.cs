﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Android.Runtime;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.ViewModels
{
    /// <summary>
    ///     Just like <see cref="MvxPropertyChangedListener"/> but
    ///     won't call handlers if the target (being an activity, fragment,
    ///     view or other object that belongs to the Java VM) is in "mono
    ///     limbo" (where the object still exists in the mono VM, but not
    ///     in the Java VM).
    /// </summary>
    public class MvxAndroidPropertyChangedListener : MvxPropertyChangedListener
    {
        private readonly WeakReference<IJavaObject> _target;

        public MvxAndroidPropertyChangedListener(INotifyPropertyChanged source, IJavaObject target) : base(source)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            _target = new WeakReference<IJavaObject>(target);
        }

        public override void NotificationObjectOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IJavaObject target;

            if (!_target.TryGetTarget(out target) || target.Handle == IntPtr.Zero)
                return;

            base.NotificationObjectOnPropertyChanged(sender, e);
        }
    }
}
