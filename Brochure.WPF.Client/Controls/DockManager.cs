using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Brochure.WPF.Client.Abstract;

namespace Brochure.WPF.Client.Controls
{
    /// <summary>
    ///
    /// </summary>
    [ContentProperty("Layout")]
    public class DockManager : UserControl
    {
        static DockManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public DockManager()
        {
        }

        #region DependencyProperty

        #region LayoutRoot

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
            "Layout", typeof(LayoutRoot), typeof(DockManager), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnLayoutChanged)));

        /// <summary>
        ///
        /// </summary>
        public LayoutRoot Layout
        {
            get => (LayoutRoot)GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }

        private static void OnLayoutChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var dockManager = sender as DockManager;
            dockManager?.OnLayoutChanged(e);
        }

        private void OnLayoutChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion LayoutRoot

        #region Theme

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(
            "Theme", typeof(Theme), typeof(DockManager), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnThemeChanged)));

        /// <summary>
        ///
        /// </summary>
        public Theme Theme
        {
            get => (Theme)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }

        private static void OnThemeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var dockManager = sender as DockManager;
            dockManager?.OnThemeChanged(e);
        }

        private void OnThemeChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldTheme = e.OldValue as Theme;
            var newTheme = e.NewValue as Theme;
            var resource = this.Resources;
            if (oldTheme != null)
            {
                var resourceDictionaryToRemove = resource.MergedDictionaries.FirstOrDefault(t => t.Source == oldTheme.GetReourceUri());
                resourceDictionaryToRemove?.MergedDictionaries.Remove(resourceDictionaryToRemove);
            }
            if (newTheme != null)
            {
                resource.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = newTheme.GetReourceUri()
                });
            }
        }

        #endregion Theme

        #region DocumentPaneTemplate

        /// <summary>
        /// DocumentPaneTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty DocumentPaneTemplateProperty =
            DependencyProperty.Register("DocumentPaneTemplate", typeof(ControlTemplate), typeof(DockManager),
                new FrameworkPropertyMetadata((ControlTemplate)null,
                    new PropertyChangedCallback(OnDocumentPaneTemplateChanged)));

        /// <summary>
        /// Gets or sets the DocumentPaneDataTemplate property.  This dependency property
        /// indicates .
        /// </summary>
        public ControlTemplate DocumentPaneTemplate
        {
            get { return (ControlTemplate)GetValue(DocumentPaneTemplateProperty); }
            set { SetValue(DocumentPaneTemplateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DocumentPaneTemplate property.
        /// </summary>
        private static void OnDocumentPaneTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DockManager)d).OnDocumentPaneTemplateChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DocumentPaneTemplate property.
        /// </summary>
        protected virtual void OnDocumentPaneTemplateChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion DocumentPaneTemplate

        #endregion DependencyProperty

        /// <summary>
        ///
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}