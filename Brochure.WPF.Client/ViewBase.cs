using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Brochure.Core.Extends;
using Brochure.Core.Helper;

namespace Brochure.WPF.Client
{
    public abstract class ViewBase : UserControl
    {
        private readonly ViewModelBase _viewModel;
        protected ViewBase() : this(null)
        {
        }
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
        public ViewModelBase ViewModel
        {
            get => DataContext as ViewModelBase;
            set => DataContext = value;
        }
    }

    public abstract class WindowViewBase : Window
    {
        private readonly ViewModelBase _viewModel;
        protected WindowViewBase() : this(null)
        {

        }
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
        public ViewModelBase ViewModel
        {
            get => DataContext as ViewModelBase;
            set => DataContext = value;
        }
    }
}
