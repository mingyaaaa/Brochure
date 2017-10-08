using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Brochure.Core.Extends;
using Brochure.Core.Helper;
using Brochure.WPF.Client.Interface;

namespace Brochure.WPF.Client
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ViewBase : UserControl
    {
        private readonly ViewModelBase _viewModel;

        /// <summary>
        ///
        /// </summary>
        protected ViewBase() : this(null)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="Exception"></exception>
        protected ViewBase(params object[] obj)
        {
            if (ViewModel == null)
            {
                var type = GetType().GetTypeInfo();
                var imp = type.ImplementedInterfaces.FirstOrDefault(t => t.Name == typeof(IViewModel<>).Name);
                if (imp == null)
                    throw new Exception("View没有实现IViewModel接口,指定ViewModel");
                var instanceType = imp.GenericTypeArguments[0];
                var constructorInfo = instanceType.GetConstructor(obj.Select(t => t.GetType()).ToArray());
                if (constructorInfo == null)
                    throw new Exception("没有对应的构造函数");
                _viewModel = constructorInfo.Invoke(obj).As<ViewModelBase>();
            }
            if (obj == null)
                _viewModel.LoadDataAsync();
            else
            {
                _viewModel.LoadDataAsync();
                _viewModel.LoadDataAsync(obj);
            }
            Initialized += (o, e) =>
            {
                ViewModel = _viewModel;
                ViewModel.InitCommand();
            };
        }

        /// <summary>
        ///
        /// </summary>
        public ViewModelBase ViewModel
        {
            get => DataContext as ViewModelBase;
            set => DataContext = value;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class WindowViewBase : Window
    {
        private readonly ViewModelBase _viewModel;

        /// <summary>
        ///
        /// </summary>
        protected WindowViewBase() : this(null)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="Exception"></exception>
        protected WindowViewBase(params object[] obj)
        {
            if (ViewModel == null)
            {
                var type = GetType().GetTypeInfo();
                var imp = type.ImplementedInterfaces.FirstOrDefault(t => t.Name == typeof(IViewModel<>).Name);
                if (imp == null)
                    throw new Exception("View没有实现IViewModel接口,指定ViewModel");
                var instanceType = imp.GenericTypeArguments[0];
                ConstructorInfo constructorInfo;
                if (obj == null)
                    constructorInfo = instanceType.GetConstructor(new Type[] { });
                else
                    constructorInfo = instanceType.GetConstructor(obj.Select(t => t.GetType()).ToArray());
                if (constructorInfo == null)
                    throw new Exception("没有对应的构造函数");
                _viewModel = constructorInfo.Invoke(obj).As<ViewModelBase>();
            }

            Initialized += (o, e) =>
            {
                ViewModel = _viewModel;
                ViewModel.InitCommand();
                if (obj == null)
                    _viewModel.LoadDataAsync();
                else
                {
                    _viewModel.LoadDataAsync();
                    _viewModel.LoadDataAsync(obj);
                }
            };
        }

        /// <summary>
        ///
        /// </summary>
        public ViewModelBase ViewModel
        {
            get => DataContext as ViewModelBase;
            set => DataContext = value;
        }
    }
}