using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WaGonLi.Controls
{

    /// <summary>
    /// This control implements a placeholder effect over a passwordbox
    /// </summary>
    [TemplatePart(Name = PasswordBoxTemplatePartName, Type=typeof(PasswordBox))]
    [TemplateVisualState(GroupName = PlaceholderStateStateGroupName, Name = PlaceholderStateVisibleName)]
    [TemplateVisualState(GroupName = PlaceholderStateStateGroupName, Name = PlaceholderStateHiddenName)]
    public class PasswordBoxWithPlaceholder
        : Control
    {
        private const string PasswordBoxTemplatePartName = "PasswordBox";
        private const string PlaceholderStateStateGroupName = "PlaceholderStateGroup";
        private const string PlaceholderStateVisibleName = "PlaceholderVisible";
        private const string PlaceholderStateHiddenName = "PlaceholderHidden";

        private PasswordBox _passwordBox;

        public PasswordBoxWithPlaceholder()
        {
            this.DefaultStyleKey = typeof (PasswordBoxWithPlaceholder);
            this.GotFocus += OnControlGotFocus;
            this.Loaded += OnLoaded;
        }

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceholderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(PasswordBoxWithPlaceholder), new PropertyMetadata(string.Empty));


        public string Password
        {
            get
            {
                return _passwordBox != null ? _passwordBox.Password : string.Empty;
            }
            set
            {
                if (_passwordBox != null)
                {
                    _passwordBox.Password = value;
                }
            }
        }


        private void OnControlGotFocus(object sender, RoutedEventArgs e)
        {
            if (_passwordBox == null)
            {
                return;
            }

            _passwordBox.Focus();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SetPlaceholderVisible();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_passwordBox != null)
            {
                _passwordBox.GotFocus -= OnPasswordBoxGotFocus;
                _passwordBox.LostFocus -= OnPasswordBoxLostFocus;
                _passwordBox.PasswordChanged -= OnPasswordChanged;
            }

            _passwordBox = this.GetTemplateChild(PasswordBoxTemplatePartName) as PasswordBox;

            if (_passwordBox != null)
            {
                _passwordBox.GotFocus += OnPasswordBoxGotFocus;
                _passwordBox.LostFocus += OnPasswordBoxLostFocus;
                _passwordBox.PasswordChanged += OnPasswordChanged;
            }
            SetPlaceholderVisible();
        }

        /// <summary>
        /// Handles the password changed event in case the password is being changed by 
        /// a twoway binding
        /// </summary>
        /// <remarks>
        /// this event is ignored if the password is changed "by hand"
        /// </remarks>
        /// <param name="sender">the object that sends the event</param>
        /// <param name="e">event arg</param>
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var focusedControl = FocusManager.GetFocusedElement();
            if (focusedControl == _passwordBox)
            {
                //this case will be managed by the lostfocus event
                return;
            }

            SetPlaceholderVisible();
        }

        private void SetPlaceholderVisible()
        {
            if (_passwordBox == null)
            {
                return;
            }

            VisualStateManager.GoToState(this,
                string.IsNullOrEmpty(this._passwordBox.Password) ? PlaceholderStateVisibleName : PlaceholderStateHiddenName,
                false);
        }

        private void OnPasswordBoxGotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, PlaceholderStateHiddenName, false);
        }

        private void OnPasswordBoxLostFocus(object sender, RoutedEventArgs e)
        {
            SetPlaceholderVisible();
        }
    }
}
